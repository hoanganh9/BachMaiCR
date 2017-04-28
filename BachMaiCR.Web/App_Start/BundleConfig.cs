using System.Web.Optimization;

namespace BachMaiCR.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include(
                        "~/Scripts/jquery-1.8.3.js",
                        "~/Scripts/setup.project.js",
                        "~/Scripts/setup.project.confirmation.js",
                        "~/Scripts/jquery.cookie.js",
                        "~/Scripts/colResizable-1.3.js",
                        "~/Scripts/jquery.tipsy.js",
                        "~/Scripts/bootstrap.input-clear.js",
                        "~/Scripts/ConvertUtils.js",
                        "~/Scripts/moment.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/underscore").Include(
                        "~/Scripts/underscore.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/backbone").Include(
                        "~/Scripts/backbone.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/setup.project.custom.validate.js"


                        ));

            bundles.Add(new ScriptBundle("~/bundles/jmenu").Include(
                        "~/Scripts/jquery.metisMenu.js",
                        "~/Scripts/sb-admin.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapjs").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootstrap-bootbox.min.js",
                        "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/bootstrap-select.js"
                        //"~/Scripts/bootstrap.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/treeview").Include(
                        "~/Scripts/jquery.treeview.js",
                        "~/Scripts/jquery.treeview.async.js",
                        "~/Scripts/jquery.treeview.edit.js",
                        "~/Scripts/jquery.treeview.sortable.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/bootstrap-theme.css",
                        "~/Content/bootstrap-select.css",
                        "~/Content/datepicker.css",
                        "~/Content/loadingbar.css",
                        "~/Content/tipsy.css",
                        "~/Content/main.css",
                        "~/Content/Site.css",
                        "~/Content/font-awesomes.css",
                        "~/Content/jquery.treeview.css",
                        "~/Content/font-flat/flaticon.css",
                        "~/Content/sb-admin.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"
                        ));
            bundles.Add(new StyleBundle("~/Content/themes/base/jquery-ui").Include(
                 "~/Content/themes/base/jquery-ui.css"
                 ));
            //Style của phần template
            bundles.Add(new StyleBundle("~/Content/basic_data").Include(
                     "~/Content/basic_data/jquery.css",
                     "~/Content/basic_data/style.css",
                     "~/Content/basic_data/prettify.css",
                     "~/Content/basic_data/jquery-ui.css"
                     ));
            bundles.Add(new ScriptBundle("~/Scripts/basic_data").Include(
                      "~/Scripts/basic_data/jquery-ui.js",
                      "~/Scripts/basic_data/prettify.js",
                      "~/Scripts/basic_data/jmultiselect.js"
                      ));
        }
    }
}