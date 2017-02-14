using System.Data.Entity.Migrations;

namespace RequestsForRights.Database.Migrations
{
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestTypes",
                c => new
                    {
                        IdRequestType = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 512),
                    })
                .PrimaryKey(t => t.IdRequestType);
            
            CreateTable(
                "dbo.ResourceGroups",
                c => new
                    {
                        IdResourceGroup = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 512),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.IdResourceGroup);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        IdResource = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 512),
                        Description = c.String(),
                        IdResourceGroup = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdResource)
                .ForeignKey("dbo.ResourceGroups", t => t.IdResourceGroup)
                .Index(t => t.IdResourceGroup);
            
            CreateTable(
                "dbo.ResourceRights",
                c => new
                    {
                        IdResourceRight = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 512),
                        Description = c.String(),
                        IdResource = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdResourceRight)
                .ForeignKey("dbo.Resources", t => t.IdResource, cascadeDelete: true)
                .Index(t => t.IdResource);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resources", "IdResourceGroup", "dbo.ResourceGroups");
            DropForeignKey("dbo.ResourceRights", "IdResource", "dbo.Resources");
            DropIndex("dbo.ResourceRights", new[] { "IdResource" });
            DropIndex("dbo.Resources", new[] { "IdResourceGroup" });
            DropTable("dbo.ResourceRights");
            DropTable("dbo.Resources");
            DropTable("dbo.ResourceGroups");
            DropTable("dbo.RequestTypes");
        }
    }
}
