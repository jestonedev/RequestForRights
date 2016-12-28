namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestExtDescription : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "IdUser", "dbo.AclUsers");
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
                .PrimaryKey(t => t.IdDescription)
                .ForeignKey("dbo.Requests", t => t.IdRequest, cascadeDelete: true)
                .ForeignKey("dbo.AclUsers", t => t.IdUser)
                .Index(t => t.IdRequest)
                .Index(t => t.IdUser);
            
            AddColumn("dbo.Requests", "DateOfFilling", c => c.DateTime(nullable: false));
            AddColumn("dbo.Requests", "DateOfCompletion", c => c.DateTime());
            AddForeignKey("dbo.Requests", "IdUser", "dbo.AclUsers", "IdUser");
            DropColumn("dbo.Requests", "FillingDate");
            DropColumn("dbo.Requests", "ComplitionDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "ComplitionDate", c => c.DateTime());
            AddColumn("dbo.Requests", "FillingDate", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Requests", "IdUser", "dbo.AclUsers");
            DropForeignKey("dbo.RequestExtDescriptions", "IdUser", "dbo.AclUsers");
            DropForeignKey("dbo.RequestExtDescriptions", "IdRequest", "dbo.Requests");
            DropIndex("dbo.RequestExtDescriptions", new[] { "IdUser" });
            DropIndex("dbo.RequestExtDescriptions", new[] { "IdRequest" });
            DropColumn("dbo.Requests", "DateOfCompletion");
            DropColumn("dbo.Requests", "DateOfFilling");
            DropTable("dbo.RequestExtDescriptions");
            AddForeignKey("dbo.Requests", "IdUser", "dbo.AclUsers", "IdUser", cascadeDelete: true);
        }
    }
}
