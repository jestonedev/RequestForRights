namespace RequestsForRights.Database.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class FederalRegistryEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResourceAuthorityActs",
                c => new
                    {
                        IdResourceAuthorityAct = c.Int(nullable: false, identity: true),
                        IdResource = c.Int(nullable: false),
                        ActType = c.String(),
                        ActName = c.String(),
                        ActDate = c.DateTime(),
                        ActNumber = c.String(),
                        FileOriginalName = c.String(),
                        FileContent = c.Binary(),
                        FileContentType = c.String(),
                    })
                .PrimaryKey(t => t.IdResourceAuthorityAct)
                .ForeignKey("dbo.Resources", t => t.IdResource, cascadeDelete: true)
                .Index(t => t.IdResource);
            
            CreateTable(
                "dbo.ResourceDeviceAddresses",
                c => new
                    {
                        IdResourceOperatorAct = c.Int(nullable: false, identity: true),
                        IdResource = c.Int(nullable: false),
                        Name = c.String(),
                        AddressIndex = c.String(maxLength: 6),
                        AddressRegion = c.String(),
                        AddressArea = c.String(),
                        AddressCity = c.String(),
                        AddressStreet = c.String(),
                        AddressHouse = c.String(maxLength: 32),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdResourceOperatorAct)
                .ForeignKey("dbo.Resources", t => t.IdResource, cascadeDelete: true)
                .Index(t => t.IdResource);
            
            CreateTable(
                "dbo.ResourceInformationTypes",
                c => new
                    {
                        IdResourceInformationType = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 512),
                    })
                .PrimaryKey(t => t.IdResourceInformationType);
            
            CreateTable(
                "dbo.ResourceInternetAddresses",
                c => new
                    {
                        IdResourceInternetAddress = c.Int(nullable: false, identity: true),
                        IdResource = c.Int(nullable: false),
                        NetName = c.String(),
                        DeviceNumber = c.String(),
                        DeviceIpAddress = c.String(maxLength: 15),
                        GateIpAddress = c.String(maxLength: 15),
                        DhcpIpAddress = c.String(maxLength: 15),
                        IsDynamicIpAddress = c.Boolean(nullable: false),
                        DomainNames = c.String(),
                        DomainIpAddress = c.String(maxLength: 15),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdResourceInternetAddress)
                .ForeignKey("dbo.Resources", t => t.IdResource, cascadeDelete: true)
                .Index(t => t.IdResource);
            
            CreateTable(
                "dbo.ResourceOperatorActs",
                c => new
                    {
                        IdResourceOperatorAct = c.Int(nullable: false, identity: true),
                        IdResource = c.Int(nullable: false),
                        ActType = c.String(),
                        ActName = c.String(),
                        ActDate = c.DateTime(),
                        ActNumber = c.String(),
                        FileOriginalName = c.String(),
                        FileContent = c.Binary(),
                        FileContentType = c.String(),
                    })
                .PrimaryKey(t => t.IdResourceOperatorAct)
                .ForeignKey("dbo.Resources", t => t.IdResource, cascadeDelete: true)
                .Index(t => t.IdResource);
            
            CreateTable(
                "dbo.ResourceOperatorPersons",
                c => new
                    {
                        IdResourceOperatorPerson = c.Int(nullable: false, identity: true),
                        Post = c.String(),
                        Surname = c.String(),
                        Name = c.String(),
                        Patronimic = c.String(),
                        IdResource = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdResourceOperatorPerson)
                .ForeignKey("dbo.Resources", t => t.IdResource, cascadeDelete: true)
                .Index(t => t.IdResource);
            
            CreateTable(
                "dbo.ResourceOwnerPersons",
                c => new
                    {
                        IdResourceOwnerPerson = c.Int(nullable: false, identity: true),
                        Post = c.String(),
                        Surname = c.String(),
                        Name = c.String(),
                        Patronimic = c.String(),
                        IdResource = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdResourceOwnerPerson)
                .ForeignKey("dbo.Resources", t => t.IdResource, cascadeDelete: true)
                .Index(t => t.IdResource);
            
            CreateTable(
                "dbo.ResourceUsingActs",
                c => new
                    {
                        IdResourceUsingAct = c.Int(nullable: false, identity: true),
                        IdResource = c.Int(nullable: false),
                        ActType = c.String(),
                        ActName = c.String(),
                        ActDate = c.DateTime(),
                        ActNumber = c.String(),
                        FileOriginalName = c.String(),
                        FileContent = c.Binary(),
                        FileContentType = c.String(),
                    })
                .PrimaryKey(t => t.IdResourceUsingAct)
                .ForeignKey("dbo.Resources", t => t.IdResource, cascadeDelete: true)
                .Index(t => t.IdResource);
            
            CreateTable(
                "dbo.ResourceOperatorPersonActs",
                c => new
                    {
                        IdResourceOperatorPersonAct = c.Int(nullable: false, identity: true),
                        IdResourceOperatorPerson = c.Int(nullable: false),
                        ActType = c.String(),
                        ActName = c.String(),
                        ActDate = c.DateTime(),
                        ActNumber = c.String(),
                        FileOriginalName = c.String(),
                        FileContent = c.Binary(),
                        FileContentType = c.String(),
                    })
                .PrimaryKey(t => t.IdResourceOperatorPersonAct)
                .ForeignKey("dbo.ResourceOperatorPersons", t => t.IdResourceOperatorPerson, cascadeDelete: true)
                .Index(t => t.IdResourceOperatorPerson);
            
            CreateTable(
                "dbo.ResourceOwnerPersonActs",
                c => new
                    {
                        IdResourceOwnerPersonAct = c.Int(nullable: false, identity: true),
                        IdResourceOwnerPerson = c.Int(nullable: false),
                        ActType = c.String(),
                        ActName = c.String(),
                        ActDate = c.DateTime(),
                        ActNumber = c.String(),
                        FileOriginalName = c.String(),
                        FileContent = c.Binary(),
                        FileContentType = c.String(),
                    })
                .PrimaryKey(t => t.IdResourceOwnerPersonAct)
                .ForeignKey("dbo.ResourceOwnerPersons", t => t.IdResourceOwnerPerson, cascadeDelete: true)
                .Index(t => t.IdResourceOwnerPerson);
            
            AddColumn("dbo.Departments", "TaxPayerNumber", c => c.String());
            AddColumn("dbo.Departments", "OfficialNameLongRu", c => c.String());
            AddColumn("dbo.Departments", "OfficialNameShortRu", c => c.String());
            AddColumn("dbo.Departments", "OfficialNameLongEn", c => c.String());
            AddColumn("dbo.Departments", "OfficialNameShortEn", c => c.String());
            AddColumn("dbo.Departments", "SelfAddressIndex", c => c.String(maxLength: 6));
            AddColumn("dbo.Departments", "SelfAddressRegion", c => c.String());
            AddColumn("dbo.Departments", "SelfAddressArea", c => c.String());
            AddColumn("dbo.Departments", "SelfAddressCity", c => c.String());
            AddColumn("dbo.Departments", "SelfAddressStreet", c => c.String());
            AddColumn("dbo.Departments", "SelfAddressHouse", c => c.String());
            AddColumn("dbo.Departments", "СontrolOrgAddressesAreEqualSelfAddress", c => c.Boolean(nullable: false));
            AddColumn("dbo.Departments", "ControlOrgAddressIndex", c => c.String(maxLength: 6));
            AddColumn("dbo.Departments", "ControlOrgAddressRegion", c => c.String());
            AddColumn("dbo.Departments", "ControlOrgAddressArea", c => c.String());
            AddColumn("dbo.Departments", "ControlOrgAddressCity", c => c.String());
            AddColumn("dbo.Departments", "ControlOrgAddressStreet", c => c.String());
            AddColumn("dbo.Departments", "ControlOrgAddressHouse", c => c.String());
            AddColumn("dbo.Resources", "IdOperatorDepartment", c => c.Int());
            AddColumn("dbo.Resources", "EmailAdministrator", c => c.String());
            AddColumn("dbo.Resources", "IdResourceInformationType", c => c.Int());
            AddColumn("dbo.Resources", "PersonalInfoDescription", c => c.String());
            AddColumn("dbo.Resources", "HasNotInternetAccess", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Resources", "IdOperatorDepartment");
            CreateIndex("dbo.Resources", "IdResourceInformationType");
            AddForeignKey("dbo.Resources", "IdOperatorDepartment", "dbo.Departments", "IdDepartment");
            AddForeignKey("dbo.Resources", "IdResourceInformationType", "dbo.ResourceInformationTypes", "IdResourceInformationType");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResourceOwnerPersonActs", "IdResourceOwnerPerson", "dbo.ResourceOwnerPersons");
            DropForeignKey("dbo.ResourceOperatorPersonActs", "IdResourceOperatorPerson", "dbo.ResourceOperatorPersons");
            DropForeignKey("dbo.ResourceUsingActs", "IdResource", "dbo.Resources");
            DropForeignKey("dbo.ResourceOwnerPersons", "IdResource", "dbo.Resources");
            DropForeignKey("dbo.ResourceOperatorPersons", "IdResource", "dbo.Resources");
            DropForeignKey("dbo.ResourceOperatorActs", "IdResource", "dbo.Resources");
            DropForeignKey("dbo.ResourceInternetAddresses", "IdResource", "dbo.Resources");
            DropForeignKey("dbo.Resources", "IdResourceInformationType", "dbo.ResourceInformationTypes");
            DropForeignKey("dbo.ResourceDeviceAddresses", "IdResource", "dbo.Resources");
            DropForeignKey("dbo.ResourceAuthorityActs", "IdResource", "dbo.Resources");
            DropForeignKey("dbo.Resources", "IdOperatorDepartment", "dbo.Departments");
            DropIndex("dbo.ResourceOwnerPersonActs", new[] { "IdResourceOwnerPerson" });
            DropIndex("dbo.ResourceOperatorPersonActs", new[] { "IdResourceOperatorPerson" });
            DropIndex("dbo.ResourceUsingActs", new[] { "IdResource" });
            DropIndex("dbo.ResourceOwnerPersons", new[] { "IdResource" });
            DropIndex("dbo.ResourceOperatorPersons", new[] { "IdResource" });
            DropIndex("dbo.ResourceOperatorActs", new[] { "IdResource" });
            DropIndex("dbo.ResourceInternetAddresses", new[] { "IdResource" });
            DropIndex("dbo.ResourceDeviceAddresses", new[] { "IdResource" });
            DropIndex("dbo.ResourceAuthorityActs", new[] { "IdResource" });
            DropIndex("dbo.Resources", new[] { "IdResourceInformationType" });
            DropIndex("dbo.Resources", new[] { "IdOperatorDepartment" });
            DropColumn("dbo.Resources", "HasNotInternetAccess");
            DropColumn("dbo.Resources", "PersonalInfoDescription");
            DropColumn("dbo.Resources", "IdResourceInformationType");
            DropColumn("dbo.Resources", "EmailAdministrator");
            DropColumn("dbo.Resources", "IdOperatorDepartment");
            DropColumn("dbo.Departments", "ControlOrgAddressHouse");
            DropColumn("dbo.Departments", "ControlOrgAddressStreet");
            DropColumn("dbo.Departments", "ControlOrgAddressCity");
            DropColumn("dbo.Departments", "ControlOrgAddressArea");
            DropColumn("dbo.Departments", "ControlOrgAddressRegion");
            DropColumn("dbo.Departments", "ControlOrgAddressIndex");
            DropColumn("dbo.Departments", "СontrolOrgAddressesAreEqualSelfAddress");
            DropColumn("dbo.Departments", "SelfAddressHouse");
            DropColumn("dbo.Departments", "SelfAddressStreet");
            DropColumn("dbo.Departments", "SelfAddressCity");
            DropColumn("dbo.Departments", "SelfAddressArea");
            DropColumn("dbo.Departments", "SelfAddressRegion");
            DropColumn("dbo.Departments", "SelfAddressIndex");
            DropColumn("dbo.Departments", "OfficialNameShortEn");
            DropColumn("dbo.Departments", "OfficialNameLongEn");
            DropColumn("dbo.Departments", "OfficialNameShortRu");
            DropColumn("dbo.Departments", "OfficialNameLongRu");
            DropColumn("dbo.Departments", "TaxPayerNumber");
            DropTable("dbo.ResourceOwnerPersonActs");
            DropTable("dbo.ResourceOperatorPersonActs");
            DropTable("dbo.ResourceUsingActs");
            DropTable("dbo.ResourceOwnerPersons");
            DropTable("dbo.ResourceOperatorPersons");
            DropTable("dbo.ResourceOperatorActs");
            DropTable("dbo.ResourceInternetAddresses");
            DropTable("dbo.ResourceInformationTypes");
            DropTable("dbo.ResourceDeviceAddresses");
            DropTable("dbo.ResourceAuthorityActs");
        }
    }
}
