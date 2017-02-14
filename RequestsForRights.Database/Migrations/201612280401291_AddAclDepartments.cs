using System.Data.Entity.Migrations;

namespace RequestsForRights.Database.Migrations
{
    public partial class AddAclDepartments : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AclUsers", "IdDepartment", "dbo.Departments");
            DropIndex("dbo.AclUsers", new[] { "IdDepartment" });
            CreateTable(
                "dbo.AclDepartments",
                c => new
                    {
                        IdDepartment = c.Int(nullable: false),
                        IdUser = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdDepartment, t.IdUser })
                .ForeignKey("dbo.AclUsers", t => t.IdDepartment, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdDepartment)
                .Index(t => t.IdUser);
            
            AddColumn("dbo.AclUsers", "Department_IdDepartment", c => c.Int(nullable: false));
            CreateIndex("dbo.AclUsers", "Department_IdDepartment");
            AddForeignKey("dbo.AclUsers", "Department_IdDepartment", "dbo.Departments", "IdDepartment");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AclUsers", "Department_IdDepartment", "dbo.Departments");
            DropForeignKey("dbo.AclDepartments", "IdUser", "dbo.Departments");
            DropForeignKey("dbo.AclDepartments", "IdDepartment", "dbo.AclUsers");
            DropIndex("dbo.AclDepartments", new[] { "IdUser" });
            DropIndex("dbo.AclDepartments", new[] { "IdDepartment" });
            DropIndex("dbo.AclUsers", new[] { "Department_IdDepartment" });
            DropColumn("dbo.AclUsers", "Department_IdDepartment");
            DropTable("dbo.AclDepartments");
            CreateIndex("dbo.AclUsers", "IdDepartment");
            AddForeignKey("dbo.AclUsers", "IdDepartment", "dbo.Departments", "IdDepartment");
        }
    }
}
