namespace TrivialWikiAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0003 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserAchievements", newName: "AchievementUsers");
            DropPrimaryKey("dbo.AchievementUsers");
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
            
            AddPrimaryKey("dbo.AchievementUsers", new[] { "Achievement_Id", "User_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrivateChats", "SecondUser_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "PrivateChat_Id", "dbo.PrivateChats");
            DropForeignKey("dbo.Messages", "Sender_Id", "dbo.Users");
            DropForeignKey("dbo.PrivateChats", "FirstUser_Id", "dbo.Users");
            DropForeignKey("dbo.Matches", "Winner_Id", "dbo.Users");
            DropForeignKey("dbo.Matches", "Topic_Id", "dbo.Topics");
            DropForeignKey("dbo.Matches", "Loser_Id", "dbo.Users");
            DropForeignKey("dbo.ChatRooms", "User_Id", "dbo.Users");
            DropIndex("dbo.Messages", new[] { "PrivateChat_Id" });
            DropIndex("dbo.Messages", new[] { "Sender_Id" });
            DropIndex("dbo.PrivateChats", new[] { "SecondUser_Id" });
            DropIndex("dbo.PrivateChats", new[] { "FirstUser_Id" });
            DropIndex("dbo.Matches", new[] { "Winner_Id" });
            DropIndex("dbo.Matches", new[] { "Topic_Id" });
            DropIndex("dbo.Matches", new[] { "Loser_Id" });
            DropIndex("dbo.ChatRooms", new[] { "User_Id" });
            DropPrimaryKey("dbo.AchievementUsers");
            DropTable("dbo.Messages");
            DropTable("dbo.PrivateChats");
            DropTable("dbo.Matches");
            DropTable("dbo.ChatRooms");
            AddPrimaryKey("dbo.AchievementUsers", new[] { "User_Id", "Achievement_Id" });
            RenameTable(name: "dbo.AchievementUsers", newName: "UserAchievements");
        }
    }
}
