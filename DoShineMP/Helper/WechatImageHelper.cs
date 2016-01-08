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
        public static ImageDownloadLog AddNewImageForRepair(string mediaid, int repairid, string openid)
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
            var fileName = WechatHelper.DownloadImgFile(mediaid);

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

        /// <summary>
        /// 为一个维修记录添加多个图片（未完成！）
        /// </summary>
        /// <param name="mediaid"></param>
        /// <param name="repairid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static IEnumerable<ImageDownloadLog> AddNewImageForRepair(IEnumerable<string> mediaid, int repairid, string openid)
        {
            //TODO 内伊做特
            throw new NotImplementedException();
        }


        /// <summary>
        /// 为经销商添加文件
        /// </summary>
        /// <param name="mediaidDic">key为文件名，value为mediaId</param>
        /// <param name="partnerid">合作伙伴id</param>
        /// <param name="openid">openid</param>
        /// <returns></returns>
        public static IEnumerable<ImageDownloadLog> AddNewImageForPartner(Dictionary<string, string> mediaidDic, int partnerid, string openid)
        {
            var db = new ModelContext();
            var pat = db.PartnerSet.FirstOrDefault(item => item.PartnerId == partnerid);
            if (pat == null)
            {
                yield break;
            }
            var ret = new List<ImageDownloadLog>();

            foreach (var media in mediaidDic)
            {
                //添加下载记录
                var log = new ImageDownloadLog
                {
                    CreateDate = DateTime.Now,
                    IsSuccess = false,
                    OpenId = openid,
                    Scene = "Add partener image",
                    MediaNumber = media.Value,
                    Remarks = partnerid.ToString(),
                };
                db.ImageDownloadLogSet.Add(log);
                db.SaveChanges();


                //下载
                var fileName = WechatHelper.DownloadImgFile(media.Value);

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
                if (pat.Files == null)
                {
                    pat.Files = new Dictionary<int, string>();
                }
                pat.Files.Add(file.ImageFileId, media.Key);
                log.FileId = file.ImageFileId;

                db.SaveChanges();

                yield return log;
            }

        }
    }
}
