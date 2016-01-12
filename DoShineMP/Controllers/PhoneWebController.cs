using DoShineMP.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DoShineMP.Controllers
{
    public class PhoneWebController : Controller
    {

        #region 参数
        private WechatUserHelper wuser = new WechatUserHelper();
        private WechatHelper wh = new WechatHelper();
        private PartnerHelper partner = new PartnerHelper();
        private RepairHelper repairHelper = new RepairHelper();
        private IdentifyingCodeController identifyingcode = new IdentifyingCodeController();
        private string openid = string.Empty;
        private bool isWebDebug = ConfigurationManager.AppSettings["isWebDebug"] == "true" ? true : false;

        #endregion

        #region 页面视图

        #region 用户

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="code">微信code</param>
        /// <param name="type">用户或者经销商</param>
        /// <returns></returns>
        public ActionResult Register(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Register", ""));
            }
            else
            {
                ViewBag.code = code;
                this.openid = CodeJjudgeByOpenid(code);
                if (!string.IsNullOrEmpty(this.openid))
                {
                    if (wuser.GetUserInfo(this.openid).UserInfo != null)
                    {
                        //Response.Write("<script language='javascript'>window.location.replace('" + WechatHelper.BackForCode("PhoneWeb", "MyMessage", "") + "')</script>");
                        //Server.Transfer(WechatHelper.BackForCode("PhoneWeb", "MyMessage", ""), true);
                        Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "MyMessage", ""));
                    }
                    else
                    {
                        ViewBag.openid = this.openid;
                    }
                }
                else
                {
                    Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Register", ""));
                }
            }
            ViewBag.code = code;
            ViewBag.urltype = string.IsNullOrEmpty(url.urltype) ? "MyMessage" : url.urltype;
            ViewBag.Title = "桑田账号-注册";

            return View();
        }

        /// <summary>
        /// 个人详细信息页面
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult MyMessage(string code)
        {
            Models.WechatUser user = new Models.WechatUser();
            url.urltype = "MyMessage";
            if (!string.IsNullOrEmpty(code))
            {
                if (!string.IsNullOrEmpty(CodeJjudgeByOpenid(code)))
                {
                    user = wuser.GetUserInfo(CodeJjudgeByOpenid(code));
                    if (user.UserInfo == null)
                    {
                        Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Register", ""));
                    }
                    ViewBag.user = user;
                }
                else
                {
                    Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "MyMessage", ""));
                }
            }
            else
            {
                Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "MyMessage", ""));
            }
            ViewBag.Title = "个人信息";
            return View();
        }

        /// <summary>
        /// 个人信息修改
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult UserUpdate(string code)
        {
            url.urltype = "MyMessage";
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    if (!string.IsNullOrEmpty(CodeJjudgeByOpenid(code)))
                    {
                        var user = wuser.GetUserInfo(this.openid);

                        if (user.UserInfo == null)
                        {
                            Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Register", ""));
                        }
                        else
                        {
                            ViewBag.user = user;
                        }
                    }
                    else
                    {
                        Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Register", ""));
                    }
                }
                else
                {
                    Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "UserUpdate", ""));
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            ViewBag.Title = "个人信息";
            ViewBag.openid = this.openid;
            return View();
        }

        #endregion

        #region 报修

        #region 用户

        /// <summary>
        /// 提交报修
        /// </summary>
        /// <returns></returns>
        public ActionResult Repair(string code)
        {
            //url.urltype = "Repair";
            //try
            //{
            //    if (!string.IsNullOrEmpty(code))
            //    {
            //        if (!string.IsNullOrEmpty(CodeJjudgeByOpenid(code)))
            //        {
            //            var user = wuser.GetUserInfo(this.openid);
            //            if (user.UserInfo != null)
            //            {
            //                ViewBag.user = user;
            //                ViewBag.openid = this.openid;
            //                //历史保修记录
            //                ViewBag.RepairList = repairHelper.GetHistoryRepair(this.openid);
            //            }
            //            else
            //            {
            //                Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Register", ""));
            //            }
            //        }
            //        else
            //        {
            //            Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Repair", ""));
            //        }
            //    }
            //    else
            //    {
            //        Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Repair", ""));
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            ViewBag.user = wuser.GetUserInfo("olQmIjjUTPHrAAAQc0aeJ5LRM3qw");
            ViewBag.openid = "olQmIjjUTPHrAAAQc0aeJ5LRM3qw";
            ViewBag.RepairList = repairHelper.GetHistoryRepair("olQmIjjUTPHrAAAQc0aeJ5LRM3qw");
            ViewBag.Title = "自助报修";
            return View();
        }

        /// <summary>
        /// 用户报修历史
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult RepairHistory(string code)
        {
            ViewBag.Title = "保修历史";
            return View();
        }

        /// <summary>
        /// 报修详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult RepairDetails(string repairid)
        {
            int a;
            if (int.TryParse(repairid, out a))
            {
                var repairdetail = repairHelper.GetDetail(a);
                if (repairdetail != null)
                {
                    if (repairdetail.Image != null)
                    {
                        repairdetail.Image.FileName = System.Configuration.ConfigurationManager.AppSettings["httpimgpath"] + repairdetail.Image.FileName;
                    }
                    ViewBag.RepairDetail = repairdetail;
                }
                else
                {
                    Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Repair", ""));
                }
            }

            ViewBag.Title = "报修详情";
            return View();
        }

        #endregion

        #region 维保

        /// <summary>
        /// 报修受理
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult RepairInterior()
        {
            ViewBag.RepairList5 = repairHelper.GetHistoryRepair(Models.RepairStatus.Apply, 10, 0).ToList();
            ViewBag.RepairList10 = repairHelper.GetHistoryRepair(Models.RepairStatus.Accept, 10, 0).ToList();
            ViewBag.RepairList20 = repairHelper.GetHistoryRepair(Models.RepairStatus.FinishHandle, 10, 0).ToList();
            ViewBag.RepairList99 = repairHelper.GetHistoryRepair(Models.RepairStatus.Finish, 10, 0).ToList();
            ViewBag.Title = "报修受理";
            return View();
        }

        /// <summary>
        /// 报修受理详情
        /// </summary>
        /// <param name="repairid"></param>
        /// <returns></returns>
        public ActionResult RepairDetailsInterior(string repairid)
        {
            int a;
            if (int.TryParse(repairid, out a))
            {
                var repairdetail = repairHelper.GetDetail(a);
                if (repairdetail != null)
                {
                    if (repairdetail.Image != null)
                    {
                        repairdetail.Image.FileName = System.Configuration.ConfigurationManager.AppSettings["httpimgpath"] + repairdetail.Image.FileName;
                    }
                    ViewBag.RepairDetail = repairdetail;
                }
                else
                {
                    Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "RepairInterior", ""));
                }
            }

            ViewBag.Title = "报修详情";
            return View();
        }

        #endregion

        #endregion

        #region 客服

        /// <summary>
        /// 在线留言
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult Messages(string code)
        {
            url.urltype = "Messages";
            if (!string.IsNullOrEmpty(code))
            {
                if (!string.IsNullOrEmpty(CodeJjudgeByOpenid(code)))
                {
                    var uuu = wuser.GetUserInfo(this.openid);
                    if (uuu.UserInfo != null)
                    {
                        ViewBag.user = wuser.GetUserInfo(this.openid);
                    }
                    else
                    {
                        Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Register", ""));
                    }
                }
                else
                {
                    Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Messages", ""));
                }
            }
            else
            {
                Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Messages", ""));
            }
            ViewBag.Title = "在线客服";
            return View();
        }

        /// <summary>
        ///客服聊天系统客服版
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult ServerMessages()
        {
            ViewBag.Title = "客服系统";
            return View();
        }

        #endregion

        #region 经销商供应商功能

        /// <summary>
        /// 经销商中心
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult Partner(string code)
        {
            Models.WechatUser user = new Models.WechatUser();
            if (!string.IsNullOrEmpty(code))
            {
                if (!string.IsNullOrEmpty(CodeJjudgeByOpenid(code)))
                {
                    user = partner.GetPartnerInfo(this.openid);

                    if (user == null)
                    {
                        Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "PersonalCenter", ""));
                    }
                    else
                    {
                        ViewBag.wuser = user;
                    }
                }
                else
                {
                    Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Partner", ""));
                }
            }
            else
            {
                Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Partner", ""));
            }
            ViewBag.Title = "经销商中心";
            return View();
        }

        /// <summary>
        /// 经销商信息修改
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonalCenter(string code, string type)
        {
            url.urltype = "PersonalCenter";
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    if (!string.IsNullOrEmpty(CodeJjudgeByOpenid(code)))
                    {
                        var user = wuser.GetUserInfo(this.openid);

                        if (user.UserInfo == null)
                        {
                            Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Register", ""));
                        }
                        else
                        {
                            ViewBag.user = user;
                            ViewBag.parnter = partner.GetPartnerInfo(openid);
                        }
                    }
                    else
                    {
                        Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Register", ""));
                    }
                }
                else
                {
                    Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "PersonalCenter", ""));
                }
            }
            catch (Exception)
            {
                throw;
            }

            ViewBag.Title = "经销商注册";
            if (ViewBag.parnter != null)
            {
                ViewBag.Title = "经销商修改";
            }
            ViewBag.openid = this.openid;
            return View();
        }

        /// <summary>
        /// 供应商
        /// </summary>
        /// <returns></returns> 
        public ActionResult Supplier()
        {
            ViewBag.Salesman = SalesmanHelper.GetAllSalesman();//获取所有销售
            ViewBag.AllDistrict = partner.GetAllDistrict();//获取所有地区
            return View();
        }

        /// <summary>
        /// 经销商
        /// </summary>
        /// <returns></returns>
        public ActionResult Distributor()
        {
            ViewBag.Salesman = SalesmanHelper.GetAllSalesman();//获取所有销售
            ViewBag.AllDistrict = partner.GetAllDistrict();//获取所有地区

            return View();
        }


        #endregion

        #endregion

        #region JsonResult功能块组

        #region 微信js配置参数

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult WechatJsConfigJson(string url)
        {
            return Json(WechatHelper.GetWechatJsConfig(url));
        }

        #endregion

        #region 个人信息
        /// <summary>
        /// 个人信息注册
        /// </summary>
        /// <param name="RealName">姓名</param>
        /// <param name="PhoneNumber">手机号</param>
        /// <param name="code">微信CODE</param>
        /// <returns>Y：修改成功；N：修改失败</returns>
        public JsonResult RegisterJson(string RealName, string PhoneNumber, string code, int sendid, string sendcode)
        {
            try
            {
                if (!string.IsNullOrEmpty(PhoneNumber))
                {
                    //判断手机验证码
                    if (!IdentifyingCodeHelper.CheckCode(sendid, sendcode, code, PhoneNumber))
                    {
                        return Json(new { msg = "验证码输入错误" });
                    }
                    else if (!string.IsNullOrEmpty(code) && wuser.Regiet(RealName, PhoneNumber, code) != null)
                    {
                        return Json(new { msg = "Y" });
                    }
                    else
                    {
                        return Json(new { msg = "N" });
                    }
                }
                else
                {
                    return Json(new { msg = "N" });
                }
            }
            catch (Exception)
            {
                return Json(new { msg = "N" });
            }
        }


        /// <summary>
        /// 个人信息修改
        /// </summary>
        /// <param name="RealName">姓名</param>
        /// <param name="PhoneNumber">手机号</param>
        /// <param name="code">微信code</param>
        /// <returns>Y：修改成功；N：修改失败</returns>
        public JsonResult CenterUpdateJson(string RealName, string PhoneNumber, string code, string address)
        {
            try
            {
                if (!(string.IsNullOrEmpty(RealName) && string.IsNullOrEmpty(PhoneNumber)))
                {
                    //逻辑代码
                    //if (wuser.EditUserInfo(WechatHelper.GetOpenidByCode(code), RealName, PhoneNumber, address) != null)
                    if (wuser.EditUserInfo(code, RealName, PhoneNumber, address) != null)
                    {
                        return Json(new { msg = "Y" });
                    }
                    else
                    {
                        return Json(new { msg = "N" });
                    }
                }
                else
                {
                    return Json(new { msg = "N" });
                }
            }
            catch (Exception)
            {
                return Json(new { msg = "N" });
            }
        }


        #endregion

        #region 合作伙伴


        /// <summary>
        /// 合作伙伴注册
        /// </summary>
        /// <param name="code">openid</param>
        /// <param name="comName">姓名</param>
        /// <param name="Type"> Sub_contractor分包商，Supplier供应商 </param>
        /// <param name="realName"></param>
        /// <param name="Address"></param>
        /// <param name="comPhone"></param>
        /// <returns></returns>
        public JsonResult ReginPartnerJson(string code, string comName, string type, string realName, string Address, string comPhone, int salesmanId, string eamil, string files, int discrictid)
        {
            try
            {
                DoShineMP.Models.PartnerType p = (DoShineMP.Models.PartnerType)Enum.Parse(typeof(DoShineMP.Models.PartnerType), type);

                if (salesmanId == 0)
                {
                    if (partner.ReginPartner(code, comName, p, realName, Address, comPhone, null, eamil, files, null) != null)
                    {
                        return Json(new { msg = "Y" });
                    }
                    else
                    {
                        return Json(new { msg = "N" });
                    }

                }
                else
                {
                if (partner.ReginPartner(code, comName, p, realName, Address, comPhone, salesmanId, eamil, files, discrictid) != null)
                {
                    return Json(new { msg = "Y" });
                }
                else
                {
                    return Json(new { msg = "N" });
                }
                }



            }
            catch (Exception e)
            {
                return Json(new { msg = "N" });
            }
        }



        /// <summary>
        /// 合作伙伴修改
        /// </summary>
        /// <param name="code"></param>
        /// <param name="comName"></param>
        /// <param name="Type"> Sub_contractor分包商，Supplier供应商 </param>
        /// <param name="realName"></param>
        /// <param name="Address"></param>
        /// <param name="comPhone"></param>
        /// <returns></returns>
        public JsonResult EditPartnerInfoJson(string code, string comName, string type, string realName, string Address, string comPhone, string email)
        {
            try
            {
                if (partner.EditPartnerInfo(code, comName, realName, Address, comPhone, email) != null)
                {
                    return Json(new { msg = "Y" });
                }
                else
                {
                    return Json(new { msg = "N" });
                }
            }
            catch (Exception)
            {
                return Json(new { msg = "N" });
            }
        }




        #endregion

        #region 报修功能

        #region 用户
        /// <summary>
        /// 添加保修记录返回数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public JsonResult RepairJson(string code, string content, string mediaid, string address, string phone, int villageid, string name)
        {
            try
            {
                var user = wuser.GetUserInfo(code);
                //user.UserInfo.Address = address;
                //wuser.EditUserInfo(code, user.UserInfo.Name, user.UserInfo.PhoneNumber, address);

                //TODO: 现在的文件为mediaid的列表，用逗号分割！
                if (repairHelper.Add(code, content, mediaid, phone, villageid, name) != null)
                {
                    return Json(repairHelper.GetHistoryRepair(code));
                }
                else
                {
                    return Json(new { msg = "N" });
                }
            }
            catch (Exception)
            {
                return Json(new { msg = "N" });
            }
        }

        /// <summary>
        /// 反馈报修评价
        /// </summary>
        /// <param name="repairID">报修id</param>
        /// <param name="response">评价内容</param>
        /// <param name="score">分数（预留，若无则填0）</param>
        /// <returns></returns>
        public JsonResult RepairsponJson(int repairID, string response, int score)
        {
            try
            {
                if (repairHelper.Response(repairID, response, score) != null)
                {
                    return Json(new { msg = "Y" });
                }
                else
                {
                    return Json(new { msg = "N" });
                }
            }
            catch (Exception)
            {
                return Json(new { msg = "N" });
            }
        }

        #endregion

        #region 内部人员

        /// <summary>
        /// 保修受理
        /// </summary>
        /// <returns></returns>
        public JsonResult RepairDetailJson(int repaidID, string exceptDate, string innderNumber, string type)
        {
            string msg = "Y";
            try
            {

                switch (type)
                {
                    case "1":
                        DateTime date;
                        DateTime.TryParse(exceptDate, out date);
                        msg = repairHelper.Accept(repaidID, date, innderNumber) != null ? "Y" : "N";
                        ; break;
                    case "2":
                        //msg = repairHelper.FinishHandlen(repaidID) != null ? "Y" : "N";
                        msg = "Y";


                        ; break;
                }
                return Json(new { msg = msg });
            }
            catch (Exception)
            {
                return Json(new { msg = msg });
            }
        }

        #endregion

        #region 地理位置

        /// <summary>
        /// 获取所有地理位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public JsonResult GetAllVillage(double x, double y)
        {
            return Json(repairHelper.GetAllVillage(x, y));
        }

        #endregion

        #endregion

        #region 杂项功能 --短信

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public JsonResult Send(string openid, string PhoneNumber)
        {
            try
            {
                int sendid = identifyingcode.GetIndentifyingCode(openid, PhoneNumber);
                if (sendid == 0)
                {
                    return Json(new { msg = "N" });
                }
                else
                {
                    return Json(new { sendid = sendid });
                }
            }
            catch (Exception)
            {
                return Json(new { msg = "N" });
            }
        }

        #endregion

        #region 跨域调用

        /// <summary>
        /// 调用其它域api，解决跨域问题
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public JsonResult ApiJsonAll(string url)
        {
            HttpClient client = new HttpClient();
            var cl = client.GetStringAsync(url).Result;
            return Json(new { msg = cl });
        }

        #endregion

        #endregion

        #region 公用模块

        /// <summary>
        /// 判断是否存在openid缓存，不存在则根据code重新获取一次openid
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string CodeJjudgeByOpenid(string code)
        {
            if (!string.IsNullOrEmpty(this.openid))
            {
                return this.openid;
            }
            else
            {
                return this.openid = WechatHelper.GetOpenidByCode(code);
            }
        }


        #endregion

        #region 测试页面

        /// <summary>
        /// ceshi
        /// </summary>
        /// <returns></returns>
        public ActionResult ceshi()
        {
            try
            {
                //ViewBag.RepairList = repairHelper.GetHistoryRepair("olQmIjjUTPHrAAAQc0aeJ5LRM3qw");
                // ViewBag.open = (dynamic)WechatHelper.GetWechatJsConfig(Request.Url.ToString());
            }
            catch (Exception e)
            {

                throw;
            }

            return View();
        }

        /// <summary>
        /// 我的主页
        /// </summary>
        /// <returns></returns>
        public ActionResult HomePage(string code)
        {
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    if (!string.IsNullOrEmpty(CodeJjudgeByOpenid(code)))
                    {
                        ViewBag.User = wuser.GetUserInfo(this.openid);
                    }
                    else
                    {
                        Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Register", ""));
                    }
                }
                else
                {
                    Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "HomePage", ""));
                }
            }
            catch (Exception)
            {
                throw;
            }

            //提取我的资料
            ViewBag.Title = "我的主页";
            return View();
        }


        #endregion

    }

    #region 临时变量

    static class url
    {
        public static string urltype { get; set; }
    }

    #endregion
}