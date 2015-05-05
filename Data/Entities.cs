// Copyright 2015 the pokefans-core authors. See copying.md for legal info.
namespace Pokefans.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using MySql.Data.Entity;

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class Entities : DbContext
    {
        // Der Kontext wurde für die Verwendung einer Pokefans-Verbindungszeichenfolge aus der 
        // Konfigurationsdatei ('App.config' oder 'Web.config') der Anwendung konfiguriert. Diese Verbindungszeichenfolge hat standardmäßig die 
        // Datenbank 'Pokefans.Data.Pokefans' auf der LocalDb-Instanz als Ziel. 
        // 
        // Wenn Sie eine andere Datenbank und/oder einen anderen Anbieter als Ziel verwenden möchten, ändern Sie die Pokefans-Zeichenfolge 
        // in der Anwendungskonfigurationsdatei.
        public Entities()
            : base()
        {
        }

        /// <summary>
        /// This sets the specified Entity modified. It just exists to work around a limitation in Entity Framework (Unit Tests hooraay)
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        // Fügen Sie ein 'DbSet' für jeden Entitätstyp hinzu, den Sie in das Modell einschließen möchten. Weitere Informationen 
        // zum Konfigurieren und Verwenden eines Code First-Modells finden Sie unter 'http://go.microsoft.com/fwlink/?LinkId=390109'.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }

        public virtual DbSet<PermissionLogEntry> PermissionLogEntries { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserLogin> UserLogins { get; set; }

        public virtual DbSet<UserPermission> UserPermissions { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}