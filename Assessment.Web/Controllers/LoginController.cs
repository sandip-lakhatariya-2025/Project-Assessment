using Microsoft.AspNetCore.Mvc;

namespace Assessment.Web.Controllers;

public class LoginController : Controller
{
    [HttpGet]
    public IActionResult Index() {
        return View();
    }
}
