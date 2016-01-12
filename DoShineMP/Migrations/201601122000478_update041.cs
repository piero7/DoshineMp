namespace DoShineMP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class update041 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Partners", "Sex", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.Partners", "Money", c => c.String());
            AddColumn("dbo.Repairs", "Describe", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Repairs", "Describe");
            DropColumn("dbo.Partners", "Money");
            DropColumn("dbo.Partners", "Sex");
        }
    }
}
