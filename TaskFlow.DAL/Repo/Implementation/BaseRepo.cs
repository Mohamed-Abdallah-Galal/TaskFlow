

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskFlow.DAL.DataBase;

namespace TaskFlow.DAL.Repo.Implementation
{
    public class BaseRepo<T> where T : class
    {
        
        protected readonly TaskFlowDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected BaseRepo(TaskFlowDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // ========== BASIC CRUD OPERATIONS ==========
        protected async Task<T> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        protected async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.Where(IsNotDeletedExpression()).ToListAsync();

        protected async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        protected void Update(T entity)
            => _dbSet.Update(entity);

        // ========== SOFT DELETE USING ENTITY METHODS ==========
        protected void SoftDelete(T entity, int deletedByUserId)
        {
            // Use the entity's own Delete method
            var deleteMethod = typeof(T).GetMethod("Delete");
            if (deleteMethod != null)
            {
                deleteMethod.Invoke(entity, new object[] { deletedByUserId });
                _dbSet.Update(entity);
            }
            else
            {
                // Fallback if entity doesn't have Delete method
                var isDeletedProperty = typeof(T).GetProperty("IsDeleted");
                var deletedAtProperty = typeof(T).GetProperty("DeletedAt");
                var deletedByUserIdProperty = typeof(T).GetProperty("DeletedByUserId");

                if (isDeletedProperty != null) isDeletedProperty.SetValue(entity, true);
                if (deletedAtProperty != null) deletedAtProperty.SetValue(entity, DateTime.Now);
                if (deletedByUserIdProperty != null) deletedByUserIdProperty.SetValue(entity, deletedByUserId);

                _dbSet.Update(entity);
            }
        }

        protected async Task SoftDeleteAsync(int id, int deletedByUserId)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                SoftDelete(entity, deletedByUserId);
            }
        }

        // ========== QUERY METHODS (AUTO-EXCLUDE DELETED) ==========
        protected async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(IsNotDeletedExpression()).FirstOrDefaultAsync(predicate);

        protected async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(IsNotDeletedExpression()).Where(predicate).ToListAsync();

        protected async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null)
        {
            var query = _dbSet.Where(IsNotDeletedExpression());
            return predicate == null ? await query.AnyAsync() : await query.AnyAsync(predicate);
        }

        protected async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            var query = _dbSet.Where(IsNotDeletedExpression());
            return predicate == null ? await query.CountAsync() : await query.CountAsync(predicate);
        }

        // ========== ADMIN METHODS (INCLUDE DELETED) ==========
        protected async Task<T> GetByIdIncludeDeletedAsync(int id)
            => await _dbSet.FindAsync(id);

        protected async Task<IEnumerable<T>> GetAllIncludeDeletedAsync()
            => await _dbSet.ToListAsync();

        protected async Task<IEnumerable<T>> FindIncludeDeletedAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        // ========== UTILITY METHODS ==========
        protected async Task<bool> ExistsAsync(int id)
            => await GetByIdAsync(id) != null;

        

        // ========== PRIVATE HELPER ==========
        protected Expression<Func<T, bool>> IsNotDeletedExpression()
        {
            var parameter = Expression.Parameter(typeof(T), "entity");
            var isDeletedProperty = typeof(T).GetProperty("IsDeleted");

            if (isDeletedProperty == null)
                return entity => true; // Entity doesn't support soft delete

            var property = Expression.Property(parameter, isDeletedProperty);
            var falseConstant = Expression.Constant(false);
            var equality = Expression.Equal(property, falseConstant);

            return Expression.Lambda<Func<T, bool>>(equality, parameter);
        }
    }
}
