using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using medical_offices.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace medical_offices.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new Initp());
        }

        public DbSet<Person> People { get; set; }
        public DbSet<MedicalOffice> MedicalOffices { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class Initp : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext ctx)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));
            if (!roleManager.RoleExists("SuperUser"))
            {
                roleManager.Create(new IdentityRole("SuperUser"));
            }

            var user = new ApplicationUser();
            user.UserName = "florin@email.com";
            user.Email = "florin@email.com";
            var userCreated = userManager.Create(user, "Parola7+");
            if (userCreated.Succeeded)
            {
                userManager.AddToRole(user.Id, "SuperUser");
            }
            var person1 = new Person
            {
                FirstName = "Florin",
                LastName = "Nacu",
                ApplicationUser = user
            };

            var user2 = new ApplicationUser();
            user2.UserName = "andreea@email.com";
            user2.Email = "andreea@email.com";
            var userCreated2 = userManager.Create(user2, "Parola7+");
            if (userCreated2.Succeeded)
            {
                userManager.AddToRole(user2.Id, "SuperUser");
            }
            var person2 = new Person
            {
                FirstName = "Andreea",
                LastName = "Cristea",
                ApplicationUser = user2
            };

            var medicalOffice1 = new MedicalOffice
            {
                Name = "Bucharest General Medical Office",
                ContactNumber = "0761234567",
                Address = new Address { Country = "Romanina", City = "Bucharest", Street = "Calea Grivitei", Number = 142 },
                Person = person1,
                Services = new List<Service>
                {
                    new Service { Name = "Vascular Surgery" },
                    new Service { Name = "General Surgery" }
                }
            };

            var medicalOffice2 = new MedicalOffice
            {
                Name = "Galati Gastroenterology Medical Office",
                ContactNumber = "0761233567",
                Address = new Address { Country = "Romanina", City = "Galati", Street = "Calea Sarpelui", Number = 143 },
                Person = person1,
                Services = new List<Service>
                {
                    new Service { Name = "Gastroenterology" }
                }
            };

            var medicalOffice3 = new MedicalOffice
            {
                Name = "Bucharest Ophtalmology Medical Office",
                ContactNumber = "0761234567",
                Address = new Address { Country = "Romanina", City = "Bucharest", Street = "Calea Veveritei", Number = 11 },
                Person = person2,
                Services = new List<Service>
                {
                    new Service { Name = "Ophtalmology" }
                }
            };

            ctx.MedicalOffices.Add(medicalOffice1);
            ctx.MedicalOffices.Add(medicalOffice2);
            ctx.MedicalOffices.Add(medicalOffice3);

            ctx.SaveChanges();
            base.Seed(ctx);
        }
    }
}