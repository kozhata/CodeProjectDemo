using CodeProjectDemo.App_Start;
using CodeProjectDemo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace CodeProjectDemo
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalConfiguration.Configuration.Filters);
            //GlobalConfiguration.Configuration.Filters.Add(new ApiAuthenticationFilter());

            //log4net.Config.XmlConfigurator.Configure();
        }
    }
}
