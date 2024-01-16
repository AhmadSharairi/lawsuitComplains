
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public class ComplaintRepository : IComplaintRepository
    {

        private readonly ComplaintContext _complaintContext;
        public ComplaintRepository(ComplaintContext complaintContext)
        {
            _complaintContext = complaintContext;

        }


        public async Task<Complaint> CreateComplaintAsync(Complaint complaint)
        {
            _complaintContext.Complaints.Add(complaint);
            await _complaintContext.SaveChangesAsync();
            return complaint;
        }


        public async Task<bool> UpdateComplaintByIdAsync(int complaintId, Complaint updatedComplaint)
        {
            try
            {
                var currentComplaint = await _complaintContext.Complaints
                    .Include(c => c.Demands)
                    .FirstOrDefaultAsync(c => c.ComplaintId == complaintId);

                if (currentComplaint == null)
                {
                    return false;
                }

                currentComplaint.Text = updatedComplaint.Text;
                currentComplaint.SubmissionDate = DateTime.Now;



                if (updatedComplaint.Demands != null)
                {
                    currentComplaint.Demands.Clear();
                    currentComplaint.Demands.AddRange(updatedComplaint.Demands);
                }


                await _complaintContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        public async Task<IReadOnlyList<Complaint>> GetComplaintAsync()
        {
            return await _complaintContext.Complaints.ToListAsync();
        }


        public async Task<bool> UpdateComplaintStatusAsync(int complaintId, string status)
        {
            try
            {
                var currentComplaint = await _complaintContext.Complaints
                    .FirstOrDefaultAsync(c => c.ComplaintId == complaintId);

                if (currentComplaint == null)
                {
                    return false;
                }

                currentComplaint.Status = status;

                await _complaintContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<Complaint> GetComplaintByIdAsync(int id)
        {
           return _complaintContext.Complaints.FirstOrDefaultAsync(x =>x.ComplaintId ==id);
        }
    }
}