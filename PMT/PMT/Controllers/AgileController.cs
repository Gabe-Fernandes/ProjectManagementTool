using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;
using PMT.Services;
using System.Security.Claims;

namespace PMT.Controllers;

[Authorize]
public class AgileController(IStoryRepo storyRepo, IAppUserRepo appUserRepo, IHttpContextAccessor contextAccessor) : Controller
{
  private readonly IStoryRepo _storyRepo = storyRepo;
  private readonly IAppUserRepo _appUserRepo = appUserRepo;
  private readonly IHttpContextAccessor _contextAccessor = contextAccessor;



  public async Task<IActionResult> MyStories(string filterString = "")
  {
    int projId = GetUser().CurrentProjId;
    ViewData[Str.Stories] = await _storyRepo.GetAllWithSearchFilterAsync(projId, filterString);
    return View();
  }



  public IActionResult NewStory()
  {
    return View();
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult NewStory(Story story)
  {
    if (ModelState.IsValid)
    {
      story.DateCreated = DateTime.Now;
      story.Status = Str.InProgress;
      story.ProjId = GetUser().CurrentProjId;
      _storyRepo.Add(story);
      return RedirectToAction(Str.MyStories, Str.Agile);
    }
    else
    {
      return View(story);
    }
  }



  public async Task<IActionResult> StoryDetails(int storyId)
  {
    Story story = await _storyRepo.GetByIdAsync(storyId);
    return View(story);
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult UpdateStoryDetails(Story story)
  {
    if (ModelState.IsValid)
    {
      story.DateResolved = (story.Status == Str.Resolved) ? DateTime.Now : DateTime.MinValue;
      _storyRepo.Update(story);
      return RedirectToAction(Str.MyStories, Str.Agile);
    }
    return RedirectToAction(Str.StoryDetails, Str.Agile, new { storyId = story.Id });
  }



  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> DeleteStory(Story storyToDel)
  {
    Story storyToDelete = await _storyRepo.GetByIdAsync(storyToDel.Id);
    _storyRepo.Delete(storyToDelete);
    return RedirectToAction(Str.MyStories, Str.Agile);
  }

  private AppUser GetUser()
  {
    string myId = _contextAccessor.HttpContext.User.FindFirstValue("Id");
    return _appUserRepo.GetById(myId);
  }
}
