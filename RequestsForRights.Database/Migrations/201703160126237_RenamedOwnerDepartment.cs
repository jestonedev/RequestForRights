namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamedOwnerDepartment : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Resources", name: "IdDepartment", newName: "IdOwnerDepartment");
            RenameIndex(table: "dbo.Resources", name: "IX_IdDepartment", newName: "IX_IdOwnerDepartment");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Resources", name: "IX_IdOwnerDepartment", newName: "IX_IdDepartment");
            RenameColumn(table: "dbo.Resources", name: "IdOwnerDepartment", newName: "IdDepartment");
        }
    }
}
