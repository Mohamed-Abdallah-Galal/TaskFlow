using Microsoft.EntityFrameworkCore;
using TaskFlow.DAL.DataBase;
using TaskFlow.DAL.Entites;
using TaskFlow.DAL.Repo.Abstraction;

namespace TaskFlow.DAL.Repo.Implementation
{
    public class CommentRepo : BaseRepo<Comment>, ICommentRepo
    {
        public CommentRepo(TaskFlowDbContext context) : base(context) { }

        // Basic CRUD
        public async Task<Comment> GetByIdAsync(int id)
            => await base.GetByIdAsync(id);

        public async Task<IEnumerable<Comment>> GetAllAsync()
            => await base.GetAllAsync();

        public async Task AddAsync(Comment comment)
            => await base.AddAsync(comment);

        public void Update(Comment comment)
            => base.Update(comment);

        public void Delete(Comment comment)
            => base.SoftDelete(comment, comment.DeletedByUserId ?? 1); // Fallback user ID

        // Soft Delete
        public async Task SoftDeleteAsync(int commentId, int deletedByUserId)
            => await base.SoftDeleteAsync(commentId, deletedByUserId);

        // Specific queries
        public async Task<IEnumerable<Comment>> GetCommentsByTaskAsync(int taskId)
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .Where(c => c.TaskId == taskId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Comment>> GetCommentsByUserAsync(int userId)
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Comment>> GetRecentCommentsAsync(int count = 10)
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .OrderByDescending(c => c.CreatedAt)
                .Take(count)
                .ToListAsync();

        public async Task<IEnumerable<Comment>> GetCommentsWithUsersAsync(int taskId)
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .Where(c => c.TaskId == taskId)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

        public async Task<int> GetCommentCountByTaskAsync(int taskId)
            => await _dbSet
                .Where(IsNotDeletedExpression())
                .CountAsync(c => c.TaskId == taskId);

        public async Task<IEnumerable<Comment>> SearchCommentsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            return await _dbSet
                .Where(IsNotDeletedExpression())
                .Where(c => c.Message.Contains(searchTerm))
                .Include(c => c.User)
                .Include(c => c.Task)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}