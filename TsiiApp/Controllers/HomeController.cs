using System;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using TsiiApp.ViewModels;

namespace TsiiApp.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			string pathToChartsFolder = AppDomain.CurrentDomain.BaseDirectory+"Charts\\";
			var filenames = Directory.GetFiles(pathToChartsFolder, "*.*").Select(Path.GetFileNameWithoutExtension).ToList();
			var homeViewModel = new HomeViewModel
			{
				Filenames = filenames
			};

			return View(homeViewModel);
		}

		[HttpGet]
		public ActionResult AddNewFile()
		{
			return View();
		}

		[HttpGet]
		public ActionResult DisplayChart(string filename)
		{
			return View();
		}
	}
}