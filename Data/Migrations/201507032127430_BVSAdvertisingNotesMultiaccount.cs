namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BVSAdvertisingNotesMultiaccount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.user_advertising",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        url = c.String(nullable: false, unicode: false),
                        screenshot_url = c.String(nullable: false, unicode: false),
                        report_time = c.DateTime(nullable: false, precision: 0),
                        author_id = c.Int(nullable: false),
                        advertising_form_id = c.Int(nullable: false),
                        advertising_from_id = c.Int(nullable: false),
                        advertising_to = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.user_advertising_forms", t => t.advertising_form_id, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.advertising_to)
                .ForeignKey("dbo.system_users", t => t.author_id, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.advertising_from_id, cascadeDelete: true)
                .Index(t => t.author_id)
                .Index(t => t.advertising_form_id)
                .Index(t => t.advertising_from_id)
                .Index(t => t.advertising_to);
            
            CreateTable(
                "dbo.user_advertising_forms",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.user_multiaccount",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserFromId = c.Int(nullable: false),
                        UserToId = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false, precision: 0),
                        ModeratorUserId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Note = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.system_users", t => t.ModeratorUserId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserFromId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserToId, cascadeDelete: true)
                .Index(t => t.UserFromId)
                .Index(t => t.UserToId)
                .Index(t => t.ModeratorUserId);
            
            CreateTable(
                "dbo.user_note_actions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        is_user_selectable = c.Boolean(nullable: false),
                        name = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        code_handle = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.user_notes", "action_id", c => c.Int(nullable: false));
            AddColumn("dbo.user_notes", "deleteable", c => c.Boolean(nullable: false));
            DropColumn("dbo.user_notes", "action");
        }
        
        public override void Down()
        {
            AddColumn("dbo.user_notes", "action", c => c.String(nullable: false, maxLength: 100, storeType: "nvarchar"));
            DropForeignKey("dbo.user_multiaccount", "UserToId", "dbo.system_users");
            DropForeignKey("dbo.user_multiaccount", "UserFromId", "dbo.system_users");
            DropForeignKey("dbo.user_multiaccount", "ModeratorUserId", "dbo.system_users");
            DropForeignKey("dbo.user_advertising", "advertising_from_id", "dbo.system_users");
            DropForeignKey("dbo.user_advertising", "author_id", "dbo.system_users");
            DropForeignKey("dbo.user_advertising", "advertising_to", "dbo.system_users");
            DropForeignKey("dbo.user_advertising", "advertising_form_id", "dbo.user_advertising_forms");
            DropIndex("dbo.user_multiaccount", new[] { "ModeratorUserId" });
            DropIndex("dbo.user_multiaccount", new[] { "UserToId" });
            DropIndex("dbo.user_multiaccount", new[] { "UserFromId" });
            DropIndex("dbo.user_advertising", new[] { "advertising_to" });
            DropIndex("dbo.user_advertising", new[] { "advertising_from_id" });
            DropIndex("dbo.user_advertising", new[] { "advertising_form_id" });
            DropIndex("dbo.user_advertising", new[] { "author_id" });
            DropColumn("dbo.user_notes", "deleteable");
            DropColumn("dbo.user_notes", "action_id");
            DropTable("dbo.user_note_actions");
            DropTable("dbo.user_multiaccount");
            DropTable("dbo.user_advertising_forms");
            DropTable("dbo.user_advertising");
        }
    }
}
