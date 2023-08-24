using Microsoft.AspNetCore.Mvc;

namespace PMT.Controllers;

public class ProjectController : Controller
{
  public IActionResult MyProjects()
  {
    return View();
  }
  public IActionResult DeleteProject()
  {
    return View();
  }
  public IActionResult ProjectDash()
  {
    return View();
  }
}
