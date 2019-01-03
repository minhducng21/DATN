using CodeExam.Models;
using CodeExam.ViewModels;
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
            var listTasks = db.Tasks.Where(w=>w.TaskStatus == Constant.Status.Active).OrderByDescending(p => p.TaskId).
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

        public JsonResult GetLeaderByTaskId(int taskId)
        {
            var results = db.LeaderBoards.Where(d => d.TaskId.Equals(taskId)).ToList();
            List<LeaderBoardViewModel> leaders = new List<LeaderBoardViewModel>();
            foreach (var item in results)
            {
                LeaderBoardViewModel leader = new LeaderBoardViewModel();
                leader.User = db.Users.FirstOrDefault(u => u.UserId.Equals(item.UserId));
                leader.Point = item.Point;
                leader.Language = db.LanguagePrograms.FirstOrDefault(d => d.LanguageId.Equals(item.LanguageId));
                leader.SourceCode = item.SourceCode;

                leaders.Add(leader);
            }
            return Json(leaders.OrderByDescending(p => p.Point), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllLanguage()
        {
            return Json(db.LanguagePrograms.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLeaderByLanguageId(int languageId, int taskId)
        {
            var results = db.LeaderBoards.Where(d => d.TaskId.Equals(taskId) && d.LanguageId.Equals(languageId)).ToList();
            if (languageId == 3)
            {
                results = db.LeaderBoards.Where(d => d.TaskId.Equals(taskId)).ToList();
            }
            List<LeaderBoardViewModel> leaders = new List<LeaderBoardViewModel>();
            foreach (var item in results)
            {
                LeaderBoardViewModel leader = new LeaderBoardViewModel();
                leader.User = db.Users.FirstOrDefault(u => u.UserId.Equals(item.UserId));
                leader.Point = item.Point;
                leader.Language = db.LanguagePrograms.FirstOrDefault(d => d.LanguageId.Equals(item.LanguageId));
                leader.SourceCode = item.SourceCode;

                leaders.Add(leader);
            }
            return Json(leaders.OrderByDescending(p => p.Point), JsonRequestBehavior.AllowGet);
        }
    }
}