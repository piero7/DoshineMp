using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    public class Repair
    {
        [Key]
        public int RepairId { get; set; }

        public DateTime CreateDate { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserInfo User { get; set; }

        public string Contenet { get; set; }

        public RepairStatus Status { get; set; }

        public int? ImageFileId { get; set; }

        [ForeignKey("ImageFileId")]
        public virtual ImageFile Image { get; set; }

        public DateTime? AccepDate { get; set; }

        public string InnerNumber { get; set; }

        /// <summary>
        /// 处理完成时间
        /// </summary>
        public DateTime? FinishHandlendDate { get; set; }

        /// <summary>
        /// 用户反馈
        /// </summary>
        public string Response { get; set; }

        public DateTime? ResponeDate { get; set; }

        public double Score { get; set; } = 0;

        public string Remarks { get; set; }

        /// <summary>
        /// 与客户商议后的上门时间
        /// </summary>
        public DateTime? ExceptHandleDate { get; set; }

        /// <summary>
        /// 用于在显示的时候标识为用户自身记录还是公共显示记录
        /// </summary>
        [NotMapped]
        public bool? IsUserself { get; set; }
    }

    public enum RepairStatus
    {
        Unknow = 0,
        /// <summary>
        /// 提出申请
        /// </summary>
        Apply = 5,
        /// <summary>
        /// 受理
        /// </summary>
        Accept = 10,
        /// <summary>
        /// 处理中
        /// </summary>
        Handle = 15,
        /// <summary>
        /// 处理完成
        /// </summary>
        FinishHandle = 20,
        /// <summary>
        /// 完成
        /// </summary>
        Finish = 99,


    }
}
