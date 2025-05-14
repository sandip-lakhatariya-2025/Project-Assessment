using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Assessment.DataAccess.Repository.IRepository;
using Assessment.Models.ViewModel;
using Assessment.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Assessment.Services;

public class LoginService : ILoginService
{

    private readonly IUserRepository _userRepository;
    private readonly IConfiguration  _configuration;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginService(IUserRepository userRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) {
        _userRepository = userRepository;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<(bool isSuccess, string message)> CheckLoginCredentials(UserViewModel user)
    {

        HttpContext httpContext = _httpContextAccessor.HttpContext;

        try
        {
            UserViewModel? existingUser = await _userRepository.GetUserByEmail(user.Email);

            if(existingUser != null) {
                if(existingUser.Password != HashPassword(user.Password)) {
                    return(false, $"Invalid Login Credentials.");
                }

                string jwttoken = GenerateJWTToken(existingUser.UserName, "Admin");

                httpContext.Response.Cookies.Append("JwtCookie", jwttoken, new CookieOptions{
                    HttpOnly = true,
                    Secure = true,
                    Expires = user.IsRememberMe ? DateTime.Now.AddMinutes(10) : DateTime.Now.AddMinutes(1)
                });

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

    private string GenerateJWTToken(string userName, string role)
    {
        Claim[] claims = new[] {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(10);

        var token = new JwtSecurityToken(
            _configuration["JwtToken:Issuer"],
            _configuration["JwtToken:Audience"],
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
