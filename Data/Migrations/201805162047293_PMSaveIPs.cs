namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PMSaveIPs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrivateMessageReports", "IpAddress", c => c.String(maxLength: 39, storeType: "nvarchar"));
            AddColumn("dbo.PrivateMessages", "SenderIpAddress", c => c.String(maxLength: 39, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PrivateMessages", "SenderIpAddress");
            DropColumn("dbo.PrivateMessageReports", "IpAddress");
        }
    }
}
