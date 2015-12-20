using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    class File
    {
        [Key]
        public int FileId { get; set; }

        public string FileName { get; set; }

        public DateTime CreateDate { get; set; }

        public string Remarks { get; set; }

    }
}
