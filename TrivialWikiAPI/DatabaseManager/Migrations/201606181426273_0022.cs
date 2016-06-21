namespace DatabaseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0022 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Topics", "Likes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Topics", "Likes");
        }
    }
}
