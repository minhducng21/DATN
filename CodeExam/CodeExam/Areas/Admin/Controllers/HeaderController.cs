using CodeExam.Models;
using CodeExam.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeExam.Areas.Admin.Controllers
{
    public class HeaderController : Controller
    {
        // GET: Admin/Header
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetCurrentUser()
        {
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                using (CodeWarDbContext db = new CodeWarDbContext())
                {
                    var obj = db.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
                    AccountViewModel acc = new AccountViewModel();
                    acc.DisplayName = obj.DisplayName;
                    acc.RoleId = obj.RoleId;
                    acc.GetRole();
                    return Json(acc, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }
    }
}