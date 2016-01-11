namespace DoShineMP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOpenidForChatLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatLogs", "Openid", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChatLogs", "Openid");
        }
    }
}
