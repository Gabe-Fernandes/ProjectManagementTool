﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using PMT.Data.Models;
using PMT.Data.RepoInterfaces;
using PMT.Services;
using PMT.Services.Email;
using PMT.Views.Account;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace PMT.Controllers;

public class AccountController : Controller
{
  private readonly IAppUserRepo _appUserRepo;
  private readonly SignInManager<AppUser> _signInManager;
  private readonly UserManager<AppUser> _userManager;
  private readonly IMyEmailSender _emailSender;
  private readonly IUserEmailStore<AppUser> _emailStore;
  private readonly IUserStore<AppUser> _userStore;
  private readonly IWebHostEnvironment _webHostEnvironment;

  public AccountController(IAppUserRepo appUserRepo,
    SignInManager<AppUser> signInManager,
    UserManager<AppUser> userManager,
    IMyEmailSender emailSender,
    IUserStore<AppUser> userStore,
    IWebHostEnvironment webHostEnvironment)
  {
    _appUserRepo = appUserRepo;
    _signInManager = signInManager;
    _userManager = userManager;
    _emailSender = emailSender;
    _userStore = userStore;
    _emailStore = (IUserEmailStore<AppUser>)_userStore;
    _webHostEnvironment = webHostEnvironment;
  }



  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Logout()
  {
    await _signInManager.SignOutAsync();
    await HttpContext.SignOutAsync(Str.Cookie);
    return RedirectToAction(Str.Login, Str.Account, new { cleanLogin = true });
  }



  public IActionResult Login(bool cleanLogin = false, bool failedLogin = false)
  {
		if (failedLogin)
		{
			TempData[Str.Login] = Str.failed_login_attempt;
		}
		ViewData[Str.CleanLogin] = cleanLogin;
		return View();
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Login(AccountVM input)
  {
    if (ModelState.GetFieldValidationState("Email") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid &&
        ModelState.GetFieldValidationState("Password") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
    {
      var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, isPersistent: false, lockoutOnFailure: false);
      if (result.Succeeded)
      {
        var user = await _userManager.FindByEmailAsync(input.Email);
        await GenerateSecurityContextAsync(user, HttpContext);
        if (user.DefaultProjId == 0)
        {
          return RedirectToAction(Str.MyProjects, Str.Project, new { appUserId = user.Id });
        }
        else
        {
          return RedirectToAction(Str.ProjectDash, Str.Project, new { projId = user.DefaultProjId });
        }
      }
    }
    await _signInManager.SignOutAsync();
    await HttpContext.SignOutAsync(Str.Cookie);
    return RedirectToAction(Str.Login, Str.Account, new { cleanLogin = true, failedLogin = true });
  }



  public IActionResult Register()
  {
    return View();
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Register(AccountVM input)
  {
    if (ModelState.IsValid)
    {
      var user = Activator.CreateInstance<AppUser>();

      user.Firstname = input.FirstName;
      user.Lastname = input.LastName;
      user.StreetAddress = input.StreetAddress;
      user.City = input.City;
      user.State = input.State;
      user.PostalCode = input.PostalCode;
      user.Dob = input.Dob;
      user.PhoneNumber = input.CellPhone;
      user.Pfp = "/Icons/Pfp0.png";
      user.DefaultProjId = 0;

      await _userStore.SetUserNameAsync(user, input.Email, CancellationToken.None);
      await _emailStore.SetEmailAsync(user, input.Email, CancellationToken.None);
      var result = await _userManager.CreateAsync(user, input.Password);

      if (result.Succeeded)
      {
        var userId = await _userManager.GetUserIdAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = Url.Action(
            action: Str.ConfirmEmail,
            controller: Str.Account,
            values: new { confirmationCode = code, appUserId = userId },
            protocol: "https");

        try
        {
          string pathToTemplate = _webHostEnvironment.WebRootPath + Str.DirSeparator + "EmailTemplates" + Str.DirSeparator + "confEmail.html";
          string htmlBody = string.Empty;

          using (StreamReader sr = System.IO.File.OpenText(pathToTemplate))
          {
            htmlBody = sr.ReadToEnd();
          }

          string msg = string.Format(htmlBody, HtmlEncoder.Default.Encode(callbackUrl));

					await _emailSender.SendEmailAsync(input.Email, "Confirm your email", msg);
				}
        catch (Exception ex)
        {
					return RedirectToAction("Exception", "Error", new { exception = ex.Message });
				}

				TempData[Str.Register] = "Account Created";
        return View();
      }
      // error handle duplicate usernames
      foreach (var err in result.Errors)
      {
        if (err.Code == "DuplicateUserName")
        {
          TempData[Str.Register] = "DuplicateUserName";
        }
      }

    }
    return View();
  }



  public IActionResult RecoverPassword()
  {
    return View();
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> ResetPassword(AccountVM input)
  {
    if (ModelState.GetFieldValidationState("Email") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid &&
        ModelState.GetFieldValidationState("Password") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
    {
      var user = await _userManager.FindByEmailAsync(input.Email);
      if (user == null)
      {
        return View();
      }

      string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input.Code));
      var result = await _userManager.ResetPasswordAsync(user, decodedToken, input.Password);
      if (result.Succeeded)
      {
        await _signInManager.PasswordSignInAsync(user.Email, input.Password, isPersistent: false, lockoutOnFailure: false);
        await GenerateSecurityContextAsync(user, HttpContext);
        return RedirectToAction(Str.MyProjects, Str.Project, new { appUserId = user.Id });
      }
    }
    return View();
  }



  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> ForgotPassword(AccountVM input)
  {
    if (ModelState.GetFieldValidationState("Email") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
    {
      var user = await _userManager.FindByEmailAsync(input.Email);
      if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
      {
        TempData[Str.Login] = Str.recovery_email_sent;
        return RedirectToAction(Str.Login, Str.Account, new { cleanLogin = true });
      }

      var code = await _userManager.GeneratePasswordResetTokenAsync(user);
      code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
      var callbackUrl = Url.Action(
          action: Str.RecoverPassword,
          controller: Str.Account,
          values: new { resetPassCode = code },
          protocol: "https");

      try
      {
        string pathToTemplate = _webHostEnvironment.WebRootPath + Str.DirSeparator + "EmailTemplates" + Str.DirSeparator + "forgotPassEmail.html";
        string htmlBody = string.Empty;

        using (StreamReader sr = System.IO.File.OpenText(pathToTemplate))
        {
          htmlBody = sr.ReadToEnd();
        }

        string msg = string.Format(htmlBody, HtmlEncoder.Default.Encode(callbackUrl));

        await _emailSender.SendEmailAsync(input.Email, "Reset Password", msg);
			}
      catch (Exception ex)
      {
				return RedirectToAction("Exception", "Error", new { exception = ex.Message });
			}


			TempData[Str.Login] = Str.recovery_email_sent;
    }
    return RedirectToAction(Str.Login, Str.Account, new { cleanLogin = true });
  }



  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> ResendEmailConf(AccountVM input)
  {
    if (ModelState.GetFieldValidationState("Email") == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
    {
      var user = await _userManager.FindByEmailAsync(input.Email);
      if (user == null)
      {
        TempData[Str.Login] = Str.conf_email_sent;
        return RedirectToAction(Str.Login, Str.Account, new { cleanLogin = true });
      }

      var userId = await _userManager.GetUserIdAsync(user);
      var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
      var callbackUrl = Url.Action(
          action: Str.ConfirmEmail,
          controller: Str.Account,
          values: new { confirmationCode = code, appUserId = userId },
          protocol: "https");

      try
      {
        string pathToTemplate = _webHostEnvironment.WebRootPath + Str.DirSeparator + "EmailTemplates" + Str.DirSeparator + "confEmail.html";
        string htmlBody = string.Empty;

        using (StreamReader sr = System.IO.File.OpenText(pathToTemplate))
        {
          htmlBody = sr.ReadToEnd();
        }

        string msg = htmlBody.Replace("{0}", HtmlEncoder.Default.Encode(callbackUrl));

        await _emailSender.SendEmailAsync(input.Email, "Confirm your email", msg);
      }
      catch (Exception ex)
			{
        return RedirectToAction("Exception", "Error", new { exception = ex.Message});
      }

      TempData[Str.Login] = Str.conf_email_sent;
    }
    return RedirectToAction(Str.Login, Str.Account, new { cleanLogin = true });
  }



  [HttpGet]
  public async Task<IActionResult> ConfirmEmail(string confirmationCode, string appUserId)
  {
    var appUser = await _appUserRepo.GetByIdAsync(appUserId);

    confirmationCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmationCode));
    var result = await _userManager.ConfirmEmailAsync(appUser, confirmationCode);
    if (result.Succeeded)
    {
      await GenerateSecurityContextAsync(appUser, HttpContext);
      return RedirectToAction(Str.MyProjects, Str.Project);
    }
    else
    {
      return RedirectToAction(Str.Login, Str.Account);
    }
  }



  public async Task GenerateSecurityContextAsync(AppUser appUser, HttpContext context)
  {
    var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, appUser.Firstname),
            new Claim("LastName", appUser.Lastname),
            new Claim("Email", appUser.Email),
            new Claim("Id", appUser.Id),
        };

    var identity = new ClaimsIdentity(claims, Str.Cookie);
    ClaimsPrincipal principal = new(identity);
    await context.SignInAsync(Str.Cookie, principal);
  }
}
