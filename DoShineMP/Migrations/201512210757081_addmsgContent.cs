namespace DoShineMP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmsgContent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ActiveLogs", "TerminalId", "dbo.Terminals");
            DropIndex("dbo.ActiveLogs", new[] { "TerminalId" });
            AlterColumn("dbo.ActiveLogs", "TerminalId", c => c.Int());
            CreateIndex("dbo.ActiveLogs", "TerminalId");
            AddForeignKey("dbo.ActiveLogs", "TerminalId", "dbo.Terminals", "TerminalId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActiveLogs", "TerminalId", "dbo.Terminals");
            DropIndex("dbo.ActiveLogs", new[] { "TerminalId" });
            AlterColumn("dbo.ActiveLogs", "TerminalId", c => c.Int(nullable: false));
            CreateIndex("dbo.ActiveLogs", "TerminalId");
            AddForeignKey("dbo.ActiveLogs", "TerminalId", "dbo.Terminals", "TerminalId", cascadeDelete: true);
        }
    }
}
