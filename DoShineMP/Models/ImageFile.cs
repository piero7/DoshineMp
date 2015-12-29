using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    public class ImageFile
    {
        [Key]
        public int ImageFileId { get; set; }

        public string FileName { get; set; }

        public DateTime CreateDate { get; set; }

        public string Remarks { get; set; }
    }
}
