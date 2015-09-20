namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SfcUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fanarts", "image_width", c => c.Short(nullable: false));
            AddColumn("dbo.Fanarts", "image_height", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fanarts", "image_height");
            DropColumn("dbo.Fanarts", "image_width");
        }
    }
}
