using CodeExam.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace CodeExam.Models
{
    public class AuthAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            using (CodeWarDbContext db = new CodeWarDbContext())
            {
                if (!string.IsNullOrEmpty(user.Identity.Name))
                {
                    var currentUser = db.Users.FirstOrDefault(u => u.UserName.Equals(user.Identity.Name));
                    if (currentUser != null)
                    {
                        var lsRoleId = db.RoleControllers.Where(a => a.RoleId.Equals(currentUser.RoleId)).ToList();
                        List<RoleNameViewModel> lsRoleName = new List<RoleNameViewModel>();
                        lsRoleId.ForEach(r =>
                        {
                            lsRoleName.Add( new RoleNameViewModel() { Controller = db.ControllerActions.FirstOrDefault(c => c.CtrlId.Equals(r.CtrlId)).Ctrl, Area = db.ControllerActions.FirstOrDefault(c => c.CtrlId.Equals(r.CtrlId)).Area });
                        });
                        ;
                        if (filterContext.RouteData.DataTokens.Where(d => d.Key == "area").Count() > 0)
                        {
                            if (lsRoleName.Where(r => r.Controller == filterContext.ActionDescriptor.ControllerDescriptor.ControllerName && r.Area == filterContext.RouteData.DataTokens["area"].ToString()).Count() == 0)
                            {
                                filterContext.Result = new RedirectResult("/Error/Permission");
                            }
                        }
                    }
                    else
                    {
                        var currentUserBySocialId = db.Users.FirstOrDefault(u => u.SocialId.Equals(user.Identity.Name));
                        var lsRoleId = db.RoleControllers.Where(a => a.RoleId.Equals(currentUserBySocialId.RoleId)).ToList();
                        List<RoleNameViewModel> lsRoleName = new List<RoleNameViewModel>();
                        lsRoleId.ForEach(r =>
                       {
                           lsRoleName.Add(new RoleNameViewModel() { Controller = db.ControllerActions.FirstOrDefault(c => c.CtrlId.Equals(r.CtrlId)).Ctrl, Area = db.ControllerActions.FirstOrDefault(c => c.CtrlId.Equals(r.CtrlId)).Area });
                       });
                        if (lsRoleName.Where(r => r.Controller == filterContext.ActionDescriptor.ControllerDescriptor.ControllerName).ToList().Count() == 0)
                        {
                            filterContext.Result = new RedirectResult("/Error/Permission");
                        }
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Login/Index");
                }
            }
        }
    }
}