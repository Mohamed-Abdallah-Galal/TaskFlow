using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskFlow.DAL.Enums.Task;

namespace TaskFlow.DAL.Entites
{
    public class TaskItem
    {
        public int Id { get; private set; }

        [Required]
        [StringLength(200)]
        public string Title { get; private set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; private set; }

        public TaskPriority Priority { get; private set; }
        public AppTaskStatus Status { get; private set; } = AppTaskStatus.Pending;
        public TaskCategory Category { get; private set; }

        [DataType(DataType.Date)]
        public DateTime? Deadline { get; private set; }

        [Range(0, 100)]
        public int ProgressPercent { get; private set; } = 0;

        public decimal? EstimatedHours { get; private set; }
        public decimal? ActualHours { get; private set; }

        // Foreign Keys
        public int AssignedToId { get; private set; }
        public int CreatedById { get; private set; }

        // Navigation Properties
        public AppUser CreatedBy { get; private set; }
        public AppUser AssignedTo { get; private set; }
        public ICollection<Comment> Comments { get; private set; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; private set; } = new List<Attachment>();
        public ICollection<TaskHistory> History { get; private set; } = new List<TaskHistory>();

        // Audit fields
        public bool IsDeleted { get; private set; } = false;
        public int? UpdatedByUserId { get; private set; }
        public int? DeletedByUserId { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        // Constructor
        public TaskItem(string title, string description, int createdById, int assignedToId,
                       TaskPriority priority = TaskPriority.Medium, TaskCategory category = TaskCategory.Development,
                       DateTime? deadline = null, decimal? estimatedHours = null)
        {
            Title = title;
            Description = description;
            CreatedById = createdById;
            AssignedToId = assignedToId;
            Priority = priority;
            Category = category;
            Deadline = deadline;
            EstimatedHours = estimatedHours;
            Status = AppTaskStatus.Pending;
            CreatedAt = DateTime.Now;
        }

        protected TaskItem() { }

        // Helper properties
        [NotMapped]
        public bool IsOverdue => Deadline.HasValue && Deadline < DateTime.Now &&
                                Status != AppTaskStatus.Completed && Status != AppTaskStatus.Cancelled;

        [NotMapped]
        public bool IsUrgent => Priority >= TaskPriority.High && Status != AppTaskStatus.Completed;

        [NotMapped]
        public string DaysRemaining => Deadline.HasValue ?
            $"{(Deadline.Value - DateTime.Now).Days} days" : "No deadline";

        // Methods
        public void UpdateProgress(int progress, int updatedByUserId)
        {
            ProgressPercent = Math.Clamp(progress, 0, 100);
            UpdatedByUserId = updatedByUserId;
            UpdatedAt = DateTime.Now;

            if (ProgressPercent == 100 && Status != AppTaskStatus.Completed)
                UpdateStatus(AppTaskStatus.Completed, updatedByUserId, "Auto-completed at 100% progress");
        }

        public void UpdateStatus(AppTaskStatus newStatus, int updatedByUserId, string changeDescription = null)
        {
            if (Status != newStatus)
            {
                // Create history record for status change
                var history = new TaskHistory(Id, updatedByUserId, (int)Status, (int)newStatus,
                    changeDescription ?? $"Status changed from {Status} to {newStatus}");

                History.Add(history);

                Status = newStatus;
                UpdatedByUserId = updatedByUserId;
                UpdatedAt = DateTime.Now;
            }
        }

        public void UpdateDetails(string title, string description, TaskPriority priority,
                                TaskCategory category, DateTime? deadline, decimal? estimatedHours, int updatedByUserId)
        {
            Title = title;
            Description = description;
            Priority = priority;
            Category = category;
            Deadline = deadline;
            EstimatedHours = estimatedHours;
            UpdatedByUserId = updatedByUserId;
            UpdatedAt = DateTime.Now;
        }

        public void Reassign(int newAssignedToId, int updatedByUserId)
        {
            AssignedToId = newAssignedToId;
            UpdatedByUserId = updatedByUserId;
            UpdatedAt = DateTime.Now;
        }

        public void LogTime(decimal hoursWorked, int updatedByUserId)
        {
            ActualHours = (ActualHours ?? 0) + hoursWorked;
            UpdatedByUserId = updatedByUserId;
            UpdatedAt = DateTime.Now;
        }

        public void Delete(int deletedByUserId)
        {
            IsDeleted = true;
            DeletedAt = DateTime.Now;
            DeletedByUserId = deletedByUserId;
        }
    }
}