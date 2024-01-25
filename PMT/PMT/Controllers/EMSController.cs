using Microsoft.AspNetCore.Mvc;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;

namespace PMT.Controllers;

public class EMSController(IAppUserRepo appUserRepo) : Controller
{
  private readonly IAppUserRepo _appUserRepo = appUserRepo;



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



  public IActionResult ManageTeam()
  {
    return View();
  }
}
