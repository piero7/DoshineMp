using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    class ImageDownloadLog
    {
        [Key]
        public int ImageDownLoadLogId { get; set; }

        public DateTime CreateDate { get; set; }

        public string MediaNumber { get; set; }

        public bool IsSuccess { get; set; }

        public DateTime? FinishDate { get; set; }

        public string OpenId { get; set; }

        /// <summary>
        /// 使用场景
        /// </summary>
        public string Scene { get; set; }

        public int? FileId { get; set; }

        [ForeignKey("FileId")]
        public virtual ImageFile File { get; set; }

        public string Remarks { get; set; }

    }
}
