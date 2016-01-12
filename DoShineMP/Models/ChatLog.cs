using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    public class ChatLog
    {
        [Key]
        public int ChatLogId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Openid { get; set; }

        /// <summary>
        /// 标识是否已读
        /// </summary>
        public bool HasReaded { get; set; }

        /// <summary>
        /// 持续时间（不记录在数据库中）
        /// </summary>
        [NotMapped]
        public TimeSpan? LastTime
        {
            get
            {
                if (this.StartDate == null)
                {
                    return null;
                }
                if (this.EndDate == null)
                {
                    return null;
                }
                return EndDate.Value.Subtract(this.StartDate.Value);
            }
        }

        public ChatStatus Status { get; set; }

        /// <summary>
        /// 记录用户信息，主要用于客服离线。格式为：名称1: 内容1;名称2 eg:手机号|13600000000;微信号|wechatnumber;
        /// </summary>
        public string UserInfo { get; set; }

        /// <summary>
        /// 记录用户信息，key为信息名称，value为信息具体内容 eg：eg:手机号|13600000000;微信号|wechatnumber
        /// </summary>
        [NotMapped]
        public Dictionary<string, string> UserInfoDic
        {
            get
            {
                var ret = new Dictionary<string, string>();
                if (string.IsNullOrEmpty(UserInfo))
                {
                    return ret;
                }
                var tmpList = this.UserInfo.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var str in tmpList)
                {
                    var tmp = str.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length == 2)
                    {
                        ret.Add(tmp[0], tmp[1]);
                    }
                }
                return ret;
            }
            set
            {
                if (value == null)
                {
                    this.UserInfo = "";
                }

                else if (value != this.UserInfoDic)
                {
                    var res = "";
                    foreach (var item in value)
                    {
                        res += item.Key + '|' + item.Value + ';';
                    }
                    this.UserInfo = res;
                }
            }
        }

    }

    /// <summary>
    /// 聊天状态
    /// </summary>
    public enum ChatStatus
    {
        Unknown = 0,
        Chatting = 1,
        /// <summary>
        /// 正常结束
        /// </summary>
        Finish = 10,
        /// <summary>
        /// 客服离线
        /// </summary>
        Leave = 20,
    }
}
