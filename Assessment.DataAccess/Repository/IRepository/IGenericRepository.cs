using System.Linq.Expressions;

namespace Assessment.DataAccess.Repository.IRepository;

public interface IGenericRepository<T> where T : class
{
    Task<TResult?> GetFirstOrDefaultSelected<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector);
}
