using System;
using System.IO;
using System.Web.Mvc;
using System.Web;
using TsiiApp.Services;
using TsiiApp.ViewModels;

namespace TsiiApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var service = new ChartService();
	        var homeViewModel = User.Identity.IsAuthenticated ? service.GetChartsFileNames(User.Identity.Name) : new HomeViewModel();

            return View(homeViewModel);
        }

        [HttpGet]
        public ActionResult AddNewFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Charts"), User.Identity.Name);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    path = Path.Combine(path, Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message;
                }
            }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }

            return View();
        }
    }
}