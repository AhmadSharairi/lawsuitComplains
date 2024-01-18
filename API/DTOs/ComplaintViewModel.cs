namespace API.DTOs
{
    public class ComplaintViewModel
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public string ComplaintText { get; set; }

        public List<LocalizedTextDto> ComplaintTexts { get; set; }

        public List<DemandDto> DemandDescriptions { get; set; }
    }
}