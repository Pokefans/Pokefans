namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Comments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentAncestors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentId = c.Int(nullable: false),
                        AncestorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CommentId)
                .Index(t => t.AncestorId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorId = c.Int(nullable: false),
                        SubmitTime = c.DateTime(nullable: false, precision: 0),
                        EditTime = c.DateTime(nullable: false, precision: 0),
                        UnparsedComment = c.String(maxLength: 8096, storeType: "nvarchar"),
                        ParsedComment = c.String(maxLength: 8096, storeType: "nvarchar"),
                        DisplayPublic = c.Boolean(nullable: false),
                        CommentedObjectId = c.Int(nullable: false),
                        ParentCommentId = c.Int(),
                        Level = c.Short(nullable: false),
                        Context = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.system_users", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.ParentCommentId)
                .Index(t => t.AuthorId)
                .Index(t => t.ParentCommentId);
            
            AddColumn("dbo.content", "CommentsEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.content", "CommentCount", c => c.Int(nullable: false));
            AddColumn("dbo.Fanarts", "CommentCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "ParentCommentId", "dbo.Comments");
            DropForeignKey("dbo.Comments", "AuthorId", "dbo.system_users");
            DropIndex("dbo.Comments", new[] { "ParentCommentId" });
            DropIndex("dbo.Comments", new[] { "AuthorId" });
            DropIndex("dbo.CommentAncestors", new[] { "AncestorId" });
            DropIndex("dbo.CommentAncestors", new[] { "CommentId" });
            DropColumn("dbo.Fanarts", "CommentCount");
            DropColumn("dbo.content", "CommentCount");
            DropColumn("dbo.content", "CommentsEnabled");
            DropTable("dbo.Comments");
            DropTable("dbo.CommentAncestors");
        }
    }
}
