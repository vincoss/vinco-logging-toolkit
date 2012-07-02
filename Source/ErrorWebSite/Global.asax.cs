using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace WebSite
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            this.Error += new System.EventHandler(MvcApplication_Error);
        }

        void MvcApplication_Error(object sender, System.EventArgs e)
        {
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{type}", // URL with parameters
                new { controller = "Elmah", action = "Index", type = UrlParameter.Optional }, // Parameter defaults
                null,
                new[] { "Elmah.Everywhere.Controllers" });
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
        }
    }
}
