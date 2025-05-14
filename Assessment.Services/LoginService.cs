using System.Security.Cryptography;
using System.Text;
using Assessment.DataAccess.Repository.IRepository;
using Assessment.Models.Models;
using Assessment.Models.ViewModel;
using Assessment.Services.IServices;

namespace Assessment.Services;

public class LoginService : ILoginService
{

    private readonly IUserRepository _userRepository;

    public LoginService(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public async Task<(bool isSuccess, string message)> CheckLoginCredentials(UserViewModel user)
    {
        try
        {
            UserViewModel? existingUser = await _userRepository.GetUserByEmail(user.Email);

            if(existingUser != null) {
                if(existingUser.Password != HashPassword(user.Password)) {
                    return(false, $"Invalid Login Credentials.");
                }
                return (true, "User Logged in successfully.");
            }

            return(false, $"Invalid Login Credentials.");
        }
        catch (Exception ex)
        {
            return(false, $"There is an error: {ex}");
        }
    }

    private string HashPassword(string password)
    {
        using SHA256? sha256 = SHA256.Create();
        byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

}
