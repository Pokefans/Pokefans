// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MySql.Data.Entity;

    public sealed class Configuration : DbMigrationsConfiguration<Entities>
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

            #region roles
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
            if (!context.Roles.Any(x => x.Name == "artikel-redakteur"))
                context.Roles.Add(
                new Role()
                {
                    Name = "artikel-redakteur",
                    FriendlyName = "Redakteur"
                });
            if (!context.Roles.Any(x => x.Name == "artikel-top-redakteur"))
                context.Roles.Add(
                new Role()
                {
                    Name = "artikel-top-redakteur",
                    FriendlyName = "Top-Redakteur"
                });
            if (!context.Roles.Any(x => x.Name == "artikel-administrator"))
                context.Roles.Add(
                new Role()
                {
                    Name = "artikel-administrator",
                    FriendlyName = "Content-Administrator"
                });
            if (!context.Roles.Any(x => x.Name == "role-manager"))
                context.Roles.Add(
                new Role()
                {
                    Name = "role-manager",
                    FriendlyName = "Zugangsberechtigung: Rechteverwaltung"
                });
            if (!context.Roles.Any(x => x.Name == "global-moderator"))
                context.Roles.Add(
                new Role()
                {
                    Name = "global-moderator",
                    FriendlyName = "Globaler Moderator"
                });
            if (!context.Roles.Any(x => x.Name == "administrator"))
                context.Roles.Add(
                new Role()
                {
                    Name = "administrator",
                    FriendlyName = "Administrator"
                });
            if (!context.Roles.Any(x => x.Name == "bereichsassistent"))
                context.Roles.Add(
                new Role()
                {
                    Name = "bereichsassistent",
                    FriendlyName = "Bereichsassistent"
                });
            if (!context.Roles.Any(x => x.Name == "bereichsleiter"))
                context.Roles.Add(
                new Role()
                {
                    Name = "bereichsleiter",
                    FriendlyName = "Bereichsleiter"
                });
            if (!context.Roles.Any(x => x.Name == "mitarbeit-manager"))
                context.Roles.Add(
                new Role()
                {
                    Name = "mitarbeit-manager",
                    FriendlyName = "Mitarbeitsleiter"
                });

            if (!context.Roles.Any(x => x.Name == "fanart-manager"))
                context.Roles.Add(
                    new Role()
                    {
                        Name = "fanart-manager",
                        FriendlyName = "Fanart-Manager"
                    });

            if (!context.Roles.Any(x => x.Name == "fanart-moderator"))
                context.Roles.Add(
                    new Role()
                    {
                        Name = "fanart-moderator",
                        FriendlyName = "Fanart-Moderator"
                    });
            context.SaveChanges();
            #endregion

            // assign metarols
            #region metaroles
            int managerid = context.Roles.Where(x => x.Name == "mitarbeit-manager").First().Id;

            Role mitarbeiter = context.Roles.Where(x => x.Name == "mitarbeiter").First();
            mitarbeiter.MetapermissionId = managerid;
            context.SetModified(mitarbeiter);

            context.SaveChanges();
            #endregion

            // set up role chain
            #region role chain
            int r1, r2;
            r1 = context.Roles.Where(x => x.Name == "mitarbeit-manager").First().Id;
            r2 = context.Roles.Where(x => x.Name == "mitarbeiter").First().Id;

            if (!context.RoleChain.Any(g => g.ParentRoleId == r1 && g.ChildRoleId == r2))
                context.RoleChain.Add(new RoleChainEntry()
                {
                    ParentRoleId = r1,
                    ChildRoleId = r2
                });


            r1 = context.Roles.Where(x => x.Name == "administrator").First().Id;
            r2 = context.Roles.Where(x => x.Name == "bereichsleiter").First().Id;
            if (!context.RoleChain.Any(g => g.ParentRoleId == r1 && g.ChildRoleId == r2))
                context.RoleChain.Add(new RoleChainEntry()
                {
                    ParentRoleId = r1,
                    ChildRoleId = r2
                });

            r1 = context.Roles.Where(x => x.Name == "bereichsleiter").First().Id;
            r2 = context.Roles.Where(x => x.Name == "global-moderator").First().Id;
            if (!context.RoleChain.Any(g => g.ParentRoleId == r1 && g.ChildRoleId == r2))
                context.RoleChain.Add(new RoleChainEntry()
                {
                    ParentRoleId = r1,
                    ChildRoleId = r2
                });

            r1 = context.Roles.Where(x => x.Name == "global-moderator").First().Id;
            r2 = context.Roles.Where(x => x.Name == "bereichsassistent").First().Id;
            if (!context.RoleChain.Any(g => g.ParentRoleId == r1 && g.ChildRoleId == r2))
                context.RoleChain.Add(new RoleChainEntry()
                {
                    ParentRoleId = r1,
                    ChildRoleId = r2
                });

            r1 = context.Roles.Where(x => x.Name == "administrator").First().Id;
            r2 = context.Roles.Where(x => x.Name == "moderator").First().Id;
            if (!context.RoleChain.Any(g => g.ParentRoleId == r1 && g.ChildRoleId == r2))
                context.RoleChain.Add(new RoleChainEntry()
                {
                    ParentRoleId = r1,
                    ChildRoleId = r2
                });

            r1 = context.Roles.Where(x => x.Name == "bereichsleiter").First().Id;
            r2 = context.Roles.Where(x => x.Name == "moderator").First().Id;
            if (!context.RoleChain.Any(g => g.ParentRoleId == r1 && g.ChildRoleId == r2))
                context.RoleChain.Add(new RoleChainEntry()
                {
                    ParentRoleId = r1,
                    ChildRoleId = r2
                });

            r1 = context.Roles.Where(x => x.Name == "global-moderator").First().Id;
            r2 = context.Roles.Where(x => x.Name == "moderator").First().Id;
            if (!context.RoleChain.Any(g => g.ParentRoleId == r1 && g.ChildRoleId == r2))
                context.RoleChain.Add(new RoleChainEntry()
                {
                    ParentRoleId = r1,
                    ChildRoleId = r2
                });

            r1 = context.Roles.Where(x => x.Name == "bereichsassistent").First().Id;
            r2 = context.Roles.Where(x => x.Name == "moderator").First().Id;
            if (!context.RoleChain.Any(g => g.ParentRoleId == r1 && g.ChildRoleId == r2))
                context.RoleChain.Add(new RoleChainEntry()
                {
                    ParentRoleId = r1,
                    ChildRoleId = r2
                });
            #endregion

            // Advertising Forms
            #region UserAdvertisingForms
            if (!context.UserAdvertisingForms.Any(x => x.Name == "PM"))
                context.UserAdvertisingForms.Add(
                    new UserAdvertisingForm()
                    {
                        Name = "PM",
                        IsTargeted = true
                    });
            if (!context.UserAdvertisingForms.Any(x => x.Name == "Forenpost"))
                context.UserAdvertisingForms.Add(
                    new UserAdvertisingForm()
                    {
                        Name = "Forenpost",
                        IsTargeted = false
                    });
            if (!context.UserAdvertisingForms.Any(x => x.Name == "Signatur"))
                context.UserAdvertisingForms.Add(
                    new UserAdvertisingForm()
                    {
                        Name = "Signatur",
                        IsTargeted = false
                    });
            if (!context.UserAdvertisingForms.Any(x => x.Name == "Chat"))
                context.UserAdvertisingForms.Add(
                    new UserAdvertisingForm()
                    {
                        Name = "Chat",
                        IsTargeted = false
                    });
            if (!context.UserAdvertisingForms.Any(x => x.Name == "Fanart-Galerie"))
                context.UserAdvertisingForms.Add(
                    new UserAdvertisingForm()
                    {
                        Name = "Fanart-Galerie",
                        IsTargeted = false
                    });
            if (!context.UserAdvertisingForms.Any(x => x.Name == "Pokédex-Kommentar"))
                context.UserAdvertisingForms.Add(
                    new UserAdvertisingForm()
                    {
                        Name = "Pokédex-Kommentar",
                        IsTargeted = false
                    });
            context.SaveChanges();
            #endregion

            // User Note Actions
            #region UserNoteActions
            if (!context.UserNoteActions.Any(x => x.Name == "Rechte-Konfiguration"))
                context.UserNoteActions.Add(
                    new UserNoteAction()
                    {
                        Name = "Rechte-Konfiguration",
                        CodeHandle = "roles",
                        IsUserSelectable = false

                    });
            if (!context.UserNoteActions.Any(x => x.Name == "Werbemeldung"))
                context.UserNoteActions.Add(
                    new UserNoteAction()
                    {
                        Name = "Werbemeldung",
                        CodeHandle = "advertising",
                        IsUserSelectable = false
                    });
            if (!context.UserNoteActions.Any(x => x.Name == "Signaturlöschung"))
                context.UserNoteActions.Add(
                    new UserNoteAction()
                    {
                        Name = "Signaturlöschung",
                        CodeHandle = "delete-signature",
                        IsUserSelectable = false
                    });
            if (!context.UserNoteActions.Any(x => x.Name == "Avatarlöschung"))
                context.UserNoteActions.Add(
                    new UserNoteAction()
                    {
                        Name = "Avatarlöschung",
                        CodeHandle = "delete-avatar",
                        IsUserSelectable = false
                    });
            if (!context.UserNoteActions.Any(x => x.Name == "Mini-Avatarlöschung"))
                context.UserNoteActions.Add(
                    new UserNoteAction()
                    {
                        Name = "Mini-Avatarlöschung",
                        CodeHandle = "delete-miniavatar",
                        IsUserSelectable = false
                    });
            if (!context.UserNoteActions.Any(x => x.Name == "Sperre"))
                context.UserNoteActions.Add(
                    new UserNoteAction()
                    {
                        Name = "Sperre",
                        CodeHandle = "lock-account",
                        IsUserSelectable = false
                    });
            if (!context.UserNoteActions.Any(x => x.Name == "Sperre (Tauschbörse)"))
                context.UserNoteActions.Add(
                    new UserNoteAction()
                    {
                        Name = "Sperre (Tauschbörse)",
                        CodeHandle = "lock-account-tb",
                        IsUserSelectable = false
                    });
            if (!context.UserNoteActions.Any(x => x.Name == "Betrug in der Tauschbörse"))
                context.UserNoteActions.Add(
                    new UserNoteAction()
                    {
                        Name = "Betrug in der Tauschbörse",
                        CodeHandle = "tb-fraud",
                        IsUserSelectable = false
                    });
            if (!context.UserNoteActions.Any(x => x.Name == "Doppelaccount"))
                context.UserNoteActions.Add(
                    new UserNoteAction()
                    {
                        Name = "Doppelaccount",
                        CodeHandle = "multiaccount",
                        IsUserSelectable = false
                    });
            if (!context.UserNoteActions.Any(x => x.Name == "..."))
                context.UserNoteActions.Add(
                    new UserNoteAction()
                    {
                        Name = "...",
                        IsUserSelectable = true
                    });
            if (!context.UserNoteActions.Any(x => x.Name == "Erlaubnisanfrage"))
                context.UserNoteActions.Add(
                    new UserNoteAction()
                    {
                        Name = "Erlaubnisanfrage",
                        IsUserSelectable = true
                    });
            if (!context.UserNoteActions.Any(x => x.Name == "Verhalten im Chat"))
                context.UserNoteActions.Add(
                    new UserNoteAction()
                    {
                        Name = "Verhalten im Chat",
                        IsUserSelectable = true
                    });
            if (!context.UserNoteActions.Any(x => x.Name == "Verhalten im Forum"))
                context.UserNoteActions.Add(
                    new UserNoteAction()
                    {
                        Name = "Verhalten im Forum",
                        IsUserSelectable = true
                    });
            if (!context.UserNoteActions.Any(x => x.Name == "Verhalten in der Fanart-Galerie"))
                context.UserNoteActions.Add(
                    new UserNoteAction()
                    {
                        Name = "Verhalten in der Fanart-Galerie",
                        IsUserSelectable = true
                    });
            if (!context.UserNoteActions.Any(x => x.Name == "Sperre im Chat"))
                context.UserNoteActions.Add(
                    new UserNoteAction()
                    {
                        Name = "Sperre im Chat",
                        IsUserSelectable = true
                    });
            if (!context.UserNoteActions.Any(x => x.Name == "Vorschlag zum Rangträger"))
                context.UserNoteActions.Add(
                    new UserNoteAction()
                    {
                        Name = "Vorschlag zum Rangträger",
                        IsUserSelectable = true
                    });

            context.SaveChanges();
            #endregion
        }
    }
}
