namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SenderToRemove : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("PrivateMessageSents", "UserToId", "system_users");
            DropIndex("PrivateMessageSents", new[] { "UserToId" });
            DropColumn("PrivateMessageSents", "UserToId");
        }
        
        public override void Down()
        {
            AddColumn("PrivateMessageSents", "UserToId", c => c.Int(nullable: false));
            CreateIndex("PrivateMessageSents", "UserToId");
            AddForeignKey("PrivateMessageSents", "UserToId", "system_users", "Id", cascadeDelete: true);
        }
    }
}
