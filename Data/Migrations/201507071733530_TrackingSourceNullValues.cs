namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackingSourceNullValues : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tracking_sources", "TargetUrl", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.tracking_sources", "SourceUrl", c => c.String(maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.tracking_sources", "SourceHost", c => c.String(maxLength: 255, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tracking_sources", "SourceHost", c => c.String(nullable: false, maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.tracking_sources", "SourceUrl", c => c.String(nullable: false, maxLength: 255, storeType: "nvarchar"));
            AlterColumn("dbo.tracking_sources", "TargetUrl", c => c.String(nullable: false, maxLength: 255, storeType: "nvarchar"));
        }
    }
}
