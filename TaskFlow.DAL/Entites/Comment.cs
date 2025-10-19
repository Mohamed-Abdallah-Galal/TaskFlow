using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskFlow.DAL.Entites
{
    public class Comment
    {
        public int Id { get; private set; }

        public int TaskId { get; private set; }
        public int UserId { get; private set; }

        [Required]
        [StringLength(1000)]
        public string Message { get; private set; }

        public bool IsInternalNote { get; private set; } = false;

        // Navigation Properties
        public TaskItem Task { get; private set; }
        public AppUser User { get; private set; }

        // Audit fields
        public bool IsDeleted { get; private set; } = false;
        public int? CreatedByUserId { get; private set; }
        public int? UpdatedByUserId { get; private set; }
        public int? DeletedByUserId { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        // Constructor
        public Comment(int taskId, int userId, string message, int createdByUserId, bool isInternalNote = false)
        {
            TaskId = taskId;
            UserId = userId;
            Message = message;
            CreatedByUserId = createdByUserId;
            IsInternalNote = isInternalNote;
            CreatedAt = DateTime.Now;
        }

        protected Comment() { }

        // Helper properties
        [NotMapped]
        public string ShortMessage => Message.Length > 100 ? Message.Substring(0, 100) + "..." : Message;

        [NotMapped]
        public bool IsLongMessage => Message.Length > 200;

        // Methods
        public void Update(string message, int updatedByUserId, bool? isInternalNote = null)
        {
            Message = message;
            if (isInternalNote.HasValue)
                IsInternalNote = isInternalNote.Value;
            UpdatedByUserId = updatedByUserId;
            UpdatedAt = DateTime.Now;
        }

        public void MarkAsInternal(int updatedByUserId)
        {
            IsInternalNote = true;
            UpdatedByUserId = updatedByUserId;
            UpdatedAt = DateTime.Now;
        }

        public void MarkAsPublic(int updatedByUserId)
        {
            IsInternalNote = false;
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