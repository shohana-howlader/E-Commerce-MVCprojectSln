namespace MVCproject.Migrations
{
    using MVCproject.Models;
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

            context.Categories.AddOrUpdate(
           c => c.CategoryId,
           new Category { CategoryId = 1, Name = "Man" },
           new Category { CategoryId = 2, Name = "Woman" },
           new Category { CategoryId = 3, Name = "Bags" },
           new Category { CategoryId = 4, Name = "Accessories" },
           new Category { CategoryId = 5, Name = "Baby" },
           new Category { CategoryId = 6, Name = "Shoes" }
       );

            // Add initial products
            context.Products.AddOrUpdate(
                p => p.ProductId,
                new Product { ProductId = 1, Name = "Piqué Biker Jacket", Description = "new", Unit = 4000, Image = "product-4.jpg", Popularity = 1, CategoryId = 1 },
                new Product { ProductId = 2, Name = "Casual Shoes", Description = null, Unit = 4000, Image = "product-1.jpg", Popularity = 1, CategoryId = 6 },
                new Product { ProductId = 3, Name = "Multi-pocket Chest Bag", Description = "new", Unit = 5000, Image = "product-11.jpg", Popularity = 1, CategoryId = 3 },
                new Product { ProductId = 4, Name = "T-shirt", Description = "new", Unit = 2000, Image = "product-8.jpg", Popularity = 1, CategoryId = 1 },
                new Product { ProductId = 5, Name = "T-shirt", Description = "new", Unit = 2000, Image = "product-9.jpg", Popularity = 1, CategoryId = 2 },
                new Product { ProductId = 7, Name = "Sunglass", Description = "Don't miss the exclusive sale", Unit = 1000, Image = "sungless.jpg", Popularity = 1, CategoryId = 4 },
                new Product { ProductId = 8, Name = "Multi-pocket Chest Bag", Description = "Don't miss the exclusive sale", Unit = 5000, Image = "product-13.jpg", Popularity = 1, CategoryId = 3 },
                new Product { ProductId = 9, Name = "Biker Jacket", Description = "exclusive", Unit = 2000, Image = "product-2.jpg", Popularity = 1, CategoryId = 2 },
                new Product { ProductId = 10, Name = "Shoes", Description = "new", Unit = 3000, Image = "product-3.jpg", Popularity = 1, CategoryId = 6 },
                new Product { ProductId = 11, Name = "Perfume", Description = "exclusive", Unit = 1000, Image = "product-10.jpg", Popularity = 1, CategoryId = 4 }
            );

            // Add initial users
            context.Users.AddOrUpdate(
                u => u.UserId,
                new User { UserId = 1, Name = "Shohana", Email = "shohana@gmail.com", Password = "123", RoleType = 1 },
                new User { UserId = 2, Name = "Farad", Email = "farad@gmail.com", Password = "1020", RoleType = 2 },
                new User { UserId = 3, Name = "Mahamud", Email = "mahamud@gmail.com", Password = "1020", RoleType = 2 },
                new User { UserId = 4, Name = "Mahamud", Email = "mahamud@gmail.com", Password = "1020", RoleType = 2 },
                new User { UserId = 5, Name = "Mahamud", Email = "mahamud@gmail.com", Password = "1236", RoleType = 2 },
                new User { UserId = 6, Name = "Aysha", Email = "AyshaMahamud@gmail.com", Password = "3214", RoleType = 2 },
                new User { UserId = 7, Name = "Mansur", Email = "mansur@gmail.com", Password = "4569", RoleType = 2 }
            );
        }
    }
}
