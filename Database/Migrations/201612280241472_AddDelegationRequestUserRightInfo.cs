namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddDelegationRequestUserRightInfo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RequestUserRightAssocs", "IdRequestRightGrantType", "dbo.RequestRightGrantTypes");
            CreateTable(
                "dbo.DelegationRequestUserRightExtInfoes",
                c => new
                    {
                        IdAssoc = c.Int(nullable: false),
                        IdDelegateToUser = c.Int(nullable: false),
                        DelegateFromDate = c.DateTime(nullable: false),
                        DelegateToDate = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdAssoc)
                .ForeignKey("dbo.RequestUsers", t => t.IdDelegateToUser, cascadeDelete: true)
                .ForeignKey("dbo.RequestUserRightAssocs", t => t.IdAssoc)
                .Index(t => t.IdAssoc)
                .Index(t => t.IdDelegateToUser);
            
            AddColumn("dbo.RequestUserRightAssocs", "Descirption", c => c.String());
            AddForeignKey("dbo.RequestUserRightAssocs", "IdRequestRightGrantType", "dbo.RequestRightGrantTypes", "IdRequestRightGrantType");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestUserRightAssocs", "IdRequestRightGrantType", "dbo.RequestRightGrantTypes");
            DropForeignKey("dbo.DelegationRequestUserRightExtInfoes", "IdAssoc", "dbo.RequestUserRightAssocs");
            DropForeignKey("dbo.DelegationRequestUserRightExtInfoes", "IdDelegateToUser", "dbo.RequestUsers");
            DropIndex("dbo.DelegationRequestUserRightExtInfoes", new[] { "IdDelegateToUser" });
            DropIndex("dbo.DelegationRequestUserRightExtInfoes", new[] { "IdAssoc" });
            DropColumn("dbo.RequestUserRightAssocs", "Descirption");
            DropTable("dbo.DelegationRequestUserRightExtInfoes");
            AddForeignKey("dbo.RequestUserRightAssocs", "IdRequestRightGrantType", "dbo.RequestRightGrantTypes", "IdRequestRightGrantType", cascadeDelete: true);
        }
    }
}
