using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProductIdentification.Core.Repositories
{
    public interface IFileRepository
    {
        Task<Stream> GetFileContentAsync(string folder, string filename);
        Task SaveFileAsync(string folder, string filename, Stream file);
        Task CopyFile(string sourceFolderName, string targetFolderName, string sourceFileName, string destinationFileName);
        Task CopyFile(string sourceFolderName, string targetFolderName, string fileName);
        Task<IEnumerable<string>> FileNamesList(string folderName);

    }
}