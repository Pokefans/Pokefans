namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tausch6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Offers", "AbilityId", "dex_ability");
            DropIndex("Offers", new[] { "AbilityId" });
            AlterColumn("Offers", "AbilityId", c => c.Int());
            CreateIndex("Offers", "AbilityId");
            AddForeignKey("Offers", "AbilityId", "dex_ability", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("Offers", "AbilityId", "dex_ability");
            DropIndex("Offers", new[] { "AbilityId" });
            AlterColumn("Offers", "AbilityId", c => c.Int(nullable: false));
            CreateIndex("Offers", "AbilityId");
            AddForeignKey("Offers", "AbilityId", "dex_ability", "id", cascadeDelete: true);
        }
    }
}
