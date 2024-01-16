
namespace API.DTOs
{
    public class UserDto
    {
      public int UserId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public List<ComplaintDto> Complaints { get; set; } 
    }
}