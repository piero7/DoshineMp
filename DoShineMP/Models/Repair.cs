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

        public string Remarks { get; set; }

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
