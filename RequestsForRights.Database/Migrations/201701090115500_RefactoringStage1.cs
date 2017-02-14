using System.Data.Entity.Migrations;

namespace RequestsForRights.Database.Migrations
{
    public partial class RefactoringStage1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RequestUsers", newName: "Users");
            DropForeignKey("dbo.Requests", "IdRequestState", "dbo.RequestStates");
            DropForeignKey("dbo.RequestUserRightAssocs", "IdRequest", "dbo.Requests");
            DropForeignKey("dbo.RequestUserRightAssocs", "IdRequestUser", "dbo.RequestUsers");
            DropIndex("dbo.RequestUserRightAssocs", new[] { "IdRequest" });
            DropIndex("dbo.RequestUserRightAssocs", new[] { "IdRequestUser" });
            DropIndex("dbo.Requests", new[] { "IdRequestState" });
            CreateTable(
                "dbo.RequestUserAssocs",
                c => new
                    {
                        IdRequestUserAssoc = c.Int(nullable: false, identity: true),
                        IdRequest = c.Int(nullable: false),
                        IdRequestUser = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdRequestUserAssoc)
                .ForeignKey("dbo.Requests", t => t.IdRequest, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.IdRequestUser, cascadeDelete: true)
                .Index(t => t.IdRequest)
                .Index(t => t.IdRequestUser);
            
            CreateTable(
                "dbo.RequestStateTypes",
                c => new
                    {
                        IdRequestStateType = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 512),
                    })
                .PrimaryKey(t => t.IdRequestStateType);
            
            AddColumn("dbo.RequestUserRightAssocs", "IdRequestUserAssoc", c => c.Int(nullable: false));
            CreateIndex("dbo.RequestUserRightAssocs", "IdRequestUserAssoc");
            AddForeignKey("dbo.RequestUserRightAssocs", "IdRequestUserAssoc", "dbo.RequestUserAssocs", "IdRequestUserAssoc", cascadeDelete: true);
            DropColumn("dbo.RequestUserRightAssocs", "IdRequest");
            DropColumn("dbo.RequestUserRightAssocs", "IdRequestUser");
            DropColumn("dbo.Requests", "IdRequestState");
            DropTable("dbo.RequestStates");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RequestStates",
                c => new
                    {
                        IdRequestState = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 512),
                    })
                .PrimaryKey(t => t.IdRequestState);
            
            AddColumn("dbo.Requests", "IdRequestState", c => c.Int(nullable: false));
            AddColumn("dbo.RequestUserRightAssocs", "IdRequestUser", c => c.Int(nullable: false));
            AddColumn("dbo.RequestUserRightAssocs", "IdRequest", c => c.Int(nullable: false));
            DropForeignKey("dbo.RequestUserRightAssocs", "IdRequestUserAssoc", "dbo.RequestUserAssocs");
            DropForeignKey("dbo.RequestUserAssocs", "IdRequestUser", "dbo.Users");
            DropForeignKey("dbo.RequestUserAssocs", "IdRequest", "dbo.Requests");
            DropIndex("dbo.RequestUserAssocs", new[] { "IdRequestUser" });
            DropIndex("dbo.RequestUserAssocs", new[] { "IdRequest" });
            DropIndex("dbo.RequestUserRightAssocs", new[] { "IdRequestUserAssoc" });
            DropColumn("dbo.RequestUserRightAssocs", "IdRequestUserAssoc");
            DropTable("dbo.RequestStateTypes");
            DropTable("dbo.RequestUserAssocs");
            CreateIndex("dbo.Requests", "IdRequestState");
            CreateIndex("dbo.RequestUserRightAssocs", "IdRequestUser");
            CreateIndex("dbo.RequestUserRightAssocs", "IdRequest");
            AddForeignKey("dbo.RequestUserRightAssocs", "IdRequestUser", "dbo.RequestUsers", "IdRequestUser", cascadeDelete: true);
            AddForeignKey("dbo.RequestUserRightAssocs", "IdRequest", "dbo.Requests", "IdRequest", cascadeDelete: true);
            AddForeignKey("dbo.Requests", "IdRequestState", "dbo.RequestStates", "IdRequestState");
            RenameTable(name: "dbo.Users", newName: "RequestUsers");
        }
    }
}
