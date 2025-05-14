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

public class AuthService : IAuthService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration  _configuration;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<(bool isSuccess, string message)> CheckLoginCredentials(UserViewModel user)
    {

        HttpContext httpContext = _httpContextAccessor.HttpContext;

        try
        {
            UserViewModel? existingUser = await _unitOfWork.Users.GetFirstOrDefaultSelected(
                u => u.Email.ToLower() == user.Email.ToLower(), 
                u => new UserViewModel {
                    Email = u.Email,
                    UserName = u.UserName,
                    Password = u.Password,
                    RoleName = u.Role.RoleName
                }
            );

            if(existingUser != null) {
                if(existingUser.Password != HashPassword(user.Password)) {
                    return(false, $"Invalid Login Credentials.");
                }

                string jwttoken = GenerateJWTToken(existingUser.UserName, existingUser.RoleName!, user.IsRememberMe);

                httpContext.Response.Cookies.Append("JwtCookie", jwttoken, new CookieOptions{
                    HttpOnly = true,
                    Secure = true,
                    Expires = user.IsRememberMe ? DateTime.UtcNow.AddDays(15) : DateTime.UtcNow.AddDays(1)
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

    private string GenerateJWTToken(string userName, string role, bool isRememberMe)
    {
        Claim[] claims = new[] {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, role)
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:Key"]!));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        DateTime expires =  isRememberMe ? DateTime.UtcNow.AddDays(15) : DateTime.UtcNow.AddDays(1);

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
