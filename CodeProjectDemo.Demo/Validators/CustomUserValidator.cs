using CodeProjectDemo.Demo.Infrastructure;
using CodeProjectDemo.Demo.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CodeProjectDemo.Demo.Validators
{
    public class CustomUserValidator : UserValidator<CustomIdentityUser>
    {

        List<string> _allowedEmailDomains = new List<string>
        { "outlook.com", "hotmail.com", "gmail.com", "yahoo.com" };

        public CustomUserValidator(CustomUserManager appUserManager)
            : base(appUserManager) { }

        public override async Task<IdentityResult> ValidateAsync(CustomIdentityUser user)
        {
            IdentityResult result = await base.ValidateAsync(user);

            var emailDomain = user.Email.Split('@')[1];

            if (!_allowedEmailDomains.Contains(emailDomain.ToLower()))
            {
                List<string> errors = result.Errors.ToList();

                errors.Add(string.Format("Email domain '{0}' is not allowed", emailDomain));

                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}