namespace DoShineMP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addWechatImage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImageDownloadLogs",
                c => new
                    {
                        ImageDownLoadLogId = c.Int(nullable: false, identity: true),
                        CreateDate = c.DateTime(nullable: false),
                        MediaNumber = c.String(),
                        IsSuccess = c.Boolean(nullable: false,defaultValue:false),
                        FinishDate = c.DateTime(nullable: true),
                        OpenId = c.String(),
                        Scene = c.String(),
                        FileId = c.Int(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.ImageDownLoadLogId)
                .ForeignKey("dbo.ImageFiles", t => t.FileId)
                .Index(t => t.FileId);
            
            CreateTable(
                "dbo.ImageFiles",
                c => new
                    {
                        ImageFileId = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.ImageFileId);
            
            AddColumn("dbo.UserInfoes", "Address", c => c.String());
            AddColumn("dbo.Messages", "DetailContent", c => c.String());
            AddColumn("dbo.Repairs", "ImageFileId", c => c.Int());
            AddColumn("dbo.Repairs", "AccepDate", c => c.DateTime());
            AddColumn("dbo.Repairs", "InnerNumber", c => c.String());
            AddColumn("dbo.Repairs", "FinishHandlendDate", c => c.DateTime());
            AddColumn("dbo.Repairs", "Response", c => c.String());
            AddColumn("dbo.Repairs", "ResponeDate", c => c.DateTime());
            AddColumn("dbo.Repairs", "Score", c => c.Double(nullable: false));
            AddColumn("dbo.Repairs", "ExceptHandleDate", c => c.DateTime());
            CreateIndex("dbo.Repairs", "ImageFileId");
            AddForeignKey("dbo.Repairs", "ImageFileId", "dbo.ImageFiles", "ImageFileId");
            DropTable("dbo.Files");
        }
        
        public override void Down()
        {
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
            
            DropForeignKey("dbo.Repairs", "ImageFileId", "dbo.ImageFiles");
            DropForeignKey("dbo.ImageDownloadLogs", "FileId", "dbo.ImageFiles");
            DropIndex("dbo.Repairs", new[] { "ImageFileId" });
            DropIndex("dbo.ImageDownloadLogs", new[] { "FileId" });
            DropColumn("dbo.Repairs", "ExceptHandleDate");
            DropColumn("dbo.Repairs", "Score");
            DropColumn("dbo.Repairs", "ResponeDate");
            DropColumn("dbo.Repairs", "Response");
            DropColumn("dbo.Repairs", "FinishHandlendDate");
            DropColumn("dbo.Repairs", "InnerNumber");
            DropColumn("dbo.Repairs", "AccepDate");
            DropColumn("dbo.Repairs", "ImageFileId");
            DropColumn("dbo.Messages", "DetailContent");
            DropColumn("dbo.UserInfoes", "Address");
            DropTable("dbo.ImageFiles");
            DropTable("dbo.ImageDownloadLogs");
        }
    }
}
