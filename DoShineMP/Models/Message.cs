using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        public DateTime CreateDate { get; set; }

        public string Content { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserInfo User { get; set; }

        public MessageType Type { get; set; }

        public int? TerminalId { get; set; }

        public virtual Terminal Terminal { get; set; }

        /// <summary>
        /// 是否收到
        /// </summary>
        public bool IsRescive { get; set; }

        public string IsReply { get; set; }

        public int? ReplyMessageId { get; set; }

        public Message()
        {
            this.CreateDate = DateTime.Now;
        }
    }

    public enum MessageType
    {
        Unknown = 0,
        Customer = 1,
        Partner = 2,
        Service = 3,
    }
}
