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

        public JsonResult GetController()
        {
            using (CodeWarDbContext db = new CodeWarDbContext())
            {
                if (!String.IsNullOrEmpty(User.Identity.Name))
                {
                    var roleCtrls = db.RoleControllers.ToList();
                    var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                    var roleControllers = db.RoleControllers.Where(rc => rc.RoleId == currentUser.RoleId).ToList();
                    List<ControllerViewModel> lsController = new List<ControllerViewModel>();
                    foreach (var item in roleControllers)
                    {
                        ControllerViewModel ctrlViewModel = new ControllerViewModel();
                        ctrlViewModel.CtrlId = item.CtrlId;
                        ctrlViewModel.Ctrl = ctrlViewModel.GetCtrl().Ctrl;
                        ctrlViewModel.Area = ctrlViewModel.GetCtrl().Area;
                        ctrlViewModel.Description = ctrlViewModel.GetCtrl().Description;
                        lsController.Add(ctrlViewModel);
                    }
                    return Json(lsController, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
        }
    }
}