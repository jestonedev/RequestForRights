namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CurrentRequestDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Requests", "CurrentRequestStateDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Requests", "CurrentRequestStateDate", c => c.DateTime(nullable: false));
        }
    }
}
