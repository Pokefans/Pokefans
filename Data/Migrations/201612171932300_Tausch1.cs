namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tausch1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "Title", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "Title");
        }
    }
}
