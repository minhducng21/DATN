using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeExam.Areas.Admin.Controllers
{
    public class SidebarController : Controller
    {
        // GET: Areas/Sidebar
        public ActionResult Index()
        {
            return View();
        }
    }
}