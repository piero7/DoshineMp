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
            ViewBag.code = code;
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
                    openid = WechatHelper.GetOpenidByCode(code);
                    ViewBag.openid = openid;
                    //openid = "olQmIjjUTPHrAAAQc0aeJ5LRM3qw";
                    if (WechatHelper.CheckOpenid(openid) != null)
                    {
                        var user = wuser.GetUserInfo(openid);
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

            ViewBag.Title = "个人中心";
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
                    openid = WechatHelper.GetOpenidByCode(code);
                    //openid = "olQmIjjUTPHrAAAQc0aeJ5LRM3qw";

                    if (WechatHelper.CheckOpenid(openid) != null)
                    {
                        ViewBag.User = wuser.GetUserInfo(openid);
                    }
                    else
                    {
                        Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Register", ""));
                        //Response.Redirect(Url.Action("Register", "PhoneWeb") + "?code=" + code);
                        //WechatHelper.BackForCode("PhoneWeb", "Register", "");
                    }
                }
                else
                {
                    Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "HomePage", ""));
                    //WechatHelper.BackForCode("PhoneWeb", "HomePage", "");
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
                    openid = WechatHelper.GetOpenidByCode(code);
                    ViewBag.openid = openid;

                    if (WechatHelper.CheckOpenid(openid) != null)
                    {
                        //历史保修记录
                        ViewBag.RepairList = repairHelper.GetHistoryRepair(openid);
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
            if (!string.IsNullOrEmpty(code))
            {
                openid = WechatHelper.GetOpenidByCode(code);
                //openid = "olQmIjjUTPHrAAAQc0aeJ5LRM3qw";
                var wuser = partner.GetPartnerInfo(openid);
                if (wuser == null)
                {
                    Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "PersonalCenter", ""));
                    //Response.Redirect(Url.Action("PersonalCenter", "PhoneWeb") + "?code=" + code);
                    //WechatHelper.BackForCode("PhoneWeb", "PerconalCenter", "");
                }
                else
                {
                    ViewBag.wuser = wuser;
                }
            }
            else
            {
                Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Partner", ""));
                //WechatHelper.BackForCode("PhoneWeb", "Partner", "");
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
            if (!string.IsNullOrEmpty(code))
            {
                openid = WechatHelper.GetOpenidByCode(code);
                //openid = "olQmIjjUTPHrAAAQc0aeJ5LRM3qw";
                var user = wuser.GetUserInfo(openid);
                if (user.UserInfo == null)
                {
                    Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Register", ""));
                }
                ViewBag.user = user;
            }
            else
            {
                Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "MyMessage", ""));
                //WechatHelper.BackForCode("PhoneWeb", "MyMessage", "");
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
                openid = WechatHelper.GetOpenidByCode(code);
                //openid = "olQmIjjUTPHrAAAQc0aeJ5LRM3qw";
                ViewBag.user = wuser.GetUserInfo(openid);
            }
            else
            {
                Response.Redirect(WechatHelper.BackForCode("PhoneWeb", "Messages", ""));
                //WechatHelper.BackForCode("PhoneWeb", "Messages", "");
            }
            ViewBag.Title = "即时交流";
            return View();


        }


        /// <summary>
        ///客服聊天系统客服版
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult ServerMessages(string code)
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
        public JsonResult RegisterJson(string RealName, string PhoneNumber, string code)
        {
            try
            {
                if (!string.IsNullOrEmpty(RealName) && !string.IsNullOrEmpty(PhoneNumber))
                {
                    //string openid = "olQmIjjUTPHrAAAQc0aeJ5LRM3qw";
                    string openid = WechatHelper.GetOpenidByCode(code);
                    //逻辑代码
                    if (wuser.Regiet(RealName, PhoneNumber, openid) != null)
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

                //openid = WechatHelper.GetOpenidByCode(code);
                //openid = "olQmIjjUTPHrAAAQc0aeJ5LRM3qw";
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
        /// 添加保修记录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public JsonResult RepairJson(string code, string content)
        {
            try
            {
                //openid = WechatHelper.GetOpenidByCode(code);
                //openid = "olQmIjjUTPHrAAAQc0aeJ5LRM3qw";
                if (repairHelper.AddRepair(code, content) != null)
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

        #endregion

        #region 公用模块


        #endregion
    }
}