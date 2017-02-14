namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class NotRequiredIdFile : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ResourceAuthorityActs", "IdFile", "dbo.ActFiles");
            DropForeignKey("dbo.ResourceOperatorActs", "IdFile", "dbo.ActFiles");
            DropForeignKey("dbo.ResourceOperatorPersonActs", "IdFile", "dbo.ActFiles");
            DropForeignKey("dbo.ResourceOwnerPersonActs", "IdFile", "dbo.ActFiles");
            DropForeignKey("dbo.ResourceUsingActs", "IdFile", "dbo.ActFiles");
            DropIndex("dbo.ResourceAuthorityActs", new[] { "IdFile" });
            DropIndex("dbo.ResourceOperatorActs", new[] { "IdFile" });
            DropIndex("dbo.ResourceOperatorPersonActs", new[] { "IdFile" });
            DropIndex("dbo.ResourceOwnerPersonActs", new[] { "IdFile" });
            DropIndex("dbo.ResourceUsingActs", new[] { "IdFile" });
            AlterColumn("dbo.ResourceAuthorityActs", "IdFile", c => c.Int());
            AlterColumn("dbo.ResourceOperatorActs", "IdFile", c => c.Int());
            AlterColumn("dbo.ResourceOperatorPersonActs", "IdFile", c => c.Int());
            AlterColumn("dbo.ResourceOwnerPersonActs", "IdFile", c => c.Int());
            AlterColumn("dbo.ResourceUsingActs", "IdFile", c => c.Int());
            CreateIndex("dbo.ResourceAuthorityActs", "IdFile");
            CreateIndex("dbo.ResourceOperatorActs", "IdFile");
            CreateIndex("dbo.ResourceOperatorPersonActs", "IdFile");
            CreateIndex("dbo.ResourceOwnerPersonActs", "IdFile");
            CreateIndex("dbo.ResourceUsingActs", "IdFile");
            AddForeignKey("dbo.ResourceAuthorityActs", "IdFile", "dbo.ActFiles", "IdFile");
            AddForeignKey("dbo.ResourceOperatorActs", "IdFile", "dbo.ActFiles", "IdFile");
            AddForeignKey("dbo.ResourceOperatorPersonActs", "IdFile", "dbo.ActFiles", "IdFile");
            AddForeignKey("dbo.ResourceOwnerPersonActs", "IdFile", "dbo.ActFiles", "IdFile");
            AddForeignKey("dbo.ResourceUsingActs", "IdFile", "dbo.ActFiles", "IdFile");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResourceUsingActs", "IdFile", "dbo.ActFiles");
            DropForeignKey("dbo.ResourceOwnerPersonActs", "IdFile", "dbo.ActFiles");
            DropForeignKey("dbo.ResourceOperatorPersonActs", "IdFile", "dbo.ActFiles");
            DropForeignKey("dbo.ResourceOperatorActs", "IdFile", "dbo.ActFiles");
            DropForeignKey("dbo.ResourceAuthorityActs", "IdFile", "dbo.ActFiles");
            DropIndex("dbo.ResourceUsingActs", new[] { "IdFile" });
            DropIndex("dbo.ResourceOwnerPersonActs", new[] { "IdFile" });
            DropIndex("dbo.ResourceOperatorPersonActs", new[] { "IdFile" });
            DropIndex("dbo.ResourceOperatorActs", new[] { "IdFile" });
            DropIndex("dbo.ResourceAuthorityActs", new[] { "IdFile" });
            AlterColumn("dbo.ResourceUsingActs", "IdFile", c => c.Int(nullable: false));
            AlterColumn("dbo.ResourceOwnerPersonActs", "IdFile", c => c.Int(nullable: false));
            AlterColumn("dbo.ResourceOperatorPersonActs", "IdFile", c => c.Int(nullable: false));
            AlterColumn("dbo.ResourceOperatorActs", "IdFile", c => c.Int(nullable: false));
            AlterColumn("dbo.ResourceAuthorityActs", "IdFile", c => c.Int(nullable: false));
            CreateIndex("dbo.ResourceUsingActs", "IdFile");
            CreateIndex("dbo.ResourceOwnerPersonActs", "IdFile");
            CreateIndex("dbo.ResourceOperatorPersonActs", "IdFile");
            CreateIndex("dbo.ResourceOperatorActs", "IdFile");
            CreateIndex("dbo.ResourceAuthorityActs", "IdFile");
            AddForeignKey("dbo.ResourceUsingActs", "IdFile", "dbo.ActFiles", "IdFile", cascadeDelete: true);
            AddForeignKey("dbo.ResourceOwnerPersonActs", "IdFile", "dbo.ActFiles", "IdFile", cascadeDelete: true);
            AddForeignKey("dbo.ResourceOperatorPersonActs", "IdFile", "dbo.ActFiles", "IdFile", cascadeDelete: true);
            AddForeignKey("dbo.ResourceOperatorActs", "IdFile", "dbo.ActFiles", "IdFile", cascadeDelete: true);
            AddForeignKey("dbo.ResourceAuthorityActs", "IdFile", "dbo.ActFiles", "IdFile", cascadeDelete: true);
        }
    }
}
