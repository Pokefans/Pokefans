namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dsgvo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DsgvoComplianceInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EffectiveTime = c.DateTime(nullable: false, precision: 0),
                        DataProtectionStatement = c.String(unicode: false),
                        TermsOfUsage = c.String(unicode: false),
                        ForumRules = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.system_users", "LastTermsOfServiceAgreement", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.system_users", "LastTermsOfServiceAgreement");
            DropTable("dbo.DsgvoComplianceInfoes");
        }
    }
}
