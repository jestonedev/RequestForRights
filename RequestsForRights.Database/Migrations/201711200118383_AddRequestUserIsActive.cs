namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestUserIsActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestUsers", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestUsers", "IsActive");
        }
    }
}
