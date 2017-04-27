namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestAllowedResourceDepartments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestAllowedResourceDepartments",
                c => new
                    {
                        IdResource = c.Int(false),
                        IdDepartment = c.Int(false),
                    })
                .PrimaryKey(t => new { t.IdResource, t.IdDepartment })
                .ForeignKey("dbo.Resources", t => t.IdResource, true)
                .ForeignKey("dbo.Departments", t => t.IdDepartment /* disabled cascade manualy */)
                .Index(t => t.IdResource)
                .Index(t => t.IdDepartment);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestAllowedResourceDepartments", "IdDepartment", "dbo.Departments");
            DropForeignKey("dbo.RequestAllowedResourceDepartments", "IdResource", "dbo.Resources");
            DropIndex("dbo.RequestAllowedResourceDepartments", new[] { "IdDepartment" });
            DropIndex("dbo.RequestAllowedResourceDepartments", new[] { "IdResource" });
            DropTable("dbo.RequestAllowedResourceDepartments");
        }
    }
}
