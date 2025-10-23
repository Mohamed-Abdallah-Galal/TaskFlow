using Microsoft.EntityFrameworkCore;
using TaskFlow.DAL.DataBase;
using TaskFlow.DAL.Entites;
using TaskFlow.DAL.Enums.Attachment;
using TaskFlow.DAL.Enums.Task;
using TaskFlow.DAL.Repo.Abstraction;

namespace TaskFlow.DAL.Repo.Implementation
{
    public class AttachmentRepo : BaseRepo<Attachment>, IAttachmentRepo
    {
        public AttachmentRepo(TaskFlowDbContext context) : base(context) { }

        // Basic CRUD
        public async Task<Attachment> GetByIdAsync(int id)
            => await base.GetByIdAsync(id);

        public async Task<IEnumerable<Attachment>> GetAllAsync()
            => await base.GetAllAsync();

        public async Task AddAsync(Attachment attachment)
            => await base.AddAsync(attachment);

        public void Update(Attachment attachment)
            => base.Update(attachment);

        public void Delete(Attachment attachment)
            => base.SoftDelete(attachment, attachment.DeletedByUserId ?? 1); // Fallback user ID

        // Soft Delete
        public async Task SoftDeleteAsync(int attachmentId, int deletedByUserId)
            => await base.SoftDeleteAsync(attachmentId, deletedByUserId);

        // Specific queries
        public async Task<IEnumerable<Attachment>> GetAttachmentsByTaskAsync(int taskId)
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .Where(a => a.TaskId == taskId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Attachment>> GetAttachmentsByTypeAsync(AttachmentType fileType)
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .Where(a => a.FileType == fileType)
                .ToListAsync();

        public async Task<IEnumerable<Attachment>> GetLargeAttachmentsAsync(long minSize)
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .Where(a => a.FileSize >= minSize)
                .OrderByDescending(a => a.FileSize)
                .ToListAsync();

        public async Task<int> GetAttachmentCountByTaskAsync(int taskId)
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .CountAsync(a => a.TaskId == taskId);

        public async Task<long> GetTotalStorageUsedAsync()
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .SumAsync(a => a.FileSize);
        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();
        // ✅ ADD THIS METHOD IMPLEMENTATION
        


        // ✅ ADD BOTH METHOD IMPLEMENTATIONS
        public async Task<bool> FileNameExistsAsync(string fileName)
        {
            return await _dbSet
                .Where(IsNotDeletedExpression())
                .AnyAsync(a => a.FileName == fileName);
        }

        public async Task<bool> FileNameExistsAsync(string fileName, int excludeId)
        {
            return await _dbSet
                .Where(IsNotDeletedExpression())
                .AnyAsync(a => a.FileName == fileName && a.Id != excludeId);
        }

    }
}