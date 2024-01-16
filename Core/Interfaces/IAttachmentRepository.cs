

using Core.Entities;

namespace Core.Interfaces
{
    public interface IAttachmentRepository
    {
     Task<Attachment> AddAttachmentAsync(Attachment attachment);
     
    }
}