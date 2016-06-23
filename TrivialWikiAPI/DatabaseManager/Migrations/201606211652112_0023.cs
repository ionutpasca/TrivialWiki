namespace DatabaseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0023 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Friends", "FirstUser_Id", "dbo.Users");
            DropForeignKey("dbo.Friends", "SecondUser_Id", "dbo.Users");
            DropIndex("dbo.Friends", new[] { "FirstUser_Id" });
            DropIndex("dbo.Friends", new[] { "SecondUser_Id" });
            AddColumn("dbo.Users", "User_Id", c => c.Int());
            CreateIndex("dbo.Users", "User_Id");
            AddForeignKey("dbo.Users", "User_Id", "dbo.Users", "Id");
            DropTable("dbo.Friends");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstUser_Id = c.Int(),
                        SecondUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Users", "User_Id", "dbo.Users");
            DropIndex("dbo.Users", new[] { "User_Id" });
            DropColumn("dbo.Users", "User_Id");
            CreateIndex("dbo.Friends", "SecondUser_Id");
            CreateIndex("dbo.Friends", "FirstUser_Id");
            AddForeignKey("dbo.Friends", "SecondUser_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Friends", "FirstUser_Id", "dbo.Users", "Id");
        }
    }
}
