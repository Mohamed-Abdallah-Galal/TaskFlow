

namespace TaskFlow.BLL.ModelVM.AppUser
{
    public class AppUserCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public TaskFlow.DAL.Enums.User.Role UserRole { get; set; }
        public int CreatedByUserId { get; set; }
    }
}
