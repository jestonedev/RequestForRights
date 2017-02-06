namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InnControlSubjectAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resources", "InnControlSubject", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Resources", "InnControlSubject");
        }
    }
}
