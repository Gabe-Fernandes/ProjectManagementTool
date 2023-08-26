﻿using Microsoft.AspNetCore.Mvc;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;
using PMT.Services;

namespace PMT.Controllers;

public class AgileController : Controller
{
  private readonly IStoryRepo _storyRepo;

  public AgileController(IStoryRepo storyRepo)
  {
    _storyRepo = storyRepo;
  }

  public IActionResult AgileOutline()
  {
    return View();
  }

  public async Task<IActionResult> MyStories()
  {
    var test = await _storyRepo.GetAllWithSearchFilterAsync(2, filterString: "");
    int count = test.Count();
    ViewData[Str.Stories] = await _storyRepo.GetAllWithSearchFilterAsync(2, filterString: "");
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
  [HttpPost]
  [AutoValidateAntiforgeryToken]
  public IActionResult NewStory(Story story)
  {
    if (ModelState.IsValid)
    {
      story.DateCreated = DateTime.Now;
      story.Status = Str.InProgress;
      story.ProjId = 2; ////////////////////////////////////////////////////////////////////////////////////////////// remove hard-coded value
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
  [AutoValidateAntiforgeryToken]
  public IActionResult UpdateStoryDetails(Story story)
  {
    if (ModelState.IsValid)
    {
      if (story.Status == Str.Resolved)
      {
        story.DateResolved = DateTime.Now;
      }
      _storyRepo.Update(story);
    }
    return RedirectToAction(Str.StoryDetails, Str.Agile, new { storyId = story.Id });
  }

  public IActionResult Timeline()
  {
    return View();
  }

  [HttpPost]
  [AutoValidateAntiforgeryToken]
  public async Task<IActionResult> DeleteStory(Story storyToDel)
  {
    Story storyToDelete = await _storyRepo.GetByIdAsync(storyToDel.Id);
    _storyRepo.Delete(storyToDelete);
    return RedirectToAction(Str.MyStories, Str.Agile);
  }
}
