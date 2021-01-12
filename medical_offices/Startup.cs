using medical_offices.Models;
using medical_offices.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(medical_offices.Startup))]
namespace medical_offices
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateAdminAndUserRoles();
        }

        private void CreateAdminAndUserRoles()
        {
            var ctx = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));

            if(!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var admin = new ApplicationUser();
                admin.UserName = "admin@admin.com";
                admin.Email = "admin@admin.com";

                var adminCreated = userManager.Create(admin, "Admin2021!");
                if(adminCreated.Succeeded)
                {
                    userManager.AddToRole(admin.Id, "Admin");
                }

                var personAdmin = new Person
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    ApplicationUser = admin
                };
                ctx.People.Add(personAdmin);
                ctx.SaveChanges();
            }

            if(!roleManager.RoleExists("SuperUser"))
            {
                var role = new IdentityRole();
                role.Name = "SuperUser";
                roleManager.Create(role);
            }
        }
    }
}
