namespace FilesApi.DataAccessLayer
{
    public interface IRepository
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName);
        Task<Stream> DownloadFileAsync(string id);
        Task DeleteFileAsync(string id);
    }
}
