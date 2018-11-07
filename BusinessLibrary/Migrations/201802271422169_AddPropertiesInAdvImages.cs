namespace BusinessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertiesInAdvImages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdvertisementImages", "ImgID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdvertisementImages", "ImgID");
        }
    }
}
