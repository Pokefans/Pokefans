namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfilePagePartOne : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.system_users", "FanartCount", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfiles", "DiscordName", c => c.String(maxLength: 100, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "DiscordName");
            DropColumn("dbo.system_users", "FanartCount");
        }
    }
}
