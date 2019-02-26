namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Content1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("content", "CategoryId", "content_categories");
            DropForeignKey("content", "DefaultUrlId", "content_urls");
            DropForeignKey("content", "PublishedByUserId", "system_users");
            DropForeignKey("content_categories", "SidebarContentId", "content");
            DropForeignKey("feedback", "AuthorId", "system_users");
            DropForeignKey("feedback", "EditorId", "system_users");
            DropForeignKey("todos", "AssigneeId", "system_users");
            DropIndex("content", new[] { "CategoryId" });
            DropIndex("content", new[] { "PublishedByUserId" });
            DropIndex("content", new[] { "DefaultUrlId" });
            DropIndex("content_categories", new[] { "SidebarContentId" });
            DropIndex("feedback", new[] { "AuthorId" });
            DropIndex("feedback", new[] { "EditorId" });
            DropIndex("todos", new[] { "AssigneeId" });
            AddColumn("content", "StylesheetCss", c => c.String(unicode: false));
            AddColumn("content", "Created", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("content_urls", "Type", c => c.Int(nullable: false));
            AddColumn("content_urls", "Enabled", c => c.Boolean(nullable: false));
            AddColumn("content_versions", "StylesheetCss", c => c.String(unicode: false));
            AlterColumn("content", "CategoryId", c => c.Int());
            AlterColumn("content", "EditPermissionId", c => c.Int());
            AlterColumn("content", "ViewCount", c => c.Int(nullable: false));
            AlterColumn("content", "PublishedByUserId", c => c.Int());
            AlterColumn("content", "DefaultUrlId", c => c.Int());
            AlterColumn("content_categories", "ForumId", c => c.Int());
            AlterColumn("content_categories", "SidebarContentId", c => c.Int());
            AlterColumn("feedback", "AuthorId", c => c.Int());
            AlterColumn("feedback", "EditorId", c => c.Int());
            AlterColumn("todos", "AssigneeId", c => c.Int());
            CreateIndex("content", "CategoryId");
            CreateIndex("content", "EditPermissionId");
            CreateIndex("content", "PublishedByUserId");
            CreateIndex("content", "DefaultUrlId");
            CreateIndex("content_categories", "SidebarContentId");
            CreateIndex("feedback", "AuthorId");
            CreateIndex("feedback", "EditorId");
            CreateIndex("todos", "AssigneeId");
            AddForeignKey("content", "EditPermissionId", "system_permissions", "id");
            AddForeignKey("content", "CategoryId", "content_categories", "Id");
            AddForeignKey("content", "DefaultUrlId", "content_urls", "Id");
            AddForeignKey("content", "PublishedByUserId", "system_users", "Id");
            AddForeignKey("content_categories", "SidebarContentId", "content", "Id");
            AddForeignKey("feedback", "AuthorId", "system_users", "Id");
            AddForeignKey("feedback", "EditorId", "system_users", "Id");
            AddForeignKey("todos", "AssigneeId", "system_users", "Id");
            DropColumn("content", "StylesheetName");
            DropColumn("content_urls", "Status");
            DropColumn("content_versions", "StylesheetName");
        }
        
        public override void Down()
        {
            AddColumn("content_versions", "StylesheetName", c => c.String(maxLength: 100, storeType: "nvarchar"));
            AddColumn("content_urls", "Status", c => c.Int(nullable: false));
            AddColumn("content", "StylesheetName", c => c.String(maxLength: 100, storeType: "nvarchar"));
            DropForeignKey("todos", "AssigneeId", "system_users");
            DropForeignKey("feedback", "EditorId", "system_users");
            DropForeignKey("feedback", "AuthorId", "system_users");
            DropForeignKey("content_categories", "SidebarContentId", "content");
            DropForeignKey("content", "PublishedByUserId", "system_users");
            DropForeignKey("content", "DefaultUrlId", "content_urls");
            DropForeignKey("content", "CategoryId", "content_categories");
            DropForeignKey("content", "EditPermissionId", "system_permissions");
            DropIndex("todos", new[] { "AssigneeId" });
            DropIndex("feedback", new[] { "EditorId" });
            DropIndex("feedback", new[] { "AuthorId" });
            DropIndex("content_categories", new[] { "SidebarContentId" });
            DropIndex("content", new[] { "DefaultUrlId" });
            DropIndex("content", new[] { "PublishedByUserId" });
            DropIndex("content", new[] { "EditPermissionId" });
            DropIndex("content", new[] { "CategoryId" });
            AlterColumn("todos", "AssigneeId", c => c.Int(nullable: false));
            AlterColumn("feedback", "EditorId", c => c.Int(nullable: false));
            AlterColumn("feedback", "AuthorId", c => c.Int(nullable: false));
            AlterColumn("content_categories", "SidebarContentId", c => c.Int(nullable: false));
            AlterColumn("content_categories", "ForumId", c => c.Int(nullable: false));
            AlterColumn("content", "DefaultUrlId", c => c.Int(nullable: false));
            AlterColumn("content", "PublishedByUserId", c => c.Int(nullable: false));
            AlterColumn("content", "ViewCount", c => c.String(unicode: false));
            AlterColumn("content", "EditPermissionId", c => c.Int(nullable: false));
            AlterColumn("content", "CategoryId", c => c.Int(nullable: false));
            DropColumn("content_versions", "StylesheetCss");
            DropColumn("content_urls", "Enabled");
            DropColumn("content_urls", "Type");
            DropColumn("content", "Created");
            DropColumn("content", "StylesheetCss");
            CreateIndex("todos", "AssigneeId");
            CreateIndex("feedback", "EditorId");
            CreateIndex("feedback", "AuthorId");
            CreateIndex("content_categories", "SidebarContentId");
            CreateIndex("content", "DefaultUrlId");
            CreateIndex("content", "PublishedByUserId");
            CreateIndex("content", "CategoryId");
            AddForeignKey("todos", "AssigneeId", "system_users", "Id", cascadeDelete: true);
            AddForeignKey("feedback", "EditorId", "system_users", "Id", cascadeDelete: true);
            AddForeignKey("feedback", "AuthorId", "system_users", "Id", cascadeDelete: true);
            AddForeignKey("content_categories", "SidebarContentId", "content", "Id", cascadeDelete: true);
            AddForeignKey("content", "PublishedByUserId", "system_users", "Id", cascadeDelete: true);
            AddForeignKey("content", "DefaultUrlId", "content_urls", "Id", cascadeDelete: true);
            AddForeignKey("content", "CategoryId", "content_categories", "Id", cascadeDelete: true);
        }
    }
}
