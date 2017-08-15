namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCurrentRequestStateOptimization : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "CurrentRequestStateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Requests", "IdCurrentRequestStateType", c => c.Int());
            CreateIndex("dbo.Requests", "IdCurrentRequestStateType");
            AddForeignKey("dbo.Requests", "IdCurrentRequestStateType", "dbo.RequestStateTypes", "IdRequestStateType");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "IdCurrentRequestStateType", "dbo.RequestStateTypes");
            DropIndex("dbo.Requests", new[] { "IdCurrentRequestStateType" });
            DropColumn("dbo.Requests", "IdCurrentRequestStateType");
            DropColumn("dbo.Requests", "CurrentRequestStateDate");
        }
    }
}
