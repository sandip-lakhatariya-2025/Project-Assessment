using Assessment.Models.ViewModel;

namespace Assessment.DataAccess.Repository.IRepository;

public interface IUserRepository
{
    Task<UserViewModel?> GetUserByEmail(string email);
}
