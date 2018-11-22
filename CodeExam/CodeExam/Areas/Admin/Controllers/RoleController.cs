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
    public class RoleController : Controller
    {
        // GET: Admin/Role
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetRole()
        {
            using (CodeWarDbContext db = new CodeWarDbContext())
            {
                return Json(db.RoleUsers.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetController()
        {
            using (CodeWarDbContext db = new CodeWarDbContext())
            {
                return Json(db.ControllerActions.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetControllerById(int id)
        {
            using (CodeWarDbContext db = new CodeWarDbContext())
            {
                var lsCtrl = db.ControllerActions.ToList();
                var lsCtrlRole = db.RoleControllers.Where(c => c.RoleId == id).ToList();
                List<ControllerViewModel> lsCtrlViewModel = new List<ControllerViewModel>();
                lsCtrl.ForEach(o =>
                    lsCtrlViewModel.Add(new ControllerViewModel() { CtrlId = o.CtrlId, Ctrl = o.Ctrl, Area = o.Area, IsChecked = lsCtrlRole.Where(c => c.CtrlId == o.CtrlId).Count() > 0 ? true : false})
                );
                return Json(new { ctrls = lsCtrlViewModel, role = db.RoleUsers.Find(id) }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddRole(RoleControllerViewModel roleControllerViewModel)
        {
            using (CodeWarDbContext db = new CodeWarDbContext())
            {
                if (db.RoleUsers.Where(r => r.RoleName == roleControllerViewModel.RoleName).Count() == 0)
                {
                    db.RoleUsers.Add(new RoleUser() { RoleName = roleControllerViewModel.RoleName, RoleStatus = 1 });
                    db.SaveChanges();
                }
                var newRole = db.RoleUsers.FirstOrDefault(r => r.RoleName == roleControllerViewModel.RoleName);
                roleControllerViewModel.ControllerViewModels.ForEach(rc =>
                    db.RoleControllers.Add(new Models.RoleController() { RoleId = newRole.RoleId, CtrlId = rc.CtrlId })
                );
                db.SaveChanges();
                return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EditRole(RoleControllerViewModel roleControllerViewModel)
        {
            using (CodeWarDbContext db = new CodeWarDbContext())
            {
                var roleControllers = db.RoleControllers.Where(o => o.RoleId == roleControllerViewModel.RoleId).ToList();
                roleControllers.ForEach(r => db.RoleControllers.Remove(r));
                roleControllerViewModel.ControllerViewModels.ForEach(rc => db.RoleControllers.Add(new Models.RoleController() { RoleId = roleControllerViewModel.RoleId, CtrlId = rc.CtrlId }));
                var role = db.RoleUsers.Find(roleControllerViewModel.RoleId);
                role.RoleName = roleControllerViewModel.RoleName;
                db.SaveChanges();
                return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteRole(int roleId)
        {
            using (CodeWarDbContext db = new CodeWarDbContext())
            {
                var role = db.RoleUsers.Find(roleId);
                db.RoleUsers.Remove(role);
                db.SaveChanges();
                return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}