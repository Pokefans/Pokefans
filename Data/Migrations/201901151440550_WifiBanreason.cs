namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WifiBanreason : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WifiBanlists", "BanReason", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WifiBanlists", "BanReason");
        }
    }
}
