namespace TrivialWikiAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0006 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "FirstName");
            DropColumn("dbo.Users", "LastName");
            DropColumn("dbo.Users", "Sex");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Sex", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "LastName", c => c.String());
            AddColumn("dbo.Users", "FirstName", c => c.String());
        }
    }
}
