namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameExtDescriptionsTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RequestExtDescriptions", "IdRequest", "dbo.Requests");
            DropForeignKey("dbo.RequestExtDescriptions", "IdUser", "dbo.AclUsers");
            DropIndex("dbo.RequestExtDescriptions", new[] { "IdRequest" });
            DropIndex("dbo.RequestExtDescriptions", new[] { "IdUser" });
            CreateTable(
                "dbo.RequestExtComments",
                c => new
                    {
                        IdComment = c.Int(nullable: false, identity: true),
                        Comment = c.String(nullable: false),
                        DateOfWriting = c.DateTime(nullable: false),
                        IdRequest = c.Int(nullable: false),
                        IdUser = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdComment)
                .ForeignKey("dbo.Requests", t => t.IdRequest, cascadeDelete: true)
                .ForeignKey("dbo.AclUsers", t => t.IdUser)
                .Index(t => t.IdRequest)
                .Index(t => t.IdUser);
            
            DropTable("dbo.RequestExtDescriptions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RequestExtDescriptions",
                c => new
                    {
                        IdDescription = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        DateOfWriting = c.DateTime(nullable: false),
                        IdRequest = c.Int(nullable: false),
                        IdUser = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdDescription);
            
            DropForeignKey("dbo.RequestExtComments", "IdUser", "dbo.AclUsers");
            DropForeignKey("dbo.RequestExtComments", "IdRequest", "dbo.Requests");
            DropIndex("dbo.RequestExtComments", new[] { "IdUser" });
            DropIndex("dbo.RequestExtComments", new[] { "IdRequest" });
            DropTable("dbo.RequestExtComments");
            CreateIndex("dbo.RequestExtDescriptions", "IdUser");
            CreateIndex("dbo.RequestExtDescriptions", "IdRequest");
            AddForeignKey("dbo.RequestExtDescriptions", "IdUser", "dbo.AclUsers", "IdUser");
            AddForeignKey("dbo.RequestExtDescriptions", "IdRequest", "dbo.Requests", "IdRequest", cascadeDelete: true);
        }
    }
}
