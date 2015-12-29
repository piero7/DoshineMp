// <copyright file="WechatImageHelperTest.cs">版权所有(C)  2015</copyright>
using System;
using System.Threading.Tasks;
using DoShineMP.Helper;
using DoShineMP.Models;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoShineMP.Helper.Tests
{
    /// <summary>此类包含 WechatImageHelper 的参数化单元测试</summary>
    [PexClass(typeof(WechatImageHelper))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class WechatImageHelperTest
    {
        /// <summary>测试 AddNewImageForRepair(String, Int32, String) 的存根</summary>
        [PexMethod]
        internal Task<ImageDownloadLog> AddNewImageForRepairTest(
            string mediaid,
            int repairid,
            string openid
        )
        {
            Task<ImageDownloadLog> result
               = WechatImageHelper.AddNewImageForRepair(mediaid, repairid, openid);
            return result;
            // 将断言添加到 方法 WechatImageHelperTest.AddNewImageForRepairTest(String, Int32, String)
        }
    }
}
