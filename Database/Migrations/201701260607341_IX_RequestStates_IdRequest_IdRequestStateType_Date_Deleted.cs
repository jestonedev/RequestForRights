namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IX_RequestStates_IdRequest_IdRequestStateType_Date_Deleted : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RequestStates", new[] { "IdRequestStateType" });
            DropIndex("dbo.RequestStates", new[] { "IdRequest" });
            CreateIndex("dbo.RequestStates", new[] { "IdRequest", "IdRequestStateType", "Date", "Deleted" }, name: "IX_RequestStates_IdRequest_IdRequestStateType_Date_Deleted");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RequestStates", "IX_RequestStates_IdRequest_IdRequestStateType_Date_Deleted");
            CreateIndex("dbo.RequestStates", "IdRequest");
            CreateIndex("dbo.RequestStates", "IdRequestStateType");
        }
    }
}
