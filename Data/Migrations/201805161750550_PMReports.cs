namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PMReports : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PrivateMessageReports",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PrivateMessageId = c.Int(nullable: false),
                        UserFromId = c.Int(nullable: false),
                        UserReportId = c.Int(nullable: false),
                        Details = c.String(unicode: false),
                        Timestamp = c.DateTime(nullable: false, precision: 0),
                        Resolved = c.Boolean(nullable: false),
                        UserResolveId = c.Int(),
                        ResolveTime = c.DateTime(precision: 0),
                        ModeratorNotes = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.UserFromId, cascadeDelete: true)
                .ForeignKey("dbo.PrivateMessages", t => t.PrivateMessageId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserReportId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserResolveId)
                .Index(t => t.PrivateMessageId)
                .Index(t => t.UserFromId)
                .Index(t => t.UserReportId)
                .Index(t => t.UserResolveId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrivateMessageReports", "UserResolveId", "dbo.system_users");
            DropForeignKey("dbo.PrivateMessageReports", "UserReportId", "dbo.system_users");
            DropForeignKey("dbo.PrivateMessageReports", "PrivateMessageId", "dbo.PrivateMessages");
            DropForeignKey("dbo.PrivateMessageReports", "UserFromId", "dbo.system_users");
            DropIndex("dbo.PrivateMessageReports", new[] { "UserResolveId" });
            DropIndex("dbo.PrivateMessageReports", new[] { "UserReportId" });
            DropIndex("dbo.PrivateMessageReports", new[] { "UserFromId" });
            DropIndex("dbo.PrivateMessageReports", new[] { "PrivateMessageId" });
            DropTable("dbo.PrivateMessageReports");
        }
    }
}
