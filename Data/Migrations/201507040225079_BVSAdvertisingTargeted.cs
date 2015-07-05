namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BVSAdvertisingTargeted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.user_advertising_forms", "is_targeted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.user_advertising_forms", "is_targeted");
        }
    }
}
