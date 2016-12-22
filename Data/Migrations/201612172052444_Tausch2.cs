namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tausch2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "Price", c => c.Int());
            AddColumn("dbo.Offers", "InterestCount", c => c.Int());
            AddColumn("dbo.Offers", "PokemonCount", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "PokemonCount");
            DropColumn("dbo.Offers", "InterestCount");
            DropColumn("dbo.Offers", "Price");
        }
    }
}
