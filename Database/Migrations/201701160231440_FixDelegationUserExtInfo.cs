namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class FixDelegationUserExtInfo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DelegationRequestUserRightsExtInfo", "IdDelegateToUser", "dbo.RequestUsers");
            DropForeignKey("dbo.DelegationRequestUserRightsExtInfo", "IdAssoc", "dbo.RequestUserRightAssocs");
            DropIndex("dbo.DelegationRequestUserRightsExtInfo", new[] { "IdAssoc" });
            DropIndex("dbo.DelegationRequestUserRightsExtInfo", new[] { "IdDelegateToUser" });
            CreateTable(
                "dbo.DelegationRequestUsersExtInfo",
                c => new
                    {
                        IdRequestUserAssoc = c.Int(false),
                        IdDelegateToUser = c.Int(false),
                        DelegateFromDate = c.DateTime(false),
                        DelegateToDate = c.DateTime(false),
                        Deleted = c.Boolean(false),
                    })
                .PrimaryKey(t => t.IdRequestUserAssoc)
                .ForeignKey("dbo.RequestUsers", t => t.IdDelegateToUser, true)
                .ForeignKey("dbo.RequestUserAssocs", t => t.IdRequestUserAssoc)
                .Index(t => t.IdRequestUserAssoc)
                .Index(t => t.IdDelegateToUser);
            
            AddColumn("dbo.RequestUserAssocs", "Description", c => c.String());
            DropTable("dbo.DelegationRequestUserRightsExtInfo");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DelegationRequestUserRightsExtInfo",
                c => new
                    {
                        IdAssoc = c.Int(false),
                        IdDelegateToUser = c.Int(false),
                        DelegateFromDate = c.DateTime(false),
                        DelegateToDate = c.DateTime(false),
                        Deleted = c.Boolean(false),
                    })
                .PrimaryKey(t => t.IdAssoc);
            
            DropForeignKey("dbo.DelegationRequestUsersExtInfo", "IdRequestUserAssoc", "dbo.RequestUserAssocs");
            DropForeignKey("dbo.DelegationRequestUsersExtInfo", "IdDelegateToUser", "dbo.RequestUsers");
            DropIndex("dbo.DelegationRequestUsersExtInfo", new[] { "IdDelegateToUser" });
            DropIndex("dbo.DelegationRequestUsersExtInfo", new[] { "IdRequestUserAssoc" });
            DropColumn("dbo.RequestUserAssocs", "Description");
            DropTable("dbo.DelegationRequestUsersExtInfo");
            CreateIndex("dbo.DelegationRequestUserRightsExtInfo", "IdDelegateToUser");
            CreateIndex("dbo.DelegationRequestUserRightsExtInfo", "IdAssoc");
            AddForeignKey("dbo.DelegationRequestUserRightsExtInfo", "IdAssoc", "dbo.RequestUserRightAssocs", "IdAssoc");
            AddForeignKey("dbo.DelegationRequestUserRightsExtInfo", "IdDelegateToUser", "dbo.RequestUsers", "IdRequestUser", true);
        }
    }
}
