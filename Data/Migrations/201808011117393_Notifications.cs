namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserNotifications", "Icon", c => c.String(unicode: false));
            DropColumn("dbo.UserNotifications", "ReadTime");
            DropColumn("dbo.UserNotifications", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserNotifications", "Title", c => c.String(unicode: false));
            AddColumn("dbo.UserNotifications", "ReadTime", c => c.DateTime(nullable: false, precision: 0));
            DropColumn("dbo.UserNotifications", "Icon");
        }
    }
}
