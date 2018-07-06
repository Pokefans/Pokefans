namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrivateMessageConversations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrivateMessages", "ConversationId", c => c.Int(nullable: false));
            AddColumn("dbo.PrivateMessages", "ReplyTo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PrivateMessages", "ReplyTo");
            DropColumn("dbo.PrivateMessages", "ConversationId");
        }
    }
}
