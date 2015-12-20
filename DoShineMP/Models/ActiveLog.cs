using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    public class ActiveLog
    {
        [Key]
        public int ActiveLogId { get; set; }

        public DateTime CreateDate { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserInfo User { get; set; }

        public int TerminalId { get; set; }

        [ForeignKey("TerminalId")]
        public virtual Terminal Terminal { get; set; }

        public string OptionContent { get; set; }

        public string Remarks { get; set; }
    }
}
