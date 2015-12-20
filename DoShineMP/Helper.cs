using DoShineMP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace VillageMP.Controllers
{
    class Helper
    {

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

            string resStr = Helper.GetResponse("", url);

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
        /// 通过code换取用户账号（企业号）
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetUserIdByCodeForCompany(string code)
        {
            string url = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}";

            string userid = "err";

            string appid = Helper.GetToken(AccountType.Company);
            url = string.Format(url, appid, code);

            string resStr = Helper.GetResponse("", url);

            //a.WriteTxt(resStr);

            //  resStr = string.Format("{{\"res\":{0} }}", resStr);
            var resXml = JsonConvert.DeserializeXNode(resStr, "res");
            var node = resXml.Element("res").Element("UserId");
            if (node != null)
            {
                userid = node.Value; ;
            }
            else
            {
                userid = resStr;
            }
            return userid;
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


                string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi ", Helper.GetToken(AccountType.Service));

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
        /// 通知设备
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="billId"></param>
        /// <returns></returns>
        //public static string NoticeMachine(Models.Machine machine, int billId)
        //{

        //    //ADXC 001 12345678
        //    string orderStr = "";
        //    orderStr += GetHexString(DateTime.Now.Year - 2015);
        //    orderStr += GetHexString(DateTime.Now.Month);
        //    orderStr += DateTime.Now.Day.ToString().PadLeft(2, '0');

        //    var random = new Random();
        //getRandom: orderStr += random.Next(9999).ToString().PadLeft(4, '0');
        //    var db = new Models.ModelContext();
        //    if (db.OpenLogSet.Any(item => item.Machine.InnerId == machine.InnerId && item.LogNumber == orderStr))
        //    {
        //        goto getRandom;
        //    }
        //    //TODO add machine log
        //    //db.MachineBillSet.FirstOrDefault(item => item.BillId == billId).innerNumber = orderStr;
        //    db.SaveChanges();

        //    byte[] msg = System.Text.Encoding.ASCII.GetBytes(machine.InnerId.ToUpper() + GetHexString(machine.UpLevel) + GetHexString(machine.LowLevel) + GetHexString(machine.OpenLength) + orderStr);

        //    Socket so = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    var retArr = new byte[1024];
        //    try
        //    {
        //        so.Connect(System.Configuration.ConfigurationManager.AppSettings["serviceaddress"], int.Parse(System.Configuration.ConfigurationManager.AppSettings["serviceport"]));
        //        so.Send(msg);

        //        so.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 15000);
        //        int retCount = 0;
        //        while (retCount < 8)
        //        {
        //            retCount = so.Receive(retArr, 0, 1024, SocketFlags.None);
        //        }
        //    }
        //    finally
        //    {
        //        so.Close(3000);
        //        so.Dispose();
        //    }
        //    // so.Disconnect(false);


        //    return Encoding.ASCII.GetString(retArr);
        //}

        /// <summary>
        /// 将int转换成16进制的一位字符串
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetHexString(int number)
        {
            if (number > 15 || number < 0)
            {
                return null;
            }
            if (number < 10)
            {
                return number.ToString();
            }
            if (number == 10)
            {
                return "a";
            }
            if (number == 11)
            {
                return "b";
            }
            if (number == 12)
            {
                return "c";
            }
            if (number == 13)
            {
                return "d";
            }
            if (number == 14)
            {
                return "e";
            }
            if (number == 15)
            {
                return "f";
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取两点之间的距离
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            double rad = 6371; //Earth radius in Km
            double p1X = x1 / 180 * Math.PI;
            double p1Y = y1 / 180 * Math.PI;
            double p2X = x2 / 180 * Math.PI;
            double p2Y = y2 / 180 * Math.PI;
            return Math.Acos(Math.Sin(p1Y) * Math.Sin(p2Y) +
                Math.Cos(p1Y) * Math.Cos(p2Y) * Math.Cos(p2X - p1X)) * rad;
        }

        public static string GetJsApiSignature(string url, string noncestr, string timestamp)
        {
            var tic = Helper.GetToken(AccountType.JsTicket);
            var cpyString = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", tic, noncestr, timestamp, url);
            var ret = GetSha1Str(cpyString);
            return ret;
        }

        public static string GetSha1Str(string str)
        {
            byte[] strRes = Encoding.UTF8.GetBytes(str);
            System.Security.Cryptography.HashAlgorithm iSha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            strRes = iSha.ComputeHash(strRes);
            var enText = new StringBuilder();
            foreach (byte iByte in strRes)
            {
                enText.AppendFormat("{0:x2}", iByte);
            }
            return enText.ToString();
        }

        /// <summary>
        /// 跳转回去，重新获取code
        /// </summary>
        /// <param name="url">控制器名+方法名</param>
        /// <param name="state">参数</param>
        /// <returns></returns>
        //public static System.Web.Mvc.ActionResult BackForCode(System.Web.Routing.RouteValueDictionary data, string state)
        //{
        //    string url = "/" + data["controller"] + "/" + data["action"];
        //    return System.Web.Mvc.Controller.Redirect(string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri=P{1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect",
        //             System.Configuration.ConfigurationManager.AppSettings["appid"],
        //             System.Configuration.ConfigurationManager.AppSettings["baseUrl"] + url,
        //             state ?? ""));
        //}

        /// <summary>
        /// 获得跳转的Url重新获取code（一般用于没有成果获得code，再次跳转获得code）
        /// </summary>
        /// <param name="data">控制器的字典</param>
        /// <param name="state">需要传递的参数</param>
        /// <returns></returns>
        public static string GetBackUrlForCode(System.Web.Routing.RouteValueDictionary data, string state)
        {
            string url = "/" + data["controller"] + "/" + data["action"];
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri=P{1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect",
                     System.Configuration.ConfigurationManager.AppSettings["appid"],
                     System.Configuration.ConfigurationManager.AppSettings["baseUrl"] + url,
                     state ?? "");
        }

        /// <summary>
        /// 获取用户信息(确保数据库中以及存在u包含openid的user记录！)
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
        /// 以 HTTP 的 POST 提交方式 发送短信(ASP.NET的网页或是C#的窗体，均可使用该方法)
        /// </summary>
        /// <param name="mobile">要发送的手机号码</param>
        /// <param name="msg">要发送的内容</param>
        /// <returns>发送结果</returns>
        public static bool SenShortdMsg(string mobile, string msg)
        {
            string name = System.Configuration.ConfigurationManager.AppSettings["smgname"];
            string pwd = System.Configuration.ConfigurationManager.AppSettings["smgpwd"];//登陆web平台 http://sms.1xinxi.cn  在管理中心--基本资料--接口密码（28位） 如登陆密码修改，接口密码会发生改变，请及时修改程序
            string sign = System.Configuration.ConfigurationManager.AppSettings["smgsign"];             //一般为企业简称
            StringBuilder arge = new StringBuilder();

            arge.AppendFormat("name={0}", name);
            arge.AppendFormat("&pwd={0}", pwd);
            arge.AppendFormat("&content={0}", msg);
            arge.AppendFormat("&mobile={0}", mobile);
            arge.AppendFormat("&sign={0}", sign);
            arge.Append("&type=pt");
            string weburl = "http://sms.1xinxi.cn/asmx/smsservice.aspx";

            //string resp = PushToWeb(weburl, , Encoding.UTF8);
            byte[] byteArray = Encoding.UTF8.GetBytes(arge.ToString());

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(weburl));
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            Stream newStream = webRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();

            //接收返回信息：
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            StreamReader aspx = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string resp = aspx.ReadToEnd();

            if (resp.Split(',')[0] == "0")
            {
                //提交成功
                return true;
            }
            else
            {
                //提交失败，可能余额不足，或者敏感词汇等等
                return false;
            }

            //TODO: 更具文档区分返回内容
            //return resp;//是一串 以逗号隔开的字符串。阅读文档查看响应的意思
        }

    }

}
