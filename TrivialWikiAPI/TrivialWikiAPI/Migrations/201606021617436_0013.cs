namespace TrivialWikiAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0013 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "AccountCreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "AccountCreationDate");
        }
    }
}
