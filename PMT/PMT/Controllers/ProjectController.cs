using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;
using PMT.Services;
using PMT.Services.ProjectMetrics;
using System.Security.Claims;

namespace PMT.Controllers;

[Authorize]
public class ProjectController(IProjectRepo projRepo,
	ISRSRepo sRSRepo,
	ITechStackRepo techStackRepo,
	IModelPlanningRepo modelPlanningRepo,
	IFileStructureRepo fileStructureRepo,
	IColorPaletteRepo colorPaletteRepo,
	IAppUserRepo appUserRepo,
	IHttpContextAccessor contextAccessor,
	IStoryRepo storyRepo,
	IBugReportRepo bugReportRepo) : Controller
{
  private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
  private readonly IAppUserRepo _appUserRepo = appUserRepo;
  private readonly IProjectRepo _projRepo = projRepo;
  private readonly ISRSRepo _SRSRepo = sRSRepo;
  private readonly ITechStackRepo _techStackRepo = techStackRepo;
  private readonly IModelPlanningRepo _modelPlanningRepo = modelPlanningRepo;
  private readonly IFileStructureRepo _fileStructureRepo = fileStructureRepo;
  private readonly IColorPaletteRepo _colorPaletteRepo = colorPaletteRepo;
  private readonly IStoryRepo _storyRepo = storyRepo;
  private readonly IBugReportRepo _bugReportRepo = bugReportRepo;

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

  public async Task<IActionResult> ProjectDash(int projId)
  {
    // -1 is arbitrary. It is used to let this method know that the user is coming from the nav menu and can access the projId cookie
    if (projId == -1)
    {
      projId = int.Parse(HttpContext.Request.Cookies["projId"]);
    }

    // use projId to load dash. If this fails, proj with this Id was likely deleted - kick user back to MyProjects
    var proj = await _projRepo.GetByIdAsync(projId);
    if (proj == null)
    {
      return RedirectToAction(Str.MyProjects, Str.Project);
    }

    //// generate some mock data
    //Random rnd = new Random();

    //for (int i = 0; i < 50; i++)
    //{
    //    int pointValue = rnd.Next(5);
    //    int dayCompleted = rnd.Next(40);

    //    Story story = new()
    //    {
    //        ProjId = proj.Id,
    //        DateCreated = DateTime.Now,
    //        Status = Str.Resolved,
    //        DateResolved = proj.StartDate.AddDays(dayCompleted),
    //        Description = "mock",
    //        Title = "mock",
    //        DueDate = proj.StartDate.AddDays(dayCompleted),
    //        Points = GetFibNum(pointValue)
    //    };

    //    _storyRepo.Add(story);
    //}
    //for (int i = 0; i < 50; i++)
    //{
    //    int pointValue = rnd.Next(5);
    //    int dayCompleted = rnd.Next(40);

    //    BugReport bugReport = new()
    //    {
    //        ProjId = proj.Id,
    //        DateCreated = DateTime.Now,
    //        Status = Str.Resolved,
    //        DateResolved = proj.StartDate.AddDays(dayCompleted),
    //        DueDate = proj.StartDate.AddDays(dayCompleted),
    //        AttemptedSolutions = "mock",
    //        Description = "mock",
    //        RecreateIssue = "mock",
    //        SuccessfulSolution = "mock",
    //        Priority = "low",
    //        Points = GetFibNum(pointValue)
    //    };

    //    _bugReportRepo.Add(bugReport);
    //}




    var unresolvedStories = await _storyRepo.GetAllUnresolvedStoriesWithSearchFilterAsync(projId, string.Empty);
    var unresolvedBugReports = await _bugReportRepo.GetAllUnresolvedReportsAsync(projId, string.Empty);

    var resolvedStories = await _storyRepo.GetAllResolved(projId);
    var resolvedBugReports = await _bugReportRepo.GetAllResolved(projId);

    ViewData[Str.CurrentProject] = proj;
    ViewData["PieChartData"] = new PieChartData(unresolvedStories.ToList(), unresolvedBugReports.ToList());
    ViewData["BarGraphData"] = new BarGraphData(proj, resolvedStories.ToList(), resolvedBugReports.ToList());

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
    // if the user's defaultProjId is already the id of the supplied project, we're removing the favorite (set it to 0), otherwise set it to the new project's Id
    user.DefaultProjId = (user.DefaultProjId == project.Id) ? 0 : project.Id;
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
      ProjId = projId,
      // html sanitation breaks when null FileStructureData is read
      FileStructureData = string.Empty
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






  private int GetFibNum(int num)
  {
    switch (num)
    {
      case 1: return 1;
      case 2: return 2;
      case 3: return 3;
      case 4: return 5;
      case 5: return 8;
      default: return 13;
    }
  }
}
