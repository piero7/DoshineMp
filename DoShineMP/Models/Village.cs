using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    /// <summary>
    /// 小区表
    /// </summary>
    public class Village
    {
        [Key]
        public int VillageId { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        public double LocationX { get; set; }

        public double LocationY { get; set; }

        public string ImagePath { get; set; }

        public bool IsEnable { get; set; }

        public string Remarks { get; set; }

        /// <summary>
        /// 标识距离
        /// </summary>
        [NotMapped]
        public double? Distance { get; set; }

    }
}
