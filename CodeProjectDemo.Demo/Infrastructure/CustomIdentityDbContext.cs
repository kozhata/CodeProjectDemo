using CodeProjectDemo.Demo.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeProjectDemo.Demo.Infrastructure
{
    public class CustomIdentityDbContext : IdentityDbContext<CustomIdentityUser>
    {
        public CustomIdentityDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static CustomIdentityDbContext Create()
        {
            return new CustomIdentityDbContext();
        }

    }
}