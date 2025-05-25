using System.Linq.Expressions;

namespace LodgeMasterWeb.Services
{
    public class GenericService
    {
        private readonly ApplicationDbContext _context; // Replace with your DbContext type

        public GenericService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int GetMaxId<TEntity>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, int>> idSelector) where TEntity : class
        {
            try
            {
                var maxId = _context.Set<TEntity>()
                    .Where(condition)
                    .AsNoTracking()
                    .Select(idSelector)
                    .Max();

                return maxId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int GetMaxSorted<TEntity>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, int>> idSelector) where TEntity : class
        {
            try
            {
                var maxId = _context.Set<TEntity>()
                    .Where(condition)
                    .AsNoTracking()
                    .Select(idSelector)
                    .Max();

                return maxId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}
