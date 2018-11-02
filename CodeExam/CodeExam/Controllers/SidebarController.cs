using CodeExam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeExam.Controllers
{
    public class SidebarController : Controller
    {
        public JsonResult GetCurrentUser()
        {
            using (CodeWarDbContext db = new CodeWarDbContext())
            {
                int id = int.Parse(Session["ID"].ToString());
                if (id > 0)
                {
                    var obj = db.Users.Where(u => u.UserId == id).FirstOrDefault();
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}