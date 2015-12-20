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
        /// 相关文件列表 文件内容:文件id; eg:经营许可证:1;
        /// </summary>
        string FileList { get; set; }

        public DateTime CreateDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string RealName { get; set; }

        public string CompanyPhone { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserInfo User { get; set; }

        public int Point { get; set; }

        public PartnerType Type { get; set; }

        //public int? WechatUserId { get; set; }

        //[ForeignKey("WechatUserId")]
        //public virtual WechatUser WechatUser { get; set; }

        public Partner()
        {
            this.CreateDate = DateTime.Now;
            this.Point = 0;
        }

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
        Both = 3,
    }
}
