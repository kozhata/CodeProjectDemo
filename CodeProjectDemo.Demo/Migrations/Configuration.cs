namespace CodeProjectDemo.Demo.Migrations
{
    using Infrastructure;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CustomIdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CustomIdentityDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var userManager = new CustomUserManager(new UserStore<CustomIdentityUser>(context));
            var roleManager = new CustomRoleManager(new RoleStore<IdentityRole>(context));

            CustomIdentityUser user = new CustomIdentityUser
            {
                UserName = "kozhata",
                Email = "kozhinkov@gmail.com",
                EmailConfirmed = true,
                FirstName = "Aleksandar",
                LastName = "Kozhinkov",
                Level = 1,
                JoinDate = DateTime.Now.AddYears(-3)
            };
            
            userManager.Create(user, "mitologija92!");

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = userManager.FindByName("kozhata");

            userManager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin" });
        }
    }
}
