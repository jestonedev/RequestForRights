namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class RequireCorrections : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Departments", new[] { "IdParentDepartment" });
            AddColumn("dbo.AclUsers", "Deleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Departments", "IdParentDepartment", c => c.Int());
            AlterColumn("dbo.RequestRightGrantTypes", "Name", c => c.String(nullable: false, maxLength: 512));
            CreateIndex("dbo.Departments", "IdParentDepartment");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Departments", new[] { "IdParentDepartment" });
            AlterColumn("dbo.RequestRightGrantTypes", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Departments", "IdParentDepartment", c => c.Int(nullable: false));
            DropColumn("dbo.AclUsers", "Deleted");
            CreateIndex("dbo.Departments", "IdParentDepartment");
        }
    }
}
