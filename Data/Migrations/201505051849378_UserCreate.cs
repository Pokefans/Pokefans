namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.system_permission_log",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        user_id = c.Int(nullable: false),
                        affected_user_id = c.Int(nullable: false),
                        permission_id = c.Int(nullable: false),
                        ip = c.String(nullable: false, maxLength: 39, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.user_id, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.affected_user_id, cascadeDelete: false)
                .ForeignKey("dbo.system_permissions", t => t.permission_id, cascadeDelete: true)
                .Index(t => t.user_id)
                .Index(t => t.affected_user_id)
                .Index(t => t.permission_id);
            
            CreateTable(
                "dbo.system_users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 45, unicode: false),
                        registered = c.DateTime(nullable: false, precision: 0),
                        registered_ip = c.String(nullable: false, maxLength: 39, storeType: "nvarchar"),
                        url = c.String(nullable: false, maxLength: 45, storeType: "nvarchar"),
                        status = c.Byte(nullable: false),
                        ban_reason = c.String(unicode: false),
                        ban_time = c.DateTime(precision: 0),
                        rank = c.String(maxLength: 45, storeType: "nvarchar"),
                        color = c.String(maxLength: 9, storeType: "nvarchar"),
                        unread_notifications = c.Short(),
                        password = c.String(nullable: false, maxLength: 44, storeType: "nvarchar"),
                        salt = c.String(nullable: false, maxLength: 44, storeType: "nvarchar"),
                        activationkey = c.String(maxLength: 32, storeType: "nvarchar"),
                        email = c.String(nullable: false, maxLength: 45, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.name)
                .Index(t => t.url);
            
            CreateTable(
                "dbo.system_user_logins",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        time = c.DateTime(nullable: false, precision: 0),
                        ip = c.String(nullable: false, maxLength: 39, storeType: "nvarchar"),
                        user_id = c.Int(nullable: false),
                        success = c.Byte(nullable: false),
                        reason = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.user_id, cascadeDelete: true)
                .Index(t => t.user_id);
            
            CreateTable(
                "dbo.system_user_persmissions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        user_id = c.Int(nullable: false),
                        permission_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_permissions", t => t.permission_id, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.user_id, cascadeDelete: true)
                .Index(t => t.user_id)
                .Index(t => t.permission_id);
            
            CreateTable(
                "dbo.system_permissions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 45, storeType: "nvarchar"),
                        friendly_name = c.String(maxLength: 45, storeType: "nvarchar"),
                        metapermission_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_permissions", t => t.metapermission_id)
                .Index(t => t.name, unique: true)
                .Index(t => t.friendly_name, unique: true)
                .Index(t => t.metapermission_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.system_user_persmissions", "user_id", "dbo.system_users");
            DropForeignKey("dbo.system_user_persmissions", "permission_id", "dbo.system_permissions");
            DropForeignKey("dbo.system_permission_log", "permission_id", "dbo.system_permissions");
            DropForeignKey("dbo.system_permissions", "metapermission_id", "dbo.system_permissions");
            DropForeignKey("dbo.system_permission_log", "affected_user_id", "dbo.system_users");
            DropForeignKey("dbo.system_user_logins", "user_id", "dbo.system_users");
            DropForeignKey("dbo.system_permission_log", "user_id", "dbo.system_users");
            DropIndex("dbo.system_permissions", new[] { "metapermission_id" });
            DropIndex("dbo.system_permissions", new[] { "friendly_name" });
            DropIndex("dbo.system_permissions", new[] { "name" });
            DropIndex("dbo.system_user_persmissions", new[] { "permission_id" });
            DropIndex("dbo.system_user_persmissions", new[] { "user_id" });
            DropIndex("dbo.system_user_logins", new[] { "user_id" });
            DropIndex("dbo.system_users", new[] { "url" });
            DropIndex("dbo.system_users", new[] { "name" });
            DropIndex("dbo.system_permission_log", new[] { "permission_id" });
            DropIndex("dbo.system_permission_log", new[] { "affected_user_id" });
            DropIndex("dbo.system_permission_log", new[] { "user_id" });
            DropTable("dbo.system_permissions");
            DropTable("dbo.system_user_persmissions");
            DropTable("dbo.system_user_logins");
            DropTable("dbo.system_users");
            DropTable("dbo.system_permission_log");
        }
    }
}
