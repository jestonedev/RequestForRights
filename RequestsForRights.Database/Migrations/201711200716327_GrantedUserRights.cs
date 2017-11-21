namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GrantedUserRights : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestUserRightAssocs", "GrantedFrom", c => c.DateTime());
            AddColumn("dbo.RequestUserRightAssocs", "GrantedTo", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestUserRightAssocs", "GrantedTo");
            DropColumn("dbo.RequestUserRightAssocs", "GrantedFrom");
        }
    }
}
