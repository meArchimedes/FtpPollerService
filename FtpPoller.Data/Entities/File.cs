
namespace FtpPoller.Data.Entities
{
    public class File
    {
        public Guid Id { get; set; }
        public Guid PayerId { get; set; }
        public string FileName { get; set; }
        public string InsertedAt { get; set; }
    }
}
