namespace TrivialWikiAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0002 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TopicRelatedTopics", newName: "RelatedTopicTopics");
            DropPrimaryKey("dbo.RelatedTopicTopics");
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Sex = c.Int(nullable: false),
                        Avatar = c.Binary(),
                        Points = c.Int(nullable: false),
                        Rank = c.Int(nullable: false),
                        SecurityToken = c.String(),
                        Statistic_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Statistics", t => t.Statistic_Id)
                .Index(t => t.Statistic_Id);
            
            CreateTable(
                "dbo.Statistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GamesPlayed = c.Int(nullable: false),
                        Wins = c.Int(nullable: false),
                        WinsInARow = c.Int(nullable: false),
                        ArchNemesis_Id = c.Int(),
                        BestTopic_Id = c.Int(),
                        MostPlayedOpponent_Id = c.Int(),
                        MostPlayedTopic_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ArchNemesis_Id)
                .ForeignKey("dbo.Topics", t => t.BestTopic_Id)
                .ForeignKey("dbo.Users", t => t.MostPlayedOpponent_Id)
                .ForeignKey("dbo.Topics", t => t.MostPlayedTopic_Id)
                .Index(t => t.ArchNemesis_Id)
                .Index(t => t.BestTopic_Id)
                .Index(t => t.MostPlayedOpponent_Id)
                .Index(t => t.MostPlayedTopic_Id);
            
            CreateTable(
                "dbo.QuestionSets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(),
                        CorrectAnswer = c.String(),
                        UsageCount = c.Int(nullable: false),
                        CorrectAnswerCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserAchievements",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Achievement_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Achievement_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Achievements", t => t.Achievement_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Achievement_Id);
            
            CreateTable(
                "dbo.RoleUsers",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.User_Id })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.QuestionSetTopics",
                c => new
                    {
                        QuestionSet_Id = c.Int(nullable: false),
                        Topic_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.QuestionSet_Id, t.Topic_Id })
                .ForeignKey("dbo.QuestionSets", t => t.QuestionSet_Id, cascadeDelete: true)
                .ForeignKey("dbo.Topics", t => t.Topic_Id, cascadeDelete: true)
                .Index(t => t.QuestionSet_Id)
                .Index(t => t.Topic_Id);
            
            AddPrimaryKey("dbo.RelatedTopicTopics", new[] { "RelatedTopic_Id", "Topic_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Statistic_Id", "dbo.Statistics");
            DropForeignKey("dbo.Statistics", "MostPlayedTopic_Id", "dbo.Topics");
            DropForeignKey("dbo.Statistics", "MostPlayedOpponent_Id", "dbo.Users");
            DropForeignKey("dbo.Statistics", "BestTopic_Id", "dbo.Topics");
            DropForeignKey("dbo.QuestionSetTopics", "Topic_Id", "dbo.Topics");
            DropForeignKey("dbo.QuestionSetTopics", "QuestionSet_Id", "dbo.QuestionSets");
            DropForeignKey("dbo.Statistics", "ArchNemesis_Id", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.UserAchievements", "Achievement_Id", "dbo.Achievements");
            DropForeignKey("dbo.UserAchievements", "User_Id", "dbo.Users");
            DropIndex("dbo.QuestionSetTopics", new[] { "Topic_Id" });
            DropIndex("dbo.QuestionSetTopics", new[] { "QuestionSet_Id" });
            DropIndex("dbo.RoleUsers", new[] { "User_Id" });
            DropIndex("dbo.RoleUsers", new[] { "Role_Id" });
            DropIndex("dbo.UserAchievements", new[] { "Achievement_Id" });
            DropIndex("dbo.UserAchievements", new[] { "User_Id" });
            DropIndex("dbo.Statistics", new[] { "MostPlayedTopic_Id" });
            DropIndex("dbo.Statistics", new[] { "MostPlayedOpponent_Id" });
            DropIndex("dbo.Statistics", new[] { "BestTopic_Id" });
            DropIndex("dbo.Statistics", new[] { "ArchNemesis_Id" });
            DropIndex("dbo.Users", new[] { "Statistic_Id" });
            DropPrimaryKey("dbo.RelatedTopicTopics");
            DropTable("dbo.QuestionSetTopics");
            DropTable("dbo.RoleUsers");
            DropTable("dbo.UserAchievements");
            DropTable("dbo.QuestionSets");
            DropTable("dbo.Statistics");
            DropTable("dbo.Users");
            AddPrimaryKey("dbo.RelatedTopicTopics", new[] { "Topic_Id", "RelatedTopic_Id" });
            RenameTable(name: "dbo.RelatedTopicTopics", newName: "TopicRelatedTopics");
        }
    }
}
