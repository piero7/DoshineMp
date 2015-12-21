namespace DoShineMP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActiveLogs",
                c => new
                {
                    ActiveLogId = c.Int(nullable: false, identity: true),
                    CreateDate = c.DateTime(nullable: false),
                    UserId = c.Int(),
                    TerminalId = c.Int(nullable: true),
                    OptionContent = c.String(),
                    Remarks = c.String(),
                })
                .PrimaryKey(t => t.ActiveLogId)
                .ForeignKey("dbo.Terminals", t => t.TerminalId, cascadeDelete: true)
                .ForeignKey("dbo.UserInfoes", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.TerminalId);

            CreateTable(
                "dbo.Terminals",
                c => new
                {
                    TerminalId = c.Int(nullable: false, identity: true),
                    InnnerNumber = c.String(),
                    Name = c.String(),
                    CreateDate = c.DateTime(nullable: false),
                    Describe = c.String(),
                    Remarks = c.String(),
                })
                .PrimaryKey(t => t.TerminalId);

            CreateTable(
                "dbo.UserInfoes",
                c => new
                {
                    UserInfoId = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    PhoneNumber = c.String(),
                    CreateDate = c.DateTime(nullable: false),
                    Remarks = c.String(),
                    LastLoginTerminalId = c.Int(),
                    LastLoginTime = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.UserInfoId)
                .ForeignKey("dbo.Terminals", t => t.LastLoginTerminalId)
                .Index(t => t.LastLoginTerminalId);

            CreateTable(
                "dbo.Administrators",
                c => new
                {
                    AdminId = c.Int(nullable: false, identity: true),
                    RealName = c.String(),
                    LoginName = c.String(),
                    Password = c.String(),
                    LastLoginDate = c.DateTime(nullable: false),
                    IsEnable = c.Boolean(nullable: false,defaultValue: true),
                    Remarks = c.String(),
                })
                .PrimaryKey(t => t.AdminId);

            CreateTable(
                "dbo.Files",
                c => new
                {
                    FileId = c.Int(nullable: false, identity: true),
                    FileName = c.String(),
                    CreateDate = c.DateTime(nullable: false),
                    Remarks = c.String(),
                })
                .PrimaryKey(t => t.FileId);

            CreateTable(
                "dbo.Messages",
                c => new
                {
                    MessageId = c.Int(nullable: false, identity: true),
                    CreateDate = c.DateTime(nullable: false),
                    Content = c.String(),
                    UserId = c.Int(),
                    Type = c.Int(nullable: false),
                    TerminalId = c.Int(),
                    IsRescive = c.Boolean(nullable: false, defaultValue: false),
                    IsReply = c.String(),
                    ReplyMessageId = c.Int(),
                })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Terminals", t => t.TerminalId)
                .ForeignKey("dbo.UserInfoes", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.TerminalId);

            CreateTable(
                "dbo.Partners",
                c => new
                {
                    PartnerId = c.Int(nullable: false, identity: true),
                    CompanyName = c.String(),
                    CreateDate = c.DateTime(nullable: false),
                    PhoneNumber = c.String(),
                    Address = c.String(),
                    RealName = c.String(),
                    CompanyPhone = c.String(),
                    UserId = c.Int(),
                    Point = c.Int(nullable: false,defaultValue:0),
                    Type = c.Int(nullable: false, defaultValue: 0),
                })
                .PrimaryKey(t => t.PartnerId)
                .ForeignKey("dbo.UserInfoes", t => t.UserId)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.Repairs",
                c => new
                {
                    RepairId = c.Int(nullable: false, identity: true),
                    CreateDate = c.DateTime(nullable: false),
                    UserId = c.Int(),
                    Contenet = c.String(),
                    Status = c.Int(nullable: false, defaultValue: 0),
                    Remarks = c.String(),
                })
                .PrimaryKey(t => t.RepairId)
                .ForeignKey("dbo.UserInfoes", t => t.UserId)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.Tokens",
                c => new
                {
                    TokenId = c.Int(nullable: false, identity: true),
                    GetTime = c.DateTime(),
                    TokenStr = c.String(),
                    Type = c.Int(nullable: false, defaultValue: 0),
                })
                .PrimaryKey(t => t.TokenId);

            CreateTable(
                "dbo.WechatUsers",
                c => new
                {
                    WechatUserId = c.Int(nullable: false, identity: true),
                    OpenId = c.String(),
                    NickName = c.String(),
                    subscribe = c.Boolean(nullable: false, defaultValue: false),
                    Sex = c.Int(nullable: false, defaultValue: 0),
                    City = c.String(),
                    Country = c.String(),
                    Province = c.String(),
                    Language = c.String(),
                    SubscribeTime = c.DateTime(),
                    Headimgurl = c.String(),
                    Remarks = c.String(),
                    UserInfoId = c.Int(),
                    PartnerId = c.Int(),
                })
                .PrimaryKey(t => t.WechatUserId)
                .ForeignKey("dbo.Partners", t => t.PartnerId)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfoId)
                .Index(t => t.UserInfoId)
                .Index(t => t.PartnerId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.WechatUsers", "UserInfoId", "dbo.UserInfoes");
            DropForeignKey("dbo.WechatUsers", "PartnerId", "dbo.Partners");
            DropForeignKey("dbo.Repairs", "UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.Partners", "UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.Messages", "UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.Messages", "TerminalId", "dbo.Terminals");
            DropForeignKey("dbo.ActiveLogs", "UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.UserInfoes", "LastLoginTerminalId", "dbo.Terminals");
            DropForeignKey("dbo.ActiveLogs", "TerminalId", "dbo.Terminals");
            DropIndex("dbo.WechatUsers", new[] { "PartnerId" });
            DropIndex("dbo.WechatUsers", new[] { "UserInfoId" });
            DropIndex("dbo.Repairs", new[] { "UserId" });
            DropIndex("dbo.Partners", new[] { "UserId" });
            DropIndex("dbo.Messages", new[] { "TerminalId" });
            DropIndex("dbo.Messages", new[] { "UserId" });
            DropIndex("dbo.UserInfoes", new[] { "LastLoginTerminalId" });
            DropIndex("dbo.ActiveLogs", new[] { "TerminalId" });
            DropIndex("dbo.ActiveLogs", new[] { "UserId" });
            DropTable("dbo.WechatUsers");
            DropTable("dbo.Tokens");
            DropTable("dbo.Repairs");
            DropTable("dbo.Partners");
            DropTable("dbo.Messages");
            DropTable("dbo.Files");
            DropTable("dbo.Administrators");
            DropTable("dbo.UserInfoes");
            DropTable("dbo.Terminals");
            DropTable("dbo.ActiveLogs");
        }
    }
}
