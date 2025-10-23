
using TaskFlow.BLL.ModelVM.AppUser;

namespace TaskFlow.BLL.Services.Abstraction
{
    public interface IAppUserService
    {
        Task<AppUserReadDto> GetUserByIdAsync(int id);
        Task<AppUserReadDto> GetUserWithDetailsAsync(int id);
        Task<List<AppUserReadDto>> GetAllUsersAsync();
        Task<List<AppUserReadDto>> GetActiveUsersAsync();
        Task<List<AppUserReadDto>> GetUsersByRoleAsync(TaskFlow.DAL.Enums.User.Role role);

        Task<int> CreateUserAsync(AppUserCreateDto dto);
        Task UpdateUserAsync(AppUserUpdateDto dto);
        Task DeleteUserAsync(int userId, int deletedByUserId);
        Task DeactivateUserAsync(int userId, int updatedByUserId);
        Task ActivateUserAsync(int userId, int updatedByUserId);

        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsEmailUniqueAsync(string email, int excludeUserId);
        Task<bool> CanUserBeDeletedAsync(int userId);

        Task<int> GetUserCountByRoleAsync(TaskFlow.DAL.Enums.User.Role role);
        Task<Dictionary<TaskFlow.DAL.Enums.User.Role, int>> GetUserRoleDistributionAsync();

    }
}
