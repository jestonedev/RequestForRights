using System.Web.Optimization;

namespace RequestsForRights
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
            .Include("~/Scripts/jquery.validate.*")
            .Include("~/Scripts/jquery.unobtrusive*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/rr-common").Include("~/Scripts/rr-common.js"));
            bundles.Add(new ScriptBundle("~/bundles/rr-index.common").Include("~/Scripts/rr-index.common.js"));
            bundles.Add(new ScriptBundle("~/bundles/rr-detail.update.common").Include("~/Scripts/rr-detail.update.common.js"));
            bundles.Add(new ScriptBundle("~/bundles/rr-resource.common").Include("~/Scripts/rr-resource.common.js"));
            bundles.Add(new ScriptBundle("~/bundles/rr-request.index").Include("~/Scripts/rr-request.index.js"));
            bundles.Add(new ScriptBundle("~/bundles/rr-request.common").Include("~/Scripts/rr-request.common.js"));
            bundles.Add(new ScriptBundle("~/bundles/rr-request.adduser").Include("~/Scripts/rr-request.adduser.js"));
            bundles.Add(new ScriptBundle("~/bundles/rr-request.comments").Include("~/Scripts/rr-request.comments.js"));
            bundles.Add(new ScriptBundle("~/bundles/rr-request.agreements").Include("~/Scripts/rr-request.agreements.js"));
            bundles.Add(new ScriptBundle("~/bundles/rr-help").Include("~/Scripts/rr-help.js"));
            bundles.Add(new ScriptBundle("~/bundles/rr-report.common").Include("~/Scripts/rr-report.common.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datepicker").Include(
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/locales/bootstrap-datepicker.ru.min.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap-datepicker").Include(
                      "~/Content/bootstrap-datepicker3.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.autocomplete").Include(
                      "~/Scripts/jquery.autocomplete.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
