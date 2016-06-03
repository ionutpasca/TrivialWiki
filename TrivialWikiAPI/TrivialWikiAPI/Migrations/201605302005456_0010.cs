namespace TrivialWikiAPI.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _0010 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "Avatar");
            AddColumn("dbo.Users", "Avatar", c => c.Binary());
        }

        public override void Down()
        {
            AlterColumn("dbo.Users", "Avatar", c => c.String());
        }
    }
}
