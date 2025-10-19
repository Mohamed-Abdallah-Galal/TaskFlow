using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskFlow.DAL.Enums.Task;

namespace TaskFlow.DAL.Entites
{
    public class Attachment
    {
        public int Id { get; private set; }
        public int TaskId { get; private set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; private set; }

        [Required]
        [StringLength(500)]
        public string FilePath { get; private set; }

        [StringLength(50)]
        public string ContentType { get; private set; }

        public AttachmentType FileType { get; private set; }
        public long FileSize { get; private set; }

        // Navigation Property
        public TaskItem Task { get; private set; }

        // Audit fields
        public bool IsDeleted { get; private set; } = false;
        public int? CreatedByUserId { get; private set; }
        public int? UpdatedByUserId { get; private set; }
        public int? DeletedByUserId { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        // Constructor
        public Attachment(int taskId, string fileName, string filePath, string contentType,
                         AttachmentType fileType, long fileSize, int createdByUserId)
        {
            TaskId = taskId;
            FileName = fileName;
            FilePath = filePath;
            ContentType = contentType;
            FileType = fileType;
            FileSize = fileSize;
            CreatedByUserId = createdByUserId;
            CreatedAt = DateTime.Now;
        }

        protected Attachment() { }

        // Helper properties
        [NotMapped]
        public string FileSizeDisplay => FileSize switch
        {
            < 1024 => $"{FileSize} B",
            < 1048576 => $"{FileSize / 1024.0:0.00} KB",
            _ => $"{FileSize / 1048576.0:0.00} MB"
        };

        [NotMapped]
        public string FileIcon => FileType switch
        {
            AttachmentType.Document => "📄",
            AttachmentType.Image => "🖼️",
            AttachmentType.Code => "💻",
            AttachmentType.Design => "🎨",
            AttachmentType.Log => "📋",
            AttachmentType.Video => "🎥",
            AttachmentType.Audio => "🎵",
            AttachmentType.Other => "📎",
            _ => "📎"
        };

        // Methods
        public void Update(string fileName, int updatedByUserId)
        {
            FileName = fileName;
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