using Microsoft.AspNetCore.Mvc;

namespace PMT.Controllers;

public class ProjectController : Controller
{
  public IActionResult MyProjects()
  {
    return View();
  }
}
