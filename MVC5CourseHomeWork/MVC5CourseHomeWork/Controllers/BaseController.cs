using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5CourseHomeWork.Controllers
{
    [Authorize(Roles ="Admin")]
    [ActionTimer]
    public class BaseController : Controller
    {
    }
}