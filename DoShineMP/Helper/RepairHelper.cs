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
        /// <param name="openid">用户openid</param>
        /// <param name="content"></param>
        /// <param name="mediaidList">图片列表，用逗号分割</param>
        /// <returns></returns>
        public Repair Add(string openid, string content, string mediaidList, string phone, int villageid, string name, int recordid)
        {
            var db = new ModelContext();
            var usr = WechatHelper.CheckOpenid(openid);
            usr = WechatHelper.CheckUser(usr);
            if (usr.UserInfoId == null || usr.UserInfoId == 0 || usr.UserInfo == null)
            {
                return null;
            }
            var mediaIdArr = mediaidList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);


            var number = GetNewInnerNumber();
            var rep = new Repair
            {
                Contenet = content,
                CreateDate = DateTime.Now,
                Status = RepairStatus.Apply,
                UserId = usr.UserInfoId,
                InnerNumber = number,
                PhoneNumber = phone,
                VillageId = villageid,
                Name = name,

            };


            db.RepairSet.Add(rep);
            db.SaveChanges();

            //下载文件

            //单个
            //if (!string.IsNullOrEmpty(mediaid))
            //{
            //    WechatImageHelper.AddNewImageForRepair(mediaid, rep.RepairId, openid);
            //}


            //多个
            if (mediaIdArr != null && mediaIdArr.Length > 0)
            {
                List<ImageDownloadLog> md = WechatImageHelper.AddNewImageForRepair(mediaIdArr, rep.RepairId, openid).ToList();
                md.Clear();
            }


            LogHelper.AddLog("Apply a new repair", rep.RepairId.ToString(), openid);

            db.SaveChanges();

            var vill = db.VillageSet.Find(villageid);
            RecordHelper.UpdateRecord(recordid, openid, RecordType.MpRepair, phone, name, vill.Name);

            //发送企业号通知
            var workernamArr = System.Configuration.ConfigurationManager.AppSettings["repairworkers"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var msg = string.Format(System.Configuration.ConfigurationManager.AppSettings["repairnoticemodelforworker"], rep.RepairId);
            WechatHelper.SendComponyMessage(workernamArr, msg);

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
            Repair rep = db.RepairSet.Include("Village").FirstOrDefault(item => item.RepairId == id);
            return rep;
        }

        /// <summary>
        /// 获得历史报修记录（限制条数）,仅用于首页展示。
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        //[Obsolete("0.4之前版本使用，之后版本需要区分不同状态，请勿调用！", true)]
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
                          select r).Take(tCount).ToList();

            foreach (var item in ownhis)
            {
                item.IsUserself = true;
            }

            //var rCount = tCount - ownhis.Count();
            //if (rCount > 0)
            //{
            //    var tmpList = (from r in db.RepairSet
            //                   select r).Take(rCount).OrderBy(item => item.RepairId).ToList();
            //    foreach (var item in tmpList)
            //    {
            //        item.IsUserself = false;
            //    }
            //    ownhis.AddRange(tmpList);
            //}

            return ownhis;
        }

        /// <summary>
        /// 获得维修记录(用于企业号)
        /// </summary>
        /// <param name="sta">需要获取的记录状态(获取 全部状态=0，提交=5,受理=10，处理完成待评价=20，完成=99)</param>
        /// <param name="count">每页的数量</param>
        /// <param name="page">当前页数（从0开始计数）</param>
        /// <returns></returns>
        public IEnumerable<Repair> GetHistoryRepair(RepairStatus status, int count, int page)
        {
            var db = new ModelContext();
            int skip = count * page;
            if (status == RepairStatus.Unknow)
            {
                return db.RepairSet.OrderByDescending(item => item.CreateDate).Skip(skip).Take(count);
            }
            else
            {
                return db.RepairSet.Where(item => item.Status == status).OrderByDescending(item => item.CreateDate).Skip(skip).Take(count);
            }
        }

        /// <summary>
        /// 获得维修记录
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="status">需要获取的记录状态(提交=5,受理=10，处理完成待评价=20，完成=99 ，未知=0)</param>
        /// <param name="count">每页数量</param>
        /// <param name="page">当前页数（从0开始计数）</param>
        /// <returns></returns>
        public IEnumerable<Repair> GetHistoryRepair(string openid, RepairStatus status, int count, int page)
        {
            var wuser = WechatHelper.CheckOpenid(openid);
            var user = WechatHelper.CheckUser(wuser);
            if (user == null)
            {
                return null;
            }

            var db = new ModelContext();

            int skip = count * page;
            return db.RepairSet.Where(item => item.UserId == user.UserInfoId && item.Status == status).OrderByDescending(item => item.CreateDate).Skip(skip).Take(count);
        }


        /// <summary>
        /// 受理报修
        /// </summary>
        /// <param name="repairId">报修记录id</param>
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
            rep.ExceptHandleDate = exceptDate;
            //  rep.InnerNumber = innderNumber;

            db.SaveChanges();

            //发送模板消息
            string content = rep.Contenet.Length > 7 ? rep.Contenet.Substring(0, 6) + "..." : rep.Contenet;
            var wuser = WechatHelper.CheckWechatUser(rep.UserId ?? 0);
            if (wuser != null && !string.IsNullOrEmpty(wuser.OpenId))
            {
                string url = string.Format("http://mp.doshine.com/doshinemp/PhoneWeb/RepairDetails?repairid={0}", rep.RepairId);
                WechatHelper.SendModelMessage(
                    wuser.OpenId,
                    url,
                    "qg8GNMOl1vwqU_1ks2p5UJqkrO6eg1bqak1qANGMkNU",
                    string.Format("\"first\":{{\"value\":\"你好，你的报修申请已被成功受理\",\"color\":\"#173177\"}},\"keyword1\":{{\"value\":\"{0}\",\"color\":\"#173177\"}},\"keyword2\":{{\"value\":\"{1}\",\"color\":\"#173177\"}},\"keyword3\":{{\"value\":\"{2}\",\"color\":\"#173177\"}},\"remark\":{{\"value\":\"我们将于 {3} 上门，敬请做好准备。点击查看详情>>>\",\"color\":\"#173177\"}}",
                    rep.Address,
                    content,
                    rep.CreateDate.ToLongDateString() + " " + rep.CreateDate.ToShortTimeString(),
                    rep.ExceptHandleDate.Value.ToLongDateString() + " " + rep.ExceptHandleDate.Value.ToShortTimeString()));
            }
            return rep;
        }

        /// <summary>
        /// 完成处理
        /// </summary>
        /// <param name="repairId">报修记录id</param>
        /// <returns></returns>
        [Obsolete("0.4之前版本使用，之后版本需添加相关确认信息！", true)]
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
        /// 完成处理
        /// </summary>
        /// <param name="repairId">报修记录id</param>
        /// <param name="mediaIdList">meidia Id列表</param>
        /// <param name="describe">文字描述</param>
        /// <param name="type">完成类型</param>
        /// <returns></returns>
        public Repair FinishHandlen(int repairId, IEnumerable<string> mediaIdList, string describe, RepairFinishType type)
        {
            var db = new ModelContext();
            var rep = db.RepairSet.FirstOrDefault(item => item.RepairId == repairId);

            if (rep == null)
            {
                return null;
            }

            rep.Status = RepairStatus.FinishHandle;
            rep.FinishHandlendDate = DateTime.Now;
            rep.FinishType = type;
            rep.Describe = describe;
            db.SaveChanges();
            var cl = WechatImageHelper.AddNewImageForHandleRepair(rep.RepairId, mediaIdList).ToList();
            cl.Clear();

            //发送模板消息
            string content = rep.Contenet.Length > 7 ? rep.Contenet.Substring(0, 6) + "..." : rep.Contenet;
            var wuser = WechatHelper.CheckWechatUser(rep.UserId ?? 0);
            if (wuser != null && !string.IsNullOrEmpty(wuser.OpenId))
            {
                string url = string.Format("http://mp.doshine.com/doshinemp/PhoneWeb/RepairDetails?repairid={0}", rep.RepairId);
                WechatHelper.SendModelMessage(
                    wuser.OpenId,
                    url,
                    "qg8GNMOl1vwqU_1ks2p5UJqkrO6eg1bqak1qANGMkNU",
                    string.Format("\"first\":{{\"value\":\"你好，你的报修申请已完成处理\",\"color\":\"#173177\"}},\"keyword1\":{{\"value\":\"{0}\",\"color\":\"#173177\"}},\"keyword2\":{{\"value\":\"{1}\",\"color\":\"#173177\"}},\"keyword3\":{{\"value\":\"{2}\",\"color\":\"#173177\"}},\"remark\":{{\"value\":\"处理结果为：{3}点击查看详情>>>\",\"color\":\"#173177\"}}",
                    rep.Address,
                    content,
                    rep.CreateDate.ToLongDateString() + " " + rep.CreateDate.ToShortTimeString(),
                    rep.FinishType.ToString()));
            }
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
            rep.Status = RepairStatus.Finish;

            db.SaveChanges();
            return rep;
        }

        /// <summary>
        /// 取消报修
        /// </summary>
        /// <param name="repairId">报修记录id</param>
        /// <param name="reason">取消原因描述</param>
        /// <returns></returns>
        public Repair Cancel(int repairId, string reason)
        {
            var db = new ModelContext();
            var rep = db.RepairSet.FirstOrDefault(item => item.RepairId == repairId);
            if (rep == null)
            {
                return null;
            }

            rep.Status = RepairStatus.Cancel;
            rep.Response = reason;
            rep.FinishHandlendDate = DateTime.Now;

            db.SaveChanges();


            //发送模板消息
            string content = rep.Response.Length > 7 ? rep.Response.Substring(0, 6) + "..." : rep.Response;
            var wuser = WechatHelper.CheckWechatUser(rep.UserId ?? 0);
            if (wuser != null && !string.IsNullOrEmpty(wuser.OpenId))
            {
                string url = string.Format("http://mp.doshine.com/doshinemp/PhoneWeb/RepairDetails?repairid={0}", rep.RepairId);
                WechatHelper.SendModelMessage(
                    wuser.OpenId,
                    url,
                    "EPg0GWHMJR0xupn1IV4VRz3iVrsFakORJ-hxqQ5CZ-U",
                    string.Format("\"first\":{{\"value\":\"抱歉，您的报修订单已经被取消\",\"color\":\"#173177\"}},\"keyword1\":{{\"value\":\"{0}\",\"color\":\"#173177\"}},\"keyword2\":{{\"value\":\"{1}\",\"color\":\"#173177\"}},\"remark\":{{\"value\":\"期待下次再为您服务。点击查看详情>>>\",\"color\":\"#173177\"}}",
                    content,
                    rep.CreateDate.ToLongDateString() + " " + rep.CreateDate.ToShortTimeString()
                   ));
            }

            return rep;
        }


        /// <summary>
        /// 获得某类维修记录的总页数（企业号使用）
        /// </summary>
        /// <param name="sta">需要获取的记录状态(获取 全部状态=0，提交=5,受理=10，处理完成待评价=20，完成=99)</param>
        /// <param name="count">每页的数量</param>
        /// <returns></returns>
        public int GetAllPageCount(RepairStatus sta, int count)
        {
            var db = new ModelContext();

            if (sta == RepairStatus.Unknow)
            {
                return db.RepairSet.Count() / count + 1;
            }
            else
            {
                return db.RepairSet.Where(item => item.Status == sta).Count() / count + 1;
            }
        }

        /// <summary>
        /// 获得某类维修记录的总页数（企业号使用）
        /// </summary>
        /// <param name="sta">需要获取的记录状态(获取 全部状态=0，提交=5,受理=10，处理完成待评价=20，完成=99)</param>
        /// <param name="count">每页的数量</param>
        /// <returns></returns>
        public int GetAllPageCount(RepairStatus sta, int count, string openid)
        {
            var db = new ModelContext();
            var wuser = WechatHelper.CheckOpenid(openid);
            var user = WechatHelper.CheckUser(wuser);
            if (user == null)
            {
                return 0;
            }

            var set = db.RepairSet.Where(item => item.UserId == user.UserInfoId);
            if (sta == RepairStatus.Unknow)
            {
                return set.Count() / count + 1;
            }
            else
            {
                return set.Where(item => item.Status == sta).Count() / count + 1;
            }
        }

        /// <summary>
        /// 生成维保订单号
        /// </summary>
        /// <returns></returns>
        public string GetNewInnerNumber()
        {
            var db = new ModelContext();
            var ran = new Random();
            var ret = "WB0";
            ret += (DateTime.Now.Year % 2000).ToString() + (DateTime.Now.ToString("MMddhhmmss").ToString());
            getRan: var ranstr = ran.Next(999999).ToString().PadLeft(5, '0').ToString();
            ret += ranstr;
            if (db.RepairSet.Any(item => item.InnerNumber == ret))
            {
                goto getRan;
            }

            return ret;
        }

        /// <summary>
        /// 获取所有小区
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Village> GetAllVillage()
        {
            var db = new ModelContext();
            return (from vil in db.VillageSet
                    where vil.IsEnable
                    select vil
                    ).ToList();
        }

        /// <summary>
        /// 根据地理位置距离升序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public IEnumerable<Village> GetAllVillage(double x, double y)
        {
            var db = new ModelContext();
            var ret = new List<Village>();
            foreach (var vil in db.VillageSet)
            {
                vil.Distance = WechatHelper.GetDistance(x, y, vil.LocationX, vil.LocationY);
                ret.Add(vil);
            }

            return ret.OrderBy(item => item.Distance).ToList();
        }

        /// <summary>
        /// 检查是否有未评论的报修，若无返回0，有则返回id，返回小于零的数表示错误！
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public int HasUnFinishedRepair(string openid)
        {
            var user = WechatHelper.CheckOpenid(openid);
            user = WechatHelper.CheckUser(user);

            if (user == null || user.UserInfo == null)
            {
                return -1;
            }

            var db = new ModelContext();
            var rep = db.RepairSet.Where(item => item.UserId == user.UserInfoId && item.Status == RepairStatus.FinishHandle);
            if (rep.Count() == 0)
            {
                return 0;
            }

            return rep.OrderBy(item => item.CreateDate).First().RepairId;
        }

        /// <summary>
        /// 获取常用报修选项
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<string>> GetOveruseRepair()
        {
            var ret = new Dictionary<string, List<string>>();

            var db = new ModelContext();
            var fList =  db.OveruseRepairSet.Where(item => item.Level != 1).ToList();

            foreach (var item in fList)
            {
                ret.Add(item.Content, (
                    from s in db.OveruseRepairSet
                    where s.Level == 1 && s.FatherItem != null && s.FatherItem == item.OverusRepairId
                    select s.Content).ToList());
            }

            return ret;
        }
    }




}
