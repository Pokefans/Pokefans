namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PokemonForumTopicOptional1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.dex_pokemon", "ForumTopicId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.dex_pokemon", "ForumTopicId", c => c.Int(nullable: false));
        }
    }
}
