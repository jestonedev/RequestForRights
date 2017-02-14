using System.Data.Entity.Migrations;

namespace RequestsForRights.Database.Migrations
{
    public partial class AddRequestRightUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "IdRequestState", "dbo.RequestStates");
            DropForeignKey("dbo.Requests", "IdRequestType", "dbo.RequestTypes");
            CreateTable(
                "dbo.RequestUserRightAssocs",
                c => new
                    {
                        IdAssoc = c.Int(nullable: false, identity: true),
                        IdRequest = c.Int(nullable: false),
                        IdRequestUser = c.Int(nullable: false),
                        IdResourceRight = c.Int(nullable: false),
                        IdRequestRightGrantType = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdAssoc)
                .ForeignKey("dbo.Requests", t => t.IdRequest, cascadeDelete: true)
                .ForeignKey("dbo.RequestRightGrantTypes", t => t.IdRequestRightGrantType, cascadeDelete: true)
                .ForeignKey("dbo.RequestUsers", t => t.IdRequestUser, cascadeDelete: true)
                .ForeignKey("dbo.ResourceRights", t => t.IdResourceRight)
                .Index(t => t.IdRequest)
                .Index(t => t.IdRequestUser)
                .Index(t => t.IdResourceRight)
                .Index(t => t.IdRequestRightGrantType);
            
            CreateTable(
                "dbo.RequestRightGrantTypes",
                c => new
                    {
                        IdRequestRightGrantType = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdRequestRightGrantType);
            
            CreateTable(
                "dbo.RequestUsers",
                c => new
                    {
                        IdRequestUser = c.Int(nullable: false, identity: true),
                        Login = c.String(maxLength: 256),
                        Snp = c.String(nullable: false, maxLength: 512),
                        Post = c.String(nullable: false, maxLength: 512),
                        Phone = c.String(maxLength: 512),
                        Department = c.String(nullable: false, maxLength: 512),
                        Unit = c.String(maxLength: 512),
                        Office = c.String(nullable: false, maxLength: 512),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdRequestUser);
            
            AddForeignKey("dbo.Requests", "IdRequestState", "dbo.RequestStates", "IdRequestState");
            AddForeignKey("dbo.Requests", "IdRequestType", "dbo.RequestTypes", "IdRequestType");
            DropColumn("dbo.RequestStates", "Deleted");
            DropColumn("dbo.RequestTypes", "Deleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RequestTypes", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequestStates", "Deleted", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Requests", "IdRequestType", "dbo.RequestTypes");
            DropForeignKey("dbo.Requests", "IdRequestState", "dbo.RequestStates");
            DropForeignKey("dbo.RequestUserRightAssocs", "IdResourceRight", "dbo.ResourceRights");
            DropForeignKey("dbo.RequestUserRightAssocs", "IdRequestUser", "dbo.RequestUsers");
            DropForeignKey("dbo.RequestUserRightAssocs", "IdRequestRightGrantType", "dbo.RequestRightGrantTypes");
            DropForeignKey("dbo.RequestUserRightAssocs", "IdRequest", "dbo.Requests");
            DropIndex("dbo.RequestUserRightAssocs", new[] { "IdRequestRightGrantType" });
            DropIndex("dbo.RequestUserRightAssocs", new[] { "IdResourceRight" });
            DropIndex("dbo.RequestUserRightAssocs", new[] { "IdRequestUser" });
            DropIndex("dbo.RequestUserRightAssocs", new[] { "IdRequest" });
            DropTable("dbo.RequestUsers");
            DropTable("dbo.RequestRightGrantTypes");
            DropTable("dbo.RequestUserRightAssocs");
            AddForeignKey("dbo.Requests", "IdRequestType", "dbo.RequestTypes", "IdRequestType", cascadeDelete: true);
            AddForeignKey("dbo.Requests", "IdRequestState", "dbo.RequestStates", "IdRequestState", cascadeDelete: true);
        }
    }
}
