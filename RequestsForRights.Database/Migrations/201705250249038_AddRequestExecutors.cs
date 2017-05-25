namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestExecutors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestExecutors",
                c => new
                    {
                        IdRequest = c.Int(nullable: false),
                        Login = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => new { t.IdRequest, t.Login })
                .ForeignKey("dbo.Requests", t => t.IdRequest, cascadeDelete: true)
                .Index(t => t.IdRequest);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestExecutors", "IdRequest", "dbo.Requests");
            DropIndex("dbo.RequestExecutors", new[] { "IdRequest" });
            DropTable("dbo.RequestExecutors");
        }
    }
}
