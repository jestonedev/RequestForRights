namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexOnRequestExecutor : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.RequestExecutors", "Login", name: "IX_RequestExecutor_Login");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RequestExecutors", "IX_RequestExecutor_Login");
        }
    }
}
