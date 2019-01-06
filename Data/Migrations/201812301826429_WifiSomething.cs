namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WifiSomething : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.system_users", "TradingPoints", c => c.Int(nullable: false));
            AddColumn("dbo.system_users", "TradingPercentPositive", c => c.Int(nullable: false));
            AddColumn("dbo.system_users", "TradingPercentNeutral", c => c.Int(nullable: false));
            AddColumn("dbo.system_users", "TradingPercentNegative", c => c.Int(nullable: false));
            AddColumn("dbo.TradeLogs", "IsValid", c => c.Boolean(nullable: false));
            AddColumn("dbo.TradeLogs", "SellerRating", c => c.Int());
            AddColumn("dbo.TradeLogs", "CustomerRating", c => c.Int());
            AlterColumn("dbo.TradeLogs", "CompletedTime", c => c.DateTime(precision: 0));
            AlterColumn("dbo.TradeLogs", "ValidOn", c => c.DateTime(precision: 0));

            Sql("UPDATE system_users SET TradingPoints=0, TradingPercentPositive=0, TradingPercentNeutral=0, TradingPercentNegative=0;");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TradeLogs", "ValidOn", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.TradeLogs", "CompletedTime", c => c.DateTime(nullable: false, precision: 0));
            DropColumn("dbo.TradeLogs", "CustomerRating");
            DropColumn("dbo.TradeLogs", "SellerRating");
            DropColumn("dbo.TradeLogs", "IsValid");
            DropColumn("dbo.system_users", "TradingPercentNegative");
            DropColumn("dbo.system_users", "TradingPercentNeutral");
            DropColumn("dbo.system_users", "TradingPercentPositive");
            DropColumn("dbo.system_users", "TradingPoints");
        }
    }
}
