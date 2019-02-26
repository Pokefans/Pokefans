// Copyright 2017 the pokefans authors. See copying.md for legal info.
namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserFeedMinusReviews : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Reviews", "ReviewItemId", "ReviewItems");
            DropForeignKey("Reviews", "UserId", "system_users");
            DropIndex("Reviews", new[] { "UserId" });
            DropIndex("Reviews", new[] { "ReviewItemId" });
            CreateTable(
                "UserFeedConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ChangedSettings = c.Boolean(nullable: false),
                        NewFanart = c.Short(nullable: false),
                        CommentsOnFanart = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            DropTable("ReviewIndexes");
            DropTable("ReviewItems");
            DropTable("Reviews");
        }
        
        public override void Down()
        {
            CreateTable(
                "Reviews",
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
                "ReviewItems",
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
                "ReviewIndexes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Url = c.String(maxLength: 255, storeType: "nvarchar"),
                        Name = c.String(maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id);
            
            DropForeignKey("UserFeedConfigs", "UserId", "system_users");
            DropIndex("UserFeedConfigs", new[] { "UserId" });
            DropTable("UserFeedConfigs");
            CreateIndex("Reviews", "ReviewItemId");
            CreateIndex("Reviews", "UserId");
            AddForeignKey("Reviews", "UserId", "system_users", "Id", cascadeDelete: true);
            AddForeignKey("Reviews", "ReviewItemId", "ReviewItems", "id", cascadeDelete: true);
        }
    }
}
