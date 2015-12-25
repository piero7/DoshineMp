namespace DoShineMP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addFlgsForIdentifyingCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IdentifyingCodes", "PhoneNumber", c => c.String());
            AddColumn("dbo.IdentifyingCodes", "IsSendSuccess", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.IdentifyingCodes", "IsUsed", c => c.Boolean(nullable: false, defaultValue: false));
        }

        public override void Down()
        {
            DropColumn("dbo.IdentifyingCodes", "IsUsed");
            DropColumn("dbo.IdentifyingCodes", "IsSendSuccess");
            DropColumn("dbo.IdentifyingCodes", "PhoneNumber");
        }
    }
}
