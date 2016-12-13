using Newtonsoft.Json;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace cdjwebapi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            HttpConfiguration httpConfig = GlobalConfiguration.Configuration;
            MediaTypeFormatterCollection mtfc = httpConfig.Formatters;
            JsonMediaTypeFormatter jmtf = mtfc.JsonFormatter;
            JsonSerializerSettings jss = jmtf.SerializerSettings;
            jss.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
                //.Re‌​ferenceLoopHandling = ReferenceLoopHandling.Ignore;

            GlobalConfiguration.Configuration.EnsureInitialized();

        }
    }
}
