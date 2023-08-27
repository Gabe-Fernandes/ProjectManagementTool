using Microsoft.AspNetCore.Mvc;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;
using PMT.Services;

namespace PMT.Controllers;

public class ProjectController : Controller
{
  private readonly IProjectRepo _projRepo;

  public ProjectController(IProjectRepo projRepo)
  {
    _projRepo = projRepo;
  }

  public async Task<IActionResult> MyProjects()
  {
    ViewData[Str.Projects] = await _projRepo.GetAllFromUserAsync(string.Empty);
    return View();
  }
  [HttpPost]
  [AutoValidateAntiforgeryToken]
  public IActionResult NewProject(Project project)
  {
    if (ModelState.IsValid)
    {
      project.StartDate = DateTime.Now;
      _projRepo.Add(project);
    }
    return RedirectToAction(Str.MyProjects, Str.Project);
  }

  public async Task<IActionResult> DeleteProject(int projIdToDelete)
  {
    var projToDelete = await _projRepo.GetByIdAsync(projIdToDelete);
    return View(projToDelete);
  }
  [HttpPost]
  [AutoValidateAntiforgeryToken]
  public IActionResult DeleteProject(Project projToDelete)
  {
    _projRepo.Delete(projToDelete);
    return RedirectToAction(Str.MyProjects, Str.Project);
  }

  public IActionResult ProjectDash(int projId)
  {
    CookieOptions options = new()
    {
      Expires = DateTime.Now.AddYears(999),
      IsEssential = true
    };
    HttpContext.Response.Cookies.Append("projId", projId.ToString(), options);
    return View();
  }
}
