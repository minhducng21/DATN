using System.Web;
using System.Web.Optimization;

namespace CodeExam
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/fonts/font-awesome-4.7.0/css/font-awesome.min.css",
                      "~/Content/css/material-dashboard.css"));
            bundles.Add(new StyleBundle("~/Bundles/js").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/angular.js",
                      "~/Scripts/app/AppController.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/Login/popper.min.js",
                      "~/Scripts/bootstrap-material-design.min.js",
                      "~/Scripts/perfect-scrollbar.jquery.min.js",
                      "~/Scripts/chartist.min.js",
                      "~/Scripts/bootstrap-notify.js",
                      "~/Scripts/material-dashboard.min.js",
                      "~/Scripts/ui-bootstrap.min.js",
                      "~/Scripts/ui-bootstrap-tpls.min.js",
                      "~/Scripts/ui-select2.js"
                ));
        }
    }
}
