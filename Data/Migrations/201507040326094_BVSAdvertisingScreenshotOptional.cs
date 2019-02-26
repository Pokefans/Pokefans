namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BVSAdvertisingScreenshotOptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.user_advertising", "screenshot_url", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.user_advertising", "screenshot_url", c => c.String(nullable: false, unicode: false));
        }
    }
}
