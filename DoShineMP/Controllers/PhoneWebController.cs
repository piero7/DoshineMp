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
        /// 注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Register(string code)
        {
            //ViewBag.openid = WechatHelper.GetOpenidByCode(code);
            ViewBag.Title = "桑田账号-注册";
            return View();
        }

        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonalCenter(string code)
        {
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    openid = WechatHelper.GetOpenidByCode(code);
                    var user = wuser.GetUserInfo(openid);
                    if (user.UserInfo == null)
                    {
                        WechatHelper.BackForCode("PhoneWeb", "Register", "");
                    }
                    else
                    {
                        ViewBag.user = user;
                    }
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
                    ViewBag.User = WechatHelper.CheckOpenid(openid);
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
        /// 保修
        /// </summary>
        /// <returns></returns>
        public ActionResult Repair(string code)
        {
            try
            {
                if (!string.IsNullOrEmpty(code))
                    //历史保修记录
                    ViewBag.RepairList = repairHelper.GetHistoryRepair(WechatHelper.GetOpenidByCode(code));
            }
            catch (Exception e)
            {
                throw;
            }
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
                    string openid = "sdjisjdijsdsijd";
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

                if (partner.ReginPartner(WechatHelper.GetOpenidByCode(code), comName, p, realName, Address, comPhone) != null)
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

                if (partner.ReginPartner(WechatHelper.GetOpenidByCode(code), comName, p, realName, Address, comPhone) != null)
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
                if (repairHelper.AddRepair(WechatHelper.GetOpenidByCode(code), content) != null)
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