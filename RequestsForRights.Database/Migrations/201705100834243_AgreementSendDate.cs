namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgreementSendDate : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RequestStates", "IX_RequestStates_IdRequest_IdRequestStateType_Date_Deleted");
            AddColumn("dbo.RequestAgreements", "SendDate", c => c.DateTime());
            CreateIndex("dbo.RequestStates", new[] { "IdRequest", "IdRequestState", "IdRequestStateType", "Date" }, name: "IX_RequestStates");
            CreateIndex("dbo.RequestStates", "Deleted", name: "IX_RequestStates_IdRequest_IdRequestStateType_Date_Deleted");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RequestStates", "IX_RequestStates_IdRequest_IdRequestStateType_Date_Deleted");
            DropIndex("dbo.RequestStates", "IX_RequestStates");
            DropColumn("dbo.RequestAgreements", "SendDate");
            CreateIndex("dbo.RequestStates", new[] { "IdRequest", "IdRequestStateType", "Date", "Deleted" }, name: "IX_RequestStates_IdRequest_IdRequestStateType_Date_Deleted");
        }
    }
}
