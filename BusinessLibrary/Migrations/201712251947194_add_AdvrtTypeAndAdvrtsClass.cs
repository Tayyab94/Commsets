namespace BusinessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_AdvrtTypeAndAdvrtsClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Advertisements",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 300, unicode: false),
                        Description = c.String(maxLength: 8000, unicode: false),
                        price = c.Single(nullable: false),
                        PostedOn = c.DateTime(nullable: false),
                        VilidUpTo = c.DateTime(nullable: false),
                        City_ID = c.Int(nullable: false),
                        Status_ID = c.Int(nullable: false),
                        SubCatagory_ID = c.Int(nullable: false),
                        Type_ID = c.Int(nullable: false),
                        User_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Cities", t => t.City_ID, cascadeDelete: true)
                .ForeignKey("dbo.AdvertisementStatus", t => t.Status_ID, cascadeDelete: true)
                .ForeignKey("dbo.SubCatagories", t => t.SubCatagory_ID, cascadeDelete: true)
                .ForeignKey("dbo.AdvertisementTypes", t => t.Type_ID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_ID, cascadeDelete: true)
                .Index(t => t.City_ID)
                .Index(t => t.Status_ID)
                .Index(t => t.SubCatagory_ID)
                .Index(t => t.Type_ID)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.AdvertisementImages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false, maxLength: 300, unicode: false),
                        Caption = c.String(maxLength: 100, unicode: false),
                        Priority = c.Int(nullable: false),
                        Advertisement_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Advertisements", t => t.Advertisement_ID)
                .Index(t => t.Advertisement_ID);
            
            CreateTable(
                "dbo.AdvertisementStatus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AdvertisementTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Advertisements", "User_ID", "dbo.Users");
            DropForeignKey("dbo.Advertisements", "Type_ID", "dbo.AdvertisementTypes");
            DropForeignKey("dbo.Advertisements", "SubCatagory_ID", "dbo.SubCatagories");
            DropForeignKey("dbo.Advertisements", "Status_ID", "dbo.AdvertisementStatus");
            DropForeignKey("dbo.AdvertisementImages", "Advertisement_ID", "dbo.Advertisements");
            DropForeignKey("dbo.Advertisements", "City_ID", "dbo.Cities");
            DropIndex("dbo.AdvertisementImages", new[] { "Advertisement_ID" });
            DropIndex("dbo.Advertisements", new[] { "User_ID" });
            DropIndex("dbo.Advertisements", new[] { "Type_ID" });
            DropIndex("dbo.Advertisements", new[] { "SubCatagory_ID" });
            DropIndex("dbo.Advertisements", new[] { "Status_ID" });
            DropIndex("dbo.Advertisements", new[] { "City_ID" });
            DropTable("dbo.AdvertisementTypes");
            DropTable("dbo.AdvertisementStatus");
            DropTable("dbo.AdvertisementImages");
            DropTable("dbo.Advertisements");
        }
    }
}
