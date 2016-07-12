using CodeProjectDemo.DataModel;
using CodeProjectDemo.DataModel.UnitOfWork;
using CodeProjectDemo.Models.AppSettings;
using CodeProjectDemo.Models.User;
using CodeProjectDemo.Services.Account;
using CodeProjectDemo.Services.Cache;
using CodeProjectDemo.Services.Product;
using CodeProjectDemo.Services.Token;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CodeProjectDemo.Helpers
{
    public class ApiIoC
    {
        public static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<CodeProjectDemoEntities>().To<CodeProjectDemoEntities>().InRequestScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            kernel.Bind<IProductService>().To<ProductService>().InRequestScope();
            kernel.Bind<ICacheService>().To<CacheService>().InRequestScope();
            kernel.Bind<IAccountService>().To<AccountService>().InRequestScope();
            kernel.Bind<ITokenService>().To<TokenService>().InRequestScope();
            kernel.Bind<ApiUser>().ToMethod<ApiUser>((context) =>
            {
                ITokenService tokenService = kernel.GetService(typeof(ITokenService)) as ITokenService;
                IAppSettings appSettings = kernel.GetService(typeof(IAppSettings)) as IAppSettings;

                string apiToken = HttpContext.Current.Request.Headers.Get(appSettings.TokenHeader);
                
                if (string.IsNullOrWhiteSpace(apiToken))
                {
                    return new ApiUser();
                }

                ApiUser currentUser = tokenService.GetUserByApiToken(apiToken);

                return currentUser ?? new ApiUser();
            }).InRequestScope();
            kernel.Bind<IAppSettings>().To<AppSettings>().InRequestScope();

            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
        }
    }
}