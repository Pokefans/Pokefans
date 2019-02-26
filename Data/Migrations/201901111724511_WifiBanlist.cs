namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WifiBanlist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WifiBanlists", "ExpireAddOffers", c => c.DateTime(precision: 0));
            AddColumn("dbo.WifiBanlists", "ExpireInterest", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WifiBanlists", "ExpireInterest");
            DropColumn("dbo.WifiBanlists", "ExpireAddOffers");
        }
    }
}
