namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BiggerFanartFileSize : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Fanarts", "FileSize", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Fanarts", "FileSize", c => c.Short(nullable: false));
        }
    }
}
