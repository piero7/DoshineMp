using DoShineMP.Helper;
using DoShineMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoShineMP.Controllers
{
    public class MessageController : ApiController
    {
        /// <summary>
        /// 添加客户端消息记录
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="content">消息内容</param>
        /// <param name="isNew">是否为新连接</param>
        /// <param name="type">消息发送方类型</param>
        /// <param name="detailInfo">消息体完成内容</param>
        /// <returns></returns>
        [HttpGet]
        public Message AddCustomMessage(string openid, string content, bool isNew, MessageType type, string detailInfo)
        {
            MessageHelper mh = new MessageHelper();
            return mh.AddCustomMessage(openid, content, isNew, type, detailInfo);
        }


        /// <summary>
        /// 添加客服端消息记录
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="detailInfo">消息体完成内容</param>
        /// <param name="ipStr">操作端的详细地址字符串</param>
        /// <returns></returns>
        [HttpGet]
        public Message AddServerMessage(string content, string detailInfo, string ipStr)
        {
            MessageHelper mh = new MessageHelper();
            return mh.AddServerMessage(content, detailInfo, ipStr);
        }

    }
}