namespace Cuttler.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Backups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FilePath = c.String(),
                        Path = c.String(),
                        TenantId = c.Guid(nullable: false),
                        OctopusTenant_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OctopusTenants", t => t.OctopusTenant_Id)
                .Index(t => t.OctopusTenant_Id);
            
            CreateTable(
                "dbo.OctopusTenants",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        MainUrl = c.String(),
                        SubscriptionId = c.Guid(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OctopusTenantUsers",
                c => new
                    {
                        OctopusTenant_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.OctopusTenant_Id, t.User_Id })
                .ForeignKey("dbo.OctopusTenants", t => t.OctopusTenant_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.OctopusTenant_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Backups", "OctopusTenant_Id", "dbo.OctopusTenants");
            DropForeignKey("dbo.OctopusTenantUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.OctopusTenantUsers", "OctopusTenant_Id", "dbo.OctopusTenants");
            DropIndex("dbo.OctopusTenantUsers", new[] { "User_Id" });
            DropIndex("dbo.OctopusTenantUsers", new[] { "OctopusTenant_Id" });
            DropIndex("dbo.Backups", new[] { "OctopusTenant_Id" });
            DropTable("dbo.OctopusTenantUsers");
            DropTable("dbo.OctopusTenants");
            DropTable("dbo.Backups");
        }
    }
}
