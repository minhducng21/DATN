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
    }
}