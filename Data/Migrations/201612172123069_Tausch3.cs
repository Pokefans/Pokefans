namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tausch3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "CheatUsed", c => c.Boolean(nullable: false));
            DropColumn("dbo.Offers", "CheatUsesd");
            DropColumn("dbo.Offers", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Offers", "Price", c => c.Int());
            AddColumn("dbo.Offers", "CheatUsesd", c => c.Boolean(nullable: false));
            DropColumn("dbo.Offers", "CheatUsed");
        }
    }
}
