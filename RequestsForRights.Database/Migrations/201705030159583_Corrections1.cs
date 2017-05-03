namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Corrections1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.RequestUsers", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RequestUsers", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
