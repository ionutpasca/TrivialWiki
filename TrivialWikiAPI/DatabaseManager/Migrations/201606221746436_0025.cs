namespace DatabaseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0025 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuestionSetTopics", "QuestionSet_Id", "dbo.QuestionSets");
            DropForeignKey("dbo.QuestionSetTopics", "Topic_Id", "dbo.Topics");
            DropIndex("dbo.QuestionSetTopics", new[] { "QuestionSet_Id" });
            DropIndex("dbo.QuestionSetTopics", new[] { "Topic_Id" });
            AddColumn("dbo.QuestionSets", "Topic_Id", c => c.Int());
            CreateIndex("dbo.QuestionSets", "Topic_Id");
            AddForeignKey("dbo.QuestionSets", "Topic_Id", "dbo.Topics", "Id");
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
            
            DropForeignKey("dbo.QuestionSets", "Topic_Id", "dbo.Topics");
            DropIndex("dbo.QuestionSets", new[] { "Topic_Id" });
            DropColumn("dbo.QuestionSets", "Topic_Id");
            CreateIndex("dbo.QuestionSetTopics", "Topic_Id");
            CreateIndex("dbo.QuestionSetTopics", "QuestionSet_Id");
            AddForeignKey("dbo.QuestionSetTopics", "Topic_Id", "dbo.Topics", "Id", cascadeDelete: true);
            AddForeignKey("dbo.QuestionSetTopics", "QuestionSet_Id", "dbo.QuestionSets", "Id", cascadeDelete: true);
        }
    }
}
