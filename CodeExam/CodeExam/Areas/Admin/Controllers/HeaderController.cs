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
                    acc.UserName = obj.UserName;
                    acc.Email = obj.Email;
                    acc.DisplayName = obj.DisplayName;
                    acc.RoleId = obj.RoleId;
                    acc.Role = acc.GetRole();
                    //acc.GetRole();
                    return Json(acc, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }

        public JsonResult EditUser(AccountViewModel acc)
        {
            using (CodeWarDbContext db = new CodeWarDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == acc.UserName);
                user.DisplayName = acc.DisplayName;
                user.Email = acc.Email;
                db.SaveChanges();
                return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}