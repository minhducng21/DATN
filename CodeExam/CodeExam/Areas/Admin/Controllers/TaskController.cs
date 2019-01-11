using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeExam.Models;

namespace CodeExam.Areas.Admin.Controllers

{
    [AuthAttribute]
    public class TaskController : Controller
    {
        CodeWarDbContext db = new CodeWarDbContext();
        // GET: Areas/Task
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetAll(int page, int pageSize)
        {
            var tasks = db.Tasks.Where(t => t.TaskStatus == Constant.Status.Active);
            var count = tasks.Count();
            var results = tasks.OrderByDescending(d => d.TaskId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(new { results, count }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreateTask(Task task)
        {
            task.TaskStatus = Constant.Status.Active;
            db.Tasks.Add(task);
            db.SaveChanges();
            var taskId = db.Tasks.OrderByDescending(f => f.TaskId).ToList()[0].TaskId;
            return Json(taskId, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(Task task)
        {
            var item = db.Tasks.FirstOrDefault(f => f.TaskId == task.TaskId);
            if (item != null)
            {
                item.Input = task.Input;
                item.OutputType = task.OutputType;
                item.Point = task.Point;
                item.TaskDescription = task.TaskDescription;
                item.TaskLevel = task.TaskLevel;
                item.TaskName = task.TaskName;
            }
            db.SaveChanges();
            return Json(task.TaskId, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaskById(int id)
        {
            var tests = db.TestCases.Where(t => t.TaskId == id).ToList();
            var task = db.Tasks.FirstOrDefault(f => f.TaskId == id);
            return Json(new { task, tests }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            var itemToDel = db.Tasks.FirstOrDefault(f => f.TaskId == id);
            if (itemToDel != null)
            {
                itemToDel.TaskStatus = Constant.Status.Deactive;
                var lstLeaders = db.LeaderBoards.Where(t => t.TaskId == id).ToList();
                lstLeaders.ForEach(d => db.LeaderBoards.Remove(d));
                db.SaveChanges();
                if (db.SaveChanges() == 1)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLastestTestCaseId()
        {
            var tests = db.TestCases.OrderByDescending(t => t.TestCaseId).ToList();
            int id = 1;
            if (tests.Count > 0)
            {
                id = tests[0].TestCaseId + 1;
            }
            return Json(id, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateTestCase(List<TestCase> tests)
        {
            for (int i = 0; i < tests.Count; i++)
            {
                TestCase test = new TestCase
                {
                    Input = tests[i].Input,
                    Output = tests[i].Output,
                    TaskId = tests[i].TaskId
                };
                db.TestCases.Add(test);
            }
            db.SaveChanges();
            return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditTestCase(List<TestCase> tests)
        {
            var taskId = tests.FirstOrDefault().TaskId;
            var lastTests = db.TestCases.Where(t => t.TaskId == taskId).ToList();
            lastTests.ForEach(t => db.TestCases.Remove(t));
            tests.ForEach(t => t.TaskId = taskId);
            foreach (var item in tests)
            {
                db.TestCases.Add(item);
                db.SaveChanges();
            }
            //tests.ForEach(t => db.TestCases.Add(t));
            //db.SaveChanges();
            return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataType()
        {
            var dataTypes = db.DataTypes.ToList();

            return Json(dataTypes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCountData()
        {
            var data = db.DataTypes.ToList();
            return Json(data[0], JsonRequestBehavior.AllowGet);
        }
    }
}