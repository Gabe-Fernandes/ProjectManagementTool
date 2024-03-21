using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;
using PMT.Services;
using System.Security.Claims;

namespace PMT.Controllers;

[Authorize]
public class BugTrackerController(IBugReportRepo bugReportRepo, IAppUserRepo appUserRepo, IHttpContextAccessor contextAccessor) : Controller
{
  private readonly IBugReportRepo _bugReportRepo = bugReportRepo;
  private readonly IAppUserRepo _appUserRepo = appUserRepo;
  private readonly IHttpContextAccessor _contextAccessor = contextAccessor;



  public async Task<IActionResult> BugTracking(string filterString = "")
  {
    int projId = GetUser().CurrentProjId;
    ViewData[Str.BugReports] = await _bugReportRepo.GetAllAsync(projId, filterString);
    return View();
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult DeleteBugReport(BugReport bugReportToDelete)
  {
    _bugReportRepo.Delete(bugReportToDelete);
    return RedirectToAction(Str.BugTracking, Str.BugTracker);
  }



  public IActionResult CreateBugReport()
  {
    return View();
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult CreateBugReport(BugReport bugReport)
  {
    if (ModelState.IsValid)
    {
      bugReport.ProjId = GetUser().CurrentProjId;
      bugReport.Status = Str.InProgress;
      bugReport.DateCreated = DateTime.Now;
      _bugReportRepo.Add(bugReport);
    }
    else { return View(); }

    return RedirectToAction(Str.BugTracking, Str.BugTracker);
  }



  public async Task<IActionResult> EditBugReport(int bugReportId)
  {
    BugReport bugReport = await _bugReportRepo.GetByIdAsync(bugReportId);
    return View(bugReport);
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult EditBugReport(BugReport bugReport)
  {
    if (ModelState.IsValid)
    {
      bugReport.DateResolved = (bugReport.Status == Str.Resolved) ? DateTime.Now : DateTime.MinValue;
      _bugReportRepo.Update(bugReport);
      return RedirectToAction(Str.BugTracking, Str.BugTracker);
    }
    return RedirectToAction(Str.EditBugReport, Str.BugTracker, new { bugReportId = bugReport.Id });
  }

  private AppUser GetUser()
  {
    string myId = _contextAccessor.HttpContext.User.FindFirstValue("Id");
    return _appUserRepo.GetById(myId);
  }
}
