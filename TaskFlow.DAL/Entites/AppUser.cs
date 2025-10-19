using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskFlow.DAL.Enums.User;

namespace TaskFlow.DAL.Entites
{
    public class AppUser
    {
        public int Id { get; private set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; private set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; private set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; private set; }

        public Role UserRole { get; private set; }
        public bool IsActive { get; private set; } = true;
        public bool IsDeleted { get; private set; } = false;

        // Audit fields
        public int? CreatedByUserId { get; private set; }
        public int? UpdatedByUserId { get; private set; }
        public int? DeletedByUserId { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        // Navigation Properties
        public ICollection<TaskItem> CreatedTasks { get; private set; } = new List<TaskItem>();
        public ICollection<TaskItem> AssignedTasks { get; private set; } = new List<TaskItem>();
        public ICollection<Comment> Comments { get; private set; } = new List<Comment>();
        public ICollection<TaskHistory> TaskHistories { get; private set; } = new List<TaskHistory>();

        // Constructor
        public AppUser(string firstName, string lastName, string email, Role userRole, int createdByUserId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserRole = userRole;
            CreatedByUserId = createdByUserId;
            CreatedAt = DateTime.Now;
        }

        protected AppUser() { }

        // Helper properties
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public string RoleDisplay => UserRole.ToString();

        // Methods
        public void Delete(int deletedByUserId)
        {
            IsDeleted = true;
            DeletedAt = DateTime.Now;
            DeletedByUserId = deletedByUserId;
        }

        public void Update(string firstName, string lastName, string email, Role userRole, int updatedByUserId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserRole = userRole;
            UpdatedByUserId = updatedByUserId;
            UpdatedAt = DateTime.Now;
        }

        public void Deactivate(int updatedByUserId)
        {
            IsActive = false;
            UpdatedByUserId = updatedByUserId;
            UpdatedAt = DateTime.Now;
        }

        public void Activate(int updatedByUserId)
        {
            IsActive = true;
            UpdatedByUserId = updatedByUserId;
            UpdatedAt = DateTime.Now;
        }
    }
}