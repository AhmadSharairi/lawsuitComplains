using Core.Entities;

namespace Core.Interfaces
{
    public interface IComplaintRepository
    {

        Task<Complaint> GetComplaintByIdAsync(int id);
        // Task<IReadOnlyList<Complaint>> GetComplaintAsync();

        Task<IReadOnlyList<Complaint>> GetComplaintAsync();
        Task<Complaint> CreateComplaintAsync(Complaint complaint);
        Task<bool> UpdateComplaintByIdAsync(int complaintId, Complaint updatedComplaintDto);

         Task<bool> UpdateComplaintStatusAsync(int complaintId, string status);
    
    }
}