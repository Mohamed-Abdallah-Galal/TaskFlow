using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.BLL.ModelVM.TaskHistory;

namespace TaskFlow.BLL.Services.Abstraction
{
    public interface ITaskHistoryService
    {
        Task<List<TaskHistoryReadDto>> GetTaskHistoryAsync(int taskId);
        Task<List<TaskHistoryReadDto>> GetUserActivityAsync(int userId);
        Task<List<TaskHistoryReadDto>> GetRecentActivityAsync(int count = 20);
        Task<List<TaskHistoryReadDto>> GetStatusChangeHistoryAsync(int taskId);

        Task<DateTime?> GetLastStatusChangeAsync(int taskId, TaskFlow.DAL.Enums.Task.AppTaskStatus status);
        Task<int> GetActivityCountForTaskAsync(int taskId);
        Task<int> GetActivityCountForUserAsync(int userId);

        Task<string> GenerateTaskTimelineAsync(int taskId);
        Task<TimeSpan> CalculateTimeInStatusAsync(int taskId, TaskFlow.DAL.Enums.Task.AppTaskStatus status);

    }
}
