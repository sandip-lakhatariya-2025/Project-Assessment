using System.Linq.Expressions;
using Assessment.DataAccess.Data;
using Assessment.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Assessment.DataAccess.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly MyDbContext _context;
    internal DbSet<T> _dbSet;

    public GenericRepository(MyDbContext context) {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public Task<TResult?> GetFirstOrDefaultSelected<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector)
    {
        return _dbSet.Where(filter).Select(selector).FirstOrDefaultAsync();
    }

}
