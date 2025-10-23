

using TaskFlow.DAL.Entites;

namespace TaskFlow.DAL.Repo.Abstraction
{
    public interface ICommentRepo
    {
        // Basic CRUD
        Task<Comment> GetByIdAsync(int id);
        Task<IEnumerable<Comment>> GetAllAsync();
        Task AddAsync(Comment comment);
        void Update(Comment comment);
        void Delete(Comment comment);

        // Specific queries
        Task<IEnumerable<Comment>> GetCommentsByTaskAsync(int taskId);
        Task<IEnumerable<Comment>> GetCommentsByUserAsync(int userId);
        Task<IEnumerable<Comment>> GetRecentCommentsAsync(int count = 10);
        Task<IEnumerable<Comment>> GetCommentsWithUsersAsync(int taskId);
        Task<int> GetCommentCountByTaskAsync(int taskId);
        Task<IEnumerable<Comment>> SearchCommentsAsync(string searchTerm);

        // Soft Delete
        Task SoftDeleteAsync(int taskId, int deletedByUserId);
        Task<int> SaveChangesAsync();

    }
}
