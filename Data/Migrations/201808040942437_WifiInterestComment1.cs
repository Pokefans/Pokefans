namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WifiInterestComment1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Interests", "Comment", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Interests", "Comment");
        }
    }
}
