namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IX_IdResourceRight_IdRequestRightGrantType_Deleted : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RequestUserRightAssocs", new[] { "IdResourceRight" });
            DropIndex("dbo.RequestUserRightAssocs", new[] { "IdRequestRightGrantType" });
            CreateIndex("dbo.RequestUserRightAssocs", new[] { "IdResourceRight", "IdRequestRightGrantType", "Deleted" });
        }
        
        public override void Down()
        {
            DropIndex("dbo.RequestUserRightAssocs", new[] { "IdResourceRight", "IdRequestRightGrantType", "Deleted" });
            CreateIndex("dbo.RequestUserRightAssocs", "IdRequestRightGrantType");
            CreateIndex("dbo.RequestUserRightAssocs", "IdResourceRight");
        }
    }
}
