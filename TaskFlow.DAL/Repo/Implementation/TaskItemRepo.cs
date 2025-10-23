using Microsoft.EntityFrameworkCore;
using TaskFlow.DAL.DataBase;
using TaskFlow.DAL.Entites;
using TaskFlow.DAL.Enums.Task;
using TaskFlow.DAL.Repo.Abstraction;

namespace TaskFlow.DAL.Repo.Implementation
{
    public class TaskItemRepo : BaseRepo<TaskItem>, ITaskItemRepo
    {
        public TaskItemRepo(TaskFlowDbContext context) : base(context) { }

        // Basic CRUD
        public async Task<TaskItem> GetByIdAsync(int id)
            => await base.GetByIdAsync(id);

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
            => await base.GetAllAsync();

        public async Task AddAsync(TaskItem task)
            => await base.AddAsync(task);

        public void Update(TaskItem task)
            => base.Update(task);

        public void Delete(TaskItem task)
            => base.SoftDelete(task, task.DeletedByUserId ?? 1); // Fallback user ID

        // Soft Delete
        public async Task SoftDeleteAsync(int taskId, int deletedByUserId)
            => await base.SoftDeleteAsync(taskId, deletedByUserId);

        // Specific queries
        public async Task<IEnumerable<TaskItem>> GetByStatusAsync(AppTaskStatus status)
            => await base.FindAsync(t => t.Status == status);

        public async Task<IEnumerable<TaskItem>> GetByPriorityAsync(TaskPriority priority)
            => await base.FindAsync(t => t.Priority == priority);

        public async Task<IEnumerable<TaskItem>> GetByCategoryAsync(TaskCategory category)
            => await base.FindAsync(t => t.Category == category);

        public async Task<IEnumerable<TaskItem>> GetOverdueTasksAsync()
            => await base.FindAsync(t =>
                t.Deadline.HasValue &&
                t.Deadline < DateTime.Now &&
                t.Status != AppTaskStatus.Completed &&
                t.Status != AppTaskStatus.Cancelled);

        public async Task<IEnumerable<TaskItem>> GetUrgentTasksAsync()
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .Where(t => t.Priority >= TaskPriority.High && t.Status != AppTaskStatus.Completed)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.Deadline)
                .ToListAsync();

        public async Task<IEnumerable<TaskItem>> GetTasksByAssigneeAsync(int assigneeId)
            => await base.FindAsync(t => t.AssignedToId == assigneeId);

        public async Task<IEnumerable<TaskItem>> GetTasksByCreatorAsync(int creatorId)
            => await base.FindAsync(t => t.CreatedById == creatorId);

        public async Task<IEnumerable<TaskItem>> GetTasksWithDetailsAsync()
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Include(t => t.Comments.Where(c => !c.IsDeleted))
                .Include(t => t.Attachments.Where(a => !a.IsDeleted))
                .Include(t => t.History.Where(h => !h.IsDeleted))
                .ToListAsync();

        public async Task<TaskItem> GetTaskWithFullDetailsAsync(int id)
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Include(t => t.Comments.Where(c => !c.IsDeleted))
                .Include(t => t.Attachments.Where(a => !a.IsDeleted))
                .Include(t => t.History.Where(h => !h.IsDeleted))
                .FirstOrDefaultAsync(t => t.Id == id);

        public async Task<IEnumerable<TaskItem>> SearchTasksAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            return await base.FindAsync(t =>
                t.Title.Contains(searchTerm) ||
                t.Description.Contains(searchTerm));
        }

        // Analytics
        public async Task<int> GetTaskCountByStatusAsync(AppTaskStatus status)
            => await base.CountAsync(t => t.Status == status);

        public async Task<Dictionary<AppTaskStatus, int>> GetTaskStatusDistributionAsync()
        {
            return await _dbSet
                .Where(IsNotDeletedExpression())
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);
        }

        public async Task<int> GetTotalTaskCountAsync()
            => await base.CountAsync();
        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        
    }
}