namespace DoShineMP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Villages",
                c => new
                {
                    VillageId = c.Int(nullable: false, identity: true),
                    Address = c.String(),
                    Name = c.String(),
                    LocationX = c.Double(nullable: false, defaultValue: 0.0),
                    LocationY = c.Double(nullable: false, defaultValue: 0.0),
                    ImagePath = c.String(),
                    IsEnable = c.Boolean(nullable: false, defaultValue: true),
                    Remarks = c.String(),
                })
                .PrimaryKey(t => t.VillageId);

            AddColumn("dbo.Partners", "DistrictId", c => c.Int());
            AddColumn("dbo.Records", "Openid", c => c.String());
            AddColumn("dbo.Repairs", "VillageId", c => c.Int());
            AddColumn("dbo.Repairs", "ImageFilesStr", c => c.String());
            AddColumn("dbo.Repairs", "FinishImageFilesStr", c => c.String());
            AddColumn("dbo.Repairs", "FinishType", c => c.Int(nullable: false, defaultValue: 0));
            CreateIndex("dbo.Partners", "DistrictId");
            CreateIndex("dbo.Repairs", "VillageId");
            AddForeignKey("dbo.Partners", "DistrictId", "dbo.Districts", "DistrictId");
            AddForeignKey("dbo.Repairs", "VillageId", "dbo.Villages", "VillageId");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Repairs", "VillageId", "dbo.Villages");
            DropForeignKey("dbo.Partners", "DistrictId", "dbo.Districts");
            DropIndex("dbo.Repairs", new[] { "VillageId" });
            DropIndex("dbo.Partners", new[] { "DistrictId" });
            DropColumn("dbo.Repairs", "FinishType");
            DropColumn("dbo.Repairs", "FinishImageFilesStr");
            DropColumn("dbo.Repairs", "ImageFilesStr");
            DropColumn("dbo.Repairs", "VillageId");
            DropColumn("dbo.Records", "Openid");
            DropColumn("dbo.Partners", "DistrictId");
            DropTable("dbo.Villages");
        }
    }
}
