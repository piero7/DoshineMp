using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    class OveruseRepair
    {
        [Key]
        public int OverusRepairId { get; set; }

        public int Level { get; set; }

        public int? FatherItem { get; set; }

        public string Content { get; set; }

        public string Remarks { get; set; }
    }
}
