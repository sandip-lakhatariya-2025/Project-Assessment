using Assessment.Models.ViewModel;
using Assessment.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.Web.Controllers;

public class LoginController : Controller
{

    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService) {
        _loginService = loginService;
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

        var result = await _loginService.CheckLoginCredentials(user);
        if(result.isSuccess){
            TempData["success"] = result.message;
            return RedirectToAction("Index", "Home");
        }
        TempData["error"] = result.message;
        return RedirectToAction("Index", "Login");
    }

    [HttpGet]
    public IActionResult Error() {
        return View();
    }
}
