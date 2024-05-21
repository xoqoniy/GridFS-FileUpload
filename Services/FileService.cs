using FilesApi.DataAccessLayer;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FilesApi.Services
{
    public class FileService : IFileService
    {
        private readonly IRepository _repository;

        public FileService(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            if (fileStream == null)
                throw new ArgumentNullException(nameof(fileStream), "File stream cannot be null.");

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

            try
            {
                return await _repository.UploadFileAsync(fileStream, fileName);
            }
            catch (Exception ex)
            {
                // Log exception (e.g., using a logging framework)
                throw new ApplicationException("An error occurred while uploading the file.", ex);
            }
        }

        public async Task<Stream> DownloadFileAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("File ID cannot be null or empty.", nameof(id));

            try
            {
                return await _repository.DownloadFileAsync(id);
            }
            catch (Exception ex)
            {
                // Log exception (e.g., using a logging framework)
                throw new ApplicationException("An error occurred while downloading the file.", ex);
            }
        }

        public async Task DeleteFileAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("File ID cannot be null or empty.", nameof(id));

            try
            {
                await _repository.DeleteFileAsync(id);
            }
            catch (Exception ex)
            {
                // Log exception (e.g., using a logging framework)
                throw new ApplicationException("An error occurred while deleting the file.", ex);
            }
        }
    }
}
