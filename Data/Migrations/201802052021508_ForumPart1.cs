namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreFeedConfig1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoardPermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BoardId = c.Int(),
                        RoleId = c.Int(),
                        Permissionset = c.Int(nullable: false),
                        GroupId = c.Int(),
                        CanRead = c.Int(nullable: false),
                        CanWrite = c.Int(nullable: false),
                        CanModerate = c.Int(nullable: false),
                        CanManage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Boards", t => t.BoardId)
                .ForeignKey("dbo.system_permissions", t => t.RoleId)
                .Index(t => new { t.Permissionset, t.RoleId, t.BoardId, t.GroupId }, unique: true, name: "IX_Permissionsets_Role");
            
            CreateTable(
                "dbo.Boards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255, storeType: "nvarchar"),
                        Url = c.String(maxLength: 255, storeType: "nvarchar"),
                        Description = c.String(maxLength: 500, storeType: "nvarchar"),
                        Type = c.Int(nullable: false),
                        ThreadCount = c.Int(nullable: false),
                        PostCount = c.Int(nullable: false),
                        ParentBoardId = c.Int(),
                        IsQAEnabled = c.Boolean(nullable: false),
                        ShowInParentBoard = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
                        LastPostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.LastPostId, cascadeDelete: true)
                .Index(t => t.Url)
                .Index(t => t.LastPostId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorId = c.Int(),
                        ThreadId = c.Int(nullable: false),
                        Subject = c.String(maxLength: 255, storeType: "nvarchar"),
                        Body = c.String(unicode: false),
                        BodyRaw = c.String(unicode: false),
                        PostTime = c.DateTime(nullable: false, precision: 0),
                        LastEditTime = c.DateTime(nullable: false, precision: 0),
                        EditReason = c.String(unicode: false),
                        IsSolution = c.Boolean(nullable: false),
                        IpAddress = c.String(maxLength: 48, storeType: "nvarchar"),
                        NeedsApproval = c.Boolean(nullable: false),
                        ReactionHeart = c.Int(nullable: false),
                        ReactionLol = c.Int(nullable: false),
                        ReactionThumbsUp = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.system_users", t => t.AuthorId)
                .ForeignKey("dbo.Threads", t => t.ThreadId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.ThreadId);
            
            CreateTable(
                "dbo.Threads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorId = c.Int(nullable: false),
                        Title = c.String(maxLength: 255, storeType: "nvarchar"),
                        ThreadStartTime = c.DateTime(nullable: false, precision: 0),
                        ThreadIcon = c.Int(nullable: false),
                        Replies = c.Int(nullable: false),
                        Visits = c.Int(nullable: false),
                        BoardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.system_users", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Boards", t => t.BoardId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.Title)
                .Index(t => t.BoardId);
            
            CreateTable(
                "dbo.ForumGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255, storeType: "nvarchar"),
                        Description = c.String(maxLength: 500, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ForumGroupsUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        IsLeader = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ForumGroups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UnreadForumTrackers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        BoardId = c.Int(nullable: false),
                        ResetTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Boards", t => t.BoardId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.BoardId);
            
            CreateTable(
                "dbo.UnreadThreadTrackers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ThreadId = c.Int(nullable: false),
                        ResetTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UnreadForumTrackers", "UserId", "dbo.system_users");
            DropForeignKey("dbo.UnreadForumTrackers", "BoardId", "dbo.Boards");
            DropForeignKey("dbo.ForumGroupsUsers", "UserId", "dbo.system_users");
            DropForeignKey("dbo.ForumGroupsUsers", "GroupId", "dbo.ForumGroups");
            DropForeignKey("dbo.BoardPermissions", "RoleId", "dbo.system_permissions");
            DropForeignKey("dbo.BoardPermissions", "BoardId", "dbo.Boards");
            DropForeignKey("dbo.Boards", "LastPostId", "dbo.Posts");
            DropForeignKey("dbo.Posts", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.Threads", "BoardId", "dbo.Boards");
            DropForeignKey("dbo.Threads", "AuthorId", "dbo.system_users");
            DropForeignKey("dbo.Posts", "AuthorId", "dbo.system_users");
            DropIndex("dbo.UnreadForumTrackers", new[] { "BoardId" });
            DropIndex("dbo.UnreadForumTrackers", new[] { "UserId" });
            DropIndex("dbo.ForumGroupsUsers", new[] { "UserId" });
            DropIndex("dbo.ForumGroupsUsers", new[] { "GroupId" });
            DropIndex("dbo.Threads", new[] { "BoardId" });
            DropIndex("dbo.Threads", new[] { "Title" });
            DropIndex("dbo.Threads", new[] { "AuthorId" });
            DropIndex("dbo.Posts", new[] { "ThreadId" });
            DropIndex("dbo.Posts", new[] { "AuthorId" });
            DropIndex("dbo.Boards", new[] { "LastPostId" });
            DropIndex("dbo.Boards", new[] { "Url" });
            DropIndex("dbo.BoardPermissions", "IX_Permissionsets_Role");
            DropTable("dbo.UnreadThreadTrackers");
            DropTable("dbo.UnreadForumTrackers");
            DropTable("dbo.ForumGroupsUsers");
            DropTable("dbo.ForumGroups");
            DropTable("dbo.Threads");
            DropTable("dbo.Posts");
            DropTable("dbo.Boards");
            DropTable("dbo.BoardPermissions");
        }
    }
}
