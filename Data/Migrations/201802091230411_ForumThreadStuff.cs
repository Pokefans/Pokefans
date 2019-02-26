namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumThreadStuff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Boards", "Rules", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Boards", "Rules");
        }
    }
}
