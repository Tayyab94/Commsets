namespace BusinessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserClassAndRole : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 50, unicode: false),
                        LoginId = c.String(nullable: false, maxLength: 50, unicode: false),
                        Password = c.String(nullable: false, maxLength: 30, unicode: false),
                        SecurityQuestion = c.String(maxLength: 250, unicode: false),
                        SecurityAns = c.String(maxLength: 50, unicode: false),
                        ContactNo = c.String(nullable: false, maxLength: 25, unicode: false),
                        Address = c.String(nullable: false, maxLength: 250, unicode: false),
                        EmailID = c.String(maxLength: 250, unicode: false),
                        ImgURL = c.String(maxLength: 250, unicode: false),
                        DOB = c.DateTime(),
                        IsActive = c.Boolean(),
                        City_ID = c.Int(),
                        Role_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Cities", t => t.City_ID)
                .ForeignKey("dbo.Roles", t => t.Role_ID, cascadeDelete: true)
                .Index(t => t.City_ID)
                .Index(t => t.Role_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Role_ID", "dbo.Roles");
            DropForeignKey("dbo.Users", "City_ID", "dbo.Cities");
            DropIndex("dbo.Users", new[] { "Role_ID" });
            DropIndex("dbo.Users", new[] { "City_ID" });
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
        }
    }
}
