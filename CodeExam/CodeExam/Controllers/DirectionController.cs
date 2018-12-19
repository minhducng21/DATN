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

        public ActionResult Code(int id)
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

        public JsonResult GetTaskById(int id)
        {
            var task = db.Tasks.FirstOrDefault(d => d.TaskId == id);

            var listTestCases = GetTestCaseByTaskId(id);

            return Json(new { task, listTestCases}, JsonRequestBehavior.AllowGet);
        }

        public List<TestCase> GetTestCaseByTaskId(int id)
        {
            var listTestCases = db.TestCases.Where(t => t.TaskId.Equals(id)).ToList();
            return listTestCases;
        }
    }
}