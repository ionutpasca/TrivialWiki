namespace TrivialWikiAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0005 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "User_Id", "dbo.Users");
            DropIndex("dbo.Users", new[] { "User_Id" });
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
            
            DropColumn("dbo.Users", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "User_Id", c => c.Int());
            DropForeignKey("dbo.Friends", "SecondUser_Id", "dbo.Users");
            DropForeignKey("dbo.Friends", "FirstUser_Id", "dbo.Users");
            DropIndex("dbo.Friends", new[] { "SecondUser_Id" });
            DropIndex("dbo.Friends", new[] { "FirstUser_Id" });
            DropTable("dbo.Friends");
            CreateIndex("dbo.Users", "User_Id");
            AddForeignKey("dbo.Users", "User_Id", "dbo.Users", "Id");
        }
    }
}
