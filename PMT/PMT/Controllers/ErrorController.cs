using Microsoft.AspNetCore.Mvc; 
 
namespace PMT.Controllers; 
 
public class ErrorController : Controller 
{ 
	// PMT Landmark
	public IActionResult AccessDenied()
	{
		return View();
	}

	public IActionResult Exception(string exception = "")
	{
		ViewData["exception"] = exception;
		return View();
	}
} 
