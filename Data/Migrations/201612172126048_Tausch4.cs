namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tausch4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Offers", "Attack1Id", "Attacks");
            DropForeignKey("Offers", "Attack2Id", "Attacks");
            DropForeignKey("Offers", "Attack3Id", "Attacks");
            DropForeignKey("Offers", "Attack4Id", "Attacks");
            DropForeignKey("Offers", "NatureId", "Natures");
            DropForeignKey("Offers", "PokeballId", "Pokeballs");
            DropIndex("Offers", new[] { "PokeballId" });
            DropIndex("Offers", new[] { "Attack1Id" });
            DropIndex("Offers", new[] { "Attack2Id" });
            DropIndex("Offers", new[] { "Attack3Id" });
            DropIndex("Offers", new[] { "Attack4Id" });
            DropIndex("Offers", new[] { "NatureId" });
            AlterColumn("Offers", "PokeballId", c => c.Int());
            AlterColumn("Offers", "Attack1Id", c => c.Int());
            AlterColumn("Offers", "Attack2Id", c => c.Int());
            AlterColumn("Offers", "Attack3Id", c => c.Int());
            AlterColumn("Offers", "Attack4Id", c => c.Int());
            AlterColumn("Offers", "NatureId", c => c.Int());
            CreateIndex("Offers", "PokeballId");
            CreateIndex("Offers", "Attack1Id");
            CreateIndex("Offers", "Attack2Id");
            CreateIndex("Offers", "Attack3Id");
            CreateIndex("Offers", "Attack4Id");
            CreateIndex("Offers", "NatureId");
            AddForeignKey("Offers", "Attack1Id", "Attacks", "id");
            AddForeignKey("Offers", "Attack2Id", "Attacks", "id");
            AddForeignKey("Offers", "Attack3Id", "Attacks", "id");
            AddForeignKey("Offers", "Attack4Id", "Attacks", "id");
            AddForeignKey("Offers", "NatureId", "Natures", "id");
            AddForeignKey("Offers", "PokeballId", "Pokeballs", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("Offers", "PokeballId", "Pokeballs");
            DropForeignKey("Offers", "NatureId", "Natures");
            DropForeignKey("Offers", "Attack4Id", "Attacks");
            DropForeignKey("Offers", "Attack3Id", "Attacks");
            DropForeignKey("Offers", "Attack2Id", "Attacks");
            DropForeignKey("Offers", "Attack1Id", "Attacks");
            DropIndex("Offers", new[] { "NatureId" });
            DropIndex("Offers", new[] { "Attack4Id" });
            DropIndex("Offers", new[] { "Attack3Id" });
            DropIndex("Offers", new[] { "Attack2Id" });
            DropIndex("Offers", new[] { "Attack1Id" });
            DropIndex("Offers", new[] { "PokeballId" });
            AlterColumn("Offers", "NatureId", c => c.Int(nullable: false));
            AlterColumn("Offers", "Attack4Id", c => c.Int(nullable: false));
            AlterColumn("Offers", "Attack3Id", c => c.Int(nullable: false));
            AlterColumn("Offers", "Attack2Id", c => c.Int(nullable: false));
            AlterColumn("Offers", "Attack1Id", c => c.Int(nullable: false));
            AlterColumn("Offers", "PokeballId", c => c.Int(nullable: false));
            CreateIndex("Offers", "NatureId");
            CreateIndex("Offers", "Attack4Id");
            CreateIndex("Offers", "Attack3Id");
            CreateIndex("Offers", "Attack2Id");
            CreateIndex("Offers", "Attack1Id");
            CreateIndex("Offers", "PokeballId");
            AddForeignKey("Offers", "PokeballId", "Pokeballs", "id", cascadeDelete: true);
            AddForeignKey("Offers", "NatureId", "Natures", "id", cascadeDelete: true);
            AddForeignKey("Offers", "Attack4Id", "Attacks", "id", cascadeDelete: true);
            AddForeignKey("Offers", "Attack3Id", "Attacks", "id", cascadeDelete: true);
            AddForeignKey("Offers", "Attack2Id", "Attacks", "id", cascadeDelete: true);
            AddForeignKey("Offers", "Attack1Id", "Attacks", "id", cascadeDelete: true);
        }
    }
}
