using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        string token = Request.Cookies["JwtCookie"]!;

        if(token == null) {
            return RedirectToAction("Index", "Login");
        }
        return View();
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Privacy()
    {
        return View();
    }
}
