namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tausch7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "BottleCapsUsed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "BottleCapsUsed");
        }
    }
}
