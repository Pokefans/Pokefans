namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumNullableAuthors : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Threads", "AuthorId", "system_users");
            DropIndex("Threads", new[] { "AuthorId" });
            AlterColumn("Posts", "LastEditTime", c => c.DateTime(precision: 0));
            AlterColumn("Threads", "AuthorId", c => c.Int());
            CreateIndex("Threads", "AuthorId");
            AddForeignKey("Threads", "AuthorId", "system_users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Threads", "AuthorId", "system_users");
            DropIndex("Threads", new[] { "AuthorId" });
            AlterColumn("Threads", "AuthorId", c => c.Int(nullable: false));
            AlterColumn("Posts", "LastEditTime", c => c.DateTime(nullable: false, precision: 0));
            CreateIndex("Threads", "AuthorId");
            AddForeignKey("Threads", "AuthorId", "system_users", "Id", cascadeDelete: true);
        }
    }
}
