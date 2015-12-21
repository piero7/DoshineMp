using DoShineMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoShineMP.Helper
{
    public class WechatUserHelper
    {

        public WechatUser Regiet(string realName, string phoneNumber, string openid)
        {
            var db = new ModelContext();
            //var temn = db.TerminalSet.FirstOrDefault(item => item.Name == System.Configuration.ConfigurationManager.AppSettings["termailname"]);
            var user = db.WechatUserSet.FirstOrDefault(item => item.OpenId == openid);
            if (user == null)
            {
                return null;
            }

            UserInfo ui = new UserInfo
            {
                Name = realName,
                PhoneNumber = phoneNumber,
                //LastLoginTerminalId = temn.TerminalId,
                //LastLoginTime = DateTime.Now
            };

            //db.ActiveLogSet.Add(new ActiveLog
            //{
            //    CreateDate = DateTime.Now,
            //    OptionContent = "Regist in Doshine wechat service",
            //    TerminalId = temn.TerminalId,
            //    UserId = ui.UserInfoId,
            //});

            user.UserInfo = ui;

            db.SaveChanges();
            LogHelper.AddLog("Regist in Doshine wechat service", "", openid);
            return user;
        }

        public WechatUser EditUserInfo(string openid, string realName, string phoneNumber)
        {
            var db = new ModelContext();
            var wusr = db.WechatUserSet.Include("UserInfo").FirstOrDefault(item => item.OpenId == openid);
            if (wusr == null)
            {
                return null;
            }

            if (wusr.UserInfo == null)
            {
                return null;
            }

            wusr.UserInfo.Name = realName;
            wusr.UserInfo.PhoneNumber = phoneNumber;
            db.SaveChanges();

            LogHelper.AddLog("Edit infomation ", "", openid);
            return wusr;
        }

        /// <summary>
        /// 获取用户信息,返回值的UserInfo字段为null则表示用户未注册
        /// </summary>
        /// <param name="openid">openid</param>
        /// <returns>若</returns>
        public WechatUser GetUserInfo(string openid)
        {
            var wuser = WechatHelper.CheckOpenid(openid);
            var user = WechatHelper.CheckUser(wuser);
            if (user == null || user.UserInfoId == null)
            {
                return wuser;
            }
            return user;
        }

    }
}
