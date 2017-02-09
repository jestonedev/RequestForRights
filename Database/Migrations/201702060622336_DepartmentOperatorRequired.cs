namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentOperatorRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Resources", "IdOperatorDepartment", "dbo.Departments");
            DropIndex("dbo.Resources", new[] { "IdOperatorDepartment" });
            AlterColumn("dbo.Resources", "IdOperatorDepartment", c => c.Int(nullable: false));
            CreateIndex("dbo.Resources", "IdOperatorDepartment");
            AddForeignKey("dbo.Resources", "IdOperatorDepartment", "dbo.Departments", "IdDepartment", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resources", "IdOperatorDepartment", "dbo.Departments");
            DropIndex("dbo.Resources", new[] { "IdOperatorDepartment" });
            AlterColumn("dbo.Resources", "IdOperatorDepartment", c => c.Int());
            CreateIndex("dbo.Resources", "IdOperatorDepartment");
            AddForeignKey("dbo.Resources", "IdOperatorDepartment", "dbo.Departments", "IdDepartment");
        }
    }
}
