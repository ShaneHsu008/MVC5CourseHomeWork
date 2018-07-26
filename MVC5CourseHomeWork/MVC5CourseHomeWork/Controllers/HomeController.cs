using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5CourseHomeWork.Controllers
{
    [ActionTimer]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult GetJson()
        {
            return Json(new { id=0,name="jay",say="Hi you see me."},JsonRequestBehavior.AllowGet);
        }
    }
}