namespace DoShineMP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addOverusrReapair : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OveruseRepairs",
                c => new
                {
                    OverusRepairId = c.Int(nullable: false, identity: true),
                    Level = c.Int(nullable: false, defaultValue: 1),
                    FatherItem = c.Int(),
                    Content = c.String(),
                    Remarks = c.String(),
                })
                .PrimaryKey(t => t.OverusRepairId);

        }

        public override void Down()
        {
            DropTable("dbo.OveruseRepairs");
        }
    }
}
