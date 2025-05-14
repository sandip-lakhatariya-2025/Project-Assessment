using Assessment.DataAccess.Data;
using Assessment.DataAccess.Repository.IRepository;
using Assessment.Models.Models;
using Assessment.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Assessment.DataAccess.Repository;

public class UserRepository : IUserRepository
{
    private readonly MyDbContext _context;

    public UserRepository(MyDbContext context) {
        _context = context;
    }

    public async Task<UserViewModel?> GetUserByEmail(string email)
    {
        return await _context.Users.Where(u => u.Email.ToLower() == email.ToLower()).Select(u => new UserViewModel{
            Email = u.Email,
            UserName = u.UserName,
            Password = u.Password
        }).FirstOrDefaultAsync();
    }

}
