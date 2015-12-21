using DoShineMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoShineMP.Helper
{
    public class RepairHelper
    {
        public Repair AddRepair(string openid, string content)
        {
            var db = new ModelContext();
            var wuser = db.WechatUserSet.Include("UserInfo").FirstOrDefault(item => item.OpenId == openid);
            if (wuser == null)
            {
                return null;
            }
            if (wuser.UserInfo == null)
            {
                return null;
            }

            var rep = new Repair
            {
                Contenet = content,
                CreateDate = DateTime.Now,
                Status = RepairStatus.Apply,
                UserId = wuser.UserInfoId,
            };

            db.RepairSet.Add(rep);
            db.SaveChanges();
            LogHelper.AddLog("Apply a new repair", rep.RepairId.ToString(), openid);
            return rep;
        }

        /// <summary>
        /// 获得历史报修记录，若有则显示，若没有则显示默认
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public IEnumerable<Repair> GetHistoryRepair(string openid)
        {
            var wuser = WechatHelper.CheckOpenid(openid);
            return null;

        }
    }
}
