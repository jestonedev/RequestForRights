using System.Data.Entity.Migrations;

namespace RequestsForRights.Database.Migrations
{
    public partial class Departments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        IdDepartment = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 512),
                        IdParentDepartment = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdDepartment)
                .ForeignKey("dbo.Departments", t => t.IdParentDepartment)
                .Index(t => t.IdParentDepartment);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Departments", "IdParentDepartment", "dbo.Departments");
            DropIndex("dbo.Departments", new[] { "IdParentDepartment" });
            DropTable("dbo.Departments");
        }
    }
}
