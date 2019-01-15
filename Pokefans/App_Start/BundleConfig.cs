// Copyright 2015-2016 the pokefans-core authors. See copying.md for legal info.
using System.Web;
using System.Web.Optimization;

namespace Pokefans
{
    public class BundleConfig
    {
        // Weitere Informationen zu Bundling finden Sie unter "http://go.microsoft.com/fwlink/?LinkId=301862"
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                    "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/global").Include(
                    "~/Scripts/notifications.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                    "~/Scripts/knockout/knockout-{version}.js",
                    "~/Scripts/knockout/knockout.mapping.js",
                    "~/Scripts/knockout/knockout-pre-rendered.js",
                    "~/Scripts/moment.js",
                    "~/Scripts/knockout/daterangepicker.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
                    "~/Scripts/backend/dashboard/charts.js",
                    "~/Scripts/backend/dashboard/table.js",
                    "~/Scripts/backend/dashboard/data.js",
                    "~/Scripts/backend/dashboard/activate.js" // this script must be included last in the bundle
            ));

            bundles.Add(new ScriptBundle("~/bundles/reports").Include(
                    "~/Scripts/backend/reports.js"
                ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/trumbowyg").Include(
                    "~/Scripts/trumbowyg/trumbowyg*",
                    "~/Scripts/trumbowyg/de.min.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/trumbowyg/css").Include(
                    "~/Content/trumbowyg*"
                ));

            bundles.Add(new StyleBundle("~/bundles/select2/css").Include(
                    "~/Content/select2.css",
                    "~/Content/select2-bootstrap.css"
                ));

            // Verwenden Sie die Entwicklungsversion von Modernizr zum Entwickeln und Erweitern Ihrer Kenntnisse. Wenn Sie dann
            // für die Produktion bereit sind, verwenden Sie das Buildtool unter "http://modernizr.com", um nur die benötigten Tests auszuwählen.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/respond.js",
                    "~/Scripts/cookieconsent.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/Site.css",
                    "~/Content/font-awesome.min.css",
                    "~/Content/bootstrap-tagsinput.css"));            

            // SFC
            bundles.Add(new StyleBundle("~/Content/sfc/css").Include(
                    "~/Content/sfc/bootstrap.css",
                    "~/Content/sfc/site.css",
                    "~/Content/css/font-awesome.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/lightbox/css").Include(
                    "~/Content/lightbox/dark.css",
                    "~/Content/lightbox/ekko-lightbox.css"
                ));

            bundles.Add(new ScriptBundle("~/Content/lightbox/js").Include(
                    "~/Content/ekko-lightbox.js",
                    "~/Content/lightboxstart.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/sfc/arcrypt").Include(
                    "~/Scripts/sfc/arcrypt2.js",
                    "~/Scripts/sfc/arcrypt.js"
                ));

            // Admin Area specific bundles
            bundles.Add(new StyleBundle("~/bundles/adminlte").Include(
                    "~/Content/adminlte/bootstrap.css",
                    "~/Content/adminlte/AdminLTE.css",
                    "~/Content/adminlte/skin-blue.css",
                    "~/Content/editable/bootstrap-editable.css",
                    "~/Content/font-awesome.css",
                    "~/Content/select2-bootstrap.css",
                    "~/Content/adminlte/datetimepicker.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/adminlte-js").Include(
                    "~/Scripts/adminlte/bootstrap.js",
                    "~/Scripts/adminlte/app.js",
                    "~/Scripts/adminlte/jquery.slimscroll.js"
                ));

            // Ace Editor
            bundles.Add(new ScriptBundle("~/bundles/ace").Include(
                    "~/Scripts/ace/ace.js",
                    "~/Scripts/ace/theme-clouds.js",
                    "~/Scripts/ace/theme-monokai.js",
                    "~/Scripts/ace/mode-html.js",
                    "~/Scripts/ace/mode-less.js",
                    "~/Scripts/ace/mode-css.js",
                    "~/Scripts/ace/worker-html.js",
                    "~/Scripts/ace/worker-css.js",
                    "~/Scripts/ace/loadace.js",
                    "~/Scripts/ace/content-edit.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/backend").Include(
                    "~/Scripts/backend/advertising.js",
                    "~/Scripts/backend/multiaccounts.js",
                    "~/Scripts/backend/rolemanager.js",
                    "~/Scripts/editable/bootstrap-editable.js",
                    "~/Scripts/backend/fanart/categories.js",
                    "~/Scripts/backend/fanart/edit.js",
                    "~/Scripts/backend/bans.js",
                    "~/Scripts/select2.js",
                    "~/Scripts/bbCodeEdit.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/charts").Include(
                    "~/Scripts/charts/Chart.bundle.js"
            ));

            // waypoints

            bundles.Add(new ScriptBundle("~/bundles/waypoints").Include(
                    "~/Scripts/waypoints/jquery.waypoints.js",
                    "~/Scripts/waypoints/shortcuts/infinite.js",
                    "~/Scripts/waypoints/shortcuts/inview.js",
                    "~/Scripts/waypoints/shortcuts/sticky.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/fanart").Include(
                    "~/Scripts/typeahead.bundle.js",
                    "~/Scripts/bootstrap-tagsinput.js",
                    "~/Scripts/fanart/fanart.js",
                    "~/Scripts/fanart/upload.js",
                    "~/Scripts/fanart/edit.js",
                    "~/Scripts/fanart/single.js",
                    "~/Scripts/bbCodeEdit.js"

                ));

            bundles.Add(new ScriptBundle("~/bundles/comments").Include(
                    "~/Scripts/comments.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
                    "~/Scripts/dropzone/dropzone.js"
                ));
            bundles.Add(new StyleBundle("~/bundles/dropzone/css").Include(
                    "~/Scripts/dropzone/dropzone.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/emojiarea").Include(
                "~/Scripts/emojione.js",
                "~/Scripts/emojionearea.js"
            ));

            bundles.Add(new StyleBundle("~/bundles/emojiarea/css").Include(
                "~/Content/emojionearea.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/pm-label").Include(
                "~/Scripts/editable/bootstrap-editable.js",
                "~/Scripts/pm-labels.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/pm-compose").Include(
                "~/Scripts/bootstrap-tagsinput.js",
                "~/Scripts/pm-compose.js"
            ));
        }
    }
}