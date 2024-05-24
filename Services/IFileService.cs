using FilesApi.Models;
using System.IO;
using System.Threading.Tasks;

namespace FilesApi.Services
{
    public interface IFileService
    {
        Task<FileEntity> UploadFileAsync(Stream fileStream, string fileName, string contentType);
        Task<FileEntity> DownloadFileAsync(string id);
        Task DeleteFileAsync(string id);
    }
}
