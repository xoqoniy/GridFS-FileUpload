using FilesApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.GridFS;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FilesApi.DataAccessLayer
{
    public class Repository : IRepository
    {
        private readonly IGridFSBucket _gridFSBucket;


        public Repository(IOptions<MongoDbSettings> mongodbSetting, string bucketName)
        {
            MongoClient client = new(mongodbSetting.Value.ConnectionString);
            var database = client.GetDatabase(mongodbSetting.Value.DatabaseName);
            _gridFSBucket = new GridFSBucket(database, new GridFSBucketOptions
            {
                BucketName = bucketName
            });
        }

        public async Task<FileEntity> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var options = new GridFSUploadOptions
            {
                Metadata = new BsonDocument
            {
                { "filename", fileName },
                { "contentType", contentType }
            }
            };
            var fileId = await _gridFSBucket.UploadFromStreamAsync(fileName, fileStream, options);

            return new FileEntity
            {
                FileId = fileId.ToString(),
                Name = fileName,
                ContentType = contentType,
                CreatedAt = DateTime.UtcNow
            };
        }

        public async Task<FileEntity> DownloadFileAsync(string id)
        {
            var objectId = new ObjectId(id);
            var fileInfo = await _gridFSBucket.Find(new BsonDocument("_id", objectId)).FirstOrDefaultAsync();
            if (fileInfo == null)
            {
                throw new Exception(); // or throw an exception
            }

            var stream = new MemoryStream();
            await _gridFSBucket.DownloadToStreamAsync(objectId, stream);
            stream.Seek(0, SeekOrigin.Begin);

            return new FileEntity
            {
                FileId = id,
                Name = fileInfo.Filename,
                ContentType = fileInfo.Metadata.GetValue("contentType", "application/octet-stream").AsString,
                CreatedAt = fileInfo.UploadDateTime,
                Content = stream
            };
        }

        public async Task DeleteFileAsync(string id)
        {
            var objectId = new ObjectId(id);
            await _gridFSBucket.DeleteAsync(objectId);
        }
    }
}