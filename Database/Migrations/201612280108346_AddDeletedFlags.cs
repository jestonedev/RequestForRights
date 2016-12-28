namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddDeletedFlags : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resources", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.ResourceGroups", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.ResourceRights", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequestTypes", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestTypes", "Deleted");
            DropColumn("dbo.ResourceRights", "Deleted");
            DropColumn("dbo.ResourceGroups", "Deleted");
            DropColumn("dbo.Resources", "Deleted");
            DropColumn("dbo.Departments", "Deleted");
        }
    }
}
