
namespace TaskFlow.BLL.ModelVM.TaskItem
{
    public class TaskItemCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatedById { get; set; }
        public int AssignedToId { get; set; }
        public TaskFlow.DAL.Enums.Task.TaskPriority Priority { get; set; }
        public TaskFlow.DAL.Enums.Task.TaskCategory Category { get; set; }
        public DateTime? Deadline { get; set; }
        public decimal? EstimatedHours { get; set; }
    }
}
