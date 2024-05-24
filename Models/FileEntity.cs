namespace FilesApi.Models
{
    public class FileEntity
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Description { get; set; }
        public string FileId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Stream Content { get; set; }
    }

}
