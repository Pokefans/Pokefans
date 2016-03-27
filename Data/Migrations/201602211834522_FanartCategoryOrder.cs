namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FanartCategoryOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FanartCategories", "Order", c => c.Int(nullable: false));
            Sql("UPDATE FanartCategories SET `Order` = 100");
        }
        
        public override void Down()
        {
            DropColumn("dbo.FanartCategories", "Order");
        }
    }
}
