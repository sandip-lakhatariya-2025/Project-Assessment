using Assessment.Models.ViewModel;
using Assessment.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.Web.Controllers;

public class AuthController : Controller
{

    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Index() {
        string? jwtToken = Request.Cookies["JwtCookie"];
        if (!string.IsNullOrEmpty(jwtToken)) {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserViewModel user) {

        var result = await _authService.CheckLoginCredentials(user);
        if(result.isSuccess){
            TempData["success"] = result.message;
            return RedirectToAction("Index", "Home");
        }
        TempData["error"] = result.message;
        return RedirectToAction("Index", "Auth");
    }

    [Authorize]
    public async Task<IActionResult> Logout() {

        string? jwtToken = Request.Cookies["JwtCookie"];
        if (!string.IsNullOrEmpty(jwtToken)) {
            Response.Cookies.Delete("JwtCookie");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["success"] = "User has been Logged out successfully.";
            return RedirectToAction("Index", "Auth");
        }
        TempData["error"] = "Some error occured";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Error() {
        return View();
    }
}
