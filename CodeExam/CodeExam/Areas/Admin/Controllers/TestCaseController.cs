using CodeExam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeExam.Areas.Admin.Controllers
{
    public class TestCaseController : Controller
    {
        CodeWarDbContext db = new CodeWarDbContext();
        // GET: Admin/TestCase
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll()
        {
            return Json(db.TestCases.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}