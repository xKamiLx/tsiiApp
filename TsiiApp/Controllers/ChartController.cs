using System;
using System.Web.Mvc;
using TsiiApp.ViewModels;
using TsiiApp.Services;

namespace TsiiApp.Controllers
{
	public class ChartController : Controller
	{
		[HttpGet]
		public ActionResult DisplayChart(String fileName)
		{
			var chartViewModel = new ChartViewModel
			{
				ChartName = fileName
			};

			return View("~/Views/Home/DisplayChart.cshtml", chartViewModel);
		}

		[HttpGet]
		public String GetChartDataAsJson(String fileName, char fileSeparator = ';', bool hasFileLabels = true)
		{
			var chartService = new ChartService();
			string userName = User.Identity.IsAuthenticated ? User.Identity.Name : "DefaultUser";

			return chartService.ChartDataToJson(chartService.GetChartDataFromFile(userName, fileName, fileSeparator), hasFileLabels);
		}
	}
}