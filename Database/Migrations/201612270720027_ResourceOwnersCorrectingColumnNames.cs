namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ResourceOwnersCorrectingColumnNames : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ResourceDepartments", newName: "DepartmentResources");
            RenameColumn(table: "dbo.DepartmentResources", name: "Resource_IdResource", newName: "IdDepartment");
            RenameColumn(table: "dbo.DepartmentResources", name: "Department_IdDepartment", newName: "IdResource");
            RenameIndex(table: "dbo.DepartmentResources", name: "IX_Department_IdDepartment", newName: "IX_IdResource");
            RenameIndex(table: "dbo.DepartmentResources", name: "IX_Resource_IdResource", newName: "IX_IdDepartment");
            DropPrimaryKey("dbo.DepartmentResources");
            AddPrimaryKey("dbo.DepartmentResources", new[] { "IdResource", "IdDepartment" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.DepartmentResources");
            AddPrimaryKey("dbo.DepartmentResources", new[] { "Resource_IdResource", "Department_IdDepartment" });
            RenameIndex(table: "dbo.DepartmentResources", name: "IX_IdDepartment", newName: "IX_Resource_IdResource");
            RenameIndex(table: "dbo.DepartmentResources", name: "IX_IdResource", newName: "IX_Department_IdDepartment");
            RenameColumn(table: "dbo.DepartmentResources", name: "IdResource", newName: "Department_IdDepartment");
            RenameColumn(table: "dbo.DepartmentResources", name: "IdDepartment", newName: "Resource_IdResource");
            RenameTable(name: "dbo.DepartmentResources", newName: "ResourceDepartments");
        }
    }
}
