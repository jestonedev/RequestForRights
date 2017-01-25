namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UniqueAclUser : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AclUsers", "Login", unique: true, name: "IDX_AclUser");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AclUsers", "IDX_AclUser");
        }
    }
}
