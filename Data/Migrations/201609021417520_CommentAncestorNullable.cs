namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentAncestorNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("CommentAncestors", "AncestorId", "Comments");
            DropIndex("CommentAncestors", new[] { "AncestorId" });
            AlterColumn("CommentAncestors", "AncestorId", c => c.Int());
            CreateIndex("CommentAncestors", "AncestorId");
            AddForeignKey("CommentAncestors", "AncestorId", "Comments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("CommentAncestors", "AncestorId", "Comments");
            DropIndex("CommentAncestors", new[] { "AncestorId" });
            AlterColumn("CommentAncestors", "AncestorId", c => c.Int(nullable: false));
            CreateIndex("CommentAncestors", "AncestorId");
            AddForeignKey("CommentAncestors", "AncestorId", "Comments", "Id", cascadeDelete: true);
        }
    }
}
