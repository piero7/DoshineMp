using DoShineMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoShineMP.Helper
{
    public class PartnerHelper
    {
        /// <summary>
        /// 合作伙伴注册
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="comName">公司名称</param>
        /// <param name="type">类型（经销商，供应商）</param>
        /// <param name="realname">真实姓名</param>
        /// <param name="address">公司地址</param>
        /// <param name="comPhone">公司电话</param>
        /// <returns></returns>
        public Partner ReginPartner(string openid, string comName, PartnerType type, string realname, string address, string comPhone)
        {
            var db = new ModelContext();
            var usr = WechatHelper.CheckOpenid(openid);
            usr = WechatHelper.CheckUser(usr);
            if (usr.UserInfoId == null || usr.UserInfoId == 0 || usr.UserInfo == null)
            {
                return null;
            }


            var pat = new Partner
            {
                RealName=realname,
                Address = address,
                CompanyName = comName,
                CreateDate = DateTime.Now,
                UserId = usr.UserInfoId,
                CompanyPhone = comPhone,
                Point = 0,
                Type = type,
            };
            db.PartnerSet.Add(pat);
            db.SaveChanges();
            db.WechatUserSet.Find(usr.WechatUserId).PartnerId = pat.PartnerId;
            db.SaveChanges();
            LogHelper.AddLog("Regist as a patner.", pat.PartnerId.ToString(), openid);

            return pat;
        }



        //[HttpGet]
        public Partner EditPartnerInfo(string openid, string comName, PartnerType type, string realname, string address, string comPhone)
        {
            var db = new ModelContext();
            var usr = WechatHelper.CheckOpenid(openid);
            usr = WechatHelper.CheckUser(usr);
            if (usr.UserInfoId == null || usr.UserInfoId == 0 || usr.UserInfo == null)
            {
                return null;
            }

            var pat = db.PartnerSet.Find(usr.PartnerId);
            pat.RealName = realname;
            pat.CompanyName = comName;
            pat.Type = type;
            pat.Address = address;
            pat.CompanyPhone = comPhone;

            db.SaveChanges();

            LogHelper.AddLog("Edit patner info .", usr.PartnerId.ToString(), openid);

            return usr.PartnerInfo;

        }


        /// <summary>
        /// 获取经销商信息
        /// </summary>
        /// <param name="openid">用户openid</param>
        /// <returns></returns>
        public WechatUser GetPartnerInfo(string openid)
        {
            WechatUser wuser = WechatHelper.CheckOpenid(openid);
            wuser = WechatHelper.CheckPartner(wuser);
            return wuser;
        }
    }
}
