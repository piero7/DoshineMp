using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    public class Partner
    {
        [Key]
        public int PartnerId { get; set; }

        public string CompanyName { get; set; }

        /// <summary>
        /// 相关文件列表 文件id:文件内容; eg:  1:经营许可证;
        /// </summary>
        string FileList { get; set; }

        public DateTime CreateDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string RealName { get; set; }

        public string CompanyPhone { get; set; }

        public string Email { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserInfo User { get; set; }

        public int Point { get; set; }

        public PartnerType Type { get; set; }

        //public int? WechatUserId { get; set; }

        //[ForeignKey("WechatUserId")]
        //public virtual WechatUser WechatUser { get; set; }

        public int? SalesmanId { get; set; }

        [ForeignKey("SalesmanId")]
        public virtual Salesman Salesman { get; set; }

        public PartnerStatus Status { get; set; }

        /// <summary>
        /// 确认时间 
        /// </summary>
        public DateTime? AcceptDate { get; set; }

        /// <summary>
        /// 取消时间
        /// </summary>
        public DateTime? CancelDate { get; set; }

        public Partner()
        {
            this.CreateDate = DateTime.Now;
            this.Point = 0;
        }

        public int? DistrictId { get; set; }

        public virtual District District { get; set; }

        public Sex Sex { get; set; }

        public string Money { get; set; }

        [NotMapped]
        public Dictionary<int, string> Files
        {
            get
            {
                if (string.IsNullOrEmpty(this.FileList))
                {
                    return new Dictionary<int, string>();
                }
                var tmpStrs = this.FileList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var ret = new Dictionary<int, string>();
                foreach (var item in tmpStrs)
                {
                    var tmp = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length != 2)
                    {
                        continue;
                    }
                    ret.Add(Convert.ToInt32(tmp[1]), tmp[0]);
                }

                return ret;
            }
            set
            {
                if (value == null || value.Count == 0)
                {
                    this.FileList = "";
                    return;
                }
                this.FileList = "";
                foreach (KeyValuePair<int, string> item in value)
                {
                    this.FileList += item.Value + ":" + item.Key + ";";
                }
            }

        }
    }

    public enum PartnerType
    {
        Unknown = 0,
        /// <summary>
        /// 分包商
        /// </summary>
        Sub_contractor = 1,
        /// <summary>
        /// 供应商
        /// </summary>
        Supplier = 2,
        /// <summary>
        /// 经销商
        /// </summary>
        Dealer = 3,
        Both = 9,
    }

    /// <summary>
    /// 合作伙伴状态
    /// </summary>
    public enum PartnerStatus
    {
        Unknow = 0,
        /// <summary>
        /// 提出申请
        /// </summary>
        Apply = 5,
        /// <summary>
        /// 已经确认
        /// </summary>
        Accept = 10,
        /// <summary>
        /// 已经取消
        /// </summary>
        Cancel = 99,
    }
}
