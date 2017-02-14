using System.Data.Entity.Migrations;

namespace RequestsForRights.Database.Migrations
{
    public partial class CorrectDelegationRequestUserRightInfo : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DelegationRequestUserRightExtInfoes", newName: "DelegationRequestUserRightsExtInfo");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.DelegationRequestUserRightsExtInfo", newName: "DelegationRequestUserRightExtInfoes");
        }
    }
}
