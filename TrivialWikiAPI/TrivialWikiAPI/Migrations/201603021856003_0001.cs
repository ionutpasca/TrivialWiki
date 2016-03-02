namespace TrivialWikiAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0001 : DbMigration
    {
        public override void Up()
        {
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
                "dbo.RelatedTopics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TopicRelatedTopics",
                c => new
                    {
                        Topic_Id = c.Int(nullable: false),
                        RelatedTopic_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Topic_Id, t.RelatedTopic_Id })
                .ForeignKey("dbo.Topics", t => t.Topic_Id, cascadeDelete: true)
                .ForeignKey("dbo.RelatedTopics", t => t.RelatedTopic_Id, cascadeDelete: true)
                .Index(t => t.Topic_Id)
                .Index(t => t.RelatedTopic_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TopicRelatedTopics", "RelatedTopic_Id", "dbo.RelatedTopics");
            DropForeignKey("dbo.TopicRelatedTopics", "Topic_Id", "dbo.Topics");
            DropIndex("dbo.TopicRelatedTopics", new[] { "RelatedTopic_Id" });
            DropIndex("dbo.TopicRelatedTopics", new[] { "Topic_Id" });
            DropTable("dbo.TopicRelatedTopics");
            DropTable("dbo.Topics");
            DropTable("dbo.RelatedTopics");
            DropTable("dbo.Achievements");
        }
    }
}
