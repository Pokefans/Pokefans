namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumMoreThreadAttributes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Threads", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Threads", "IsLocked", c => c.Boolean(nullable: false));
            AddColumn("dbo.Threads", "LastPostId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Threads", "LastPostId");
            DropColumn("dbo.Threads", "IsLocked");
            DropColumn("dbo.Threads", "Type");
        }
    }
}
