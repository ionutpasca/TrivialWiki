namespace DatabaseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0019 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Topics", "ThumbnailPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Topics", "ThumbnailPath");
        }
    }
}
