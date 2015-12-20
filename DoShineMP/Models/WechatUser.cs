using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoShineMP.Models
{
    public class WechatUser
    {
        [Key]

        public int WechatUserId { get; set; }

        public string OpenId { get; set; }

        public string NickName { get; set; }

        public bool subscribe { get; set; }

        [EnumDataType(typeof(Sex))]
        public Sex Sex { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Province { get; set; }

        public string Language { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? SubscribeTime { get; set; }

        public string Headimgurl { get; set; }

        public string Remarks { get; set; }

        public int? UserInfoId { get; set; }

        [ForeignKey("UserInfoId")]
        public UserInfo UserInfo { get; set; }

        public int? PartnerId { get; set; }

        [ForeignKey("PartnerId")]
        public virtual Partner PartnerInfo { get; set; }


    }

    public enum Sex
    {
        Unknown = 0,
        Man = 1,
        Female,
    }
}