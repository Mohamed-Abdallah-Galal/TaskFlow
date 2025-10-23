using Microsoft.EntityFrameworkCore;
using TaskFlow.DAL.DataBase;
using TaskFlow.DAL.Entites;
using TaskFlow.DAL.Enums.Task;
using TaskFlow.DAL.Repo.Abstraction;

namespace TaskFlow.DAL.Repo.Implementation
{
    public class TaskHistoryRepo : BaseRepo<TaskHistory>, ITaskHistoryRepo
    {
        public TaskHistoryRepo(TaskFlowDbContext context) : base(context) { }

        // Basic CRUD
        public async Task<TaskHistory> GetByIdAsync(int id)
            => await base.GetByIdAsync(id);

        public async Task<IEnumerable<TaskHistory>> GetAllAsync()
            => await base.GetAllAsync();

        public async Task AddAsync(TaskHistory history)
            => await base.AddAsync(history);

        public void Update(TaskHistory history)
            => base.Update(history);

        public void Delete(TaskHistory history)
            => base.SoftDelete(history, history.DeletedByUserId ?? 1); // Fallback user ID

        // Soft Delete
        public async Task SoftDeleteAsync(int historyId, int deletedByUserId)
            => await base.SoftDeleteAsync(historyId, deletedByUserId);

        // Specific queries
        public async Task<IEnumerable<TaskHistory>> GetHistoryByTaskAsync(int taskId)
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .Where(h => h.TaskId == taskId)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();

        public async Task<IEnumerable<TaskHistory>> GetHistoryByUserAsync(int userId)
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .Where(h => h.ChangedByUserId == userId)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();

        public async Task<IEnumerable<TaskHistory>> GetRecentHistoryAsync(int count = 20)
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .OrderByDescending(h => h.ChangedAt)
                .Take(count)
                .ToListAsync();

        public async Task<IEnumerable<TaskHistory>> GetStatusChangeHistoryAsync(int taskId)
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .Where(h => h.TaskId == taskId && h.ActionType == HistoryActionType.StatusChange)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();

        public async Task<IEnumerable<TaskHistory>> GetHistoryWithDetailsAsync(int taskId)
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .Where(h => h.TaskId == taskId)
                .Include(h => h.ChangedByUser)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();

        public async Task<DateTime?> GetLastStatusChangeAsync(int taskId, AppTaskStatus status)
        {
            var history = await _dbSet
                .Where(IsNotDeletedExpression())
                .Where(h => h.TaskId == taskId &&
                           h.NewStatusId == (int)status &&
                           h.ActionType == HistoryActionType.StatusChange)
                .OrderByDescending(h => h.ChangedAt)
                .FirstOrDefaultAsync();

            return history?.ChangedAt;
        }
        
    }
}