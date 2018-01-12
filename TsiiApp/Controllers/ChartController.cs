using System;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using System.Web.Script.Serialization;
using TsiiApp.ViewModels;
using TsiiApp.Services;

namespace TsiiApp.Controllers
{
	public class ChartController : Controller
	{

		[HttpGet]
		public ActionResult DisplayChart(String fileName){
			
			var chartViewModel = new ChartViewModel
			{
				ChartName = fileName
			}
			
			return new View("~/Views/Home/DisplayChart.cshtml", chartViewModel);
			
		}
		
		[HttpGet]
		public async String GetChartDataAsJson(String fileName, char fileSeparator = ';', boolean hasFileLabels = true){
			
			var chartService = new ChartService();
			return chartService.ChartDataToJson(chartService.GetChartData(fileName, fileSeparator), hasFileLabels);
			
		}

	}
}