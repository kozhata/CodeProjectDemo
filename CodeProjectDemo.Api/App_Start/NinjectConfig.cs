using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Owin;
using Ninject.Web.Common.OwinHost;
using Ninject;
using System.Reflection;
using CodeProjectDemo.Api.Helpers;

namespace CodeProjectDemo.Api.App_Start
{
    public class NinjectConfig
    {
        public static void Register(IAppBuilder app, HttpConfiguration config)
        {
            app.UseNinjectMiddleware(CreateKernel);
        }

        private static IKernel CreateKernel()
        {
            IKernel kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            ApiIoC.RegisterServices(kernel);

            return kernel;
        }
    }
}