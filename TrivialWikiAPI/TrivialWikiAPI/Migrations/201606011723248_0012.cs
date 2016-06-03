namespace TrivialWikiAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0012 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "Avatar");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Avatar", c => c.String());
        }
    }
}
