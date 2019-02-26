namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Content : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.content_boilerplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContentId = c.Int(nullable: false),
                        BoilerplateId = c.Int(nullable: false),
                        ContentBoilerplateName = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.content", t => t.ContentId, cascadeDelete: true)
                .ForeignKey("dbo.content", t => t.BoilerplateId, cascadeDelete: true)
                .Index(t => t.ContentId)
                .Index(t => t.BoilerplateId);
            
            CreateTable(
                "dbo.content",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 255, storeType: "nvarchar"),
                        UnparsedContent = c.String(unicode: false),
                        ParsedContent = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        StylesheetName = c.String(maxLength: 100, storeType: "nvarchar"),
                        StylesheetCode = c.String(unicode: false),
                        Teaser = c.String(unicode: false),
                        Notes = c.String(unicode: false),
                        Status = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        EditPermissionId = c.Int(nullable: false),
                        HomePageOptions = c.Int(nullable: false),
                        ViewCount = c.String(unicode: false),
                        Published = c.DateTime(nullable: false, precision: 0),
                        PublishedByUserId = c.Int(nullable: false),
                        Updated = c.DateTime(nullable: false, precision: 0),
                        AuthorUserId = c.Int(nullable: false),
                        DefaultUrlId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.system_users", t => t.AuthorUserId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.PublishedByUserId, cascadeDelete: true)
                .ForeignKey("dbo.content_categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.content_urls", t => t.DefaultUrlId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.PublishedByUserId)
                .Index(t => t.AuthorUserId)
                .Index(t => t.DefaultUrlId);
            
            CreateTable(
                "dbo.content_categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, unicode: false),
                        AreaName = c.String(nullable: false, unicode: false),
                        ForumId = c.Int(nullable: false),
                        SidebarContentId = c.Int(nullable: false),
                        OrderingPosition = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.content", t => t.SidebarContentId, cascadeDelete: true)
                .Index(t => t.SidebarContentId);
            
            CreateTable(
                "dbo.content_urls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false, maxLength: 90, storeType: "nvarchar"),
                        ContentId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.content", t => t.ContentId, cascadeDelete: true)
                .Index(t => t.Url, unique: true, name: "idx_content_url")
                .Index(t => t.ContentId);
            
            CreateTable(
                "dbo.feedback",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Priority = c.Int(nullable: false),
                        AuthorId = c.Int(nullable: false),
                        IpAddress = c.String(nullable: false, unicode: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Edited = c.DateTime(nullable: false, precision: 0),
                        EditorId = c.Int(nullable: false),
                        Note = c.String(unicode: false),
                        Quality = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.system_users", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.EditorId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.EditorId);
            
            CreateTable(
                "dbo.content_versions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContentId = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                        Title = c.String(maxLength: 255, storeType: "nvarchar"),
                        UnparsedContent = c.String(unicode: false),
                        ParsedContent = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        StylesheetName = c.String(maxLength: 100, storeType: "nvarchar"),
                        StylesheetCode = c.String(unicode: false),
                        Teaser = c.String(unicode: false),
                        Notes = c.String(unicode: false),
                        UserId = c.Int(nullable: false),
                        Note = c.String(unicode: false),
                        Updated = c.DateTime(nullable: false, precision: 0),
                        UpdateMagnificance = c.Double(nullable: false),
                        UpdateCharsChanged = c.Int(nullable: false),
                        UpdateCharsDeleted = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.content", t => t.ContentId, cascadeDelete: true)
                .Index(t => t.ContentId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.todos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Priority = c.Int(nullable: false),
                        AuthorId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Edited = c.DateTime(nullable: false, precision: 0),
                        AssigneeId = c.Int(nullable: false),
                        Title = c.String(nullable: false, unicode: false),
                        Note = c.String(unicode: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.system_users", t => t.AssigneeId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.AssigneeId);
            
            CreateTable(
                "dbo.content_feedback",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ContentId = c.Int(nullable: false),
                        Proposal = c.String(unicode: false),
                        ProposedTitle = c.String(unicode: false),
                        ProposedStylesheet = c.String(unicode: false),
                        ProposedTeaser = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.feedback", t => t.Id)
                .ForeignKey("dbo.content", t => t.ContentId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.ContentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.content_feedback", "ContentId", "dbo.content");
            DropForeignKey("dbo.content_feedback", "Id", "dbo.feedback");
            DropForeignKey("dbo.todos", "AuthorId", "dbo.system_users");
            DropForeignKey("dbo.todos", "AssigneeId", "dbo.system_users");
            DropForeignKey("dbo.feedback", "EditorId", "dbo.system_users");
            DropForeignKey("dbo.feedback", "AuthorId", "dbo.system_users");
            DropForeignKey("dbo.content_versions", "ContentId", "dbo.content");
            DropForeignKey("dbo.content_versions", "UserId", "dbo.system_users");
            DropForeignKey("dbo.content", "DefaultUrlId", "dbo.content_urls");
            DropForeignKey("dbo.content_urls", "ContentId", "dbo.content");
            DropForeignKey("dbo.content_categories", "SidebarContentId", "dbo.content");
            DropForeignKey("dbo.content", "CategoryId", "dbo.content_categories");
            DropForeignKey("dbo.content_boilerplates", "BoilerplateId", "dbo.content");
            DropForeignKey("dbo.content_boilerplates", "ContentId", "dbo.content");
            DropForeignKey("dbo.content", "PublishedByUserId", "dbo.system_users");
            DropForeignKey("dbo.content", "AuthorUserId", "dbo.system_users");
            DropIndex("dbo.content_feedback", new[] { "ContentId" });
            DropIndex("dbo.content_feedback", new[] { "Id" });
            DropIndex("dbo.todos", new[] { "AssigneeId" });
            DropIndex("dbo.todos", new[] { "AuthorId" });
            DropIndex("dbo.content_versions", new[] { "UserId" });
            DropIndex("dbo.content_versions", new[] { "ContentId" });
            DropIndex("dbo.feedback", new[] { "EditorId" });
            DropIndex("dbo.feedback", new[] { "AuthorId" });
            DropIndex("dbo.content_urls", new[] { "ContentId" });
            DropIndex("dbo.content_urls", "idx_content_url");
            DropIndex("dbo.content_categories", new[] { "SidebarContentId" });
            DropIndex("dbo.content", new[] { "DefaultUrlId" });
            DropIndex("dbo.content", new[] { "AuthorUserId" });
            DropIndex("dbo.content", new[] { "PublishedByUserId" });
            DropIndex("dbo.content", new[] { "CategoryId" });
            DropIndex("dbo.content_boilerplates", new[] { "BoilerplateId" });
            DropIndex("dbo.content_boilerplates", new[] { "ContentId" });
            DropTable("dbo.content_feedback");
            DropTable("dbo.todos");
            DropTable("dbo.content_versions");
            DropTable("dbo.feedback");
            DropTable("dbo.content_urls");
            DropTable("dbo.content_categories");
            DropTable("dbo.content");
            DropTable("dbo.content_boilerplates");
        }
    }
}
