

using TaskFlow.DAL.Entites;
using TaskFlow.DAL.Enums.Task;

namespace TaskFlow.DAL.Repo.Abstraction
{
    public interface ITaskHistoryRepo
    {
        // Basic CRUD
        Task<TaskHistory> GetByIdAsync(int id);
        Task<IEnumerable<TaskHistory>> GetAllAsync();
        Task AddAsync(TaskHistory history);
        void Update(TaskHistory history);
        void Delete(TaskHistory history);

        // Specific queries
        Task<IEnumerable<TaskHistory>> GetHistoryByTaskAsync(int taskId);
        Task<IEnumerable<TaskHistory>> GetHistoryByUserAsync(int userId);
        Task<IEnumerable<TaskHistory>> GetRecentHistoryAsync(int count = 20);
        Task<IEnumerable<TaskHistory>> GetStatusChangeHistoryAsync(int taskId);
        Task<IEnumerable<TaskHistory>> GetHistoryWithDetailsAsync(int taskId);
        Task<DateTime?> GetLastStatusChangeAsync(int taskId, AppTaskStatus status);

        // Soft Delete
        Task SoftDeleteAsync(int taskId, int deletedByUserId);

    }
}
