namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FanartBanlistRatings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FanartBanlists",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CanUpload = c.Boolean(nullable: false),
                        CanRate = c.Boolean(nullable: false),
                        CanEdit = c.Boolean(nullable: false),
                        CanDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.FanartRatings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Rating = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FanartId = c.Int(nullable: false),
                        RatingUserId = c.Int(nullable: false),
                        RatingTime = c.DateTime(nullable: false, precision: 0),
                        RatingIp = c.String(maxLength: 47, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.RatingUserId, cascadeDelete: true)
                .ForeignKey("dbo.Fanarts", t => t.FanartId, cascadeDelete: true)
                .Index(t => t.FanartId)
                .Index(t => t.RatingUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FanartRatings", "FanartId", "dbo.Fanarts");
            DropForeignKey("dbo.FanartRatings", "RatingUserId", "dbo.system_users");
            DropForeignKey("dbo.FanartBanlists", "UserId", "dbo.system_users");
            DropIndex("dbo.FanartRatings", new[] { "RatingUserId" });
            DropIndex("dbo.FanartRatings", new[] { "FanartId" });
            DropIndex("dbo.FanartBanlists", new[] { "UserId" });
            DropTable("dbo.FanartRatings");
            DropTable("dbo.FanartBanlists");
        }
    }
}
