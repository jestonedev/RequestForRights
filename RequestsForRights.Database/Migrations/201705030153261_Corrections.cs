namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Corrections : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "IsAlienDepartment", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequestUsers", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.RequestUsers", "Post", c => c.String(maxLength: 512));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestUsers", "Post", c => c.String(nullable: false, maxLength: 512));
            DropColumn("dbo.RequestUsers", "IsActive");
            DropColumn("dbo.Departments", "IsAlienDepartment");
        }
    }
}
