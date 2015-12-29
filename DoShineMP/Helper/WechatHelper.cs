using DoShineMP.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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
                if (DateTime.Now.Subtract(token.GetTime.Value).TotalSeconds < 3000)
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
            user.SubscribeTime = UnixTimestampToDateTime(long.Parse(dic["subscribe_time"]));

            nUser.NickName = dic["nickname"];
            nUser.Sex = dic["sex"] == "1" ? Sex.Man : dic["sex"] == "2" ? Sex.Female : Sex.Unknown;
            nUser.City = dic["city"];
            nUser.Country = dic["country"];
            nUser.Province = dic["province"];
            nUser.Language = dic["language"];
            nUser.Headimgurl = dic["headimgurl"];
            nUser.SubscribeTime = UnixTimestampToDateTime(long.Parse(dic["subscribe_time"]));
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

            if (user == null || string.IsNullOrEmpty(user.OpenId))
            {
                user = new WechatUser { OpenId = openid };
                db.WechatUserSet.Add(user);
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

        /// <summary>
        /// 检查用户是否注册为经销商
        /// </summary>
        /// <param name="wuser">微信用户信息</param>
        /// <returns></returns>
        public static WechatUser CheckPartner(WechatUser wuser)
        {
            var db = new ModelContext();
            wuser = db.WechatUserSet.Include("PartnerInfo").Include("UserInfo").FirstOrDefault(item => item.WechatUserId == wuser.WechatUserId);
            if (wuser == null || wuser.PartnerInfo == null || wuser.UserInfo == null)
            {
                return null;
            }
            return wuser;
        }

        /// <summary>
        /// unix时间戳转换成日期
        /// </summary>
        /// <param name="unixTimeStamp">时间戳（秒）</param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0);
            return start.AddSeconds(timestamp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }



        /// <summary>
        /// 支付使用的密钥创建
        /// </summary>
        /// <param name="pp"></param>
        /// <returns></returns>
        //public static string GetMD5(ref Models.PayParms pp)
        //{
        //    Dictionary<string, string> dic = new Dictionary<string, string>();

        //    dic.Add("appId", pp.appId);
        //    dic.Add("timeStamp", pp.timeStamp);
        //    dic.Add("package", pp.package);
        //    dic.Add("signType", pp.signType);
        //    dic.Add("nonceStr", pp.nonceStr);
        //    pp.paySign = GetMD5(dic);

        //    return pp.paySign;。
        //}

        public static string GetMD5(Dictionary<string, string> dic)
        {
            var enStr = "";
            ArrayList al = new ArrayList(dic.Keys);
            al.Sort();
            foreach (string item in al)
            {
                enStr += item + "=" + dic[item] + "&";
            }
            enStr += "key=" + System.Configuration.ConfigurationManager.AppSettings["paykey"];
            return GetMD5(enStr);

        }

        /// <summary>
        /// 用于网页载入js时获取签名
        /// </summary>
        /// <param name="noncestr"></param>
        /// <param name="jsTicket"></param>
        /// <param name="times"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetMD5(string noncestr, string jsTicket, string times, string url)
        {
            string dStr = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}");
            return GetMD5(dStr);
        }

        public static string GetMD5(string encypStr)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);

            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        public static string GetSha1(string noncestr, string jsTicket, string times, string url)
        {
            string dStr = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsTicket, noncestr, times, url);
            return GetSha1(dStr);
        }

        public static string GetSha1(string dstr)
        {
            byte[] tmp = Encoding.UTF8.GetBytes(dstr);

            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            byte[] tmp2 = sha.ComputeHash(tmp);
            sha.Clear();

            string ret = BitConverter.ToString(tmp2).Replace("-", "").ToLower();
            return ret;
        }

        /// <summary>
        /// 获取微信前台js完整导入包
        /// </summary>
        /// <param name="url">当前url，用于生成签名</param>
        /// <returns></returns>
        public static Object GetWechatJsConfig(string url)
        {
            string ticke = GetToken(AccountType.JsTicket);

            string nonceStr = GetMD5(DateTime.Now.ToString());
            string appid = System.Configuration.ConfigurationManager.AppSettings["appid"];
            string times = GetTimestamp();
            string singature = GetSha1(nonceStr, ticke, times, url);
            var ret = new
            {
                debug = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["isjsdebug"]),
                appId = appid,
                timestamp = times,
                nonceStr = nonceStr,
                signature = singature,
            };

            return ret;
        }



        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="mediaid"></param>
        /// <returns></returns>
        public static string DownloadImgFile(string mediaid)
        {
            string file = string.Empty;
            string content = string.Empty;
            string strpath = string.Empty;
            string savepath = string.Empty;
            string stUrl = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + Helper.WechatHelper.GetToken(AccountType.Service) + "&media_id=" + mediaid;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(stUrl);

            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();

                strpath = myResponse.ResponseUri.ToString();
                //WriteLog("接收类别://" + myResponse.ContentType);
                WebClient mywebclient = new WebClient();
                //生成文件名
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random(99999999)).Next().ToString().PadRight(7, '0').Substring(0, 4) + ".jpg";

                savepath = System.Web.Hosting.HostingEnvironment.MapPath(System.Configuration.ConfigurationManager.AppSettings["downimgpath"]) + fileName;
                //WriteLog("路径://" + savepath);
                try
                {
                    mywebclient.DownloadFile(strpath, savepath);
                    file = fileName;
                }
                catch (Exception ex)
                {
                    throw ex;
                    //savepath = ex.ToString();
                }

            }
            return file;
        }

    }

    /// <summary>
    /// 微信网页js导入使用config
    /// </summary>
    public class WechatJsConfig
    {
        public bool debug { get; set; } = false;

        public string appId { get; set; }

        public string timestamp { get; set; }

        public string nonceStr { get; set; }

        public string signature { get; set; }

        /// <summary>
        /// 有参构造函数，自动完成相关congif填写
        /// </summary>
        /// <param name="url">访问的完整uranium</param>
        public WechatJsConfig(string url)
        {
            string ticke = Helper.WechatHelper.GetToken(AccountType.JsTicket);

            string nonceStr = Helper.WechatHelper.GetMD5(DateTime.Now.ToString());
            string appid = System.Configuration.ConfigurationManager.AppSettings["appid"];
            string times = Helper.WechatHelper.GetTimestamp();
            string singature = Helper.WechatHelper.GetSha1(nonceStr, ticke, times, url);

            this.debug = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["isjsdebug"]);
            this.appId = appid;
            this.timestamp = times;
            this.nonceStr = nonceStr;
            this.signature = singature;
        }
    }
}
