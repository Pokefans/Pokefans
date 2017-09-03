namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserFeedMinusReviews : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reviews", "ReviewItemId", "dbo.ReviewItems");
            DropForeignKey("dbo.Reviews", "UserId", "dbo.system_users");
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropIndex("dbo.Reviews", new[] { "ReviewItemId" });
            CreateTable(
                "dbo.UserFeedConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ChangedSettings = c.Boolean(nullable: false),
                        NewFanart = c.Short(nullable: false),
                        CommentsOnFanart = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            DropTable("dbo.ReviewIndexes");
            DropTable("dbo.ReviewItems");
            DropTable("dbo.Reviews");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 255, storeType: "nvarchar"),
                        Rating = c.Byte(nullable: false),
                        Text = c.String(unicode: false),
                        TextCode = c.String(unicode: false),
                        CreatorIp = c.String(unicode: false),
                        CreationTime = c.DateTime(nullable: false, precision: 0),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        UserId = c.Int(nullable: false),
                        ReviewItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ReviewItems",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Url = c.String(maxLength: 255, storeType: "nvarchar"),
                        Name = c.String(maxLength: 255, storeType: "nvarchar"),
                        Rating = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RatingCount = c.Int(nullable: false),
                        Requests = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ReviewIndexes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Url = c.String(maxLength: 255, storeType: "nvarchar"),
                        Name = c.String(maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id);
            
            DropForeignKey("dbo.UserFeedConfigs", "UserId", "dbo.system_users");
            DropIndex("dbo.UserFeedConfigs", new[] { "UserId" });
            DropTable("dbo.UserFeedConfigs");
            CreateIndex("dbo.Reviews", "ReviewItemId");
            CreateIndex("dbo.Reviews", "UserId");
            AddForeignKey("dbo.Reviews", "UserId", "dbo.system_users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Reviews", "ReviewItemId", "dbo.ReviewItems", "id", cascadeDelete: true);
        }
    }
}
