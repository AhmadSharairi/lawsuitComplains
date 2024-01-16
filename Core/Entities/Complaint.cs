

using Core.Entities;

public class Complaint
{
    public int ComplaintId { get; set; }
     public string Text { get; set; }
     public string Status { get; set; }
    public DateTime SubmissionDate { get; set; }

     public int UserId { get; set; } 
     public User User { get; set; }
     public List<Demand> Demands { get; set; } = new List<Demand>();

}
