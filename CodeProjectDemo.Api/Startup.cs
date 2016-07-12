using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using CodeProjectDemo.Api.App_Start;
using Ninject.Web.Common.OwinHost;
using System.Web.Http;

[assembly: OwinStartup(typeof(CodeProjectDemo.Api.Startup))]

namespace CodeProjectDemo.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            NinjectConfig.Register(app, config);
            WebApiConfig.Register(app, config);
            //app.UseWebApi(config);
        }
    }
}
