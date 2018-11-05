using System.Web.Mvc;

namespace CodeExam.Areas.Areas
{
    public class AreasAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                 "Admin",
                 "Admin/{controller}/{action}/{id}",
                 defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "CodeExam.Areas.Admin.Controllers" }
             );
        }
    }
}