namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserColorClass : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.system_users", "color", c => c.String(maxLength: 25, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.system_users", "color", c => c.String(maxLength: 9, storeType: "nvarchar"));
        }
    }
}
