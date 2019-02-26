namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FanartCategoryProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FanartCategories", "MaxFileSize", c => c.Int(nullable: false));
            AddColumn("dbo.FanartCategories", "MaximumDimension", c => c.Int(nullable: false));

            Sql("UPDATE FanartCategories SET MaxFileSize = 1048576, MaximumDimension = 2000 WHERE MaxFileSize = 0");

        }
        
        public override void Down()
        {
            DropColumn("dbo.FanartCategories", "MaximumDimension");
            DropColumn("dbo.FanartCategories", "MaxFileSize");
        }
    }
}
