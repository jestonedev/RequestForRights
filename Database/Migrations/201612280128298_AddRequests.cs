namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddRequests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        IdRequest = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        FillingDate = c.DateTime(nullable: false),
                        ComplitionDate = c.DateTime(),
                        IdUser = c.Int(nullable: false),
                        IdRequestState = c.Int(nullable: false),
                        IdRequestType = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdRequest)
                .ForeignKey("dbo.RequestStates", t => t.IdRequestState, cascadeDelete: true)
                .ForeignKey("dbo.RequestTypes", t => t.IdRequestType, cascadeDelete: true)
                .ForeignKey("dbo.AclUsers", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdUser)
                .Index(t => t.IdRequestState)
                .Index(t => t.IdRequestType);
            
            CreateTable(
                "dbo.RequestStates",
                c => new
                    {
                        IdRequestState = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 512),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdRequestState);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "IdUser", "dbo.AclUsers");
            DropForeignKey("dbo.Requests", "IdRequestType", "dbo.RequestTypes");
            DropForeignKey("dbo.Requests", "IdRequestState", "dbo.RequestStates");
            DropIndex("dbo.Requests", new[] { "IdRequestType" });
            DropIndex("dbo.Requests", new[] { "IdRequestState" });
            DropIndex("dbo.Requests", new[] { "IdUser" });
            DropTable("dbo.RequestStates");
            DropTable("dbo.Requests");
        }
    }
}
