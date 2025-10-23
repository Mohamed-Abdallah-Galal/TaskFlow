using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.BLL.ModelVM.TaskItem;

namespace TaskFlow.BLL.Services.Abstraction
{
    public interface ITaskItemService
    {
        Task<TaskItemReadDto> GetTaskByIdAsync(int id);
        Task<TaskItemReadDto> GetTaskWithFullDetailsAsync(int id);
        Task<List<TaskItemReadDto>> GetAllTasksAsync();
        Task<List<TaskItemReadDto>> GetTasksWithDetailsAsync();
        Task<List<TaskItemReadDto>> GetTasksByAssigneeAsync(int assigneeId);
        Task<List<TaskItemReadDto>> GetTasksByCreatorAsync(int creatorId);

        Task<int> CreateTaskAsync(TaskItemCreateDto dto);
        Task UpdateTaskDetailsAsync(TaskItemUpdateDto dto);
        Task UpdateTaskProgressAsync(TaskItemProgressUpdateDto dto);
        Task UpdateTaskStatusAsync(TaskItemStatusUpdateDto dto);
        Task ReassignTaskAsync(int taskId, int newAssigneeId, int updatedByUserId);
        Task LogTaskTimeAsync(int taskId, decimal hoursWorked, int updatedByUserId);
        Task DeleteTaskAsync(int taskId, int deletedByUserId);

        Task<List<TaskItemReadDto>> GetTasksByStatusAsync(TaskFlow.DAL.Enums.Task.AppTaskStatus status);
        Task<List<TaskItemReadDto>> GetTasksByPriorityAsync(TaskFlow.DAL.Enums.Task.TaskPriority priority);
        Task<List<TaskItemReadDto>> GetTasksByCategoryAsync(TaskFlow.DAL.Enums.Task.TaskCategory category);
        Task<List<TaskItemReadDto>> GetOverdueTasksAsync();
        Task<List<TaskItemReadDto>> GetUrgentTasksAsync();
        Task<List<TaskItemReadDto>> SearchTasksAsync(string searchTerm);

        Task<bool> CanAssignTaskToUserAsync(int taskId, int userId);
        Task<bool> IsTaskOverdueAsync(int taskId);
        Task<decimal> CalculateTaskCompletionEfficiencyAsync(int taskId);

        Task<Dictionary<TaskFlow.DAL.Enums.Task.AppTaskStatus, int>> GetTaskStatusDistributionAsync();
        Task<Dictionary<TaskFlow.DAL.Enums.Task.TaskPriority, int>> GetTaskPriorityDistributionAsync();
        Task<Dictionary<TaskFlow.DAL.Enums.Task.TaskCategory, int>> GetTaskCategoryDistributionAsync();
        Task<int> GetCompletedTasksCountAsync(DateTime? startDate = null, DateTime? endDate = null);

    }
}
