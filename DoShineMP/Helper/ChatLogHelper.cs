using DoShineMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Helper
{
    class ChatLogHelper
    {
        /// <summary>
        /// 开始新的会话
        /// </summary>
        /// <param name="status"></param>
        /// <param name="userinfoDic">记录用户信息，key为信息名称，value为信息具体内容 eg：eg:手机号:13600000000;微信号:wechatnumber</param>
        /// <returns></returns>
        public static ChatLog AddNewLog(ChatStatus status, Dictionary<string, string> userinfoDic)
        {
            var cl = new ChatLog
            {
                StartDate = DateTime.Now,
                Status = status,
                HasReaded = status == ChatStatus.Chatting ? true : false,
                UserInfoDic = userinfoDic,
            };
            var db = new ModelContext();
            db.ChatLogSet.Add(cl);
            db.SaveChanges();
            return cl;
        }


        /// <summary>
        /// 将聊天记录设为已读
        /// </summary>
        /// <param name="logId"></param>
        /// <returns></returns>
        public static ChatLog SetRead(int logId)
        {
            var db = new ModelContext();
            var cl = db.ChatLogSet.FirstOrDefault(item => item.ChatLogId == logId);
            if (cl == null)
            {
                return null;
            }
            cl.HasReaded = true;
            cl.EndDate = DateTime.Now;

            db.SaveChanges();

            return cl;
        }


        /// <summary>
        /// 获得所有的未读记录
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ChatLog> GetAllNotReadedLog()
        {
            var db = new ModelContext();
            return (from cl in db.ChatLogSet
                    where !cl.HasReaded
                    select cl).ToList();
        }
    }
}