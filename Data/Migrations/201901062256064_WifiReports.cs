namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WifiReports : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OfferReports",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Comment = c.String(unicode: false),
                        OfferId = c.Int(nullable: false),
                        ReportedOn = c.DateTime(nullable: false, precision: 0),
                        Resolved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Offers", t => t.OfferId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.OfferId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OfferReports", "UserId", "dbo.system_users");
            DropForeignKey("dbo.OfferReports", "OfferId", "dbo.Offers");
            DropIndex("dbo.OfferReports", new[] { "OfferId" });
            DropIndex("dbo.OfferReports", new[] { "UserId" });
            DropTable("dbo.OfferReports");
        }
    }
}
