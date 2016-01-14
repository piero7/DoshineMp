using DoShineMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Helper
{
    class IdentifyingCodeHelper
    {
        /// <summary>
        /// 验证验证码的正确性
        /// </summary>
        /// <param name="codeId">验证码的id</param>
        /// <param name="codeStr">验证码</param>
        /// <param name="openid">用户的openid</param>
        /// <param name="phone">用户的手机号</param>
        /// <returns></returns>
        public static bool CheckCode(int codeId, string codeStr, string openid, string phone)
        {
            var db = new ModelContext();
            var code = db.IdentifyingCodeSet.FirstOrDefault(item => item.IdentifyingCodeId == codeId);
            if (code == null)
            {
                return false;
            }
            if (!code.IsUsed)
            {
                if (code.Content == codeStr)
                {
                    code.IsUsed = true;
                    db.SaveChanges();

                    if (code.OpenId == openid && code.PhoneNumber == phone && code.IsSendSuccess)
                    {
                        if (DateTime.Now.Subtract(code.CreateDate).TotalMinutes < 30)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
