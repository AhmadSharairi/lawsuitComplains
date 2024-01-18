using Core.Entities;

namespace Core.Interfaces
{
    public interface IComplaintRepository
    {
      Task<Complaint> CreateComplaintAsync(Complaint complaint);
       Task<bool> UpdateComplaintByIdAsync(int complaintId, Complaint updatedComplaintDto);
     Task<IReadOnlyList<Complaint>> GetComplaintsAsync();
         Task<bool> UpdateComplaintStatusAsync(int complaintId, string status);
        Task<Complaint> GetComplaintByIdAsync(int id);


       
   
       

     
    
    }
}