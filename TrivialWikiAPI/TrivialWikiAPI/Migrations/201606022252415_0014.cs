namespace TrivialWikiAPI.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class _0014 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TriviaMessages",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Sender = c.String(),
                    MessageText = c.String(),
                    Timestamp = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.TriviaMessages");
        }
    }
}
