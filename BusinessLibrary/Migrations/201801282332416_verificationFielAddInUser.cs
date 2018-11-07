namespace BusinessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class verificationFielAddInUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "VerificationCode", c => c.String(maxLength: 30, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "VerificationCode");
        }
    }
}
