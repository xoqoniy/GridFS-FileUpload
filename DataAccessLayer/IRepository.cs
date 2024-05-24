using FilesApi.Models;

namespace FilesApi.DataAccessLayer
{
    public interface IRepository
    {
        Task<FileEntity> UploadFileAsync(Stream fileStream, string fileName, string contentType);
        Task<FileEntity> DownloadFileAsync(string id);
        Task DeleteFileAsync(string id);
    }

}
