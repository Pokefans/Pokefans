namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserFollowers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "UserFollowers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FollowerId = c.Int(nullable: false),
                        FollowedId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("system_users", t => t.FollowedId, cascadeDelete: true)
                .ForeignKey("system_users", t => t.FollowerId, cascadeDelete: true)
                .Index(t => t.FollowerId)
                .Index(t => t.FollowedId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("UserFollowers", "FollowerId", "system_users");
            DropForeignKey("UserFollowers", "FollowedId", "system_users");
            DropIndex("UserFollowers", new[] { "FollowedId" });
            DropIndex("UserFollowers", new[] { "FollowerId" });
            DropTable("UserFollowers");
        }
    }
}
