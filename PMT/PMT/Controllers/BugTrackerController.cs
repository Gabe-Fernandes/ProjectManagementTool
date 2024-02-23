using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;
using PMT.Services;

namespace PMT.Controllers;

[Authorize]
public class BugTrackerController(IBugReportRepo bugReportRepo) : Controller
{
  private readonly IBugReportRepo _bugReportRepo = bugReportRepo;



	public async Task<IActionResult> BugTracking(string filterString = "")
  {
    int projId = int.Parse(HttpContext.Request.Cookies["projId"]);
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
      bugReport.ProjId = int.Parse(HttpContext.Request.Cookies["projId"]);
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
    return RedirectToAction(Str.EditBugReport, Str.BugTracker, new { bugReportId = bugReport.Id} );
  }
}
