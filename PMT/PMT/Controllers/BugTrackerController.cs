using Microsoft.AspNetCore.Mvc;
using PMT.Data.Models;
using PMT.Services;

namespace PMT.Controllers;

public class BugTrackerController : Controller
{
  public IActionResult BugTracking()
  {
    return View();
  }

  public IActionResult CreateBugReport()
  {
    return View();
  }

  public IActionResult EditBugReport()
  {
    return View();
  }

  [HttpPost]
  [AutoValidateAntiforgeryToken]
  public IActionResult DeleteBugReport(BugReport bugReportToDelete)
  {
    return RedirectToAction(Str.BugTracking, Str.BugTracker);
  }
}
