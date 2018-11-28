using CodeExam.Models;
using CodeExam.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeExam.Controllers
{
    public class UserHeaderController : Controller
    {
        // GET: UserHeader
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
                    if (obj != null)
                    {
                        acc.ID = obj.UserId;
                        acc.UserName = obj.UserName;
                        acc.Email = obj.Email;
                        acc.DisplayName = obj.DisplayName;
                        acc.RoleId = obj.RoleId;
                        acc.Role = acc.GetRole();
                    }
                    else
                    {
                        obj = db.Users.FirstOrDefault(u => u.SocialId.Equals(User.Identity.Name));

                        acc.ID = obj.UserId;
                        acc.UserName = obj.UserName;
                        acc.Email = obj.Email;
                        acc.DisplayName = obj.DisplayName;
                        acc.RoleId = obj.RoleId;
                        acc.Role = acc.GetRole();
                    }
                    return Json(acc, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }

        public JsonResult UpdateUser(AccountViewModel acc)
        {
            using (CodeWarDbContext db = new CodeWarDbContext())
            {
                var obj = db.Users.FirstOrDefault(u => u.UserId == acc.ID);
                obj.DisplayName = acc.DisplayName;
                obj.Email = acc.Email;

                if (!String.IsNullOrEmpty(acc.Password))
                {
                    obj.Password = Encryption.Encrypt(acc.Password);
                    db.SaveChanges();
                    var ctx = Request.GetOwinContext();
                    var authenticationManager = ctx.Authentication;
                    //Sign out
                    authenticationManager.SignOut();
                    return Json(0, JsonRequestBehavior.AllowGet);
                }

                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}