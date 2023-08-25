using Microsoft.AspNetCore.Mvc;
using PMT.Data.Models;
using PMT.Services;

namespace PMT.Controllers;

public class ProjectController : Controller
{
  public IActionResult MyProjects()
  {
    return View();
  }
  [HttpPost]
  [AutoValidateAntiforgeryToken]
  public IActionResult NewProject(Project project)
  {
    return RedirectToAction(Str.Project, Str.MyProjects);
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
