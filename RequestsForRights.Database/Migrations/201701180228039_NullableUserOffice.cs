namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class NullableUserOffice : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequestUsers", "Office", c => c.String(maxLength: 512));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestUsers", "Office", c => c.String(nullable: false, maxLength: 512));
        }
    }
}
