namespace Cuttler.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Mail = c.String(),
                        Verified = c.Boolean(nullable: false),
                        UserId = c.Guid(nullable: false),
                        VerifyCode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserName = c.String(),
                        Street = c.String(),
                        Zip = c.String(),
                        Location = c.String(),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Logins",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PasswordHash = c.String(),
                        Enabled = c.Boolean(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Logins", "UserId", "dbo.Users");
            DropForeignKey("dbo.Emails", "UserId", "dbo.Users");
            DropIndex("dbo.Logins", new[] { "UserId" });
            DropIndex("dbo.Emails", new[] { "UserId" });
            DropTable("dbo.Logins");
            DropTable("dbo.Users");
            DropTable("dbo.Emails");
        }
    }
}
