namespace MVCproject.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCproject.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MVCproject.Models.AppDbContext context)
        {
            context.Users.AddOrUpdate(e => e.UserId,
     new Models.User() { UserId = 1, Email = "shohana@gmail.com", Name = "shohana", Password = "123", RoleType = 1 });
        }
    }
}
