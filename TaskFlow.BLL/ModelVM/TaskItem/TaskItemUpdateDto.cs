

namespace TaskFlow.BLL.ModelVM.TaskItem
{
    public class TaskItemUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskFlow.DAL.Enums.Task.TaskPriority Priority { get; set; }
        public TaskFlow.DAL.Enums.Task.TaskCategory Category { get; set; }
        public DateTime? Deadline { get; set; }
        public decimal? EstimatedHours { get; set; }
        public int UpdatedByUserId { get; set; }
    }
}
