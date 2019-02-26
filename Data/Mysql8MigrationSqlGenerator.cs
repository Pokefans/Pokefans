using System;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using MySql.Data.Entity;

namespace Pokefans.Data
{
    /// <summary>
    /// The whole purpose of this generator is to fix the rather stupid beavior
    /// of the original generator of generating invalid indices. well.
    /// </summary>
    public class Mysql8MigrationSqlGenerator : MySqlMigrationSqlGenerator
    {
        public Mysql8MigrationSqlGenerator() 
            : base()
        {
        }

        protected override MigrationStatement Generate(CreateIndexOperation op)
        {
            MigrationStatement migrationStatement = base.Generate(op);

            string old = migrationStatement.Sql.TrimEnd();

            if (old.EndsWith("using HASH", StringComparison.OrdinalIgnoreCase))
            {
                migrationStatement.Sql = old.Replace("using HASH", " using BTREE");
            }

            return migrationStatement;
        }
    }
}
