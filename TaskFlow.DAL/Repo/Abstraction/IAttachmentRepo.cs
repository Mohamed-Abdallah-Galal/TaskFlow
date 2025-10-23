

using TaskFlow.DAL.Entites;
using TaskFlow.DAL.Enums.Attachment;
using TaskFlow.DAL.Enums.Task;

namespace TaskFlow.DAL.Repo.Abstraction
{
    public interface IAttachmentRepo
    {
        // Basic CRUD
        Task<Attachment> GetByIdAsync(int id);
        Task<IEnumerable<Attachment>> GetAllAsync();
        Task AddAsync(Attachment attachment);
        void Update(Attachment attachment);
        void Delete(Attachment attachment);

        // Specific queries
        Task<IEnumerable<Attachment>> GetAttachmentsByTaskAsync(int taskId);
        Task<IEnumerable<Attachment>> GetAttachmentsByTypeAsync(AttachmentType fileType);
        Task<IEnumerable<Attachment>> GetLargeAttachmentsAsync(long minSize);
        Task<int> GetAttachmentCountByTaskAsync(int taskId);
        Task<long> GetTotalStorageUsedAsync();

        // Soft Delete
        Task SoftDeleteAsync(int taskId, int deletedByUserId);
        Task<int> SaveChangesAsync();

        // ✅ ADD BOTH METHODS
        Task<bool> FileNameExistsAsync(string fileName); // Parameterless version
        Task<bool> FileNameExistsAsync(string fileName, int excludeId); // Version with excludeId

    }
}
