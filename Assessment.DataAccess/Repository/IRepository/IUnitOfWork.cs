namespace Assessment.DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
    public IUserRepository Users { get; }
}
