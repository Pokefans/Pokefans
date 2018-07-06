namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PMSentLabelsRemoveFK : DbMigration
    {
        public override void Up()
        {
            DropIndex("PrivateMessageSentLabels", new[] { "PrivateMessageInboxId" });
            DropColumn("PrivateMessageSentLabels", "PrivateMessageInboxId");
        }
        
        public override void Down()
        {
            AddColumn("PrivateMessageSentLabels", "PrivateMessageInboxId", c => c.Int(nullable: false));
            CreateIndex("PrivateMessageSentLabels", "PrivateMessageInboxId");
        }
    }
}
