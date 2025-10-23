using Microsoft.EntityFrameworkCore;
using TaskFlow.DAL.DataBase;
using TaskFlow.DAL.Entites;
using TaskFlow.DAL.Enums.User;
using TaskFlow.DAL.Repo.Abstraction;
using TaskFlow.DAL.Repo.Implementation;


namespace TaskFlow.DAL.Repo.Implementation
{
    public class AppUserRepo : BaseRepo<AppUser>, IAppUserRepo
    {
        public AppUserRepo(TaskFlowDbContext context) : base(context) { }

        // Basic CRUD
        public async Task<AppUser> GetByIdAsync(int id)
            => await base.GetByIdAsync(id);

        public async Task<IEnumerable<AppUser>> GetAllAsync()
            => await base.GetAllAsync();

        public async Task AddAsync(AppUser user)
            => await base.AddAsync(user);

        public void Update(AppUser user)
            => base.Update(user);

        // Soft Delete - Uses the entity's Delete method
        public async Task SoftDeleteAsync(int userId, int deletedByUserId)
            => await base.SoftDeleteAsync(userId, deletedByUserId);

        // Specific queries
        public async Task<AppUser> GetByEmailAsync(string email)
            => await base.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<IEnumerable<AppUser>> GetByRoleAsync(Role role)
            => await base.FindAsync(u => u.UserRole == role);

        public async Task<IEnumerable<AppUser>> GetActiveUsersAsync()
            => await base.FindAsync(u => u.IsActive);

        public async Task<IEnumerable<AppUser>> GetUsersWithTasksAsync()
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .Include(u => u.CreatedTasks.Where(t => !t.IsDeleted))
                .Include(u => u.AssignedTasks.Where(t => !t.IsDeleted))
                .ToListAsync();

        public async Task<AppUser> GetUserWithDetailsAsync(int id)
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .Include(u => u.CreatedTasks.Where(t => !t.IsDeleted))
                .Include(u => u.AssignedTasks.Where(t => !t.IsDeleted))
                .Include(u => u.Comments.Where(c => !c.IsDeleted))
                .Include(u => u.TaskHistories.Where(th => !th.IsDeleted))
                .FirstOrDefaultAsync(u => u.Id == id);

        public async Task<bool> EmailExistsAsync(string email)
            => await base.AnyAsync(u => u.Email == email);

        public async Task<bool> EmailExistsAsync(string email, int excludeUserId)
            => await base.AnyAsync(u => u.Email == email && u.Id != excludeUserId);

        public async Task<int> GetTotalCountAsync()
            => await base.CountAsync();

        public async Task<bool> UserExistsAsync(int id)
            => await base.ExistsAsync(id);

        void IAppUserRepo.Delete(AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}