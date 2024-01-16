namespace Core.Entities
{
    public class Attachment
    {
        public int AttachmentId { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public DateTime UploadDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}