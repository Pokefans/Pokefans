namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleChain : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.system_role_chain",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        parent_role_id = c.Int(nullable: false),
                        child_role_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_permissions", t => t.child_role_id, cascadeDelete: true)
                .ForeignKey("dbo.system_permissions", t => t.parent_role_id, cascadeDelete: true)
                .Index(t => t.parent_role_id)
                .Index(t => t.child_role_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.system_role_chain", "parent_role_id", "dbo.system_permissions");
            DropForeignKey("dbo.system_role_chain", "child_role_id", "dbo.system_permissions");
            DropIndex("dbo.system_role_chain", new[] { "child_role_id" });
            DropIndex("dbo.system_role_chain", new[] { "parent_role_id" });
            DropTable("dbo.system_role_chain");
        }
    }
}
