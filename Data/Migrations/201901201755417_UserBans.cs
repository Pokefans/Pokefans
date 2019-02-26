namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserBans : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserBans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        IsBanned = c.Boolean(nullable: false),
                        ExpiresOn = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserBans", "UserId", "dbo.system_users");
            DropIndex("dbo.UserBans", new[] { "UserId" });
            DropTable("dbo.UserBans");
        }
    }
}
