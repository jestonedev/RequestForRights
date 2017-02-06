namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtractActFilesToCommonTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActFiles",
                c => new
                    {
                        IdFile = c.Int(nullable: false, identity: true),
                        FileOriginalName = c.String(),
                        FileContent = c.Binary(),
                        FileContentType = c.String(),
                    })
                .PrimaryKey(t => t.IdFile);
            
            AddColumn("dbo.ResourceAuthorityActs", "IdFile", c => c.Int(nullable: false));
            AddColumn("dbo.ResourceAuthorityActs", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.ResourceOperatorActs", "IdFile", c => c.Int(nullable: false));
            AddColumn("dbo.ResourceOperatorActs", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.ResourceUsingActs", "IdFile", c => c.Int(nullable: false));
            AddColumn("dbo.ResourceUsingActs", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.ResourceOperatorPersonActs", "IdFile", c => c.Int(nullable: false));
            AddColumn("dbo.ResourceOperatorPersonActs", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.ResourceOwnerPersonActs", "IdFile", c => c.Int(nullable: false));
            AddColumn("dbo.ResourceOwnerPersonActs", "Deleted", c => c.Boolean(nullable: false));
            CreateIndex("dbo.ResourceAuthorityActs", "IdFile");
            CreateIndex("dbo.ResourceOperatorActs", "IdFile");
            CreateIndex("dbo.ResourceUsingActs", "IdFile");
            CreateIndex("dbo.ResourceOperatorPersonActs", "IdFile");
            CreateIndex("dbo.ResourceOwnerPersonActs", "IdFile");
            AddForeignKey("dbo.ResourceAuthorityActs", "IdFile", "dbo.ActFiles", "IdFile", cascadeDelete: true);
            AddForeignKey("dbo.ResourceOperatorActs", "IdFile", "dbo.ActFiles", "IdFile", cascadeDelete: true);
            AddForeignKey("dbo.ResourceUsingActs", "IdFile", "dbo.ActFiles", "IdFile", cascadeDelete: true);
            AddForeignKey("dbo.ResourceOperatorPersonActs", "IdFile", "dbo.ActFiles", "IdFile", cascadeDelete: true);
            AddForeignKey("dbo.ResourceOwnerPersonActs", "IdFile", "dbo.ActFiles", "IdFile", cascadeDelete: true);
            DropColumn("dbo.ResourceAuthorityActs", "FileOriginalName");
            DropColumn("dbo.ResourceAuthorityActs", "FileContent");
            DropColumn("dbo.ResourceAuthorityActs", "FileContentType");
            DropColumn("dbo.ResourceOperatorActs", "FileOriginalName");
            DropColumn("dbo.ResourceOperatorActs", "FileContent");
            DropColumn("dbo.ResourceOperatorActs", "FileContentType");
            DropColumn("dbo.ResourceUsingActs", "FileOriginalName");
            DropColumn("dbo.ResourceUsingActs", "FileContent");
            DropColumn("dbo.ResourceUsingActs", "FileContentType");
            DropColumn("dbo.ResourceOperatorPersonActs", "FileOriginalName");
            DropColumn("dbo.ResourceOperatorPersonActs", "FileContent");
            DropColumn("dbo.ResourceOperatorPersonActs", "FileContentType");
            DropColumn("dbo.ResourceOwnerPersonActs", "FileOriginalName");
            DropColumn("dbo.ResourceOwnerPersonActs", "FileContent");
            DropColumn("dbo.ResourceOwnerPersonActs", "FileContentType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ResourceOwnerPersonActs", "FileContentType", c => c.String());
            AddColumn("dbo.ResourceOwnerPersonActs", "FileContent", c => c.Binary());
            AddColumn("dbo.ResourceOwnerPersonActs", "FileOriginalName", c => c.String());
            AddColumn("dbo.ResourceOperatorPersonActs", "FileContentType", c => c.String());
            AddColumn("dbo.ResourceOperatorPersonActs", "FileContent", c => c.Binary());
            AddColumn("dbo.ResourceOperatorPersonActs", "FileOriginalName", c => c.String());
            AddColumn("dbo.ResourceUsingActs", "FileContentType", c => c.String());
            AddColumn("dbo.ResourceUsingActs", "FileContent", c => c.Binary());
            AddColumn("dbo.ResourceUsingActs", "FileOriginalName", c => c.String());
            AddColumn("dbo.ResourceOperatorActs", "FileContentType", c => c.String());
            AddColumn("dbo.ResourceOperatorActs", "FileContent", c => c.Binary());
            AddColumn("dbo.ResourceOperatorActs", "FileOriginalName", c => c.String());
            AddColumn("dbo.ResourceAuthorityActs", "FileContentType", c => c.String());
            AddColumn("dbo.ResourceAuthorityActs", "FileContent", c => c.Binary());
            AddColumn("dbo.ResourceAuthorityActs", "FileOriginalName", c => c.String());
            DropForeignKey("dbo.ResourceOwnerPersonActs", "IdFile", "dbo.ActFiles");
            DropForeignKey("dbo.ResourceOperatorPersonActs", "IdFile", "dbo.ActFiles");
            DropForeignKey("dbo.ResourceUsingActs", "IdFile", "dbo.ActFiles");
            DropForeignKey("dbo.ResourceOperatorActs", "IdFile", "dbo.ActFiles");
            DropForeignKey("dbo.ResourceAuthorityActs", "IdFile", "dbo.ActFiles");
            DropIndex("dbo.ResourceOwnerPersonActs", new[] { "IdFile" });
            DropIndex("dbo.ResourceOperatorPersonActs", new[] { "IdFile" });
            DropIndex("dbo.ResourceUsingActs", new[] { "IdFile" });
            DropIndex("dbo.ResourceOperatorActs", new[] { "IdFile" });
            DropIndex("dbo.ResourceAuthorityActs", new[] { "IdFile" });
            DropColumn("dbo.ResourceOwnerPersonActs", "Deleted");
            DropColumn("dbo.ResourceOwnerPersonActs", "IdFile");
            DropColumn("dbo.ResourceOperatorPersonActs", "Deleted");
            DropColumn("dbo.ResourceOperatorPersonActs", "IdFile");
            DropColumn("dbo.ResourceUsingActs", "Deleted");
            DropColumn("dbo.ResourceUsingActs", "IdFile");
            DropColumn("dbo.ResourceOperatorActs", "Deleted");
            DropColumn("dbo.ResourceOperatorActs", "IdFile");
            DropColumn("dbo.ResourceAuthorityActs", "Deleted");
            DropColumn("dbo.ResourceAuthorityActs", "IdFile");
            DropTable("dbo.ActFiles");
        }
    }
}
