namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Renaming : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.RequestAgreements", "Description", "RejectReason");
            RenameColumn("dbo.RequestAgreements", "Date", "AgreementDate");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.RequestAgreements", "RejectReason", "Description");
            RenameColumn("dbo.RequestAgreements", "AgreementDate", "Date");
        }
    }
}
