namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FanartCategoryUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FanartCategories", "Uri", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FanartCategories", "Uri");
        }
    }
}
