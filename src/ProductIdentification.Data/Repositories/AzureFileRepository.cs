using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using ProductIdentification.Core.Repositories;
using Microsoft.Azure.Storage.DataMovement;
using ProductIdentification.Common;
using ProductIdentification.Core.Models;

namespace ProductIdentification.Data.Repositories
{
    public class AzureFileRepository : IFileRepository
    {
        private readonly CloudBlobClient _blobClient;

        public AzureFileRepository(string connectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        public async Task<Stream> GetFileContentAsync(string folder, string filename)
        {
            var (containerName, blobName) = GetContainerAndBlobName(folder, filename);

            var cloudBlobContainer = await CreateContainer(containerName);

            var blob = cloudBlobContainer.GetBlockBlobReference(blobName);

            ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount * 8;
            ServicePointManager.Expect100Continue = false;
            TransferManager.Configurations.ParallelOperations = 64;

            var stream = new MemoryStream();

            await TransferManager.DownloadAsync(blob, stream);

            stream.Position = 0;

            return stream;
        }

        public async Task SaveFileAsync(string folder, string filename, Stream file)
        {
            var (containerName, blobName) = GetContainerAndBlobName(folder, filename);

            var cloudBlobContainer = await CreateContainer(containerName);

            file.Position = 0;

            var blob = cloudBlobContainer.GetBlockBlobReference(blobName);

            ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount * 8;
            ServicePointManager.Expect100Continue = false;
            TransferManager.Configurations.ParallelOperations = 64;

            await TransferManager.UploadAsync(file, blob);
        }

        public async Task CopyFile(string sourceFolderName,
                                   string targetFolderName,
                                   string sourceFileName,
                                   string destinationFileName)
        {
            var (sourceContainerName, sourceBlobName) = GetContainerAndBlobName(sourceFolderName, sourceFileName);
            var (targetContainerName, destinationBlobName) =
                GetContainerAndBlobName(targetFolderName, destinationFileName);

            CloudBlobContainer sourceContainer = await CreateContainer(sourceContainerName);
            CloudBlobContainer targetContainer = await CreateContainer(targetContainerName);

            var sourceBlob = sourceContainer.GetBlockBlobReference(sourceBlobName);
            var targetBlob = targetContainer.GetBlockBlobReference(destinationBlobName);

            await TransferManager.CopyAsync(sourceBlob, targetBlob, CopyMethod.ServiceSideAsyncCopy);
        }

        public async Task CopyFile(string sourceFolderName, string targetFolderName, string fileName)
        {
            await CopyFile(sourceFolderName, targetFolderName, fileName, fileName);
        }

        public async Task<IEnumerable<string>> FileNamesList(string folderName)
        {
            var splitted = folderName.Split('/');

            CloudBlobContainer cloudBlobContainer = await CreateContainer(splitted[0]);

            IEnumerable<IListBlobItem> results;

            if (splitted.Length > 1)
            {
                var folder = GetDeepestFolder(cloudBlobContainer, splitted);

                results = folder.ListBlobsSegmentedAsync(false, BlobListingDetails.Metadata, 500, null, null, null)
                                .Result.Results;
            }
            else
            {
                results = cloudBlobContainer.ListBlobsSegmentedAsync(null).Result.Results;
            }

            List<string> blobsList = new List<string>();

            foreach (IListBlobItem item in results)
            {
                var itemName = Path.GetFileName(item.Uri.ToString());
                blobsList.Add(itemName);
            }

            return blobsList;
        }

        public async Task<IEnumerable<PhotoFile>> FilesList(string folderName)
        {
            var splitted = folderName.Split('/');

            CloudBlobContainer cloudBlobContainer = await CreateContainer(splitted[0]);

            IEnumerable<CloudBlob> results;

            if (splitted.Length > 1)
            {
                var folder = GetDeepestFolder(cloudBlobContainer, splitted);

                results = folder.ListBlobsSegmentedAsync(false, BlobListingDetails.Metadata, 500, null, null, null)
                                .Result.Results
                                .Select(x =>
                                    folder.GetBlobReference(
                                        Path.GetFileName(x.Uri.ToString())
                                    ));
            }
            else
            {
                results = cloudBlobContainer.ListBlobsSegmentedAsync(null).Result.Results
                                            .Select(x =>
                                                cloudBlobContainer.GetBlobReference(
                                                    Path.GetFileName(x.Uri.ToString())
                                                ));
            }

            List<PhotoFile> blobsList = new List<PhotoFile>();

            foreach (var item in results)
            {
                var itemName = Path.GetFileName(item.Uri.ToString());
                blobsList.Add(new PhotoFile(itemName,GetBlobUriWithSasToken(item)));
            }

            return blobsList;
        }

        private static CloudBlobDirectory GetDeepestFolder(CloudBlobContainer cloudBlobContainer, string[] splitted)
        {
            var folder = cloudBlobContainer.GetDirectoryReference(splitted[1]);
            for (int i = 2; i < splitted.Length; i++)
            {
                folder = folder.GetDirectoryReference(splitted[i]);
            }

            return folder;
        }

        private static (string container, string blobName) GetContainerAndBlobName(string folderName, string file)
        {
            var splitted = folderName.Split('/');

            var containerName = splitted[0];

            var blobName = file;

            if (splitted.Length > 1)
            {
                var path = folderName.ReplaceFirst($"{splitted[0]}/", "");
                blobName = $"{path}/{file}";
            }

            return (containerName, blobName);
        }

        private async Task<CloudBlobContainer> CreateContainer(string containerName)
        {
            var cloudBlobContainer = _blobClient.GetContainerReference(containerName);
            await cloudBlobContainer.CreateIfNotExistsAsync();
            return cloudBlobContainer;
        }

        private string GetBlobUriWithSasToken(CloudBlob blob)
        {
            return $"{blob.Uri}{GetBlobSasToken(blob, SharedAccessBlobPermissions.Read)}";
        }

        private string GetBlobSasToken(CloudBlob blob,
                                       SharedAccessBlobPermissions permissions)
        {
            var adHocSas = CreateSharingSasPolicy(permissions);

            return blob.GetSharedAccessSignature(adHocSas);
        }

        private static SharedAccessBlobPolicy CreateSharingSasPolicy(SharedAccessBlobPermissions permissions)
        {
            return new SharedAccessBlobPolicy()
            {
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = DateTime.UtcNow.AddYears(2),
                Permissions = permissions
            };
        }
    }
}