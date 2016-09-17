using System.Web.Optimization;

namespace uTest.WEB
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/ajax").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/materialize").Include(
                        "~/Scripts/materialize/materialize.js"));

            bundles.Add(new ScriptBundle("~/bundles/init").Include(
                        "~/Scripts/initialization.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/materialize").Include(
                      "~/Content/materialize/css/materialize.css"));
        }
    }
}
