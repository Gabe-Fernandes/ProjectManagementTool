using Microsoft.AspNetCore.Mvc;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;
using PMT.Services;

namespace PMT.Controllers;

public class BugTrackerController : Controller
{
  private readonly IBugReportRepo _bugReportRepo;

  public BugTrackerController(IBugReportRepo bugReportRepo)
  {
      _bugReportRepo = bugReportRepo;
  }

  public async Task<IActionResult> BugTracking()
  {
      int projId = int.Parse(HttpContext.Request.Cookies["projId"]);
      ViewData[Str.BugReports] = await _bugReportRepo.GetAllAsync(projId);
      return View();
  }
  [HttpPost]
  [AutoValidateAntiforgeryToken]
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
  [AutoValidateAntiforgeryToken]
  public IActionResult CreateBugReport(BugReport bugReport)
  {
    if (ModelState.IsValid)
    {
      bugReport.ProjId = int.Parse(HttpContext.Request.Cookies["projId"]);
      bugReport.Status = Str.InProgress;
      bugReport.DateCreated = DateTime.Now;
      _bugReportRepo.Add(bugReport);
    }
    return RedirectToAction(Str.BugTracking, Str.BugTracker);
  }

  public async Task<IActionResult> EditBugReport(int bugReportId)
  {
    BugReport bugReport = await _bugReportRepo.GetByIdAsync(bugReportId);
    return View(bugReport);
  }
  [HttpPost]
  [AutoValidateAntiforgeryToken]
  public IActionResult EditBugReport(BugReport bugReport)
  {
    if (ModelState.IsValid)
    {
      bugReport.DateResolved = (bugReport.Status == Str.Resolved) ? DateTime.Now : DateTime.MinValue;
      _bugReportRepo.Update(bugReport);
      return RedirectToAction(Str.BugTracking, Str.BugTracker);
    }
    return RedirectToAction(Str.EditBugReport, Str.BugTracker, new { bugReportId = bugReport.Id} );
  }
}
