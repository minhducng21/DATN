using CodeExam.Models;
using CodeExam.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeExam.Areas.Admin.Controllers
{
    public class SidebarController : Controller
    {
        // GET: Areas/Sidebar
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetCurrentUser()
        {
            AccountViewModel acc = new AccountViewModel();
            try
            {
                using (CodeWarDbContext db = new CodeWarDbContext())
                {
                    if (!string.IsNullOrEmpty(User.Identity.Name))
                    {
                        var obj = db.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
                        acc.ID = obj.UserId;
                        acc.DisplayName = obj.DisplayName;
                        acc.RoleId = obj.RoleId;
                        acc.GetRole();
                        return Json(acc, JsonRequestBehavior.AllowGet);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}