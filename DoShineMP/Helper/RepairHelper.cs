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
            var usr = WechatHelper.CheckOpenid(openid);
            usr = WechatHelper.CheckUser(usr);
            if (usr.UserInfoId == null || usr.UserInfoId == 0 || usr.UserInfo == null)
            {
                return null;
            }


            var rep = new Repair
            {
                Contenet = content,
                CreateDate = DateTime.Now,
                Status = RepairStatus.Apply,
                UserId = usr.UserInfoId,
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
            var user = WechatHelper.CheckUser(wuser);
            //获取config中记录的需要现实的历史纪录数量。
            int tCount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["historyrepaircount"]);
            if (user == null)
            {
                return null;
            }

            var db = new ModelContext();

            var ownhis = (
                          from r in db.RepairSet
                          where r.UserId == user.UserInfoId
                          orderby r.CreateDate descending
                          select r).ToList();

            var rCount = tCount - ownhis.Count();
            if (rCount > 0)
            {
                ownhis.AddRange(
                    (
                    from r in db.RepairSet
                    orderby r.RepairId descending
                    select r).Take(rCount).ToList()
                    );
            }

            return ownhis;
        }
    }
}
