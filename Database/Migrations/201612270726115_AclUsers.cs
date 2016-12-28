namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AclUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AclUsers",
                c => new
                    {
                        IdUser = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false, maxLength: 256),
                        IdDepartment = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdUser)
                .ForeignKey("dbo.Departments", t => t.IdDepartment)
                .Index(t => t.IdDepartment);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AclUsers", "IdDepartment", "dbo.Departments");
            DropIndex("dbo.AclUsers", new[] { "IdDepartment" });
            DropTable("dbo.AclUsers");
        }
    }
}
