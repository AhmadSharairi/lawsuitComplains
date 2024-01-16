using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly ComplaintContext _complaintContext;
        public AttachmentRepository(ComplaintContext complaintContext)
        {
            _complaintContext = complaintContext;
        }
        public async Task<Attachment> AddAttachmentAsync(Attachment attachment)
        {
            await _complaintContext.AddAsync(attachment);
            await _complaintContext.SaveChangesAsync();
            return attachment;
        }

    }


}