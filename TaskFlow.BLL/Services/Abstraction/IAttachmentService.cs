using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.BLL.ModelVM.Attachment;

namespace TaskFlow.BLL.Services.Abstraction
{
    public interface IAttachmentService
    {
        Task<AttachmentReadDto> GetAttachmentByIdAsync(int id);
        Task<List<AttachmentReadDto>> GetAttachmentsByTaskAsync(int taskId);
        Task<List<AttachmentReadDto>> GetAttachmentsByTypeAsync(TaskFlow.DAL.Enums.Attachment.AttachmentType fileType);

        Task<int> AddAttachmentAsync(AttachmentCreateDto dto);
        Task UpdateAttachmentAsync(AttachmentUpdateDto dto);
        Task DeleteAttachmentAsync(int attachmentId, int deletedByUserId);

        Task<int> GetAttachmentCountForTaskAsync(int taskId);
        Task<long> GetTotalStorageUsedAsync();
        Task<bool> IsFileSizeWithinLimitAsync(long fileSize);
        Task<List<AttachmentReadDto>> GetLargeAttachmentsAsync(long minSize);

    }
}
