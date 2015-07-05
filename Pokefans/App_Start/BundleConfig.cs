// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
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

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Verwenden Sie die Entwicklungsversion von Modernizr zum Entwickeln und Erweitern Ihrer Kenntnisse. Wenn Sie dann
            // für die Produktion bereit sind, verwenden Sie das Buildtool unter "http://modernizr.com", um nur die benötigten Tests auszuwählen.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/css/font-awesome.min.css"));

            // Admin Area specific bundles
            bundles.Add(new StyleBundle("~/bundles/adminlte").Include(
                      "~/Content/adminlte/bootstrap.css",
                      "~/Content/adminlte/AdminLTE.css",
                      "~/Content/adminlte/skin-blue.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/adminlte-js").Include(
                      "~/Scripts/adminlte/bootstrap.min.js",
                      "~/Scripts/adminlte/app.min.js",
                      "~/Scripts/adminlte/jquery.slimscroll.min.js"
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
                      "~/Scripts/backend/rolemanager.js"
                ));
        }
    }
}
