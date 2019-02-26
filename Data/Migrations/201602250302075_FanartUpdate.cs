namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FanartUpdate : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Fanarts", "UploadUserId");
            AddForeignKey("dbo.Fanarts", "UploadUserId", "dbo.system_users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fanarts", "UploadUserId", "dbo.system_users");
            DropIndex("dbo.Fanarts", new[] { "UploadUserId" });
        }
    }
}
