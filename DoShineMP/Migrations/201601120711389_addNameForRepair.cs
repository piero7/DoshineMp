namespace DoShineMP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNameForRepair : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Repairs", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Repairs", "Name");
        }
    }
}
