namespace DoShineMP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIdentifyingCode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IdentifyingCodes",
                c => new
                    {
                        IdentifyingCodeId = c.Int(nullable: false, identity: true),
                        CreateDate = c.DateTime(nullable: false),
                        OpenId = c.String(),
                        Content = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.IdentifyingCodeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.IdentifyingCodes");
        }
    }
}
