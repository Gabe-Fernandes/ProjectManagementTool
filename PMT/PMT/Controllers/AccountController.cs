using Microsoft.AspNetCore.Mvc;
using PMT.Services;
using PMT.Views.Account;

namespace PMT.Controllers;

public class AccountController : Controller
{
  public IActionResult Login()
  {
    return View();
  }
  [HttpPost]
  [AutoValidateAntiforgeryToken]
  public IActionResult Login(AccountVM viewModel)
  {
    return RedirectToAction(Str.MyProjects, Str.Project);
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
