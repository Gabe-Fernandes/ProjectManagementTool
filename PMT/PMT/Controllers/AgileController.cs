using Microsoft.AspNetCore.Mvc;
using PMT.Data.Models;
using PMT.Services;

namespace PMT.Controllers;

public class AgileController : Controller
{
  public IActionResult AgileOutline()
  {
    return View();
  }

  public IActionResult MyStories()
  {
    return View();
  }
  [HttpPost]
  [AutoValidateAntiforgeryToken]
  public IActionResult MyStories(Story story)
  {
    return View();
  }

  public IActionResult NewStory()
  {
    return View();
  }

  public IActionResult StoryDetails()
  {
    return View();
  }
  [HttpPost]
  [AutoValidateAntiforgeryToken]
  public IActionResult StoryDetails(Story story)
  {
    return View();
  }

  public IActionResult Timeline()
  {
    return View();
  }

  [HttpPost]
  [AutoValidateAntiforgeryToken]
  public IActionResult DeleteStory(Story storyToDelete, string redirectLocation)
  {
    return RedirectToAction(Str.Agile, redirectLocation);
  }
}
