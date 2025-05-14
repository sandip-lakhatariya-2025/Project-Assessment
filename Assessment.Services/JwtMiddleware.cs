using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Assessment.Services;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next) {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? token = context.Request.Cookies["JwtCookie"];

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = handler.ReadJwtToken(token);
                IEnumerable<Claim> claims = jwtToken.Claims;
                Claim? roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                Claim? userNameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

                if (roleClaim != null)
                {
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "jwt");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    context.User = claimsPrincipal;
                    context.Items["Role"] = roleClaim.Value;
                    context.Items["UserName"] = userNameClaim!.Value;
                }
                else
                {
                    context.Response.Redirect("/Login/Index");
                    return;
                }
            }
            catch (Exception)
            {
                context.Response.Redirect("/Login/Index");
                return;
            }
        }
        await _next(context);
    }
}
