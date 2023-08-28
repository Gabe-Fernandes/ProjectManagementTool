using Microsoft.AspNetCore.Mvc;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;
using PMT.Services;

namespace PMT.Controllers;

public class ProjectController : Controller
{
  private readonly IProjectRepo _projRepo;
  private readonly ISRSRepo _SRSRepo;
  private readonly ITechStackRepo _techStackRepo;
  private readonly IModelPlanningRepo _modelPlanningRepo;
  private readonly IFileStructureRepo _fileStructureRepo;
  private readonly IColorPaletteRepo _colorPaletteRepo;

  public ProjectController(IProjectRepo projRepo,
    ISRSRepo sRSRepo,
    ITechStackRepo techStackRepo,
    IModelPlanningRepo modelPlanningRepo,
    IFileStructureRepo fileStructureRepo,
    IColorPaletteRepo colorPaletteRepo)
  {
    _projRepo = projRepo;
    _SRSRepo = sRSRepo;
    _techStackRepo = techStackRepo;
    _modelPlanningRepo = modelPlanningRepo;
    _fileStructureRepo = fileStructureRepo;
    _colorPaletteRepo = colorPaletteRepo;
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
      InitializeSRS(project.Id);
    }
    // try to keep modal open here
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

  private void InitializeSRS(int projId)
  {
    SRS SRS = new()
    {
      ProjId = projId
    };
    _SRSRepo.Add(SRS);

    ColorPalette colorPalette = new()
    {
      ProjId = projId,
      // this is just because the property happens to be [Required] - might change this
      Colors = string.Empty
    };
    _colorPaletteRepo.Add(colorPalette);

    FileStructure filStructure = new()
    {
      ProjId = projId
    };
    _fileStructureRepo.Add(filStructure);

    ModelPlanning modelPlanning = new()
    {
      ProjId = projId
    };
    _modelPlanningRepo.Add(modelPlanning);

    TechStack techStack = new()
    {
      ProjId = projId
    };
    _techStackRepo.Add(techStack);
  }
}
