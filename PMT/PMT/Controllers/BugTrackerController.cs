using Microsoft.AspNetCore.Mvc;

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
}
