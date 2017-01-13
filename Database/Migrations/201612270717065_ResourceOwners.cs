using System.Data.Entity.Migrations;

namespace RequestsForRights.Database.Migrations
{
    public partial class ResourceOwners : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResourceDepartments",
                c => new
                    {
                        Resource_IdResource = c.Int(nullable: false),
                        Department_IdDepartment = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Resource_IdResource, t.Department_IdDepartment })
                .ForeignKey("dbo.Resources", t => t.Resource_IdResource, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.Department_IdDepartment, cascadeDelete: true)
                .Index(t => t.Resource_IdResource)
                .Index(t => t.Department_IdDepartment);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResourceDepartments", "Department_IdDepartment", "dbo.Departments");
            DropForeignKey("dbo.ResourceDepartments", "Resource_IdResource", "dbo.Resources");
            DropIndex("dbo.ResourceDepartments", new[] { "Department_IdDepartment" });
            DropIndex("dbo.ResourceDepartments", new[] { "Resource_IdResource" });
            DropTable("dbo.ResourceDepartments");
        }
    }
}
