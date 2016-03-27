namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FanartOrderSigned : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fanarts", "Order", c => c.Short(nullable: false));
            DropColumn("dbo.Fanarts", "ForceComments");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Fanarts", "ForceComments", c => c.Boolean(nullable: false));
            DropColumn("dbo.Fanarts", "Order");
        }
    }
}
