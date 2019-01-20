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
                    "~/Scripts/lib/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/global").Include(
                    "~/Scripts/common/notifications.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                    "~/Scripts/lib/knockout/knockout-{version}.js",
                    "~/Scripts/lib/knockout/knockout.mapping.js",
                    "~/Scripts/lib/knockout/knockout-pre-rendered.js",
                    "~/Scripts/lib/moment.js",
                    "~/Scripts/lib/knockout/daterangepicker.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
                    "~/Scripts/backend/dashboard/charts.js",
                    "~/Scripts/backend/dashboard/table.js",
                    "~/Scripts/backend/dashboard/data.js",
                    "~/Scripts/backend/dashboard/activate.js" // this script must be included last in the bundle
            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/lib/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/trumbowyg").Include(
                    "~/Scripts/lib/trumbowyg/trumbowyg*",
                    "~/Scripts/lib/trumbowyg/de.min.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/trumbowyg/css").Include(
                    "~/Content/trumbowyg*"
                ));

            bundles.Add(new StyleBundle("~/bundles/select2/css").Include(
                    "~/Content/select2.css"
                ));

            // Verwenden Sie die Entwicklungsversion von Modernizr zum Entwickeln und Erweitern Ihrer Kenntnisse. Wenn Sie dann
            // für die Produktion bereit sind, verwenden Sie das Buildtool unter "http://modernizr.com", um nur die benötigten Tests auszuwählen.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/lib/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/lib/bs4/bootstrap.js",
                    "~/Scripts/common/cookieconsent.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/pokefans.css",
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
                    "~/Content/lib/ekko-lightbox.js",
                    "~/Scripts/sfc/lightboxstart.js"
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
                    "~/Scripts/lib/adminlte/bootstrap.js",
                    "~/Scripts/lib/adminlte/app.js",
                    "~/Scripts/lib/adminlte/jquery.slimscroll.js"
                ));

            // Ace Editor
            bundles.Add(new ScriptBundle("~/bundles/ace").Include(
                    "~/Scripts/lib/ace/ace.js",
                    "~/Scripts/lib/ace/theme-clouds.js",
                    "~/Scripts/lib/ace/theme-monokai.js",
                    "~/Scripts/lib/ace/mode-html.js",
                    "~/Scripts/lib/ace/mode-less.js",
                    "~/Scripts/lib/ace/mode-css.js",
                    "~/Scripts/lib/ace/worker-html.js",
                    "~/Scripts/lib/ace/worker-css.js",
                    "~/Scripts/lib/ace/loadace.js",
                    "~/Scripts/lib/ace/content-edit.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/backend").Include(
                    "~/Scripts/lib/select2.js",
                    "~/Scripts/lib/editable/bootstrap-editable.js",
                    "~/Scripts/backend/bvs/ad_vertising.js",
                    "~/Scripts/backend/bvs/multiaccounts.js",
                    "~/Scripts/backend/bvs/rolemanager.js",
                    "~/Scripts/backend/fanart/categories.js",
                    "~/Scripts/backend/fanart/edit.js",
                    "~/Scripts/backend/bvs/bans.js",
                    "~/Scripts/common/bbCodeEdit.js",
                    "~/Scripts/backend/wifi/reports.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/charts").Include(
                    "~/Scripts/lib/charts/Chart.bundle.js"
            ));

            // waypoints

            bundles.Add(new ScriptBundle("~/bundles/waypoints").Include(
                    "~/Scripts/lib/waypoints/jquery.waypoints.js",
                    "~/Scripts/lib/waypoints/shortcuts/infinite.js",
                    "~/Scripts/lib/waypoints/shortcuts/inview.js",
                    "~/Scripts/lib/waypoints/shortcuts/sticky.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/fanart").Include(
                    "~/Scripts/lib/typeahead.bundle.js",
                    "~/Scripts/lib/bootstrap-tagsinput.js",
                    "~/Scripts/fanart/fanart.js",
                    "~/Scripts/fanart/upload.js",
                    "~/Scripts/fanart/edit.js",
                    "~/Scripts/fanart/single.js",
                    "~/Scripts/common/bbCodeEdit.js"

                ));

            bundles.Add(new ScriptBundle("~/bundles/comments").Include(
                    "~/Scripts/common/comments.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
                    "~/Scripts/lib/dropzone/dropzone.js"
                ));

            // TODO: cleanup
            bundles.Add(new StyleBundle("~/bundles/dropzone/css").Include(
                    "~/Scripts/lib/dropzone/dropzone.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/emojiarea").Include(
                "~/Scripts/lib/emojione.js",
                "~/Scripts/lib/emojionearea.js"
            ));

            bundles.Add(new StyleBundle("~/bundles/emojiarea/css").Include(
                "~/Content/emojionearea.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/pm-label").Include(
                "~/Scripts/lib/editable/bootstrap-editable.js",
                "~/Scripts/user/pm-labels.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/pm-compose").Include(
                "~/Scripts/lib/bootstrap-tagsinput.js",
                "~/Scripts/user/pm-compose.js"
            ));
        }
    }
}