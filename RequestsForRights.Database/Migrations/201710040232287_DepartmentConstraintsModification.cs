namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentConstraintsModification : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Resources", "IdOperatorDepartment", "dbo.Departments");
            AddForeignKey("dbo.Resources", "IdOperatorDepartment", "dbo.Departments", "IdDepartment");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resources", "IdOperatorDepartment", "dbo.Departments");
            AddForeignKey("dbo.Resources", "IdOperatorDepartment", "dbo.Departments", "IdDepartment", cascadeDelete: true);
        }
    }
}
