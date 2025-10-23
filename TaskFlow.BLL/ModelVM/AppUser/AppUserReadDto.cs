

namespace TaskFlow.BLL.ModelVM.AppUser
{
    public class AppUserReadDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public TaskFlow.DAL.Enums.User.Role UserRole { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FullName { get; set; }
        public string RoleDisplay { get; set; }
    }
}
