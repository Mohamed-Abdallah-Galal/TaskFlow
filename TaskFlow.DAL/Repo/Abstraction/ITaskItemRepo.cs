

using TaskFlow.DAL.Entites;
using TaskFlow.DAL.Enums.Task;

namespace TaskFlow.DAL.Repo.Abstraction
{
    public interface ITaskItemRepo
    {
        Task<TaskItem> GetByIdAsync(int id);
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task AddAsync(TaskItem task);
        void Update(TaskItem task);
        void Delete(TaskItem task);

        // Specific queries
        Task<IEnumerable<TaskItem>> GetByStatusAsync(AppTaskStatus status);
        Task<IEnumerable<TaskItem>> GetByPriorityAsync(TaskPriority priority);
        Task<IEnumerable<TaskItem>> GetByCategoryAsync(TaskCategory category);
        Task<IEnumerable<TaskItem>> GetOverdueTasksAsync();
        Task<IEnumerable<TaskItem>> GetUrgentTasksAsync();
        Task<IEnumerable<TaskItem>> GetTasksByAssigneeAsync(int assigneeId);
        Task<IEnumerable<TaskItem>> GetTasksByCreatorAsync(int creatorId);
        Task<IEnumerable<TaskItem>> GetTasksWithDetailsAsync();
        Task<TaskItem> GetTaskWithFullDetailsAsync(int id);
        Task<IEnumerable<TaskItem>> SearchTasksAsync(string searchTerm);

        // Analytics
        Task<int> GetTaskCountByStatusAsync(AppTaskStatus status);
        Task<Dictionary<AppTaskStatus, int>> GetTaskStatusDistributionAsync();
        Task<int> GetTotalTaskCountAsync();

        // Soft Delete
        Task SoftDeleteAsync(int taskId, int deletedByUserId);
        Task<int> SaveChangesAsync();

       

    }
}
