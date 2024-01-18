
namespace Core.Entities
{
    public class LocalizedText
    {
         public int LocalizedTextId { get; set; }
        public string LanguageCode { get; set; }
        public string Text { get; set; }
    
        public int ComplaintId { get; set; }
        public Complaint Complaint { get; set; }
        
        public int DemandId { get; set; }
        public Demand Demand { get; set; }
        
    }
}