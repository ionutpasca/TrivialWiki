namespace DatabaseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0015 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatRooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageText = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        Points = c.Int(nullable: false),
                        Rank = c.Int(nullable: false),
                        SecurityToken = c.String(),
                        AccountCreationDate = c.DateTime(nullable: false),
                        Role_Id = c.Int(),
                        Statistic_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.Role_Id)
                .ForeignKey("dbo.Statistics", t => t.Statistic_Id)
                .Index(t => t.Role_Id)
                .Index(t => t.Statistic_Id);
            
            CreateTable(
                "dbo.Achievements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Threshold = c.Int(nullable: false),
                        Icon = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.Topics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.RelatedTopics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstUser_Id = c.Int(),
                        SecondUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.FirstUser_Id)
                .ForeignKey("dbo.Users", t => t.SecondUser_Id)
                .Index(t => t.FirstUser_Id)
                .Index(t => t.SecondUser_Id);
            
            CreateTable(
                "dbo.Matches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionCount = c.Int(nullable: false),
                        FirstUserCorrectAnswers = c.Int(nullable: false),
                        SecondUserCorrectAnswers = c.Int(nullable: false),
                        Loser_Id = c.Int(),
                        Topic_Id = c.Int(),
                        Winner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Loser_Id)
                .ForeignKey("dbo.Topics", t => t.Topic_Id)
                .ForeignKey("dbo.Users", t => t.Winner_Id)
                .Index(t => t.Loser_Id)
                .Index(t => t.Topic_Id)
                .Index(t => t.Winner_Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageText = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        Sender_Id = c.Int(),
                        PrivateChat_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Sender_Id)
                .ForeignKey("dbo.PrivateChats", t => t.PrivateChat_Id)
                .Index(t => t.Sender_Id)
                .Index(t => t.PrivateChat_Id);
            
            CreateTable(
                "dbo.PrivateChats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstUser_Id = c.Int(),
                        SecondUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.FirstUser_Id)
                .ForeignKey("dbo.Users", t => t.SecondUser_Id)
                .Index(t => t.FirstUser_Id)
                .Index(t => t.SecondUser_Id);
            
            CreateTable(
                "dbo.TriviaMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sender = c.String(),
                        MessageText = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AchievementUsers",
                c => new
                    {
                        Achievement_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Achievement_Id, t.User_Id })
                .ForeignKey("dbo.Achievements", t => t.Achievement_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Achievement_Id)
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
            
            CreateTable(
                "dbo.RelatedTopicTopics",
                c => new
                    {
                        RelatedTopic_Id = c.Int(nullable: false),
                        Topic_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RelatedTopic_Id, t.Topic_Id })
                .ForeignKey("dbo.RelatedTopics", t => t.RelatedTopic_Id, cascadeDelete: true)
                .ForeignKey("dbo.Topics", t => t.Topic_Id, cascadeDelete: true)
                .Index(t => t.RelatedTopic_Id)
                .Index(t => t.Topic_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrivateChats", "SecondUser_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "PrivateChat_Id", "dbo.PrivateChats");
            DropForeignKey("dbo.PrivateChats", "FirstUser_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "Sender_Id", "dbo.Users");
            DropForeignKey("dbo.Matches", "Winner_Id", "dbo.Users");
            DropForeignKey("dbo.Matches", "Topic_Id", "dbo.Topics");
            DropForeignKey("dbo.Matches", "Loser_Id", "dbo.Users");
            DropForeignKey("dbo.Friends", "SecondUser_Id", "dbo.Users");
            DropForeignKey("dbo.Friends", "FirstUser_Id", "dbo.Users");
            DropForeignKey("dbo.ChatRooms", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Statistic_Id", "dbo.Statistics");
            DropForeignKey("dbo.Statistics", "MostPlayedTopic_Id", "dbo.Topics");
            DropForeignKey("dbo.Statistics", "MostPlayedOpponent_Id", "dbo.Users");
            DropForeignKey("dbo.Statistics", "BestTopic_Id", "dbo.Topics");
            DropForeignKey("dbo.RelatedTopicTopics", "Topic_Id", "dbo.Topics");
            DropForeignKey("dbo.RelatedTopicTopics", "RelatedTopic_Id", "dbo.RelatedTopics");
            DropForeignKey("dbo.QuestionSetTopics", "Topic_Id", "dbo.Topics");
            DropForeignKey("dbo.QuestionSetTopics", "QuestionSet_Id", "dbo.QuestionSets");
            DropForeignKey("dbo.Statistics", "ArchNemesis_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.AchievementUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.AchievementUsers", "Achievement_Id", "dbo.Achievements");
            DropIndex("dbo.RelatedTopicTopics", new[] { "Topic_Id" });
            DropIndex("dbo.RelatedTopicTopics", new[] { "RelatedTopic_Id" });
            DropIndex("dbo.QuestionSetTopics", new[] { "Topic_Id" });
            DropIndex("dbo.QuestionSetTopics", new[] { "QuestionSet_Id" });
            DropIndex("dbo.AchievementUsers", new[] { "User_Id" });
            DropIndex("dbo.AchievementUsers", new[] { "Achievement_Id" });
            DropIndex("dbo.PrivateChats", new[] { "SecondUser_Id" });
            DropIndex("dbo.PrivateChats", new[] { "FirstUser_Id" });
            DropIndex("dbo.Messages", new[] { "PrivateChat_Id" });
            DropIndex("dbo.Messages", new[] { "Sender_Id" });
            DropIndex("dbo.Matches", new[] { "Winner_Id" });
            DropIndex("dbo.Matches", new[] { "Topic_Id" });
            DropIndex("dbo.Matches", new[] { "Loser_Id" });
            DropIndex("dbo.Friends", new[] { "SecondUser_Id" });
            DropIndex("dbo.Friends", new[] { "FirstUser_Id" });
            DropIndex("dbo.Statistics", new[] { "MostPlayedTopic_Id" });
            DropIndex("dbo.Statistics", new[] { "MostPlayedOpponent_Id" });
            DropIndex("dbo.Statistics", new[] { "BestTopic_Id" });
            DropIndex("dbo.Statistics", new[] { "ArchNemesis_Id" });
            DropIndex("dbo.Users", new[] { "Statistic_Id" });
            DropIndex("dbo.Users", new[] { "Role_Id" });
            DropIndex("dbo.ChatRooms", new[] { "User_Id" });
            DropTable("dbo.RelatedTopicTopics");
            DropTable("dbo.QuestionSetTopics");
            DropTable("dbo.AchievementUsers");
            DropTable("dbo.TriviaMessages");
            DropTable("dbo.PrivateChats");
            DropTable("dbo.Messages");
            DropTable("dbo.Matches");
            DropTable("dbo.Friends");
            DropTable("dbo.RelatedTopics");
            DropTable("dbo.QuestionSets");
            DropTable("dbo.Topics");
            DropTable("dbo.Statistics");
            DropTable("dbo.Roles");
            DropTable("dbo.Achievements");
            DropTable("dbo.Users");
            DropTable("dbo.ChatRooms");
        }
    }
}
