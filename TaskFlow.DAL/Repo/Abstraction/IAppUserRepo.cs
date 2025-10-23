

using TaskFlow.DAL.Entites;
using TaskFlow.DAL.Enums.User;

namespace TaskFlow.DAL.Repo.Abstraction
{
    public interface IAppUserRepo
    {
        // Basic CRUD
        Task<AppUser> GetByIdAsync(int id);
        Task<IEnumerable<AppUser>> GetAllAsync();
        Task AddAsync(AppUser user);
        void Update(AppUser user);
        void Delete(AppUser user);

        // Specific queries
        Task<AppUser> GetByEmailAsync(string email);
        Task<IEnumerable<AppUser>> GetByRoleAsync(Role role);
        Task<IEnumerable<AppUser>> GetActiveUsersAsync();
        Task<IEnumerable<AppUser>> GetUsersWithTasksAsync();
        Task<AppUser> GetUserWithDetailsAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> EmailExistsAsync(string email, int excludeUserId);

        // Soft Delete
        Task SoftDeleteAsync(int taskId, int deletedByUserId);


        // Additional useful methods
        Task<int> GetTotalCountAsync();
        Task<bool> UserExistsAsync(int id);
        Task<int> SaveChangesAsync();
    }
}
