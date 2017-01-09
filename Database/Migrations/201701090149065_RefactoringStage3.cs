namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefactoringStage3 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Users", newName: "RequestUsers");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.RequestUsers", newName: "Users");
        }
    }
}
