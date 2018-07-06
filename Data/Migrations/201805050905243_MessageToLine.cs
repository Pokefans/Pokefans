namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageToLine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrivateMessages", "ToLine", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PrivateMessages", "ToLine");
        }
    }
}
