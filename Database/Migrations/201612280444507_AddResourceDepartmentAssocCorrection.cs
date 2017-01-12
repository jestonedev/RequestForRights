namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddResourceDepartmentAssocCorrection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DepartmentResources", "IdDepartment", "dbo.Departments");
            DropForeignKey("dbo.DepartmentResources", "IdResource", "dbo.Resources");
            DropIndex("dbo.DepartmentResources", new[] { "IdDepartment" });
            DropIndex("dbo.DepartmentResources", new[] { "IdResource" });
            AddColumn("dbo.Resources", "IdDepartment", c => c.Int(nullable: false));
            CreateIndex("dbo.Resources", "IdDepartment");
            AddForeignKey("dbo.Resources", "IdDepartment", "dbo.Departments", "IdDepartment");
            DropTable("dbo.DepartmentResources");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DepartmentResources",
                c => new
                    {
                        IdDepartment = c.Int(nullable: false),
                        IdResource = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdDepartment, t.IdResource });
            
            DropForeignKey("dbo.Resources", "IdDepartment", "dbo.Departments");
            DropIndex("dbo.Resources", new[] { "IdDepartment" });
            DropColumn("dbo.Resources", "IdDepartment");
            CreateIndex("dbo.DepartmentResources", "IdResource");
            CreateIndex("dbo.DepartmentResources", "IdDepartment");
            AddForeignKey("dbo.DepartmentResources", "IdResource", "dbo.Resources", "IdResource", cascadeDelete: true);
            AddForeignKey("dbo.DepartmentResources", "IdDepartment", "dbo.Departments", "IdDepartment", cascadeDelete: true);
        }
    }
}
