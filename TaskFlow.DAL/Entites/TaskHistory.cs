using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskFlow.DAL.Enums.Task;

namespace TaskFlow.DAL.Entites
{
    public class TaskHistory
    {
        public int Id { get; private set; }

        // Foreign Keys
        public int TaskId { get; private set; }
        public int ChangedByUserId { get; private set; }

        // Navigation Properties
        public TaskItem Task { get; private set; }
        public AppUser ChangedByUser { get; private set; }

        // Status change
        public int OldStatusId { get; private set; }
        public int NewStatusId { get; private set; }
        public DateTime ChangedAt { get; private set; } = DateTime.Now;

        // Additional change tracking
        [StringLength(500)]
        public string ChangeDescription { get; private set; }

        public HistoryActionType ActionType { get; private set; } = HistoryActionType.StatusChange;

        // Audit fields
        public bool IsDeleted { get; private set; } = false;
        public int? CreatedByUserId { get; private set; }
        public int? UpdatedByUserId { get; private set; }
        public int? DeletedByUserId { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        // Constructor for status changes
        public TaskHistory(int taskId, int changedByUserId, int oldStatusId, int newStatusId,
                          string changeDescription = null, HistoryActionType actionType = HistoryActionType.StatusChange)
        {
            TaskId = taskId;
            ChangedByUserId = changedByUserId;
            OldStatusId = oldStatusId;
            NewStatusId = newStatusId;
            ChangeDescription = changeDescription;
            ActionType = actionType;
            ChangedAt = DateTime.Now;
            CreatedAt = DateTime.Now;
        }

        protected TaskHistory() { }

        // Helper properties
        [NotMapped]
        public AppTaskStatus OldStatus => (AppTaskStatus)OldStatusId;

        [NotMapped]
        public AppTaskStatus NewStatus => (AppTaskStatus)NewStatusId;

        [NotMapped]
        public string ChangeSummary => $"{OldStatus} → {NewStatus}";

        [NotMapped]
        public bool IsSignificantChange => NewStatusId != OldStatusId;

        // Methods
        public void Delete(int deletedByUserId)
        {
            IsDeleted = true;
            DeletedAt = DateTime.Now;
            DeletedByUserId = deletedByUserId;
        }
    }
}