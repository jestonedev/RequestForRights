namespace RequestsForRights.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectResourceDeviceAddressPrimaryKey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ResourceDeviceAddresses");
            DropColumn("dbo.ResourceDeviceAddresses", "IdResourceOperatorAct");
            AddColumn("dbo.ResourceDeviceAddresses", "IdResourceDeviceAddress", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ResourceDeviceAddresses", "IdResourceDeviceAddress");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ResourceDeviceAddresses");
            DropColumn("dbo.ResourceDeviceAddresses", "IdResourceDeviceAddress");
            AddColumn("dbo.ResourceDeviceAddresses", "IdResourceOperatorAct", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ResourceDeviceAddresses", "IdResourceOperatorAct");
        }
    }
}
