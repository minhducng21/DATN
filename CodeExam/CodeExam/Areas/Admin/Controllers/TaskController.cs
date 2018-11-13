using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeExam.Models;

namespace CodeExam.Areas.Admin.Controllers

{
    public class TaskController : Controller
    {
        CodeWarDbContext db = new CodeWarDbContext();
        // GET: Areas/Task
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public JsonResult GetAll()
        {
            return Json(db.Tasks, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreateTask(Task task)
        {
            db.Tasks.Add(task);
            if (db.SaveChanges() == 1)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
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
                item.TestCaseId = task.TestCaseId;
                if (db.SaveChanges() == 1)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaskById(int id)
        {
            return Json(db.Tasks.FirstOrDefault(f => f.TaskId == id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int id)
        {
            var itemToDel = db.Tasks.FirstOrDefault(f => f.TaskId == id);
            if (itemToDel != null)
            {
                itemToDel.TaskStatus = Constant.Status.Deactive;
                if (db.SaveChanges() == 1)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}