using AutoMapper;
using TaskFlow.BLL.ModelVM.TaskItem;
using TaskFlow.BLL.Services.Abstraction;
using TaskFlow.DAL.Entites;
using TaskFlow.DAL.Repo.Abstraction;
using TaskFlow.DAL.Repo.Implementation;


namespace TaskFlow.BLL.Services.Implementation
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepo _taskRepo;
        private readonly IAppUserRepo _userRepo;
        private readonly IMapper _mapper;

        public TaskItemService(ITaskItemRepo taskRepo, IAppUserRepo userRepo, IMapper mapper)
        {
            _taskRepo = taskRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<TaskItemReadDto> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepo.GetByIdAsync(id);
            return _mapper.Map<TaskItemReadDto>(task);
        }

        public async Task<TaskItemReadDto> GetTaskWithFullDetailsAsync(int id)
        {
            var task = await _taskRepo.GetTaskWithFullDetailsAsync(id);
            return _mapper.Map<TaskItemReadDto>(task);
        }

        public async Task<List<TaskItemReadDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepo.GetAllAsync();
            return _mapper.Map<List<TaskItemReadDto>>(tasks);
        }

        public async Task<List<TaskItemReadDto>> GetTasksWithDetailsAsync()
        {
            var tasks = await _taskRepo.GetTasksWithDetailsAsync();
            return _mapper.Map<List<TaskItemReadDto>>(tasks);
        }

        public async Task<List<TaskItemReadDto>> GetTasksByAssigneeAsync(int assigneeId)
        {
            var tasks = await _taskRepo.GetTasksByAssigneeAsync(assigneeId);
            return _mapper.Map<List<TaskItemReadDto>>(tasks);
        }

        public async Task<List<TaskItemReadDto>> GetTasksByCreatorAsync(int creatorId)
        {
            var tasks = await _taskRepo.GetTasksByCreatorAsync(creatorId);
            return _mapper.Map<List<TaskItemReadDto>>(tasks);
        }
        

        public async Task<int> CreateTaskAsync(TaskItemCreateDto dto)
        {
            // Validate assignee exists and is active
            var assignee = await _userRepo.GetByIdAsync(dto.AssignedToId);
            if (assignee == null || !assignee.IsActive)
                throw new ArgumentException("Invalid or inactive assignee.");

            // Validate creator exists
            var creator = await _userRepo.GetByIdAsync(dto.CreatedById);
            if (creator == null)
                throw new ArgumentException("Invalid creator.");

            // Validate deadline
            if (dto.Deadline.HasValue && dto.Deadline < DateTime.Now)
                throw new ArgumentException("Deadline cannot be in the past.");

            var task = _mapper.Map<TaskItem>(dto);
            await _taskRepo.AddAsync(task);
            await _taskRepo.SaveChangesAsync();

            return task.Id;
        }

        public async Task UpdateTaskDetailsAsync(TaskItemUpdateDto dto)
        {
            var task = await _taskRepo.GetByIdAsync(dto.Id);
            if (task == null)
                throw new ArgumentException($"Task with ID {dto.Id} not found.");

            // Business rule: Cannot update completed or cancelled tasks
            if (task.Status == TaskFlow.DAL.Enums.Task.AppTaskStatus.Completed ||
                task.Status == TaskFlow.DAL.Enums.Task.AppTaskStatus.Cancelled)
                throw new InvalidOperationException("Cannot update completed or cancelled tasks.");

            // Validate deadline
            if (dto.Deadline.HasValue && dto.Deadline < DateTime.Now)
                throw new ArgumentException("Deadline cannot be in the past.");

            _mapper.Map(dto, task);
            _taskRepo.Update(task);
            await _taskRepo.SaveChangesAsync();
        }

        public async Task UpdateTaskProgressAsync(TaskItemProgressUpdateDto dto)
        {
            var task = await _taskRepo.GetByIdAsync(dto.Id);
            if (task == null)
                throw new ArgumentException($"Task with ID {dto.Id} not found.");

            _mapper.Map(dto, task);
            _taskRepo.Update(task);
            await _taskRepo.SaveChangesAsync();
        }

        public async Task UpdateTaskStatusAsync(TaskItemStatusUpdateDto dto)
        {
            var task = await _taskRepo.GetByIdAsync(dto.Id);
            if (task == null)
                throw new ArgumentException($"Task with ID {dto.Id} not found.");

            // Validate status transition
            if (!IsValidStatusTransition(task.Status, dto.NewStatus))
                throw new InvalidOperationException($"Invalid status transition from {task.Status} to {dto.NewStatus}.");

            _mapper.Map(dto, task);
            _taskRepo.Update(task);
            await _taskRepo.SaveChangesAsync();
        }

        public async Task ReassignTaskAsync(int taskId, int newAssigneeId, int updatedByUserId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null)
                throw new ArgumentException($"Task with ID {taskId} not found.");

            // Validate new assignee
            var newAssignee = await _userRepo.GetByIdAsync(newAssigneeId);
            if (newAssignee == null || !newAssignee.IsActive)
                throw new ArgumentException("Invalid or inactive assignee.");

            task.Reassign(newAssigneeId, updatedByUserId);
            _taskRepo.Update(task);
            await _taskRepo.SaveChangesAsync();
        }

        public async Task LogTaskTimeAsync(int taskId, decimal hoursWorked, int updatedByUserId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null)
                throw new ArgumentException($"Task with ID {taskId} not found.");

            // Business rule: Cannot log time for completed or cancelled tasks
            if (task.Status == TaskFlow.DAL.Enums.Task.AppTaskStatus.Completed ||
                task.Status == TaskFlow.DAL.Enums.Task.AppTaskStatus.Cancelled)
                throw new InvalidOperationException("Cannot log time for completed or cancelled tasks.");

            task.LogTime(hoursWorked, updatedByUserId);
            _taskRepo.Update(task);
            await _taskRepo.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int taskId, int deletedByUserId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null)
                throw new ArgumentException($"Task with ID {taskId} not found.");

            await _taskRepo.SoftDeleteAsync(taskId, deletedByUserId);
            await _taskRepo.SaveChangesAsync();
        }

        public async Task<List<TaskItemReadDto>> GetTasksByStatusAsync(TaskFlow.DAL.Enums.Task.AppTaskStatus status)
        {
            var tasks = await _taskRepo.GetByStatusAsync(status);
            return _mapper.Map<List<TaskItemReadDto>>(tasks);
        }

        public async Task<List<TaskItemReadDto>> GetTasksByPriorityAsync(TaskFlow.DAL.Enums.Task.TaskPriority priority)
        {
            var tasks = await _taskRepo.GetByPriorityAsync(priority);
            return _mapper.Map<List<TaskItemReadDto>>(tasks);
        }

        public async Task<List<TaskItemReadDto>> GetTasksByCategoryAsync(TaskFlow.DAL.Enums.Task.TaskCategory category)
        {
            var tasks = await _taskRepo.GetByCategoryAsync(category);
            return _mapper.Map<List<TaskItemReadDto>>(tasks);
        }

        public async Task<List<TaskItemReadDto>> GetOverdueTasksAsync()
        {
            var tasks = await _taskRepo.GetOverdueTasksAsync();
            return _mapper.Map<List<TaskItemReadDto>>(tasks);
        }

        public async Task<List<TaskItemReadDto>> GetUrgentTasksAsync()
        {
            var tasks = await _taskRepo.GetUrgentTasksAsync();
            return _mapper.Map<List<TaskItemReadDto>>(tasks);
        }

        public async Task<List<TaskItemReadDto>> SearchTasksAsync(string searchTerm)
        {
            var tasks = await _taskRepo.SearchTasksAsync(searchTerm);
            return _mapper.Map<List<TaskItemReadDto>>(tasks);
        }

        public async Task<bool> CanAssignTaskToUserAsync(int taskId, int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            var task = await _taskRepo.GetByIdAsync(taskId);
            return user != null && user.IsActive && task?.AssignedToId != userId;
        }

        public async Task<bool> IsTaskOverdueAsync(int taskId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            return task?.IsOverdue ?? false;
        }

        public async Task<decimal> CalculateTaskCompletionEfficiencyAsync(int taskId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null || !task.EstimatedHours.HasValue || task.ActualHours == 0)
                return 0;

            return (task.EstimatedHours.Value / task.ActualHours.Value) * 100;
        }

        public async Task<Dictionary<TaskFlow.DAL.Enums.Task.AppTaskStatus, int>> GetTaskStatusDistributionAsync()
        {
            return await _taskRepo.GetTaskStatusDistributionAsync();
        }

        public async Task<Dictionary<TaskFlow.DAL.Enums.Task.TaskPriority, int>> GetTaskPriorityDistributionAsync()
        {
            var tasks = await _taskRepo.GetAllAsync();
            return tasks
                .GroupBy(t => t.Priority)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<TaskFlow.DAL.Enums.Task.TaskCategory, int>> GetTaskCategoryDistributionAsync()
        {
            var tasks = await _taskRepo.GetAllAsync();
            return tasks
                .GroupBy(t => t.Category)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<int> GetCompletedTasksCountAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var tasks = await _taskRepo.GetByStatusAsync(TaskFlow.DAL.Enums.Task.AppTaskStatus.Completed);

            if (startDate.HasValue)
                tasks = tasks.Where(t => t.UpdatedAt >= startDate.Value);

            if (endDate.HasValue)
                tasks = tasks.Where(t => t.UpdatedAt <= endDate.Value);

            return tasks.Count();
        }

        private bool IsValidStatusTransition(TaskFlow.DAL.Enums.Task.AppTaskStatus currentStatus, TaskFlow.DAL.Enums.Task.AppTaskStatus newStatus)
        {
            var allowedTransitions = new Dictionary<TaskFlow.DAL.Enums.Task.AppTaskStatus, TaskFlow.DAL.Enums.Task.AppTaskStatus[]>
            {
                [TaskFlow.DAL.Enums.Task.AppTaskStatus.Pending] = new[] { TaskFlow.DAL.Enums.Task.AppTaskStatus.InProgress, TaskFlow.DAL.Enums.Task.AppTaskStatus.Cancelled },
                [TaskFlow.DAL.Enums.Task.AppTaskStatus.InProgress] = new[] { TaskFlow.DAL.Enums.Task.AppTaskStatus.InReview, TaskFlow.DAL.Enums.Task.AppTaskStatus.Blocked, TaskFlow.DAL.Enums.Task.AppTaskStatus.Cancelled },
                [TaskFlow.DAL.Enums.Task.AppTaskStatus.InReview] = new[] { TaskFlow.DAL.Enums.Task.AppTaskStatus.Completed, TaskFlow.DAL.Enums.Task.AppTaskStatus.Reopened },
                [TaskFlow.DAL.Enums.Task.AppTaskStatus.Blocked] = new[] { TaskFlow.DAL.Enums.Task.AppTaskStatus.InProgress, TaskFlow.DAL.Enums.Task.AppTaskStatus.Cancelled },
                [TaskFlow.DAL.Enums.Task.AppTaskStatus.Completed] = new[] { TaskFlow.DAL.Enums.Task.AppTaskStatus.Reopened },
                [TaskFlow.DAL.Enums.Task.AppTaskStatus.Cancelled] = new[] { TaskFlow.DAL.Enums.Task.AppTaskStatus.Pending },
                [TaskFlow.DAL.Enums.Task.AppTaskStatus.Reopened] = new[] { TaskFlow.DAL.Enums.Task.AppTaskStatus.InProgress }
            };

            return allowedTransitions.ContainsKey(currentStatus) &&
                   allowedTransitions[currentStatus].Contains(newStatus);
        }
    }
}