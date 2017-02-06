namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TradeLog1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("TradeLogs", "PokemonId", "dex_pokemon");
            DropIndex("TradeLogs", new[] { "PokemonId" });
            AddColumn("TradeLogs", "UserToId", c => c.Int(nullable: false));
            AddColumn("TradeLogs", "UserFromId", c => c.Int(nullable: false));
            CreateIndex("TradeLogs", "UserToId");
            CreateIndex("TradeLogs", "UserFromId");
            AddForeignKey("TradeLogs", "UserFromId", "system_users", "Id", cascadeDelete: true);
            AddForeignKey("TradeLogs", "UserToId", "system_users", "Id", cascadeDelete: true);
            DropColumn("TradeLogs", "PokemonId");
        }
        
        public override void Down()
        {
            AddColumn("TradeLogs", "PokemonId", c => c.Int(nullable: false));
            DropForeignKey("TradeLogs", "UserToId", "system_users");
            DropForeignKey("TradeLogs", "UserFromId", "system_users");
            DropIndex("TradeLogs", new[] { "UserFromId" });
            DropIndex("TradeLogs", new[] { "UserToId" });
            DropColumn("TradeLogs", "UserFromId");
            DropColumn("TradeLogs", "UserToId");
            CreateIndex("TradeLogs", "PokemonId");
            AddForeignKey("TradeLogs", "PokemonId", "dex_pokemon", "id", cascadeDelete: true);
        }
    }
}
