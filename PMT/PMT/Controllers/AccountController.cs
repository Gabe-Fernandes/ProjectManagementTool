using Microsoft.AspNetCore.Mvc;

namespace PMT.Controllers;

public class AccountController : Controller
{
  public IActionResult Login()
  {
    return View();
  }
  public IActionResult Register()
  {
    return View();
  }
  public IActionResult RecoverPassword()
  {
    return View();
  }
}
