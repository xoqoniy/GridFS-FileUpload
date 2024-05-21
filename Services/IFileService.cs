using System.IO;
using System.Threading.Tasks;

namespace FilesApi.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName);
        Task<Stream> DownloadFileAsync(string id);
        Task DeleteFileAsync(string id);
    }
}
