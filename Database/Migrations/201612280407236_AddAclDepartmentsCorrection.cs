using System.Data.Entity.Migrations;

namespace RequestsForRights.Database.Migrations
{
    public partial class AddAclDepartmentsCorrection : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AclUserAclRoles", name: "IdRole", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.AclUserAclRoles", name: "IdUser", newName: "IdRole");
            RenameColumn(table: "dbo.AclDepartments", name: "IdDepartment", newName: "__mig_tmp__1");
            RenameColumn(table: "dbo.AclDepartments", name: "IdUser", newName: "IdDepartment");
            RenameColumn(table: "dbo.DepartmentResources", name: "IdResource", newName: "__mig_tmp__2");
            RenameColumn(table: "dbo.DepartmentResources", name: "IdDepartment", newName: "IdResource");
            RenameColumn(table: "dbo.AclUserAclRoles", name: "__mig_tmp__0", newName: "IdUser");
            RenameColumn(table: "dbo.AclDepartments", name: "__mig_tmp__1", newName: "IdUser");
            RenameColumn(table: "dbo.DepartmentResources", name: "__mig_tmp__2", newName: "IdDepartment");
            RenameIndex(table: "dbo.DepartmentResources", name: "IX_IdResource", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.DepartmentResources", name: "IX_IdDepartment", newName: "IX_IdResource");
            RenameIndex(table: "dbo.AclDepartments", name: "IX_IdDepartment", newName: "__mig_tmp__1");
            RenameIndex(table: "dbo.AclDepartments", name: "IX_IdUser", newName: "IX_IdDepartment");
            RenameIndex(table: "dbo.AclUserAclRoles", name: "IX_IdRole", newName: "__mig_tmp__2");
            RenameIndex(table: "dbo.AclUserAclRoles", name: "IX_IdUser", newName: "IX_IdRole");
            RenameIndex(table: "dbo.DepartmentResources", name: "__mig_tmp__0", newName: "IX_IdDepartment");
            RenameIndex(table: "dbo.AclDepartments", name: "__mig_tmp__1", newName: "IX_IdUser");
            RenameIndex(table: "dbo.AclUserAclRoles", name: "__mig_tmp__2", newName: "IX_IdUser");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AclUserAclRoles", name: "IX_IdUser", newName: "__mig_tmp__2");
            RenameIndex(table: "dbo.AclDepartments", name: "IX_IdUser", newName: "__mig_tmp__1");
            RenameIndex(table: "dbo.DepartmentResources", name: "IX_IdDepartment", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.AclUserAclRoles", name: "IX_IdRole", newName: "IX_IdUser");
            RenameIndex(table: "dbo.AclUserAclRoles", name: "__mig_tmp__2", newName: "IX_IdRole");
            RenameIndex(table: "dbo.AclDepartments", name: "IX_IdDepartment", newName: "IX_IdUser");
            RenameIndex(table: "dbo.AclDepartments", name: "__mig_tmp__1", newName: "IX_IdDepartment");
            RenameIndex(table: "dbo.DepartmentResources", name: "IX_IdResource", newName: "IX_IdDepartment");
            RenameIndex(table: "dbo.DepartmentResources", name: "__mig_tmp__0", newName: "IX_IdResource");
            RenameColumn(table: "dbo.DepartmentResources", name: "IdDepartment", newName: "__mig_tmp__2");
            RenameColumn(table: "dbo.AclDepartments", name: "IdUser", newName: "__mig_tmp__1");
            RenameColumn(table: "dbo.AclUserAclRoles", name: "IdUser", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.DepartmentResources", name: "IdResource", newName: "IdDepartment");
            RenameColumn(table: "dbo.DepartmentResources", name: "__mig_tmp__2", newName: "IdResource");
            RenameColumn(table: "dbo.AclDepartments", name: "IdDepartment", newName: "IdUser");
            RenameColumn(table: "dbo.AclDepartments", name: "__mig_tmp__1", newName: "IdDepartment");
            RenameColumn(table: "dbo.AclUserAclRoles", name: "IdRole", newName: "IdUser");
            RenameColumn(table: "dbo.AclUserAclRoles", name: "__mig_tmp__0", newName: "IdRole");
        }
    }
}
