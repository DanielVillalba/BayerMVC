using System.Web;
using System.Web.Optimization;

namespace PSD.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/site").Include(
            "~/Scripts/Site.js",
            "~/Scripts/CommonUtils.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"
                    , "~/Content/Components/DataTables/js/jquery.dataTables.js"
                    , "~/Content/Components/DataTables/js/dataTables.bootstrap.js"
                    , "~/Content/Components/Multiselect/bootstrap-multiselect.js"
                    , "~/Content/Components/DatePicker/bootstrap-datepicker.js"
                    , "~/Content/Components/FileInput/bootstrap.file-input.js"
                    , "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"
                       , "~/Content/Components/DataTables/css/jquery.dataTables.css"
                       , "~/Content/Components/DataTables/css/dataTables.bootstrap.css"
                       , "~/Content/themes/base/jquery.ui.datepicker.css"
                       , "~/Content/Components/Multiselect/bootstrap-multiselect.css"
                       , "~/Content/Components/DatePicker/datepicker3.css"
                       , "~/Content/bootstrap.micultivo.bayer.css"
                       , "~/Content/site.css"
                       ));
        }
    }
}
