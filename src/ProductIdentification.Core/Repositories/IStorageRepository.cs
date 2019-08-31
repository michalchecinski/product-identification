using System.IO;
using System.Threading.Tasks;

namespace ProductIdentification.Core.Repositories
{
    public interface IStorageRepository
    {
        Task<Stream> GetFileContentAsync(string folder, string filename);
        Task SaveFileAsync(string folder, string filename, Stream file);
    }
}