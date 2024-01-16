namespace Core.Entities
{
    public class User
    {
        public int UserId { get;  set; }
        public string Name { get;  set; }
        public int Number { get;  set; }
        public List<Complaint> Complaints { get; set; } = new List<Complaint>();
         public List<Attachment> Attachments { get; set; } = new List<Attachment>();
       
    }


}