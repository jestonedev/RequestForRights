using System.Web.Optimization;

namespace RequestsForRights
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js").Include(
                        "~/Scripts/jquery.mousewheel.*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
            .Include("~/Scripts/jquery.validate.*")
            .Include("~/Scripts/jquery.unobtrusive*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.*",
                      "~/Scripts/respond.*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datepicker").Include(
                      "~/Scripts/bootstrap-datepicker.*",
                      "~/Scripts/locales/bootstrap-datepicker.ru.min.*"));

            bundles.Add(new StyleBundle("~/Content/bootstrap-datepicker").Include(
                      "~/Content/bootstrap-datepicker3.*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.autocomplete").Include(
                      "~/Scripts/jquery.autocomplete.*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.*",
                      "~/Content/site.*"));

            bundles.Add(new ScriptBundle("~/bundles/json2").Include(
                      "~/Scripts/json2.*"));

            bundles.Add(new ScriptBundle("~/bundles/globalize")
                .Include("~/Scripts/cldr.*")
                .Include("~/Scripts/cldr/event.*")
                .Include("~/Scripts/cldr/supplemental.*")
                .Include("~/Scripts/globalize.*")
                .Include("~/Scripts/globalize/number.*")
                .Include("~/Scripts/globalize/date.*"));

            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
                "~/Scripts/jquery.inputmask/inputmask.js",
                "~/Scripts/jquery.inputmask/inputmask.extensions.js",
                "~/Scripts/jquery.inputmask/inputmask.regex.extensions.js",
                "~/Scripts/jquery.inputmask/jquery.inputmask.js"));

            bundles.Add(new ScriptBundle("~/bundles/rr-common").Include("~/Scripts/rr-common.*"));
            bundles.Add(new ScriptBundle("~/bundles/rr-index.common").Include("~/Scripts/rr-index.common.*"));
            bundles.Add(new ScriptBundle("~/bundles/rr-request.common").Include("~/Scripts/rr-request.common.*"));
            bundles.Add(new ScriptBundle("~/bundles/rr-detail.update.common").Include("~/Scripts/rr-detail.update.common.*"));
            bundles.Add(new ScriptBundle("~/bundles/rr-request.update.insert.common").Include("~/Scripts/rr-request.update.insert.common.*"));
            bundles.Add(new ScriptBundle("~/bundles/rr-resource.common").Include("~/Scripts/rr-resource.common.*"));
            bundles.Add(new ScriptBundle("~/bundles/rr-request.index").Include("~/Scripts/rr-request.index.*"));
            bundles.Add(new ScriptBundle("~/bundles/rr-request.add-user").Include("~/Scripts/rr-request.add-user.*"));
            bundles.Add(new ScriptBundle("~/bundles/rr-request.delegate-permissions").Include("~/Scripts/rr-request.delegate-permissions.*"));
            bundles.Add(new ScriptBundle("~/bundles/rr-request.comments").Include("~/Scripts/rr-request.comments.*"));
            bundles.Add(new ScriptBundle("~/bundles/rr-request.agreements").Include("~/Scripts/rr-request.agreements.*"));
            bundles.Add(new ScriptBundle("~/bundles/rr-help").Include("~/Scripts/rr-help.*"));
            bundles.Add(new ScriptBundle("~/bundles/rr-report.common").Include("~/Scripts/rr-report.common.*"));
            bundles.Add(new ScriptBundle("~/bundles/rr-report.user-rights").Include("~/Scripts/rr-report.user-rights.*"));
        }
    }
}
