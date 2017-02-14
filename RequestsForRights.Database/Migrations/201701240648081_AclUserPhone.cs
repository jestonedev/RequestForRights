namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AclUserPhone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AclUsers", "Phone", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AclUsers", "Phone");
        }
    }
}
