namespace BusinessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIMgId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AdvertisementImages", "ImgID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdvertisementImages", "ImgID", c => c.Int(nullable: false));
        }
    }
}
