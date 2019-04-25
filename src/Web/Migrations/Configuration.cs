namespace TouchTest.Web.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TouchTest.Web.Controllers.api;
    using TouchTest.Web.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddOrUpdate(
                new ApplicationUser() { Id = 1, Email = "admin@admin.com", UserName = "admin", PasswordHash = new AccountController().UserManager.PasswordHasher.HashPassword("admin"), EmailConfirmed = false, PhoneNumberConfirmed = false, TwoFactorEnabled = false, LockoutEnabled = false, AccessFailedCount = 0 }
                );
            }

            if (!context.Clients.Any())
            {
                context.Clients.AddOrUpdate(
                new Client() { Name = "test client", Address = "القاهرة", Phone = "024545452" }
                );
            }
        }
    }
}
