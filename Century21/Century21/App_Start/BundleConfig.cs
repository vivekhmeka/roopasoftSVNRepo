using System.Web;
using System.Web.Optimization;

namespace Century21
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-1.10.2.js",
                        "~/Scripts/jquery-ui.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/wow.min.js",
                        "~/Scripts/jquery.prettyPhoto.js"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.min.css",
                "~/Content/animate.min.css",
                "~/Content/Main.css",
                "~/Content/responsive.css",
                "~/Content/jquery-ui.css",
                "~/Content/font-awesome.min.css"
                 ));
        }
    }
}
