namespace Pokefans.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChallengeVoteTimestampIP : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FanartChallenges",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        TagId = c.Int(nullable: false),
                        ExpireDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.fanart_tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.FanartChallengeVotes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FanartId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ChallengeId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false, precision: 0),
                        VoteIp = c.String(maxLength: 47, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.FanartChallenges", t => t.ChallengeId, cascadeDelete: true)
                .ForeignKey("dbo.Fanarts", t => t.FanartId, cascadeDelete: true)
                .ForeignKey("dbo.system_users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.FanartId)
                .Index(t => t.UserId)
                .Index(t => t.ChallengeId);
            
            AddColumn("dbo.Fanarts", "ChallengeId", c => c.Int());
            CreateIndex("dbo.Fanarts", "ChallengeId");
            AddForeignKey("dbo.Fanarts", "ChallengeId", "dbo.FanartChallenges", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FanartChallengeVotes", "UserId", "dbo.system_users");
            DropForeignKey("dbo.FanartChallengeVotes", "FanartId", "dbo.Fanarts");
            DropForeignKey("dbo.FanartChallengeVotes", "ChallengeId", "dbo.FanartChallenges");
            DropForeignKey("dbo.FanartChallenges", "TagId", "dbo.fanart_tags");
            DropForeignKey("dbo.Fanarts", "ChallengeId", "dbo.FanartChallenges");
            DropIndex("dbo.FanartChallengeVotes", new[] { "ChallengeId" });
            DropIndex("dbo.FanartChallengeVotes", new[] { "UserId" });
            DropIndex("dbo.FanartChallengeVotes", new[] { "FanartId" });
            DropIndex("dbo.Fanarts", new[] { "ChallengeId" });
            DropIndex("dbo.FanartChallenges", new[] { "TagId" });
            DropColumn("dbo.Fanarts", "ChallengeId");
            DropTable("dbo.FanartChallengeVotes");
            DropTable("dbo.FanartChallenges");
        }
    }
}
