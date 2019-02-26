namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserBanReason : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserBans", "BanReason", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserBans", "BanReason");
        }
    }
}
