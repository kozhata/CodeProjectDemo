using CodeProjectDemo.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace CodeProjectDemo.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(HttpFilterCollection filters)
        {
            //filters.Add(new ApiAuthorizationAttribute());
            filters.Add(new CustomExceptionAttribute());
        }
    }
}