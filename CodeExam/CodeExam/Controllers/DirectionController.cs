using CodeExam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeExam.Controllers
{
    [AuthAttribute]
    public class DirectionController : Controller
    {
        private CodeWarDbContext db = new CodeWarDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Intro()
        {
            return View();
        }

        public ActionResult Code()
        {
            return View();
        }

        public JsonResult GetAllTask(int page, int pageSize)
        {
            var listTasks = db.Tasks.OrderByDescending(p => p.TaskId).
                Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var count = db.Tasks.Count();
            return Json(new { listTasks, count}, JsonRequestBehavior.AllowGet);
        }
    }
}