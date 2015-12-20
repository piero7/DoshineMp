using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    public class UserInfo
    {
        [Key]
        public int UserInfoId { get; set; }

        //public int? WechatUserId { get; set; }

        //[ForeignKey("WechatUserId")]
        //public virtual WechatUser WechatUser { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreateDate { get; set; }

        public string Remarks { get; set; }

        public int? LastLoginTerminalId { get; set; }

        public DateTime LastLoginTime { get; set; }

        [ForeignKey("LastLoginTerminalId")]
        public virtual Terminal LastLoginTerminal { get; set; }

    }

}
