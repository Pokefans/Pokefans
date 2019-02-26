namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserNotes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.user_notes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        content = c.String(nullable: false, unicode: false),
                        unparsed_content = c.String(unicode: false),
                        action = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        author_id = c.Int(nullable: false),
                        user_id = c.Int(nullable: false),
                        creation_time = c.DateTime(nullable: false, precision: 0),
                        role_id_needed = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_permissions", t => t.role_id_needed, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.author_id, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.user_id, cascadeDelete: true)
                .Index(t => t.author_id)
                .Index(t => t.user_id)
                .Index(t => t.role_id_needed);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.user_notes", "user_id", "dbo.system_users");
            DropForeignKey("dbo.user_notes", "author_id", "dbo.system_users");
            DropForeignKey("dbo.user_notes", "role_id_needed", "dbo.system_permissions");
            DropIndex("dbo.user_notes", new[] { "role_id_needed" });
            DropIndex("dbo.user_notes", new[] { "user_id" });
            DropIndex("dbo.user_notes", new[] { "author_id" });
            DropTable("dbo.user_notes");
        }
    }
}
