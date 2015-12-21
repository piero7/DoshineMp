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
        /// <summary>
        /// 添加客户端消息记录
        /// </summary>
        /// <param name="openid">openid</param>
        /// <param name="content">消息内容</param>
        /// <param name="isNew">是否为新连接</param>
        /// <param name="type">消息发送方类型</param>
        /// <param name="detailInfo">消息体完成内容</param>
        /// <returns></returns>
        public Message AddCustomMessage(string openid, string content, bool isNew, MessageType type, string detailInfo)
        {
            var wuser = WechatHelper.CheckOpenid(openid);
            wuser = WechatHelper.CheckUser(wuser);
            if (wuser == null || wuser.UserInfoId == null)
            {
                return null;
            }
            //var tmnName = type == MessageType.Service ? System.Configuration.ConfigurationManager.AppSettings["ServerService"] : System.Configuration.ConfigurationManager.AppSettings["DoshineWechatService"];
            var db = new ModelContext();
            var tmn = db.TerminalSet.FirstOrDefault(item => item.Name == System.Configuration.ConfigurationManager.AppSettings["DoshineWechatService"]);
            var msg = new Message
            {
                Content = content,
                CreateDate = DateTime.Now,
                Type = type,
                UserId = wuser.UserInfoId,
                TerminalId = tmn.TerminalId,
            };
            db.MessageSet.Add(msg);
            db.SaveChanges();
            if (isNew)
            {
                LogHelper.AddLog("send a message", msg.MessageId.ToString(), openid);
            }

            return null;
        }

        /// <summary>
        /// 添加客服端消息记录
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="detailInfo">消息体完成内容</param>
        /// <param name="ipStr">操作端的详细地址字符串</param>
        /// <returns></returns>
        public Message AddServerMessage(string content, string detailInfo, string ipStr)
        {
            var db = new ModelContext();
            var tmn = db.TerminalSet.FirstOrDefault(item => item.Name == System.Configuration.ConfigurationManager.AppSettings["ServerService"]);

            var msg = new Message
            {
                Content = content,
                CreateDate = DateTime.Now,
                TerminalId = tmn.TerminalId,
                Type = MessageType.Customer,
                IpStr = ipStr,
            };

            db.SaveChanges();
            return msg;
        }
    }
}
