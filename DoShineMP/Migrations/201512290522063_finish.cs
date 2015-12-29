namespace DoShineMP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class finish : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ImageDownloadLogs", "FinishDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ImageDownloadLogs", "FinishDate", c => c.DateTime(nullable: false));
        }
    }
}
