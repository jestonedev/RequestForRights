namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAclUserDateCreatedField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AclUsers", "DateCreated", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AclUsers", "DateCreated");
        }
    }
}
