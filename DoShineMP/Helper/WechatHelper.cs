using DoShineMP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace DoShineMP.Helper
{
    public class WechatHelper
    {

        /// <summary>
        /// 发送POST包，获得回复。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetResponse(string data, string url)
        {
            HttpWebRequest myHttpWebRequest = null;
            string strReturnCode = string.Empty;
            //ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            myHttpWebRequest.ProtocolVersion = HttpVersion.Version10;

            byte[] bs;

            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            bs = Encoding.UTF8.GetBytes(data);

            myHttpWebRequest.ContentLength = bs.Length;

            using (Stream reqStream = myHttpWebRequest.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }


            using (WebResponse myWebResponse = myHttpWebRequest.GetResponse())
            {
                StreamReader readStream = new StreamReader(myWebResponse.GetResponseStream(), Encoding.UTF8);
                strReturnCode = readStream.ReadToEnd();
            }

            return strReturnCode;
        }

        /// <summary>
        /// 通过网页获取的code换取用户openid
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetOpenidByCode(string code)
        {
            //  var a = new Helper();
            // a.WriteTxt(code);
            string openid = "err";

            string apps = System.Configuration.ConfigurationManager.AppSettings["appsecrect"];
            string appid = System.Configuration.ConfigurationManager.AppSettings["appid"];
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appid, apps, code);

            string resStr = WechatHelper.GetResponse("", url);

            //a.WriteTxt(resStr);

            //  resStr = string.Format("{{\"res\":{0} }}", resStr);
            var resXml = JsonConvert.DeserializeXNode(resStr, "res");
            var node = resXml.Element("res").Element("openid");
            if (node != null)
            {
                openid = node.Value; ;
            }
            else
            {
                openid = resStr;
            }
            if (openid.Contains("{"))
            {
                openid = null;
            }

            return openid;
        }


        /// <summary>
        /// 获取跳转链接，重新获取Code。一般用于没有获取到用户Code。
        /// </summary>
        /// <param name="data">自动获取控制器及方法名称</param>
        /// <param name="state">需要添加的参数</param>
        /// <returns></returns>
        public static string BackForCode(System.Web.Routing.RouteValueDictionary data, string state)
        {
            string url = "/" + data["controller"] + "/" + data["action"];
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect",
                     System.Configuration.ConfigurationManager.AppSettings["appid"],
                     System.Configuration.ConfigurationManager.AppSettings["baseUrl"] + url,
                     state ?? "");
        }

        /// <summary>
        /// 获取跳转链接，重新获取Code。一般用于没有获取到用户Code。
        /// </summary>
        /// <param name="controllerName">控制器名</param>
        /// <param name="actionName">方法名</param>
        /// <param name="state">需要添加的参数</param>
        /// <returns></returns>
        public static string BackForCode(string controllerName, string actionName, string state)
        {
            string url = "/" + controllerName + "/" + actionName;
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect",
                     System.Configuration.ConfigurationManager.AppSettings["appid"],
                     System.Configuration.ConfigurationManager.AppSettings["baseUrl"] + url,
                     state ?? "");
        }


        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="type">Accesstoken类型</param>
        /// <returns></returns>
        static public string GetToken(AccountType type)
        {
            if (type == AccountType.Service)
            {
                string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
                var context = new ModelContext();
                var token = context.TokenSet.First(item => item.Type == AccountType.Service);

                //现有token是否可用
                if (DateTime.Now.Subtract(token.GetTime.Value).TotalSeconds < 7000)
                {
                    return token.TokenStr;
                }


                url = string.Format(url, System.Configuration.ConfigurationManager.AppSettings["appid"], System.Configuration.ConfigurationManager.AppSettings["appsecrect"]);
                #region  获得新的token
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url); ;
                req.Method = "GET";
                req.Timeout = 2000;

                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);

                var retString = sr.ReadToEnd();

                JavaScriptSerializer js = new JavaScriptSerializer();
                var retDic = js.Deserialize<Dictionary<string, string>>(retString);

                string newToken = "";
                if (retDic.ContainsKey("access_token"))
                {
                    newToken = retDic["access_token"];
                }
                #endregion
                token.TokenStr = newToken;
                token.GetTime = DateTime.Now;
                context.SaveChanges();

                return newToken;
            }

            if (type == AccountType.Company)
            {
                string url = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}";
                var context = new ModelContext();
                var token = context.TokenSet.First(item => item.Type == AccountType.Company);

                //现有token是否可用
                if (DateTime.Now.Subtract(token.GetTime.Value).TotalSeconds < 6000)
                {
                    return token.TokenStr;
                }


                url = string.Format(url, System.Configuration.ConfigurationManager.AppSettings["eid"], System.Configuration.ConfigurationManager.AppSettings["esecrect"]);
                #region  获得新的token
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url); ;
                req.Method = "GET";
                req.Timeout = 2000;

                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);

                var retString = sr.ReadToEnd();

                JavaScriptSerializer js = new JavaScriptSerializer();
                var retDic = js.Deserialize<Dictionary<string, string>>(retString);

                string newToken = "";
                if (retDic.ContainsKey("access_token"))
                {
                    newToken = retDic["access_token"];
                }
                #endregion
                token.TokenStr = newToken;
                token.GetTime = DateTime.Now;
                context.SaveChanges();

                return newToken;
            }
            if (type == AccountType.JsTicket)
            {
                var db = new ModelContext();
                var token = db.TokenSet.FirstOrDefault(item => item.Type == AccountType.JsTicket);
                if (DateTime.Now.Subtract(token.GetTime.Value).TotalSeconds < 6000)
                {
                    return token.TokenStr;
                }


                string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi ", WechatHelper.GetToken(AccountType.Service));

                WebClient client = new WebClient();
                string res = client.UploadString(url, "");

                JavaScriptSerializer js = new JavaScriptSerializer();
                var retDic = js.Deserialize<Dictionary<string, string>>(res);
                if (!retDic.Keys.Contains("ticket"))
                {
                    return null;
                }

                var ret = retDic["ticket"];
                token.GetTime = DateTime.Now;
                token.TokenStr = ret;
                db.SaveChanges();
                return ret;

            }

            return "";
        }


        /// <summary>
        /// 获取用户信息(确保数据库中以及存在包含openid的user记录！)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        static public WechatUser GetUserInfo(WechatUser user)
        {
            var db = new ModelContext();

            var nUser = db.WechatUserSet.Find(user.WechatUserId);
            //var user = db.UserSet.FirstOrDefault(u => u.OpenId == userOpenId);
            //if (userOpenId == null)
            //{
            //    throw new NotFindCustomerOpenIDException(userOpenId);
            //}

            //if (firm == null)
            //{
            //    throw new NotFindFirmTokenException();
            //}

            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";
            var access = GetToken(AccountType.Service);
            url = string.Format(url, access, user.OpenId);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Timeout = 2000;
            req.Method = "GET";

            var res = (HttpWebResponse)req.GetResponse();
            var s = res.GetResponseStream();
            var sr = new StreamReader(s);
            var resString = sr.ReadToEnd();

            JavaScriptSerializer js = new JavaScriptSerializer();
            var dic = js.Deserialize<Dictionary<string, string>>(resString);
            if (dic.Keys.Contains("errcode"))
            {
                return null;
            }
            user.subscribe = (dic["subscribe"] == "1");
            user.NickName = dic["nickname"];
            user.Sex = dic["sex"] == "1" ? Sex.Man : dic["sex"] == "2" ? Sex.Female : Sex.Unknown;
            user.City = dic["city"];
            user.Country = dic["country"];
            user.Province = dic["province"];
            user.Language = dic["language"];
            user.Headimgurl = dic["headimgurl"];

            nUser.NickName = dic["nickname"];
            nUser.Sex = dic["sex"] == "1" ? Sex.Man : dic["sex"] == "2" ? Sex.Female : Sex.Unknown;
            nUser.City = dic["city"];
            nUser.Country = dic["country"];
            nUser.Province = dic["province"];
            nUser.Language = dic["language"];
            nUser.Headimgurl = dic["headimgurl"];
            db.SaveChanges();

            return user;
        }



        /// <summary>
        /// 检查用户的openid，若不存在则添加并获取用户信息。
        /// </summary>
        /// <param name="openid"></param>
        /// <returns>111</returns>
        public static WechatUser CheckOpenid(string openid)
        {
            var db = new ModelContext();
            var user = db.WechatUserSet.Include("UserInfo").FirstOrDefault(item => item.OpenId == openid);

            if (user == null)
            {
                db.WechatUserSet.Add(new WechatUser { OpenId = openid });
                db.SaveChanges();
                WechatHelper.GetUserInfo(user);
            }

            return user;
        }

        /// <summary>
        /// 检查用户是否注册
        /// </summary>
        /// <param name="wuser">微信用户信息</param>
        /// <returns>若已经注册则封装，若没有注册则返回null</returns>
        public static WechatUser CheckUser(WechatUser wuser)
        {
            var db = new ModelContext();
            wuser = db.WechatUserSet.Include("UserInfo").FirstOrDefault(item => item.WechatUserId == wuser.WechatUserId);
            if (wuser == null || wuser.UserInfo == null)
            {
                return null;
            }

            return wuser;
        }

    }
}
