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
            var wuser = db.WechatUserSet.Include("UserInfo").FirstOrDefault(item => item.OpenId == openid);
            if (wuser == null)
            {
                return null;
            }
            if (wuser.UserInfo == null)
            {
                return null;
            }

            var pat = new Partner
            {
                Address = address,
                CompanyName = comName,
                CreateDate = DateTime.Now,
                UserId = wuser.UserInfoId,
                CompanyPhone = comPhone,
                Point = 0,
                Type = type,
            };
            db.PartnerSet.Add(pat);
            wuser.PartnerInfo = pat;
            db.SaveChanges();
            LogHelper.AddLog("Regist as a patner.", pat.PartnerId.ToString(), openid);

            return pat;
        }



        [HttpGet]
        public Partner EditPartnerInfo(string openid, string comName, PartnerType type, string realname, string address, string comPhone)
        {
            var db = new ModelContext();
            var wuser = db.WechatUserSet.Include("PartnerInfo").FirstOrDefault(item => item.OpenId == openid);
            if (wuser == null)
            {
                return null;
            }
            if (wuser.PartnerId == null)
            {
                return null;
            }

            wuser.PartnerInfo.RealName = realname;
            wuser.PartnerInfo.CompanyName = comName;
            wuser.PartnerInfo.Type = type;
            wuser.PartnerInfo.Address = address;
            wuser.PartnerInfo.CompanyPhone = comPhone;

            db.SaveChanges();

            LogHelper.AddLog("Edit patner info .", wuser.PartnerId.ToString(), openid);

            return wuser.PartnerInfo;

        }
    }
}
