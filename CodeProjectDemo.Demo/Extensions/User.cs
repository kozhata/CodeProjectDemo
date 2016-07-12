using CodeProjectDemo.Demo.Infrastructure;
using CodeProjectDemo.Demo.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace CodeProjectDemo.Demo.Extensions
{
    public static class User
    {
        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(this CustomIdentityUser user, CustomUserManager manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(user, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}