// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokefans.Data.Migrations
{
    /// <summary>
    /// This class just exists to work around the *cough* perfectly working *cough* stuff from oracle
    /// Who would've thought. Problem is that indexes can maximum be 767 bytes long, but using unicode (utf8) charset exceeds that. THANKS ORACLE.
    /// </summary>
    public class MySqlHistoryContext : HistoryContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlHistoryContext"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="defaultSchema">The default schema.</param>
        public MySqlHistoryContext(DbConnection connection, string defaultSchema) : base(connection, defaultSchema)
        {

        }

        /// <summary>
        /// Übernimmt die Standardkonfiguration für die Migrationsverlaufstabelle.Wenn Sie diese Methode überschreiben, wird empfohlen, dass Sie diese Basisimplementierung aufrufen, bevor Sie die benutzerdefinierte Konfiguration anwenden.
        /// </summary>
        /// <param name="modelBuilder">Der Generator, der das Modell für den zu erstellenden Kontext definiert.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HistoryRow>().Property(h => h.MigrationId).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<HistoryRow>().Property(h => h.ContextKey).HasMaxLength(200).IsRequired();
        }
    }
}
