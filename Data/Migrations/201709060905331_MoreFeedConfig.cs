namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreFeedConfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("UserFeedConfigs", "NewWifiOffers", c => c.Short(nullable: false));
            AddColumn("UserFeedConfigs", "CommentsOnNews", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("UserFeedConfigs", "CommentsOnNews");
            DropColumn("UserFeedConfigs", "NewWifiOffers");
        }
    }
}
