// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MySql.Data.Entity;

    internal sealed class Configuration : DbMigrationsConfiguration<Entities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("MySql.Data.MySqlClient", new MySqlMigrationSqlGenerator());
            SetHistoryContextFactory("MySql.Data.MySqlClient", (conn, schema) => new MySqlHistoryContext(conn, schema));
        }

        protected override void Seed(Entities context)
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


            // If you add a Role in Code, be sure to add it here.
            // Although it shouldn't matter, please add it *to the end of the list*.

            if (!context.Roles.Any(x => x.Name == "superadmin"))
                context.Roles.Add(new Role()
                {
                    Name = "superadmin",
                    FriendlyName = "Super-Administrator"
                });

            if (!context.Roles.Any(x => x.Name == "moderator"))
                context.Roles.Add(
                new Role()
                {
                    Name = "moderator",
                    FriendlyName = "Moderator"
                });
            if (!context.Roles.Any(x => x.Name == "mitarbeiter"))
                context.Roles.Add(
                new Role()
                {
                    Name = "mitarbeiter",
                    FriendlyName = "Mitarbeiter"
                });
            context.SaveChanges();


        }
    }
}
