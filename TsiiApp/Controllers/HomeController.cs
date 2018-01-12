using System;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using TsiiApp.ViewModels;
using TsiiApp.Services;

namespace TsiiApp.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			
			var service = new ChartService();
			var homeViewModel = service.GetChartsFileNames();
			
			return View(homeViewModel);
			
		}

		[HttpGet]
		public ActionResult AddNewFile()
		{
			return View();
		}

	}
}