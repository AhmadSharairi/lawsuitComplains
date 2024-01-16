namespace API.DTOs
{
    public class ComplaintViewModel
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public string ComplaintText { get; set; }
    
        public List<string> DemandDescriptions { get; set; }
    }
}