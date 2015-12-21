using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    class DebugInfo
    {
        [Key]
        public int InfoId { get; set; }

        public string Info { get; set; }

        public DateTime Create { get; set; }

        public string Remarls { get; set; }
    }
}
