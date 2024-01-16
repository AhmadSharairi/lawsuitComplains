namespace API.DTOs
{
    public class ComplaintAdminDto 
    {
        public string Text { get; set; }
        public string Status { get; set; }
        public List<DemandDto> Demands { get; set; } 
    }
}