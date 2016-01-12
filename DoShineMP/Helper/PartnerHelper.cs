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
        /// <param name="email">电子邮件</param>
        /// <param name="salesmanId">对应的销售id</param>
        /// <param name="files">相关资质图片字符串，格式为文件 名称1 :mediaid; eg:经营许可证:000001;组织机构代码:000005;</param>
        /// <returns></returns>
        public Partner ReginPartner(string openid, string comName, PartnerType type, string realname, string address, string comPhone, int salesmanId, string email, string files, int discrictid)
        {
            var db = new ModelContext();
            var usr = WechatHelper.CheckOpenid(openid);
            usr = WechatHelper.CheckUser(usr);
            if (usr.UserInfoId == null || usr.UserInfoId == 0 || usr.UserInfo == null)
            {
                return null;
            }

            // 将用户信息中的姓名更新
            usr.UserInfo.Name = realname;

            var pat = new Partner
            {
                RealName = realname,
                Address = address,
                CompanyName = comName,
                CreateDate = DateTime.Now,
                UserId = usr.UserInfoId,
                CompanyPhone = comPhone,
                Point = 0,
                Type = type,
                Email = email,
                SalesmanId = salesmanId,
                DistrictId = discrictid,
                Status = PartnerStatus.Apply,
            };
            db.PartnerSet.Add(pat);
            db.SaveChanges();
            db.WechatUserSet.Find(usr.WechatUserId).PartnerId = pat.PartnerId;
            db.SaveChanges();

            //下载文件
            var filestrList = files.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var fileDic = new Dictionary<string, string>();
            foreach (var filestr in filestrList)
            {
                var fileinfo = filestr.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (fileinfo.Length == 2)
                {
                    fileDic.Add(fileinfo[0], fileinfo[1]);
                }
            }
            if (fileDic != null && fileDic.Count > 0)
            {
                WechatImageHelper.AddNewImageForPartner(fileDic, pat.PartnerId, openid);
            }

            LogHelper.AddLog("Regist as a patner.", pat.PartnerId.ToString(), openid);

            return pat;
        }


        //[HttpGet]
        /// <summary>
        /// 编辑合作伙伴
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="comName"></param>
        /// <param name="realname"></param>
        /// <param name="address"></param>
        /// <param name="comPhone"></param>
        /// <returns></returns>
        public Partner EditPartnerInfo(string openid, string comName, string realname, string address, string comPhone, string email)
        {
            var db = new ModelContext();
            var usr = WechatHelper.CheckOpenid(openid);
            usr = WechatHelper.CheckUser(usr);
            if (usr.UserInfoId == null || usr.UserInfoId == 0 || usr.UserInfo == null)
            {
                return null;
            }

            usr.UserInfo.Name = realname;
            var pat = db.PartnerSet.Find(usr.PartnerId);
            pat.RealName = realname;
            pat.CompanyName = comName;
            //pat.Type = type;
            pat.Address = address;
            pat.CompanyPhone = comPhone;
            pat.Email = email;

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

        /// <summary>
        /// 添加相关资质
        /// </summary>
        /// <param name="mediaDic">key为文件名，value为mediaId</param>
        /// <returns></returns>
        public Partner AddFile(int partnerId, Dictionary<string, string> mediaDic, string openid)
        {
            var db = new ModelContext();
            var par = db.PartnerSet.FirstOrDefault(item => item.PartnerId == partnerId);
            if (par == null)
            {
                return null;
            }
            WechatImageHelper.AddNewImageForPartner(mediaDic, partnerId, openid);
            return par;
        }

        /// <summary>
        /// 确认合作伙伴
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public Partner Accrpt(int partnerId)
        {
            var db = new ModelContext();
            var par = db.PartnerSet.FirstOrDefault(item => item.PartnerId == partnerId);

            if (par == null)
            {
                return null;
            }

            par.Status = PartnerStatus.Accept;
            db.SaveChanges();
            return par;
        }

        /// <summary>
        /// 注销合作伙伴
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public Partner Cancel(int partnerId)
        {
            var db = new ModelContext();
            var par = db.PartnerSet.FirstOrDefault(item => item.PartnerId == partnerId);
            if (par == null)
            {
                return null;
            }

            par.Status = PartnerStatus.Cancel;
            db.SaveChanges();

            return par;
        }

        /// <summary>
        /// 获取所有的地区
        /// </summary>
        /// <returns></returns>
        public IEnumerable<District> GetAllDistrict()
        {
            var db = new ModelContext();
            return db.DistrictSet.ToList();
        }
    }
}
