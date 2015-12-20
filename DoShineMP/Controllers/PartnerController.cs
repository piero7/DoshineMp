using DoShineMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoShineMP.Controllers
{
    public class PartnerController : ApiController
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
        [HttpGet]
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
            db.SaveChanges();
            LogController.AddLog("Regist as a patner.", pat.PartnerId.ToString(), openid);

            return pat;
        }

        [HttpGet]
        public Partner EditPartnerInfo(string openid, string comName, PartnerType type, string realname, string address, string comPhone)
        {
            var db = new ModelContext();

        }
    }
}
