using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;
using PMT.Services;

namespace PMT.Controllers;

[Authorize]
public class SRSController : Controller
{
  private readonly ISRSRepo _SRSRepo;
  private readonly ITechStackRepo _techStackRepo;
  private readonly IModelPlanningRepo _modelPlanningRepo;
  private readonly IFileStructureRepo _fileStructureRepo;
  private readonly IColorPaletteRepo _colorPaletteRepo;

  public SRSController(ISRSRepo sRSRepo,
    ITechStackRepo techStackRepo,
    IModelPlanningRepo modelPlanningRepo,
    IFileStructureRepo fileStructureRepo,
    IColorPaletteRepo colorPaletteRepo)
  {
    _SRSRepo = sRSRepo;
    _techStackRepo = techStackRepo;
    _modelPlanningRepo = modelPlanningRepo;
    _fileStructureRepo = fileStructureRepo;
    _colorPaletteRepo = colorPaletteRepo;
  }

  public async Task<IActionResult> ColorPalette()
  {
    int projId = int.Parse(HttpContext.Request.Cookies["projId"]);
    ColorPalette colorPalette = await _colorPaletteRepo.GetByProjectIdAsync(projId);
    return View(colorPalette);
  }
  [HttpPost]
  [AutoValidateAntiforgeryToken]
  public IActionResult ColorPalette(ColorPalette colorPalette)
  {
    if (ModelState.IsValid)
    {
      _colorPaletteRepo.Update(colorPalette);
      return RedirectToAction(Str.SRS, Str.SRS);
    }
    return View();
  }

  public async Task<IActionResult> FileStructure()
  {
    int projId = int.Parse(HttpContext.Request.Cookies["projId"]);
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
  [AutoValidateAntiforgeryToken]
  public IActionResult FileStructure(FileStructure fileStructure)
  {
    if (ModelState.IsValid)
    {
      _fileStructureRepo.Update(fileStructure);
      return RedirectToAction(Str.SRS, Str.SRS);
    }
    return View();
  }

  public async Task<IActionResult> ModelsAndValidation()
  {
    int projId = int.Parse(HttpContext.Request.Cookies["projId"]);
    ModelPlanning modelPlanning = await _modelPlanningRepo.GetByProjectIdAsync(projId);
    return View(modelPlanning);
  }
  [HttpPost]
  [AutoValidateAntiforgeryToken]
  public IActionResult ModelsAndValidation(ModelPlanning modelPlanning)
  {
    if (ModelState.IsValid)
    {
      _modelPlanningRepo.Update(modelPlanning);
      return RedirectToAction(Str.SRS, Str.SRS);
    }
    return View();
  }

  public async Task<IActionResult> TechStack()
  {
    int projId = int.Parse(HttpContext.Request.Cookies["projId"]);
    TechStack techstack = await _techStackRepo.GetByProjectIdAsync(projId);
    return View(techstack);
  }
  [HttpPost]
  [AutoValidateAntiforgeryToken]
  public IActionResult TechStack(TechStack techStack)
  {
    if (ModelState.IsValid)
    {
      _techStackRepo.Update(techStack);
      return RedirectToAction(Str.SRS, Str.SRS);
    }
    return View();
  }

  public async Task<IActionResult> SRS()
  {
    int projId = int.Parse(HttpContext.Request.Cookies["projId"]);
    SRS SRS = await _SRSRepo.GetByProjectIdAsync(projId);
    ViewData[Str.TechStack] = await _techStackRepo.GetByProjectIdAsync(projId);
    ViewData[Str.ColorPalette] = await _colorPaletteRepo.GetByProjectIdAsync(projId);
    ViewData[Str.FileStructure] = await _fileStructureRepo.GetByProjectIdAsync(projId);
    ViewData[Str.ModelsAndValidation] = await _modelPlanningRepo.GetByProjectIdAsync(projId);
    return View(SRS);
  }
  [HttpPost]
  [AutoValidateAntiforgeryToken]
  public IActionResult SRS(SRS SRS)
  {
    if (ModelState.IsValid)
    {
      _SRSRepo.Update(SRS);
    }
    return RedirectToAction(Str.SRS, Str.SRS);
  }
}
