namespace DoShineMP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addDebugInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DebugInfoes",
                c => new
                {
                    InfoId = c.Int(nullable: false, identity: true),
                    Info = c.String(),
                    Create = c.DateTime(nullable: true),
                    Remarls = c.String(),
                })
                .PrimaryKey(t => t.InfoId);

            AddColumn("dbo.Messages", "IpStr", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Messages", "IpStr");
            DropTable("dbo.DebugInfoes");
        }
    }
}
