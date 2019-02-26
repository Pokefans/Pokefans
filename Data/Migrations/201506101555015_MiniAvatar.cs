namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MiniAvatar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.system_users", "mini_avatar_filename", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.system_users", "mini_avatar_filename");
        }
    }
}
