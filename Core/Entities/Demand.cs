
namespace Core.Entities
{
    public class Demand
    {
        public int DemandId { get; set; }
        public string Description { get; set; }
        public int ComplaintId { get; set; }
        public Complaint Complaint { get;  set; }
    }
}