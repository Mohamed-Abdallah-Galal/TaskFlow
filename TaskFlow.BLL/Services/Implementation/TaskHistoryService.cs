using AutoMapper;
using TaskFlow.BLL.ModelVM.TaskHistory;
using TaskFlow.BLL.Services.Abstraction;
using TaskFlow.DAL.Entites;
using TaskFlow.DAL.Repo.Abstraction;

namespace TaskFlow.BLL.Services.Implementation
{
    public class TaskHistoryService : ITaskHistoryService
    {
        private readonly ITaskHistoryRepo _historyRepo;
        private readonly IMapper _mapper;

        public TaskHistoryService(ITaskHistoryRepo historyRepo, IMapper mapper)
        {
            _historyRepo = historyRepo;
            _mapper = mapper;
        }

        public async Task<List<TaskHistoryReadDto>> GetTaskHistoryAsync(int taskId)
        {
            var history = await _historyRepo.GetHistoryByTaskAsync(taskId);
            return _mapper.Map<List<TaskHistoryReadDto>>(history);
        }

        public async Task<List<TaskHistoryReadDto>> GetUserActivityAsync(int userId)
        {
            var history = await _historyRepo.GetHistoryByUserAsync(userId);
            return _mapper.Map<List<TaskHistoryReadDto>>(history);
        }

        public async Task<List<TaskHistoryReadDto>> GetRecentActivityAsync(int count = 20)
        {
            var history = await _historyRepo.GetRecentHistoryAsync(count);
            return _mapper.Map<List<TaskHistoryReadDto>>(history);
        }

        public async Task<List<TaskHistoryReadDto>> GetStatusChangeHistoryAsync(int taskId)
        {
            var history = await _historyRepo.GetStatusChangeHistoryAsync(taskId);
            return _mapper.Map<List<TaskHistoryReadDto>>(history);
        }

        public async Task<DateTime?> GetLastStatusChangeAsync(int taskId, TaskFlow.DAL.Enums.Task.AppTaskStatus status)
        {
            return await _historyRepo.GetLastStatusChangeAsync(taskId, status);
        }

        public async Task<int> GetActivityCountForTaskAsync(int taskId)
        {
            var history = await _historyRepo.GetHistoryByTaskAsync(taskId);
            return history.Count();
        }

        public async Task<int> GetActivityCountForUserAsync(int userId)
        {
            var history = await _historyRepo.GetHistoryByUserAsync(userId);
            return history.Count();
        }

        public async Task<string> GenerateTaskTimelineAsync(int taskId)
        {
            var history = await _historyRepo.GetHistoryWithDetailsAsync(taskId);
            if (!history.Any())
                return "No activity recorded for this task.";

            var timeline = new System.Text.StringBuilder();
            timeline.AppendLine("Task Timeline:");
            timeline.AppendLine("==============");

            foreach (var record in history.OrderBy(h => h.ChangedAt))
            {
                var user = record.ChangedByUser?.FullName ?? "Unknown User";
                var action = GetActionDescription(record);
                timeline.AppendLine($"{record.ChangedAt:yyyy-MM-dd HH:mm} - {user} {action}");
            }

            return timeline.ToString();
        }

        public async Task<TimeSpan> CalculateTimeInStatusAsync(int taskId, TaskFlow.DAL.Enums.Task.AppTaskStatus status)
        {
            var statusChanges = await _historyRepo.GetStatusChangeHistoryAsync(taskId);
            var relevantChanges = statusChanges.Where(h => h.NewStatusId == (int)status).ToList();

            if (!relevantChanges.Any())
                return TimeSpan.Zero;

            var totalTime = TimeSpan.Zero;
            foreach (var change in relevantChanges)
            {
                var nextChange = statusChanges
                    .Where(h => h.ChangedAt > change.ChangedAt)
                    .OrderBy(h => h.ChangedAt)
                    .FirstOrDefault();

                var endTime = nextChange?.ChangedAt ?? DateTime.Now;
                totalTime += endTime - change.ChangedAt;
            }

            return totalTime;
        }

        private string GetActionDescription(TaskHistory history)
        {
            return history.ActionType switch
            {
                TaskFlow.DAL.Enums.Task.HistoryActionType.StatusChange => $"changed status from {((TaskFlow.DAL.Enums.Task.AppTaskStatus)history.OldStatusId)} to {((TaskFlow.DAL.Enums.Task.AppTaskStatus)history.NewStatusId)}",
                TaskFlow.DAL.Enums.Task.HistoryActionType.AssignmentChange => "reassigned the task",
                TaskFlow.DAL.Enums.Task.HistoryActionType.PriorityChange => "changed priority",
                TaskFlow.DAL.Enums.Task.HistoryActionType.DescriptionUpdate => "updated description",
                TaskFlow.DAL.Enums.Task.HistoryActionType.DeadlineChange => "updated deadline",
                TaskFlow.DAL.Enums.Task.HistoryActionType.ProgressUpdate => "updated progress",
                _ => "performed an action"
            };
        }
    }
}