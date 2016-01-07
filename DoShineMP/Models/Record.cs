using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    public class Record
    {
        [Key]
        public int RecordId { get; set; }

        public DateTime CreateDate { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Name { get; set; }

        public string Remarks { get; set; }

        public RecordType Type { get; set; }

        public int? UserInfoId { get; set; }

        [ForeignKey("UserInfoId")]
        public virtual UserInfo UserInfo { get; set; }
    }

    public enum RecordType
    {
        Unknown = 0,
        MpRepair = 1,
        TaobaoShop = 2,
        WechatShop = 3,
    }
}
