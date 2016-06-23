namespace DatabaseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0028 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuestionSetTopics", "QuestionSet_Id", "dbo.QuestionSets");
            DropForeignKey("dbo.QuestionSetTopics", "Topic_Id", "dbo.Topics");
            DropForeignKey("dbo.Friends", "FirstUser_Id", "dbo.Users");
            DropForeignKey("dbo.Friends", "SecondUser_Id", "dbo.Users");
            DropIndex("dbo.Friends", new[] { "FirstUser_Id" });
            DropIndex("dbo.Friends", new[] { "SecondUser_Id" });
            DropIndex("dbo.QuestionSetTopics", new[] { "QuestionSet_Id" });
            DropIndex("dbo.QuestionSetTopics", new[] { "Topic_Id" });
            AddColumn("dbo.Users", "User_Id", c => c.Int());
            AddColumn("dbo.QuestionSets", "Topic_Id", c => c.Int());
            CreateIndex("dbo.Users", "User_Id");
            CreateIndex("dbo.QuestionSets", "Topic_Id");
            AddForeignKey("dbo.Users", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.QuestionSets", "Topic_Id", "dbo.Topics", "Id");
            DropTable("dbo.Friends");
            DropTable("dbo.QuestionSetTopics");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.QuestionSetTopics",
                c => new
                    {
                        QuestionSet_Id = c.Int(nullable: false),
                        Topic_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.QuestionSet_Id, t.Topic_Id });
            
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstUser_Id = c.Int(),
                        SecondUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.QuestionSets", "Topic_Id", "dbo.Topics");
            DropForeignKey("dbo.Users", "User_Id", "dbo.Users");
            DropIndex("dbo.QuestionSets", new[] { "Topic_Id" });
            DropIndex("dbo.Users", new[] { "User_Id" });
            DropColumn("dbo.QuestionSets", "Topic_Id");
            DropColumn("dbo.Users", "User_Id");
            CreateIndex("dbo.QuestionSetTopics", "Topic_Id");
            CreateIndex("dbo.QuestionSetTopics", "QuestionSet_Id");
            CreateIndex("dbo.Friends", "SecondUser_Id");
            CreateIndex("dbo.Friends", "FirstUser_Id");
            AddForeignKey("dbo.Friends", "SecondUser_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Friends", "FirstUser_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.QuestionSetTopics", "Topic_Id", "dbo.Topics", "Id", cascadeDelete: true);
            AddForeignKey("dbo.QuestionSetTopics", "QuestionSet_Id", "dbo.QuestionSets", "Id", cascadeDelete: true);
        }
    }
}
