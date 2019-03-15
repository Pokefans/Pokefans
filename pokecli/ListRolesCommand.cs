using System;
using System.Linq;
using ManyConsole;
using Pokefans.Data;

namespace pokecli
{
    public class ListRolesCommand : ConsoleCommand
    {
        public ListRolesCommand()
        {
            IsCommand("list-roles", "Lists all Roles and handles for the add-role Command.");
        }

        public override int Run(string[] remainingArguments)
        {
            Entities db = new Entities();

            var roles = db.Roles.ToList();

            int leftlongest = "Code Handle".Length;
            int rightlongest = "Friendly Name".Length;

            foreach(var role in roles)
            {
                if (role.Name.Length > leftlongest)
                    leftlongest = role.Name.Length;

                if (role.FriendlyName.Length > rightlongest)
                    rightlongest = role.FriendlyName.Length;
            }

            leftlongest += 4;
            rightlongest += 2;

            Console.Write("Code Handle".PadRight(leftlongest));
            Console.Write("Firendly Name");
            Console.WriteLine();

            for (int i = 0; i < (rightlongest + leftlongest); i++)
                Console.Write("-");

            Console.WriteLine();

            foreach(var role in roles)
            {
                Console.WriteLine(role.Name.PadRight(leftlongest) + role.FriendlyName);
            }
            Console.WriteLine();

            return 0;

        }
    }
}
