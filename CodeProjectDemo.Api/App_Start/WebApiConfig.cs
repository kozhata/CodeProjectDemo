using Ninject.Web.WebApi.OwinHost;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CodeProjectDemo.Api.App_Start
{
    public class WebApiConfig
    {
        public static void Register(IAppBuilder app, HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            app.UseNinjectWebApi(config);
        }
    }
}