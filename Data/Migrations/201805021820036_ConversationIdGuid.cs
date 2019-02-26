namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConversationIdGuid : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PrivateMessages", "ConversationId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PrivateMessages", "ConversationId", c => c.Int(nullable: false));
        }
    }
}
