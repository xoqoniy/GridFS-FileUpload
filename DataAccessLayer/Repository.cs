
namespace FilesApi.DataAccessLayer
{
    public class Repository : IRepository
    {
            
        public Task DeleteFileAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownloadFileAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
