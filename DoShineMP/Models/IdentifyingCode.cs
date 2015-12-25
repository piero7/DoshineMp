using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    class IdentifyingCode
    {
        [Key]
        public int IdentifyingCodeId { get; set; }

        public DateTime CreateDate { get; set; }

        public string OpenId { get; set; }

        public string Content { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsSendSuccess { get; set; }

        public bool IsUsed { get; set; }

        public string Remarks { get; set; }

        public IdentifyingCode()
        {
            this.CreateDate = DateTime.Now;
            this.IsUsed = false;
            this.IsSendSuccess = false;
        }
    }
}
