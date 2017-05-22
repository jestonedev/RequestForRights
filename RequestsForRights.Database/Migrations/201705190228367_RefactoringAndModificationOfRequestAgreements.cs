namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefactoringAndModificationOfRequestAgreements : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestAgreements", "SendDescription", c => c.String());
            RenameColumn("dbo.RequestAgreements", "RejectReason", "AgreementDescription");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.RequestAgreements", "AgreementDescription", "RejectReason");
            DropColumn("dbo.RequestAgreements", "SendDescription");
        }
    }
}
