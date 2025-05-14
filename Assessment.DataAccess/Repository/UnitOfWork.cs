using Assessment.DataAccess.Data;
using Assessment.DataAccess.Repository.IRepository;

namespace Assessment.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly MyDbContext _context;
    public UnitOfWork(MyDbContext context) {
        _context = context;
        Users = new UserRepository(_context);
    }
    public IUserRepository Users { get; private set; }
}
