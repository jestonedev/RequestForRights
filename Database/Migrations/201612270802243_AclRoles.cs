namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AclRoles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AclRoles",
                c => new
                    {
                        IdRole = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.IdRole);
            
            CreateTable(
                "dbo.AclUserAclRoles",
                c => new
                    {
                        IdRole = c.Int(nullable: false),
                        IdUser = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdRole, t.IdUser })
                .ForeignKey("dbo.AclUsers", t => t.IdRole, cascadeDelete: true)
                .ForeignKey("dbo.AclRoles", t => t.IdUser, cascadeDelete: true)
                .Index(t => t.IdRole)
                .Index(t => t.IdUser);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AclUserAclRoles", "IdUser", "dbo.AclRoles");
            DropForeignKey("dbo.AclUserAclRoles", "IdRole", "dbo.AclUsers");
            DropIndex("dbo.AclUserAclRoles", new[] { "IdUser" });
            DropIndex("dbo.AclUserAclRoles", new[] { "IdRole" });
            DropTable("dbo.AclUserAclRoles");
            DropTable("dbo.AclRoles");
        }
    }
}
