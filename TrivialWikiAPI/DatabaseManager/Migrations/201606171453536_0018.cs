namespace DatabaseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0018 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "Seen", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "Seen");
        }
    }
}
