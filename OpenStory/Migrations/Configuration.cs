namespace OpenStory.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using OpenStory.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<OpenStory.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OpenStory.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            ApplicationUser user = context.Users.Single(u => u.Email == "alex@gmail.com");

            context.Topics.AddOrUpdate(x => x.Id,
                new Topic() { Title = "Dks Suck", ApplicationUser = user, PostDate = DateTime.Now, Content = "Nothing", Likes = 0, Dislikes = 0} ,
                 new Topic() { Title = "OMG ROGUES ARE SO OP", ApplicationUser = user, PostDate = DateTime.Now, Content = "Nothing", Likes = 0, Dislikes = 0 },
                  new Topic() { Title = "OMG DESTRO LOCKS" , ApplicationUser = user, PostDate = DateTime.Now, Content = "Nothing", Likes = 0, Dislikes = 0 }
                );
        }
    }
}
