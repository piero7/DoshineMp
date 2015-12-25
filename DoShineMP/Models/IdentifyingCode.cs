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

        public string Remarks { get; set; }
    }
}
