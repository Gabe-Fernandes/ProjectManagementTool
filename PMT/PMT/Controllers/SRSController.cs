using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;
using PMT.Services;
using System.Security.Claims;

namespace PMT.Controllers;

[Authorize]
public class SRSController(ISRSRepo sRSRepo,
  ITechStackRepo techStackRepo,
  IModelPlanningRepo modelPlanningRepo,
  IFileStructureRepo fileStructureRepo,
  IColorPaletteRepo colorPaletteRepo,
  IHttpContextAccessor contextAccessor,
  IAppUserRepo appUserRepo) : Controller
{
  private readonly ISRSRepo _SRSRepo = sRSRepo;
  private readonly ITechStackRepo _techStackRepo = techStackRepo;
  private readonly IModelPlanningRepo _modelPlanningRepo = modelPlanningRepo;
  private readonly IFileStructureRepo _fileStructureRepo = fileStructureRepo;
  private readonly IColorPaletteRepo _colorPaletteRepo = colorPaletteRepo;
  private readonly IAppUserRepo _appUserRepo = appUserRepo;
  private readonly IHttpContextAccessor _contextAccessor = contextAccessor;



  public async Task<IActionResult> ColorPalette()
  {
    int projId = GetUser().CurrentProjId;
    ColorPalette colorPalette = await _colorPaletteRepo.GetByProjectIdAsync(projId);
    return View(colorPalette);
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult ColorPalette(ColorPalette colorPalette)
  {
    if (ModelState.IsValid)
    {
      _colorPaletteRepo.Update(colorPalette);
      return RedirectToAction(Str.SRS, Str.SRS, new { jumpTarget = "#colorPaletteTarget" });
    }
    return View();
  }



  public async Task<IActionResult> FileStructure()
  {
    int projId = GetUser().CurrentProjId;
    FileStructure fileStructure = await _fileStructureRepo.GetByProjectIdAsync(projId);

    // sanitize html data (consider abstracting this in a service, but leave in controller for now)
    HtmlSanitizer sanitizer = new();
    sanitizer.AllowedAttributes.Add("class");
    sanitizer.AllowedAttributes.Add("id");
    string[] allowedClasses = { "fs-item", "dir", "file", "root", "dir-content", "dir-container", "dir-btn" };
    for (int i = 0; i < allowedClasses.Length; i++)
    {
      sanitizer.AllowedClasses.Add(allowedClasses[i]);
    }
    string sanitized = sanitizer.Sanitize(fileStructure.FileStructureData);
    sanitized = Str.ReformatSanitizedHtml(sanitized);
    if (sanitized != fileStructure.FileStructureData)
    {
      // handle possible XSS attack
      return RedirectToAction(Str.Login, Str.Account);
    }
    else
    {
      return View(fileStructure);
    }
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult FileStructureSave(FileStructure fileStructure)
  {
    if (ModelState.IsValid)
    {
      _fileStructureRepo.Update(fileStructure);
      return RedirectToAction(Str.FileStructure, Str.SRS);
    }
    return RedirectToAction(Str.FileStructure, Str.SRS);
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult FileStructureSubmit(FileStructure fileStructure)
  {
    if (ModelState.IsValid)
    {
      _fileStructureRepo.Update(fileStructure);
      return RedirectToAction(Str.SRS, Str.SRS, new { jumpTarget = "#fileStructureTarget" });
    }
    return RedirectToAction(Str.FileStructure, Str.SRS);
  }



  public async Task<IActionResult> ModelsAndValidation()
  {
    int projId = GetUser().CurrentProjId;
    ModelPlanning modelPlanning = await _modelPlanningRepo.GetByProjectIdAsync(projId);
    return View(modelPlanning);
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult ModelsAndValidationSave(ModelPlanning modelPlanning)
  {
    if (ModelState.IsValid)
    {
      _modelPlanningRepo.Update(modelPlanning);
      return RedirectToAction(Str.ModelsAndValidation, Str.SRS);
    }
    return RedirectToAction(Str.ModelsAndValidation, Str.SRS);
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult ModelsAndValidationSubmit(ModelPlanning modelPlanning)
  {
    if (ModelState.IsValid)
    {
      _modelPlanningRepo.Update(modelPlanning);
      return RedirectToAction(Str.SRS, Str.SRS, new { jumpTarget = "#modelsAndValidationTarget" });
    }
    return RedirectToAction(Str.ModelsAndValidation, Str.SRS);
  }



  public async Task<IActionResult> TechStack()
  {
    int projId = GetUser().CurrentProjId;
    TechStack techstack = await _techStackRepo.GetByProjectIdAsync(projId);
    return View(techstack);
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult TechStack(TechStack techStack)
  {
    if (ModelState.IsValid)
    {
      _techStackRepo.Update(techStack);
      return RedirectToAction(Str.SRS, Str.SRS, new { jumpTarget = "#techStackTarget" });
    }
    return View();
  }



  public async Task<IActionResult> SRS(string jumpTarget = "")
  {
    int projId = GetUser().CurrentProjId;
    SRS SRS = await _SRSRepo.GetByProjectIdAsync(projId);
    ViewData[Str.TechStack] = await _techStackRepo.GetByProjectIdAsync(projId);
    ViewData[Str.ColorPalette] = await _colorPaletteRepo.GetByProjectIdAsync(projId);
    ViewData[Str.FileStructure] = await _fileStructureRepo.GetByProjectIdAsync(projId);
    ViewData[Str.ModelsAndValidation] = await _modelPlanningRepo.GetByProjectIdAsync(projId);
    ViewData["JumpTarget"] = jumpTarget;
    return View(SRS);
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult SRS(SRS SRS)
  {
    if (ModelState.IsValid)
    {
      _SRSRepo.Update(SRS);
    }
    return RedirectToAction(Str.SRS, Str.SRS);
  }

  private AppUser GetUser()
  {
    string myId = _contextAccessor.HttpContext.User.FindFirstValue("Id");
    return _appUserRepo.GetById(myId);
  }
}
