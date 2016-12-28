namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddAclUsersCorrection : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AclUsers", "IdDepartment");
            RenameColumn(table: "dbo.AclUsers", name: "Department_IdDepartment", newName: "IdDepartment");
            RenameIndex(table: "dbo.AclUsers", name: "IX_Department_IdDepartment", newName: "IX_IdDepartment");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AclUsers", name: "IX_IdDepartment", newName: "IX_Department_IdDepartment");
            RenameColumn(table: "dbo.AclUsers", name: "IdDepartment", newName: "Department_IdDepartment");
            AddColumn("dbo.AclUsers", "IdDepartment", c => c.Int(nullable: false));
        }
    }
}
