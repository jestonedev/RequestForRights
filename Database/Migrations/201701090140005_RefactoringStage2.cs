using System.Data.Entity.Migrations;

namespace RequestsForRights.Database.Migrations
{
    public partial class RefactoringStage2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestStates",
                c => new
                    {
                        IdRequestState = c.Int(nullable: false, identity: true),
                        IdRequestStateType = c.Int(nullable: false),
                        IdRequest = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdRequestState)
                .ForeignKey("dbo.Requests", t => t.IdRequest, cascadeDelete: true)
                .ForeignKey("dbo.RequestStateTypes", t => t.IdRequestStateType)
                .Index(t => t.IdRequestStateType)
                .Index(t => t.IdRequest);
            
            DropColumn("dbo.Requests", "DateOfFilling");
            DropColumn("dbo.Requests", "DateOfCompletion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "DateOfCompletion", c => c.DateTime());
            AddColumn("dbo.Requests", "DateOfFilling", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.RequestStates", "IdRequestStateType", "dbo.RequestStateTypes");
            DropForeignKey("dbo.RequestStates", "IdRequest", "dbo.Requests");
            DropIndex("dbo.RequestStates", new[] { "IdRequest" });
            DropIndex("dbo.RequestStates", new[] { "IdRequestStateType" });
            DropTable("dbo.RequestStates");
        }
    }
}
