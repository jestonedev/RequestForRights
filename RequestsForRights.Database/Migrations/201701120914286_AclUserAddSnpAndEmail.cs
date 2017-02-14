namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AclUserAddSnpAndEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AclUsers", "Snp", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.AclUsers", "Email", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AclUsers", "Email");
            DropColumn("dbo.AclUsers", "Snp");
        }
    }
}
