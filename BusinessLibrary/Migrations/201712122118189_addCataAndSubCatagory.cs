namespace BusinessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCataAndSubCatagory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Catagories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SubCatagories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        Catagory_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Catagories", t => t.Catagory_ID, cascadeDelete: true)
                .Index(t => t.Catagory_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubCatagories", "Catagory_ID", "dbo.Catagories");
            DropIndex("dbo.SubCatagories", new[] { "Catagory_ID" });
            DropTable("dbo.SubCatagories");
            DropTable("dbo.Catagories");
        }
    }
}
