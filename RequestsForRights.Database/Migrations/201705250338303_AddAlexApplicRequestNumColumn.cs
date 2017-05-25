namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAlexApplicRequestNumColumn : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.RequestExecutors");
            AddColumn("dbo.RequestExecutors", "AlexApplicRequestNum", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.RequestExecutors", new[] { "IdRequest", "Login", "AlexApplicRequestNum" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.RequestExecutors");
            DropColumn("dbo.RequestExecutors", "AlexApplicRequestNum");
            AddPrimaryKey("dbo.RequestExecutors", new[] { "IdRequest", "Login" });
        }
    }
}
