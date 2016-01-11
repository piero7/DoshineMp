namespace DoShineMP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddModelsfFor040 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatLogs",
                c => new
                {
                    ChatLogId = c.Int(nullable: false, identity: true),
                    StartDate = c.DateTime(),
                    EndDate = c.DateTime(),
                    HasReaded = c.Boolean(nullable: false, defaultValue: false),
                    Status = c.Int(nullable: false, defaultValue: 0),
                    UserInfo = c.String(),
                })
                .PrimaryKey(t => t.ChatLogId);

            CreateTable(
                "dbo.CustomServers",
                c => new
                {
                    CustomServerId = c.Int(nullable: false, identity: true),
                    LastLoginDate = c.DateTime(),
                    Name = c.String(),
                    Remarks = c.String(),
                    Status = c.Int(nullable: false, defaultValue: 0),
                })
                .PrimaryKey(t => t.CustomServerId);

            CreateTable(
                "dbo.UsefulChats",
                c => new
                {
                    UsefulChatId = c.Int(nullable: false, identity: true),
                    Level = c.Int(nullable: false, defaultValue: 1),
                    Contenet = c.String(),
                    IsEnable = c.Boolean(nullable: false, defaultValue: true),
                    Remarks = c.String(),
                })
                .PrimaryKey(t => t.UsefulChatId);

            AddColumn("dbo.Partners", "Email", c => c.String());
            AddColumn("dbo.Partners", "SalesmanId", c => c.Int());
            AddColumn("dbo.Partners", "Status", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.Partners", "AcceptDate", c => c.DateTime());
            AddColumn("dbo.Partners", "CancelDate", c => c.DateTime());
            CreateIndex("dbo.Partners", "SalesmanId");
            AddForeignKey("dbo.Partners", "SalesmanId", "dbo.Salesmen", "SalesmanId");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Partners", "SalesmanId", "dbo.Salesmen");
            DropIndex("dbo.Partners", new[] { "SalesmanId" });
            DropColumn("dbo.Partners", "CancelDate");
            DropColumn("dbo.Partners", "AcceptDate");
            DropColumn("dbo.Partners", "Status");
            DropColumn("dbo.Partners", "SalesmanId");
            DropColumn("dbo.Partners", "Email");
            DropTable("dbo.UsefulChats");
            DropTable("dbo.CustomServers");
            DropTable("dbo.ChatLogs");
        }
    }
}
