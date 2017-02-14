using System.Data.Entity.Migrations;

namespace RequestsForRights.Database.Migrations
{
    public partial class AddRequestUserLastSeen : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestUserLastSeens",
                c => new
                    {
                        IdRequestUserLastSeen = c.Int(nullable: false, identity: true),
                        DateOfLastSeen = c.DateTime(nullable: false),
                        IdUser = c.Int(nullable: false),
                        IdRequest = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdRequestUserLastSeen)
                .ForeignKey("dbo.Requests", t => t.IdRequest, cascadeDelete: true)
                .ForeignKey("dbo.AclUsers", t => t.IdUser)
                .Index(t => new { t.IdRequest, t.IdUser }, name: "IDX_RequestUserLastSeen_IdRequest_IdUser");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestUserLastSeens", "IdUser", "dbo.AclUsers");
            DropForeignKey("dbo.RequestUserLastSeens", "IdRequest", "dbo.Requests");
            DropIndex("dbo.RequestUserLastSeens", "IDX_RequestUserLastSeen_IdRequest_IdUser");
            DropTable("dbo.RequestUserLastSeens");
        }
    }
}
