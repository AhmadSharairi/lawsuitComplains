namespace API.DTOs
{
    public class ComplaintDto
    {
        public string Text { get; set; }
         public string MessageKey { get; set; } 
     
        public List<DemandDto> Demands { get; set; } 
    }
}