namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gravatar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.system_users", "GravatarEnabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.system_users", "GravatarEnabled");
        }
    }
}
