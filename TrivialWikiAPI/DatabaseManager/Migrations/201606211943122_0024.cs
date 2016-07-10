namespace DatabaseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0024 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TriviaMessages", "TableName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TriviaMessages", "TableName");
        }
    }
}
