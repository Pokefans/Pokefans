namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumAllowNullLastPost : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Boards", "LastPostId", "Posts");
            DropIndex("Boards", new[] { "LastPostId" });
            AlterColumn("Boards", "LastPostId", c => c.Int());
            CreateIndex("Boards", "LastPostId");
            AddForeignKey("Boards", "LastPostId", "Posts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Boards", "LastPostId", "Posts");
            DropIndex("Boards", new[] { "LastPostId" });
            AlterColumn("Boards", "LastPostId", c => c.Int(nullable: false));
            CreateIndex("Boards", "LastPostId");
            AddForeignKey("Boards", "LastPostId", "Posts", "Id", cascadeDelete: true);
        }
    }
}
