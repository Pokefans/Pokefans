namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class FanartTags : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "FanartTags", newName: "fanarts_tags");
            RenameTable(name: "FanartTags1", newName: "fanart_tags");
            AlterColumn("dbo.UserPages", "RedirectUrl", c => c.String(maxLength: 255, storeType: "nvarchar"));
        }

        public override void Down()
        {
            AlterColumn("dbo.UserPages", "RedirectUrl", c => c.String(unicode: false));
            RenameTable(name: "fanart_tags", newName: "FanartTags1");
            RenameTable(name: "fanarts_tags", newName: "FanartTags");
        }
    }
}
