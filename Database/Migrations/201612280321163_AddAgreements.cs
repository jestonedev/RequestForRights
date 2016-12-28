namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddAgreements : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestAgreements",
                c => new
                    {
                        IdRequestAgreement = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false),
                        IdUser = c.Int(nullable: false),
                        IdRequest = c.Int(nullable: false),
                        IdAgreementState = c.Int(nullable: false),
                        IdAgreementType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdRequestAgreement)
                .ForeignKey("dbo.RequestAgreementStates", t => t.IdAgreementState)
                .ForeignKey("dbo.RequestAgreementTypes", t => t.IdAgreementType)
                .ForeignKey("dbo.Requests", t => t.IdRequest, cascadeDelete: true)
                .ForeignKey("dbo.AclUsers", t => t.IdUser)
                .Index(t => t.IdUser)
                .Index(t => t.IdRequest)
                .Index(t => t.IdAgreementState)
                .Index(t => t.IdAgreementType);
            
            CreateTable(
                "dbo.RequestAgreementStates",
                c => new
                    {
                        IdAgreementState = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdAgreementState);
            
            CreateTable(
                "dbo.RequestAgreementTypes",
                c => new
                    {
                        IdAgreementType = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IdAgreementType);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestAgreements", "IdUser", "dbo.AclUsers");
            DropForeignKey("dbo.RequestAgreements", "IdRequest", "dbo.Requests");
            DropForeignKey("dbo.RequestAgreements", "IdAgreementType", "dbo.RequestAgreementTypes");
            DropForeignKey("dbo.RequestAgreements", "IdAgreementState", "dbo.RequestAgreementStates");
            DropIndex("dbo.RequestAgreements", new[] { "IdAgreementType" });
            DropIndex("dbo.RequestAgreements", new[] { "IdAgreementState" });
            DropIndex("dbo.RequestAgreements", new[] { "IdRequest" });
            DropIndex("dbo.RequestAgreements", new[] { "IdUser" });
            DropTable("dbo.RequestAgreementTypes");
            DropTable("dbo.RequestAgreementStates");
            DropTable("dbo.RequestAgreements");
        }
    }
}
