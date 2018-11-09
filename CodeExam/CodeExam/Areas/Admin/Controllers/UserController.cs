using CodeExam.Models;
using CodeExam.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeExam.Areas.Admin.Controllers
{
    [AuthAttribute]
    public class UserController : Controller
    {
        CodeWarDbContext db = new CodeWarDbContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetRole()
        {
            var roles = db.RoleUsers.Where(r => r.RoleId != (int)RoleCommon.Admin).ToList();
            return Json(roles, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddUser(AccountViewModel acc)
        {
            User user = new User();
            user.UserName = acc.UserName;
            user.Password = Encryption.Encrypt(acc.Password);
            user.DisplayName = acc.DisplayName;
            user.Email = acc.Email;
            user.RoleId = acc.RoleId;
            user.UserStatus = 1;
            db.Users.Add(user);
            db.SaveChanges();
            return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUser(int page, int pageSize)
        {
            var lstUser = db.Users.Where(u => u.RoleId == (int)RoleCommon.Staff && u.UserStatus == 1).ToList();
            List<AccountViewModel> lstAcc = new List<AccountViewModel>();
            foreach (User user in lstUser)
            {
                AccountViewModel acc = new AccountViewModel();
                acc.ID = user.UserId;
                acc.UserName = user.UserName;
                acc.Password = user.Password;
                acc.Email = user.Email;
                acc.DisplayName = user.DisplayName;
                acc.RoleId = user.RoleId;
                acc.Role = acc.GetRole();
                lstAcc.Add(acc);
            }
            var count = lstAcc.Count;
            var results = lstAcc.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(new { results, count }, JsonRequestBehavior.AllowGet);
        }

        //Get user by id
        [HttpGet]
        public JsonResult GetUserById(int id)
        {
            var user = db.Users.Find(id);
            AccountViewModel acc = new AccountViewModel();
            acc.ID = user.UserId;
            acc.UserName = user.UserName;
            acc.Password = user.Password;
            acc.Email = user.Email;
            acc.DisplayName = user.DisplayName;
            acc.RoleId = user.RoleId;
            acc.Role = acc.GetRole();
            return Json(acc, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditUser(AccountViewModel acc)
        {
            var user = db.Users.Find(acc.ID);
            user.UserName = acc.UserName;
            user.Password = acc.Password;
            user.DisplayName = acc.DisplayName;
            user.Email = acc.Email;
            user.RoleId = acc.RoleId;
            db.SaveChanges();
            return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteUser(int id)
        {
            var user = db.Users.Find(id);
            user.UserStatus = 0;
            db.SaveChanges();
            return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
        }
    }
}