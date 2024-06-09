using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers
{
	public class HomeController : Controller
	{
		[Route("Error")]
		public IActionResult Error()
		{
			IExceptionHandlerPathFeature? exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>(); // the way to get a information about current information inside the application

			if(exceptionHandlerPathFeature is not null && exceptionHandlerPathFeature.Error is not null)
			{
				ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
			}
			return View(); //Views/Shared/Error
		}
	}
}
