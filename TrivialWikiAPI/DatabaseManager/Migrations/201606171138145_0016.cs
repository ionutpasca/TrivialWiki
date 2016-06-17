namespace DatabaseManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionSets", "IsValidated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionSets", "IsValidated");
        }
    }
}
