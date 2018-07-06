namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrivateMessages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PrivateMessageLabels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        OwnerUserId = c.Int(nullable: false),
                        Label = c.String(maxLength: 50, storeType: "nvarchar"),
                        Color = c.String(maxLength: 9, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.OwnerUserId, cascadeDelete: true)
                .Index(t => t.OwnerUserId);
            
            CreateTable(
                "dbo.PrivateMessages",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Subject = c.String(maxLength: 100, storeType: "nvarchar"),
                        Body = c.String(unicode: false),
                        BodyRaw = c.String(unicode: false),
                        Sent = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.PrivateMessageInboxes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PrivateMessageId = c.Int(nullable: false),
                        UserFromId = c.Int(nullable: false),
                        UserToId = c.Int(nullable: false),
                        Read = c.Boolean(nullable: false),
                        IsBcc = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.UserFromId, cascadeDelete: true)
                .ForeignKey("dbo.PrivateMessages", t => t.PrivateMessageId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserToId, cascadeDelete: true)
                .Index(t => t.PrivateMessageId)
                .Index(t => t.UserFromId)
                .Index(t => t.UserToId);
            
            CreateTable(
                "dbo.PrivateMessageInboxLabels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PrivateMessageInboxId = c.Int(nullable: false),
                        PrivateMessageLabelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.PrivateMessageLabels", t => t.PrivateMessageLabelId, cascadeDelete: true)
                .ForeignKey("dbo.PrivateMessageInboxes", t => t.PrivateMessageInboxId, cascadeDelete: true)
                .Index(t => t.PrivateMessageInboxId)
                .Index(t => t.PrivateMessageLabelId);
            
            CreateTable(
                "dbo.PrivateMessageSents",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PrivateMessageId = c.Int(nullable: false),
                        UserFromId = c.Int(nullable: false),
                        UserToId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.UserFromId, cascadeDelete: true)
                .ForeignKey("dbo.PrivateMessages", t => t.PrivateMessageId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserToId, cascadeDelete: true)
                .Index(t => t.PrivateMessageId)
                .Index(t => t.UserFromId)
                .Index(t => t.UserToId);
            
            CreateTable(
                "dbo.PrivateMessageSentLabels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PrivateMessageInboxId = c.Int(nullable: false),
                        PrivateMessageLabelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.PrivateMessageLabels", t => t.PrivateMessageLabelId, cascadeDelete: true)
                //.ForeignKey("dbo.PrivateMessageInboxes", t => t.PrivateMessageInboxId, cascadeDelete: true)
                .Index(t => t.PrivateMessageInboxId)
                .Index(t => t.PrivateMessageLabelId);
            
            AddColumn("dbo.UserProfiles", "Pronoun", c => c.String(maxLength: 20, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrivateMessageSentLabels", "PrivateMessageInboxId", "dbo.PrivateMessageInboxes");
            DropForeignKey("dbo.PrivateMessageSentLabels", "PrivateMessageLabelId", "dbo.PrivateMessageLabels");
            DropForeignKey("dbo.PrivateMessageSents", "UserToId", "dbo.system_users");
            DropForeignKey("dbo.PrivateMessageSents", "PrivateMessageId", "dbo.PrivateMessages");
            DropForeignKey("dbo.PrivateMessageSents", "UserFromId", "dbo.system_users");
            DropForeignKey("dbo.PrivateMessageInboxLabels", "PrivateMessageInboxId", "dbo.PrivateMessageInboxes");
            DropForeignKey("dbo.PrivateMessageInboxLabels", "PrivateMessageLabelId", "dbo.PrivateMessageLabels");
            DropForeignKey("dbo.PrivateMessageInboxes", "UserToId", "dbo.system_users");
            DropForeignKey("dbo.PrivateMessageInboxes", "PrivateMessageId", "dbo.PrivateMessages");
            DropForeignKey("dbo.PrivateMessageInboxes", "UserFromId", "dbo.system_users");
            DropForeignKey("dbo.PrivateMessageLabels", "OwnerUserId", "dbo.system_users");
            DropIndex("dbo.PrivateMessageSentLabels", new[] { "PrivateMessageLabelId" });
            DropIndex("dbo.PrivateMessageSentLabels", new[] { "PrivateMessageInboxId" });
            DropIndex("dbo.PrivateMessageSents", new[] { "UserToId" });
            DropIndex("dbo.PrivateMessageSents", new[] { "UserFromId" });
            DropIndex("dbo.PrivateMessageSents", new[] { "PrivateMessageId" });
            DropIndex("dbo.PrivateMessageInboxLabels", new[] { "PrivateMessageLabelId" });
            DropIndex("dbo.PrivateMessageInboxLabels", new[] { "PrivateMessageInboxId" });
            DropIndex("dbo.PrivateMessageInboxes", new[] { "UserToId" });
            DropIndex("dbo.PrivateMessageInboxes", new[] { "UserFromId" });
            DropIndex("dbo.PrivateMessageInboxes", new[] { "PrivateMessageId" });
            DropIndex("dbo.PrivateMessageLabels", new[] { "OwnerUserId" });
            DropColumn("dbo.UserProfiles", "Pronoun");
            DropTable("dbo.PrivateMessageSentLabels");
            DropTable("dbo.PrivateMessageSents");
            DropTable("dbo.PrivateMessageInboxLabels");
            DropTable("dbo.PrivateMessageInboxes");
            DropTable("dbo.PrivateMessages");
            DropTable("dbo.PrivateMessageLabels");
        }
    }
}
