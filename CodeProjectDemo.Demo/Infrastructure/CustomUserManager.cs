using CodeProjectDemo.Demo.Models;
using CodeProjectDemo.Demo.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeProjectDemo.Demo.Infrastructure
{
    public class CustomUserManager : UserManager<CustomIdentityUser>
    {
        public CustomUserManager(IUserStore<CustomIdentityUser> store)
            : base(store) { }

        public static CustomUserManager Create(IdentityFactoryOptions<CustomUserManager> options, IOwinContext context)
        {
            var userDbContext = context.Get<CustomIdentityDbContext>();
            var userManager = new CustomUserManager(new UserStore<CustomIdentityUser>(userDbContext));

            userManager.EmailService = new EmailService();

            IDataProtectionProvider dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                userManager.UserTokenProvider = 
                    new DataProtectorTokenProvider<CustomIdentityUser>(dataProtectionProvider.Create("Education"))
                {
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }

            userManager.UserValidator = new UserValidator<CustomIdentityUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            return userManager;
        }
    }

}