namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PMSentLabelsAddFK1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrivateMessageSentLabels", "PrivateMessageSentId", c => c.Int(nullable: false));
            CreateIndex("dbo.PrivateMessageSentLabels", "PrivateMessageSentId");
            AddForeignKey("dbo.PrivateMessageSentLabels", "PrivateMessageSentId", "dbo.PrivateMessageSents", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrivateMessageSentLabels", "PrivateMessageSentId", "dbo.PrivateMessageSents");
            DropIndex("dbo.PrivateMessageSentLabels", new[] { "PrivateMessageSentId" });
            DropColumn("dbo.PrivateMessageSentLabels", "PrivateMessageSentId");
        }
    }
}
