namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumThreadPrefix2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "BoardEnabledPrefixes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BoardId = c.Int(nullable: false),
                        PrefixId = c.Int(nullable: false),
                        IsUserVisible = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "ThreadPrefixes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Prefix = c.String(maxLength: 100, storeType: "nvarchar"),
                        CSS = c.String(maxLength: 20, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("Threads", "PrefixId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("Threads", "PrefixId");
            DropTable("ThreadPrefixes");
            DropTable("BoardEnabledPrefixes");
        }
    }
}
