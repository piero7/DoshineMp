using DoShineMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoShineMP.Helper
{
    public class LogHelper
    {
        public static ActiveLog AddLog(string option, string remaeks, string openid)
        {
            var db = new ModelContext();
            var usr = db.WechatUserSet.Include("UserInfo").FirstOrDefault(item => item.OpenId == openid);
            if (usr == null)
            {
                return null;
            }
            if (usr.UserInfoId == null || usr.UserInfoId == 0 || usr.UserInfo == null)
            {
                return null;
            }

            var tmn = db.TerminalSet.FirstOrDefault(item => item.Name == System.Configuration.ConfigurationManager.AppSettings["termailname"]);
            usr.UserInfo.LastLoginTerminalId = tmn.TerminalId;
            usr.UserInfo.LastLoginTime = DateTime.Now;

            var log = new ActiveLog
            {
                CreateDate = DateTime.Now,
                TerminalId = tmn.TerminalId,
                UserId = usr.UserInfoId,
                OptionContent = option,
                Remarks = remaeks,
            };
            db.ActiveLogSet.Add(log);

            usr.UserInfo.LastLoginTime = DateTime.Now;
            usr.UserInfo.LastLoginTerminalId = tmn.TerminalId;
            db.SaveChanges();

            return log;

        }
    }
}
