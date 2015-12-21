using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoShineMP.Controllers
{
    public class PhoneWebController : Controller
    {
        #region 参数


        private WechatUserController wuser = new WechatUserController();
        private PartnerController partner = new PartnerController();
        private string openid = string.Empty;

        #endregion

        #region 页面视图

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            ViewBag.Title = "注册";
            return View();
        }

        /// <summary>
        /// 个人中心
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonalCenter(string code)
        {

            openid = WechatHelper.GetOpenidByCode(code);
            //提取我的资料
            ViewBag.Title = "个人中心";
            return View();
        }



        /// <summary>
        /// 我的主页
        /// </summary>
        /// <returns></returns>
        public ActionResult HomePage(string code)
        {
            openid = WechatHelper.GetOpenidByCode(code);
            //提取我的资料
            ViewBag.Title = "我的主页";
            return View();
        }



        #endregion

        #region JsonResult功能块组

        /// <summary>
        /// 个人注册
        /// </summary>
        /// <param name="RealName">姓名</param>
        /// <param name="PhoneNumber"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public JsonResult RegisterJson(string RealName, string PhoneNumber, string code)
        {
            try
            {
                if (!(string.IsNullOrEmpty(RealName) && string.IsNullOrEmpty(PhoneNumber)))
                {
                    //逻辑代码
                    if (wuser.Regiet(RealName, PhoneNumber, WechatHelper.GetOpenidByCode(code)) != null)
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
        /// 个人中心更新
        /// </summary>
        /// <returns></returns>
        public JsonResult CenterUpdateJson(string code, string Phone, string Pwd)
        {
            try
            {
                if (!(string.IsNullOrEmpty(Phone) && string.IsNullOrEmpty(Pwd)))
                {
                    //逻辑代码

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


        #endregion

        #region 公用模块


        #endregion
    }
}