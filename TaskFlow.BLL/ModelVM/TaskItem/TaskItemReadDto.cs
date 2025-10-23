
using TaskFlow.BLL.ModelVM.AppUser;
using TaskFlow.BLL.ModelVM.Attachment;
using TaskFlow.BLL.ModelVM.Comment;

namespace TaskFlow.BLL.ModelVM.TaskItem
{
    public class TaskItemReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskFlow.DAL.Enums.Task.TaskPriority Priority { get; set; }
        public TaskFlow.DAL.Enums.Task.AppTaskStatus Status { get; set; }
        public TaskFlow.DAL.Enums.Task.TaskCategory Category { get; set; }
        public DateTime? Deadline { get; set; }
        public int ProgressPercent { get; set; }
        public decimal? EstimatedHours { get; set; }
        public decimal? ActualHours { get; set; }
        public int AssignedToId { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsOverdue { get; set; }
        public bool IsUrgent { get; set; }
        public string DaysRemaining { get; set; }

        // Navigation properties as DTOs
        public AppUserReadDto AssignedTo { get; set; }
        public AppUserReadDto CreatedBy { get; set; }
        public List<CommentReadDto> Comments { get; set; }
        public List<AttachmentReadDto> Attachments { get; set; }
    }
}
