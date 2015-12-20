using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    public class Terminal
    {
        [Key]
        public int TerminalId { get; set; }

        public string InnnerNumber { get; set; }

        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public string Describe { get; set; }

        public string Remarks { get; set; }
    }
}
