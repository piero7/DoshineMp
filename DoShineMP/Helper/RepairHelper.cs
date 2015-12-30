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
        /// <summary>
        /// 提交一个新的报修申请
        /// </summary>
        /// <param name="openid">用户op</param>
        /// <param name="content"></param>
        /// <returns></returns>
        public Repair AddRepair(string openid, string content, string mediaid)
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
            if (!string.IsNullOrEmpty(mediaid))
            {
                WechatImageHelper.AddNewImageForRepair(mediaid, rep.RepairId, openid);
            }

            LogHelper.AddLog("Apply a new repair", rep.RepairId.ToString(), openid);
            return rep;
        }

        /// <summary>
        /// 获得一个维修记录的详细情况
        /// </summary>
        /// <param name="id">维修记录id</param>
        /// <returns></returns>
        public Repair GetDetail(int id)
        {
            var db = new ModelContext();
            Repair rep = db.RepairSet.FirstOrDefault(item => item.RepairId == id);
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

            foreach (var item in ownhis)
            {
                item.IsUserself = true;
            }

            var rCount = tCount - ownhis.Count();
            if (rCount > 0)
            {
                var tmpList = (from r in db.RepairSet
                               select r).Take(rCount).OrderBy(item => item.RepairId).ToList();
                foreach (var item in tmpList)
                {
                    item.IsUserself = false;
                }
                ownhis.AddRange(tmpList);
            }

            return ownhis;
        }

        /// <summary>
        /// 受理报修
        /// </summary>
        /// <param name="repairId">保修记录id</param>
        /// <param name="exceptDate">预计上门时间</param>
        /// <returns></returns>
        public Repair Accept(int repairId, DateTime exceptDate, string innderNumber)
        {
            var db = new ModelContext();
            var rep = db.RepairSet.FirstOrDefault(item => item.RepairId == repairId);

            if (rep == null)
            {
                return null;
            }

            rep.Status = RepairStatus.Accept;
            rep.AccepDate = DateTime.Now;
            rep.ExceptHandleDate = DateTime.Now;
            rep.InnerNumber = innderNumber;

            db.SaveChanges();

            return rep;
        }

        /// <summary>
        /// 完成处理
        /// </summary>
        /// <param name="repairId">报修记录id</param>
        /// <returns></returns>
        public Repair FinishHandlen(int repairId)
        {
            var db = new ModelContext();
            var rep = db.RepairSet.FirstOrDefault(item => item.RepairId == repairId);

            if (rep == null)
            {
                return null;
            }

            rep.Status = RepairStatus.FinishHandle;
            rep.FinishHandlendDate = DateTime.Now;

            db.SaveChanges();
            return rep;
        }

        /// <summary>
        /// 评价报修
        /// </summary>
        /// <param name="repairId">报修id</param>
        /// <param name="response">评价内容</param>
        /// <param name="score">分数（预留，若无则填0）</param>
        /// <returns></returns>
        public Repair Response(int repairId, string response, double score)
        {
            var db = new ModelContext();
            var rep = db.RepairSet.FirstOrDefault(item => item.RepairId == repairId);
            if (rep == null)
            {
                return null;
            }

            rep.Score = score;
            rep.ResponeDate = DateTime.Now;
            rep.Response = response;

            db.SaveChanges();
            return rep;
        }
    }
}
