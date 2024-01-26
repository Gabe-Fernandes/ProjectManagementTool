using Microsoft.AspNetCore.Mvc;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;
using PMT.Services;
using System.Security.Claims;

namespace PMT.Controllers;

public class EMSController(IAppUserRepo appUserRepo,
  IHttpContextAccessor contextAccessor,
  IProject_AppUserRepo paRepo) : Controller
{
  private readonly IAppUserRepo _appUserRepo = appUserRepo;
  private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
  private readonly IProject_AppUserRepo _paRepo = paRepo;



  public async Task<IActionResult> PersonalInfo(string appUserId)
  {
    var appUser = await _appUserRepo.GetByIdAsync(appUserId);
    return View(appUser);
  }



  public async Task<IActionResult> EditPersonalInfo(string appUserId)
  {
    var appUser = await _appUserRepo.GetByIdAsync(appUserId);
    return View(appUser);
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult EditPersonalInfo(AppUser appUserChanges)
  {
    return View();
  }



  public async Task<IActionResult> ManageTeam()
  {
    AppUser appUser = GetUser();
    int projId = int.Parse(HttpContext.Request.Cookies["projId"]);

    // kick user out if role is incorrect
    Project_AppUser pa = await _paRepo.GetByForeignKeysAsync(projId, appUser.Id);
    if (pa.Role != "ProjectManager")
    {
      return RedirectToAction(Str.MyProjects, Str.Project);
    }

    // get data
    List<Project_AppUser> paList = await _paRepo.GetAllWithProjId(projId);
    List<AppUser> team = [];
    List<bool> approvalStatus = [];
    for (int i = 0; i < paList.Count; i++)
    {
      AppUser teamMember = await _appUserRepo.GetByIdAsync(paList[i].AppUserId);
      team.Add(teamMember);
      approvalStatus.Add(paList[i].Approved);
    }
    ViewData["Team"] = team;
    ViewData["OwnId"] = appUser.Id;
    ViewData["ApprovalStatus"] = approvalStatus;

    return View();
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> ManageTeamRemove(string appUserIdToRemove)
  {
    int projId = int.Parse(HttpContext.Request.Cookies["projId"]);
    Project_AppUser paToDelete = await _paRepo.GetByForeignKeysAsync(projId, appUserIdToRemove);
    _paRepo.Delete(paToDelete);

    return RedirectToAction("ManageTeam", "EMS");
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> ManageTeamApprove(string appUserIdToApprove)
  {
    int projId = int.Parse(HttpContext.Request.Cookies["projId"]);
    Project_AppUser paToDelete = await _paRepo.GetByForeignKeysAsync(projId, appUserIdToApprove);
    paToDelete.Approved = true;
    _paRepo.Update(paToDelete);

    return RedirectToAction("ManageTeam", "EMS");
  }



  private AppUser GetUser()
  {
    string myId = _contextAccessor.HttpContext.User.FindFirstValue("Id");
    return _appUserRepo.GetById(myId);
  }
}
