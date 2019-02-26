namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FanartFileSize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fanarts", "FileSize", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fanarts", "FileSize");
        }
    }
}
