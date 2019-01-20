using System;
using System.Data.Entity.Migrations;
namespace Pokefans.Data.Migrations
{


    
    public partial class TrackingSource : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tracking_sources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TargetUrl = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        SourceUrl = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        SourceHost = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                        SearchTerm = c.String(maxLength: 255, storeType: "nvarchar"),
                        RequestTime = c.DateTime(nullable: false, precision: 0),
                        IpAdress = c.String(nullable: false, maxLength: 46, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.content_tracking",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ContentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tracking_sources", t => t.Id)
                .ForeignKey("dbo.content", t => t.ContentId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.ContentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.content_tracking", "ContentId", "dbo.content");
            DropForeignKey("dbo.content_tracking", "Id", "dbo.tracking_sources");
            DropIndex("dbo.content_tracking", new[] { "ContentId" });
            DropIndex("dbo.content_tracking", new[] { "Id" });
            DropTable("dbo.content_tracking");
            DropTable("dbo.tracking_sources");
        }
    }
}
