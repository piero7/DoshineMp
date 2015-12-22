namespace DoShineMP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20151222 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DebugInfoes", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.DebugInfoes", "Remarks", c => c.String());
            DropColumn("dbo.DebugInfoes", "Create");
            DropColumn("dbo.DebugInfoes", "Remarls");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DebugInfoes", "Remarls", c => c.String());
            AddColumn("dbo.DebugInfoes", "Create", c => c.DateTime(nullable: false));
            DropColumn("dbo.DebugInfoes", "Remarks");
            DropColumn("dbo.DebugInfoes", "CreateDate");
        }
    }
}
