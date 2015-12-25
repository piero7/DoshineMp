using DoShineMP.Helper;
using DoShineMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoShineMP.Controllers
{
    public class IdentifyingCodeController : ApiController
    {
        /// <summary>
        /// 发送验证码，若成功则返回ID后续一并提交，若未成功则返回0
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="number">电话号码</param>
        /// <returns></returns>
        [HttpGet]
        public int GetIndentifyingCode(string openid, string number)
        {
            var code = (DateTime.Now.ToString() + openid).GetHashCode().ToString().Replace("-", "");
            code = code.PadLeft(6, '0').Substring(code.Length - 6, 6);

            var ic = new IdentifyingCode
            {
                OpenId = openid,
                Content = code,
                PhoneNumber = number,
            };

            var codeStirng = string.Format(System.Configuration.ConfigurationManager.AppSettings["identifyingcodebasestring"], code);
            var ret = ShortMessageHelper.SendMsg(number, codeStirng);

            var db = new ModelContext();

            if (ret == "s")
            {
                ic.IsSendSuccess = true;
                db.IdentifyingCodeSet.Add(ic);
                db.SaveChanges();
                return ic.IdentifyingCodeId;
            }
            else
            {
                ic.IsSendSuccess = false;
                ic.Remarks = ret;
                db.IdentifyingCodeSet.Add(ic);
                db.SaveChanges();
                return 0;
            }
        }

        /// <summary>
        /// 测试方法，请勿调用！！！
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <param name="codeid"></param>
        /// <returns></returns>
        [HttpGet]
        private bool TestCheckIdentifyingCode(string openid, string phone, string code, int codeid)
        {
            return IdentifyingCodeHelper.CheckCode(codeid, code, openid, phone);
        }

    }
}
