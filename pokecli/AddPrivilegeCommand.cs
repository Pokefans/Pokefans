using System;
using ManyConsole;
using System.Linq;
using Pokefans.Data;

namespace pokecli
{
    public class AddPrivilegeCommand : ConsoleCommand
    {

        private string username;
        private string role;

        public AddPrivilegeCommand()
        {
            IsCommand("add-role", "Adds an existing user to a role.");
            // Required options/flags, append '=' to obtain the required value.
            HasRequiredOption("u|user=", "The Username.", p => username = p);
            // Required options/flags, append '=' to obtain the required value.
            HasRequiredOption("r|role=", "The role code handle. Use list-roles to see a list of them", p => role = p);
        }

        public override int Run(string[] remainingArguments)
        {
            Entities db = new Entities();

            User u = db.Users.FirstOrDefault(g => g.UserName == username);

            if(u == null)
            {
                Console.Error.WriteLine("Username not found.");
                return -1;
            }

            Role r = db.Roles.FirstOrDefault(g => g.Name == role);

            if(r == null)
            {
                Console.Error.WriteLine("Role not found");
                return -2;
            }

            do
            {
                db.UserRoles.Add(new UserRole()
                {
                    PermissionId = r.Id,
                    UserId = u.Id
                });
                Console.WriteLine("Added User to Role {0} ({1})", r.FriendlyName, r.Name);

                if (r.MetapermissionId != null)
                    r = db.Roles.Find(r.MetapermissionId);
            }
            while (r.MetapermissionId != null);

            db.SaveChanges();

            return 0;
        }
    }
}
