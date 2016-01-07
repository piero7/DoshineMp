namespace DoShineMP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addRecord : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Districts",
                c => new
                {
                    DistrictId = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    LocationX = c.Double(),
                    LocationY = c.Double(),
                    Remarks = c.String(),
                })
                .PrimaryKey(t => t.DistrictId);

            CreateTable(
                "dbo.Records",
                c => new
                {
                    RecordId = c.Int(nullable: false, identity: true),
                    CreateDate = c.DateTime(nullable: false),
                    Address = c.String(),
                    PhoneNumber = c.String(),
                    Name = c.String(),
                    Remarks = c.String(),
                    Type = c.Int(nullable: false, defaultValue: 0),
                    UserInfoId = c.Int(),
                })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfoId)
                .Index(t => t.UserInfoId);

            CreateTable(
                "dbo.Salesmen",
                c => new
                {
                    SalesmanId = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    PhoneNumber = c.String(),
                    InnerNumber = c.String(),
                    Remarks = c.String(),
                    NickName = c.String(),
                })
                .PrimaryKey(t => t.SalesmanId);

            AddColumn("dbo.Repairs", "PhoneNumber", c => c.String());
            AddColumn("dbo.Repairs", "Address", c => c.String());
        }

        public override void Down()
        {
            DropForeignKey("dbo.Records", "UserInfoId", "dbo.UserInfoes");
            DropIndex("dbo.Records", new[] { "UserInfoId" });
            DropColumn("dbo.Repairs", "Address");
            DropColumn("dbo.Repairs", "PhoneNumber");
            DropTable("dbo.Salesmen");
            DropTable("dbo.Records");
            DropTable("dbo.Districts");
        }
    }
}
