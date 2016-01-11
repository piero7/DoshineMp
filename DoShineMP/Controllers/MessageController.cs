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

        /// <summary>
        /// 获取是所有的未读记录
        /// </summary>
        [HttpGet]
        public IEnumerable<ChatLog> GetAllNotReadedLog()
        {
            return ChatLogHelper.GetAllNotReadedLog();
        }


        /// <summary>
        /// 将一条设置为已读
        /// </summary>
        /// <param name="logId"></param>
        /// <returns></returns>
        [HttpGet]
        public ChatLog SetRead(int logId)
        {
            return ChatLogHelper.SetRead(logId);
        }

        /// <summary>
        /// 更改客服状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public CustomServer SetServerStatus(CustomServerStatus status)
        {
            var serName = System.Configuration.ConfigurationManager.AppSettings["customsservername"];
            return CustomServerHelper.SetServerStatus(serName, status);
        }

        /// <summary>
        /// 添加离线记录
        /// </summary>
        /// <param name="openid">用户openid</param>
        /// <param name="userinfo">用户信息记录 格式为字段名:内容 eg： 手机号:13699844553;邮件xxx@163.com</param>
        /// <returns></returns>
        [HttpGet]
        public ChatLog AddLeaveChatLog(string openid, string userinfo)
        {
            var infoarr = userinfo.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> userInfoDic = new Dictionary<string, string>();
            foreach (var item in infoarr)
            {
                var tmp = item.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (tmp.Length == 2)
                {
                    userInfoDic.Add(tmp[0], tmp[1]);
                }
            }

            return ChatLogHelper.AddOfflineChatLog(openid, userInfoDic);
        }


    }
}