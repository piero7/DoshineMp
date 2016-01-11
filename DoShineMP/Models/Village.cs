using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    }
}
