using DoShineMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoShineMP.Helper
{
    public class MessageHelper
    {
        public Message AddCustomMessage(string openid, string content, bool isNew, MessageType type, string detailInfo)
        {
            var wuser = WechatHelper.CheckOpenid(openid);

            return null;
        }
    }
}
