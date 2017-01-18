namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class NullableDateRequestAgreement : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequestAgreements", "Date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestAgreements", "Date", c => c.DateTime(nullable: false));
        }
    }
}
