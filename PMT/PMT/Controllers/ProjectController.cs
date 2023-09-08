using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;
using PMT.Services;
using System.Security.Claims;

namespace PMT.Controllers;

[Authorize]
public class ProjectController : Controller
{
  private readonly IHttpContextAccessor _contextAccessor;
  private readonly IAppUserRepo _appUserRepo;
  private readonly IProjectRepo _projRepo;
  private readonly ISRSRepo _SRSRepo;
  private readonly ITechStackRepo _techStackRepo;
  private readonly IModelPlanningRepo _modelPlanningRepo;
  private readonly IFileStructureRepo _fileStructureRepo;
  private readonly IColorPaletteRepo _colorPaletteRepo;
  private readonly IStoryRepo _storyRepo;
  private readonly IBugReportRepo _bugReportRepo;

  public ProjectController(IProjectRepo projRepo,
    ISRSRepo sRSRepo,
    ITechStackRepo techStackRepo,
    IModelPlanningRepo modelPlanningRepo,
    IFileStructureRepo fileStructureRepo,
    IColorPaletteRepo colorPaletteRepo,
    IAppUserRepo appUserRepo,
    IHttpContextAccessor contextAccessor,
    IStoryRepo storyRepo,
    IBugReportRepo bugReportRepo)
  {
    _projRepo = projRepo;
    _SRSRepo = sRSRepo;
    _techStackRepo = techStackRepo;
    _modelPlanningRepo = modelPlanningRepo;
    _fileStructureRepo = fileStructureRepo;
    _colorPaletteRepo = colorPaletteRepo;
    _appUserRepo = appUserRepo;
    _contextAccessor = contextAccessor;
    _storyRepo = storyRepo;
    _bugReportRepo = bugReportRepo;
  }

  public async Task<IActionResult> MyProjects()
  {
    AppUser user = GetUser();
    ViewData["defaultProjId"] = user.DefaultProjId;
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
  public async Task<IActionResult> DeleteProject(Project projToDelete)
  {
    var SRS = await _SRSRepo.GetByProjectIdAsync(projToDelete.Id);
    var colorPalette = await _colorPaletteRepo.GetByProjectIdAsync(projToDelete.Id);
    var fileStructure = await _fileStructureRepo.GetByProjectIdAsync(projToDelete.Id);
    var modelPlan = await _modelPlanningRepo.GetByProjectIdAsync(projToDelete.Id);
    var techStack = await _techStackRepo.GetByProjectIdAsync(projToDelete.Id);
    var bugReports = await _bugReportRepo.GetAllAsync(projToDelete.Id, string.Empty);
    List<BugReport> bugReportsList = bugReports.ToList();
    var stories = await _storyRepo.GetAllWithSearchFilterAsync(projToDelete.Id, string.Empty);
    List<Story> storiesList = stories.ToList();

    _SRSRepo.Delete(SRS);
    _colorPaletteRepo.Delete(colorPalette);
    _fileStructureRepo.Delete(fileStructure);
    _modelPlanningRepo.Delete(modelPlan);
    _techStackRepo.Delete(techStack);

    for (int i = 0; i < bugReportsList.Count; i++)
    {
      _bugReportRepo.Delete(bugReportsList[i]);
    }
    for (int i = 0; i < storiesList.Count; i++)
    {
      _storyRepo.Delete(storiesList[i]);
    }

    _projRepo.Delete(projToDelete);
    return RedirectToAction(Str.MyProjects, Str.Project);
  }

  public IActionResult ProjectDash(int projId)
  {
    // use projId to load dash. If this fails, proj with this Id was likely deleted - kick user back to MyProjects
    
    if (projId != 0)
    {
      CookieOptions options = new()
      {
        Expires = DateTime.Now.AddYears(999),
        IsEssential = true
      };
      HttpContext.Response.Cookies.Append("projId", projId.ToString(), options);
    }

    return View();
  }
  [HttpPost]
  [AutoValidateAntiforgeryToken]
  public IActionResult SetDefaultProjId(Project project)
  {
    AppUser user = GetUser();
    user.DefaultProjId = project.Id;
    _appUserRepo.Update(user);
    return RedirectToAction(Str.MyProjects, Str.Project);
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

  private AppUser GetUser()
  {
    string myId = _contextAccessor.HttpContext.User.FindFirstValue("Id");
    return _appUserRepo.GetById(myId);
  }
}
