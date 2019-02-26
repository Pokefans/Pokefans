namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BanSomething : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.system_users", "phpBBPassword", c => c.String(maxLength: 34, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.system_users", "phpBBPassword");
        }
    }
}
