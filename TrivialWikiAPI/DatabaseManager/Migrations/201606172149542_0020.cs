namespace DatabaseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0020 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProposedTopics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TopicName = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProposedTopics", "User_Id", "dbo.Users");
            DropIndex("dbo.ProposedTopics", new[] { "User_Id" });
            DropTable("dbo.ProposedTopics");
        }
    }
}
