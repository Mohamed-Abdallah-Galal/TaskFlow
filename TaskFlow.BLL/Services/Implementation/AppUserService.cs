using AutoMapper;
using TaskFlow.BLL.ModelVM.AppUser;
using TaskFlow.BLL.Services.Abstraction;
using TaskFlow.DAL.Entites;
using TaskFlow.DAL.Repo.Abstraction;
using TaskFlow.DAL.Repo.Implementation;


namespace TaskFlow.BLL.Services.Implementation
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepo _userRepo;
        private readonly IMapper _mapper;

        public AppUserService(IAppUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<AppUserReadDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            return _mapper.Map<AppUserReadDto>(user);
        }

        public async Task<AppUserReadDto> GetUserWithDetailsAsync(int id)
        {
            var user = await _userRepo.GetUserWithDetailsAsync(id);
            return _mapper.Map<AppUserReadDto>(user);
        }

        public async Task<List<AppUserReadDto>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllAsync();
            return _mapper.Map<List<AppUserReadDto>>(users);
        }

        public async Task<List<AppUserReadDto>> GetActiveUsersAsync()
        {
            var users = await _userRepo.GetActiveUsersAsync();
            return _mapper.Map<List<AppUserReadDto>>(users);
        }

        public async Task<List<AppUserReadDto>> GetUsersByRoleAsync(TaskFlow.DAL.Enums.User.Role role)
        {
            var users = await _userRepo.GetByRoleAsync(role);
            return _mapper.Map<List<AppUserReadDto>>(users);
        }

        public async Task<int> CreateUserAsync(AppUserCreateDto dto)
        {
            // Validate email uniqueness
            if (await _userRepo.EmailExistsAsync(dto.Email))
                throw new InvalidOperationException($"Email '{dto.Email}' is already registered.");

            // Validate email format
            if (!IsValidEmail(dto.Email))
                throw new ArgumentException("Invalid email format.");

            var user = _mapper.Map<AppUser>(dto);
            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            return user.Id;
        }

        public async Task UpdateUserAsync(AppUserUpdateDto dto)
        {
            var user = await _userRepo.GetByIdAsync(dto.Id);
            if (user == null)
                throw new ArgumentException($"User with ID {dto.Id} not found.");

            // Validate email uniqueness (excluding current user)
            if (await _userRepo.EmailExistsAsync(dto.Email, dto.Id))
                throw new InvalidOperationException($"Email '{dto.Email}' is already registered by another user.");

            // Business rule: SuperAdmin cannot be demoted
            if (user.UserRole == TaskFlow.DAL.Enums.User.Role.SuperAdmin && dto.UserRole != TaskFlow.DAL.Enums.User.Role.SuperAdmin)
                throw new InvalidOperationException("Cannot change SuperAdmin role.");

            _mapper.Map(dto, user);
            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId, int deletedByUserId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found.");

            if (!await CanUserBeDeletedAsync(userId))
                throw new InvalidOperationException($"User cannot be deleted because they have active associated tasks or comments.");

            await _userRepo.SoftDeleteAsync(userId, deletedByUserId);
            await _userRepo.SaveChangesAsync();
        }

        public async Task DeactivateUserAsync(int userId, int updatedByUserId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found.");

            user.Deactivate(updatedByUserId);
            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();
        }

        public async Task ActivateUserAsync(int userId, int updatedByUserId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found.");

            user.Activate(updatedByUserId);
            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _userRepo.EmailExistsAsync(email);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int excludeUserId)
        {
            return !await _userRepo.EmailExistsAsync(email, excludeUserId);
        }

        public async Task<bool> CanUserBeDeletedAsync(int userId)
        {
            var user = await _userRepo.GetUserWithDetailsAsync(userId);
            if (user == null) return false;

            bool hasActiveCreatedTasks = user.CreatedTasks.Any(t => !t.IsDeleted);
            bool hasActiveAssignedTasks = user.AssignedTasks.Any(t => !t.IsDeleted);
            bool hasActiveComments = user.Comments.Any(c => !c.IsDeleted);

            return !hasActiveCreatedTasks && !hasActiveAssignedTasks && !hasActiveComments;
        }

        public async Task<int> GetUserCountByRoleAsync(TaskFlow.DAL.Enums.User.Role role)
        {
            var users = await _userRepo.GetByRoleAsync(role);
            return users.Count();
        }

        public async Task<Dictionary<TaskFlow.DAL.Enums.User.Role, int>> GetUserRoleDistributionAsync()
        {
            var allUsers = await _userRepo.GetAllAsync();
            return allUsers
                .GroupBy(u => u.UserRole)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}