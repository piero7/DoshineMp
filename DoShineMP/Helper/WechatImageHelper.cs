using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoShineMP.Models;

namespace DoShineMP.Helper
{
    class WechatImageHelper
    {
        /// <summary>
        /// 下载报修时候添加的图片，请先保证数据库中已经该报修记录
        /// </summary>
        /// <param name="mediaid">media _id 由微信分发</param>
        /// <param name="repairid">报修记录id</param>
        /// <param name="openid">用户openid</param>
        /// <returns></returns>
        async public static Task<ImageDownloadLog> AddNewImageForRepair(string mediaid, int repairid, string openid)
        {
            var db = new ModelContext();
            var rep = db.RepairSet.FirstOrDefault(item => item.RepairId == repairid);
            if (rep == null)
            {
                return null;
            }

            //添加下载记录
            var log = new ImageDownloadLog
            {
                CreateDate = DateTime.Now,
                IsSuccess = false,
                OpenId = openid,
                Scene = "Add repair ",
                MediaNumber = mediaid,
                Remarks = repairid.ToString(),
            };
            db.ImageDownloadLogSet.Add(log);
            db.SaveChanges();


            //下载
            var fileName = await Task.Run<string>(() => { return WechatHelper.DownloadImgFile(mediaid); });

            var file = new ImageFile
            {
                CreateDate = DateTime.Now,
                FileName = fileName,
            };
            db.ImageFileSet.Add(file);
            log.IsSuccess = true;
            log.Remarks = "";
            log.FinishDate = DateTime.Now;
            db.SaveChanges();

            //将下载的文件关联到报修记录中
            rep.ImageFileId = file.ImageFileId;
            log.FileId = file.ImageFileId;

            db.SaveChanges();

            return log;
        }
    }
}
