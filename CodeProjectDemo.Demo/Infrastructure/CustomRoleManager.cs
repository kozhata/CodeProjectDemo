using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeProjectDemo.Demo.Infrastructure
{
    public class CustomRoleManager : RoleManager<IdentityRole>
    {
        public CustomRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore) { }

        public static CustomRoleManager Create(IdentityFactoryOptions<CustomRoleManager> options, IOwinContext context)
        {
            var userDbContext = context.Get<CustomIdentityDbContext>();
            var userRoleManager = new CustomRoleManager(new RoleStore<IdentityRole>(userDbContext));

            return userRoleManager;
        }
    }
}