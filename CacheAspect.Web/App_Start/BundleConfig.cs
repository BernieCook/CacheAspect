using System.Web.Optimization;

namespace CacheAspect.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(
                new StyleBundle("~/css")
                .Include("~/Assets/Bootstrap/Css/bootstrap.css")
                .Include("~/Assets/Styles/html.css")
                .Include("~/Assets/Bootstrap/Css/bootstrap-responsive.css"));

            bundles.Add(
                new ScriptBundle("~/js")
                .Include("~/Assets/Scripts/jquery-1.8.3.js"));
        }
    }
}