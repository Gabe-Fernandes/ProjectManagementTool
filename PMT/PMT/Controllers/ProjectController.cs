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
  IBugReportRepo bugReportRepo,
  IProject_AppUserRepo paRepo) : Controller
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
  private readonly IProject_AppUserRepo _paRepo = paRepo;



  public async Task<IActionResult> MyProjects(string joinProjErrMsg = "")
  {
    ViewData["Join_Proj_Err_Msg"] = joinProjErrMsg;

    AppUser appUser = GetUser();
    List<Project> projects = await _projRepo.GetAllFromUserAsync(appUser.Id) as List<Project>;
    ViewData[Str.Projects] = projects;
    ViewData["ProjRoles"] = await GetProjRoles(projects, appUser.Id);

    // confirm user is part of proj from defaultProjId and reset if not
    Project_AppUser pa = await _paRepo.GetByForeignKeysAsync(appUser.DefaultProjId, appUser.Id);
    if (pa == null)
    {
      appUser.DefaultProjId = 0;
      _appUserRepo.Update(appUser);
    }
    ViewData["defaultProjId"] = appUser.DefaultProjId;
    return View();
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> NewProject(Project project)
  {
    if (ModelState.IsValid && project.StartDate < project.DueDate)
    {
      // Create project record
      project.JoinCode = await UniqueProjectCodeAsync();
      project.Id = 0;
      _projRepo.Add(project);

      // Create association record between project and the appUser creating it
      AppUser user = GetUser();
      Project_AppUser pa = new()
      {
        ProjId = project.Id,
        AppUserId = user.Id,
        Approved = true,
        Role = "ProjectManager"
      };
      _paRepo.Add(pa);

      // Create SRS records
      InitializeSRS(project.Id);
    }
    return RedirectToAction(Str.MyProjects, Str.Project);
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> EditProject(Project project)
  {
    if (ModelState.IsValid && project.StartDate < project.DueDate)
    {
      Project projToEdit = await _projRepo.GetByIdAsync(project.Id);
      projToEdit.DueDate = project.DueDate;
      projToEdit.StartDate = project.StartDate;
      projToEdit.Name = project.Name;
      _projRepo.Update(projToEdit);
    }
    return RedirectToAction(Str.MyProjects, Str.Project);
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> JoinProject(string joinCode)
  {
    string joinProjErrMsg = string.Empty;
    Project projToJoin = await _projRepo.GetDuplicateProject(joinCode);

    if (projToJoin == null)
    {
      // error handling when the join code is wrong
      joinProjErrMsg = "Incorrect join code.";
      return RedirectToAction(Str.MyProjects, Str.Project, new { joinProjErrMsg });
    }

    AppUser appUser = GetUser();
    Project_AppUser pa = await _paRepo.GetByForeignKeysAsync(projToJoin.Id, appUser.Id);
    if (pa == null)
    {
      Project_AppUser createPa = new()
      {
        ProjId = projToJoin.Id,
        AppUserId = appUser.Id,
        Approved = false,
        Role = "Developer"
      };
      _paRepo.Add(createPa);

      // case where the join code is correct and the project is joinable
      return RedirectToAction(Str.MyProjects, Str.Project);
    }

    // error handling when the user is already part of this project
    joinProjErrMsg = "You are alraedy part of this project.";
    return RedirectToAction(Str.MyProjects, Str.Project, new { joinProjErrMsg });
  }



  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> LeaveProject(int projIdToLeave)
  {
    AppUser appUser = GetUser();
    Project_AppUser paToDelete = await _paRepo.GetByForeignKeysAsync(projIdToLeave, appUser.Id);
    _paRepo.Delete(paToDelete);
    return RedirectToAction(Str.MyProjects, Str.Project);
  }



  public async Task<IActionResult> DeleteProject(int projIdToDelete)
  {
    var projToDelete = await _projRepo.GetByIdAsync(projIdToDelete);
    return View(projToDelete);
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
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
    List<Project_AppUser> paList = await _paRepo.GetAllWithProjId(projToDelete.Id);

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
    for (int i = 0; i < paList.Count; i++)
    {
      _paRepo.Delete(paList[i]);
    }

    _projRepo.Delete(projToDelete);
    return RedirectToAction(Str.MyProjects, Str.Project);
  }



  public async Task<IActionResult> ProjectDash(int projId)
  {
		AppUser user = GetUser();

		// -1 is arbitrary. It is used to let this method know that the user is coming from the nav menu and can access the projId property
		if (projId == -1)
    {
      projId = user.CurrentProjId;
    }

    // use projId to load dash. If this fails, proj with this Id was likely deleted - kick user back to MyProjects
    var proj = await _projRepo.GetByIdAsync(projId);
    if (proj == null)
    {
      return RedirectToAction(Str.MyProjects, Str.Project);
    }

    var unresolvedStories = await _storyRepo.GetAllUnresolvedStoriesWithSearchFilterAsync(projId, string.Empty);
    var unresolvedBugReports = await _bugReportRepo.GetAllUnresolvedReportsAsync(projId, string.Empty);

    var resolvedStories = await _storyRepo.GetAllResolved(projId);
    var resolvedBugReports = await _bugReportRepo.GetAllResolved(projId);

    ViewData[Str.CurrentProject] = proj;
    ViewData["PieChartData"] = new PieChartData(unresolvedStories.ToList(), unresolvedBugReports.ToList());
    ViewData["BarGraphData"] = new BarGraphData(proj, resolvedStories.ToList(), resolvedBugReports.ToList());

    // set current projId
    user.CurrentProjId = projId;
    _appUserRepo.Update(user);

    return View();
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
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



  private async Task<string> UniqueProjectCodeAsync()
  {
    string projectCode = GenerateProjectCode();
    Project duplicateProj = await _projRepo.GetDuplicateProject(projectCode);
    if (duplicateProj != null) { await UniqueProjectCodeAsync(); }

    return projectCode;
  }
  private static string GenerateProjectCode()
  {
    string charArr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    Random rnd = new();

    string projectCode = "";
    for (int i = 0; i < 6; i++)
    {
      int rndIndex = rnd.Next(36);
      projectCode += charArr[rndIndex];
    }

    return projectCode;
  }

  private AppUser GetUser()
  {
    string myId = _contextAccessor.HttpContext.User.FindFirstValue("Id");
    return _appUserRepo.GetById(myId);
  }



  private async Task<List<string>> GetProjRoles(List<Project> projects, string appUserId)
  {
    List<string> projRoles = [];

    for (int i = 0; i < projects.Count; i++)
    {
      Project_AppUser pa = await _paRepo.GetByForeignKeysAsync(projects[i].Id, appUserId);
      projRoles.Add(pa.Role);
    }

    return projRoles;
  }
}
