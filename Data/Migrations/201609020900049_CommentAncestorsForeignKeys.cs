namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentAncestorsForeignKeys : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.CommentAncestors", "AncestorId", "dbo.Comments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CommentAncestors", "CommentId", "dbo.Comments", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentAncestors", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.CommentAncestors", "AncestorId", "dbo.Comments");
        }
    }
}
