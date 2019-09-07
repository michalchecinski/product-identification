using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using ProductIdentification.Core.Repositories;
using Microsoft.Azure.Storage.DataMovement;

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
            var cloudBlobContainer = await CreateContainer(folder);

            var blob = cloudBlobContainer.GetBlockBlobReference(filename);

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
            var cloudBlobContainer = await CreateContainer(folder);

            file.Position = 0;

            var blob = cloudBlobContainer.GetBlockBlobReference(filename);

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
            CloudBlobContainer sourceContainer = await CreateContainer(sourceFolderName);
            CloudBlobContainer targetContainer = await CreateContainer(targetFolderName);

            var sourceBlob = sourceContainer.GetBlockBlobReference(sourceFileName);
            var targetBlob = targetContainer.GetBlockBlobReference(destinationFileName);

            await TransferManager.CopyAsync(sourceBlob, targetBlob, CopyMethod.ServiceSideAsyncCopy);
        }

        public async Task CopyFile(string sourceFolderName, string targetFolderName, string fileName)
        {
            await CopyFile(sourceFolderName, targetFolderName, fileName, fileName);
        }

        
        private async Task<CloudBlobContainer> CreateContainer(string containerName)
        {
            var cloudBlobContainer = _blobClient.GetContainerReference(containerName);
            await cloudBlobContainer.CreateIfNotExistsAsync();
            return cloudBlobContainer;
        }
    }
}