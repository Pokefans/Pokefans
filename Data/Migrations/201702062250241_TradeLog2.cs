namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TradeLog11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WifiBanlists",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CanAddOffers = c.Boolean(nullable: false),
                        CanInterest = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WifiBanlists", "UserId", "dbo.system_users");
            DropIndex("dbo.WifiBanlists", new[] { "UserId" });
            DropTable("dbo.WifiBanlists");
        }
    }
}
