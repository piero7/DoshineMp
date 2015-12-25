using DoShineMP.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        #endregion

        #region 页面视图

        /// <summary>
        /// 个人注册
        /// </summary>
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
                    ViewBag.openid = this.openid;
                }
                else
                {
                    Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Register", ""));
                }
            }
            ViewBag.Title = "桑田账号-注册";
            return View();
        }


        /// <summary>
        /// 经销商注册
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonalCenter(string code)
        {
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
            catch (Exception e)
            {
                throw;
            }

            ViewBag.Title = "经销商注册";
            ViewBag.openid = this.openid;
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
            catch (Exception e)
            {
                throw;
            }

            //提取我的资料
            ViewBag.Title = "我的主页";
            return View();
        }


        /// <summary>
        /// 报修
        /// </summary>
        /// <returns></returns>
        public ActionResult Repair(string code)
        {
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    if (!string.IsNullOrEmpty(CodeJjudgeByOpenid(code)))
                    {
                        ViewBag.openid = this.openid;
                        //历史保修记录
                        ViewBag.RepairList = repairHelper.GetHistoryRepair(this.openid);
                    }
                    else
                    {
                        Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Register", ""));
                    }
                }
                else
                {
                    Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Repair", ""));
                }
            }
            catch (Exception e)
            {
                throw;
            }
            ViewBag.Title = "报修";
            return View();
        }


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
            ViewBag.Title = "公司信息";
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
        /// 在线留言
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult Messages(string code)
        {
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

        #region JsonResult功能块组

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
            catch (Exception e)
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
        public JsonResult CenterUpdateJson(string RealName, string PhoneNumber, string code)
        {
            try
            {
                if (!(string.IsNullOrEmpty(RealName) && string.IsNullOrEmpty(PhoneNumber)))
                {
                    //逻辑代码
                    if (wuser.EditUserInfo(WechatHelper.GetOpenidByCode(code), RealName, PhoneNumber) != null)
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
            catch (Exception e)
            {
                return Json(new { msg = "N" });
            }
        }


        #endregion

        #region 合作伙伴


        /// <summary>
        /// 合作伙伴注册
        /// </summary>
        /// <param name="code"></param>
        /// <param name="comName"></param>
        /// <param name="Type"> Sub_contractor分包商，Supplier供应商 </param>
        /// <param name="realName"></param>
        /// <param name="Address"></param>
        /// <param name="comPhone"></param>
        /// <returns></returns>
        public JsonResult ReginPartnerJson(string code, string comName, string type, string realName, string Address, string comPhone)
        {
            try
            {
                DoShineMP.Models.PartnerType p = (DoShineMP.Models.PartnerType)Enum.Parse(typeof(DoShineMP.Models.PartnerType), type);


                if (partner.ReginPartner(code, comName, p, realName, Address, comPhone) != null)
                {
                    return Json(new { msg = "Y" });
                }
                else
                {
                    return Json(new { msg = "N" });
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
        public JsonResult EditPartnerInfoJson(string code, string comName, string type, string realName, string Address, string comPhone)
        {
            try
            {
                DoShineMP.Models.PartnerType p = (DoShineMP.Models.PartnerType)Enum.Parse(typeof(DoShineMP.Models.PartnerType), type);

                //openid = WechatHelper.GetOpenidByCode(code);
                //openid = "olQmIjjUTPHrAAAQc0aeJ5LRM3qw";
                if (partner.EditPartnerInfo(code, comName, p, realName, Address, comPhone) != null)
                {
                    return Json(new { msg = "Y" });
                }
                else
                {
                    return Json(new { msg = "N" });
                }
            }
            catch (Exception e)
            {
                return Json(new { msg = "N" });
            }
        }


        #endregion

        #region 杂项功能

        /// <summary>
        /// 添加保修记录返回数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public JsonResult RepairJson(string code, string content)
        {
            try
            {
                if (repairHelper.AddRepair(code, content) != null)
                {
                    return Json(repairHelper.GetHistoryRepair(code));
                }
                else
                {
                    return Json(new { msg = "N" });
                }
            }
            catch (Exception e)
            {
                return Json(new { msg = "N" });
            }
        }



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
            catch (Exception e)
            {
                return Json(new { msg = "N" });
            }
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
    }
}