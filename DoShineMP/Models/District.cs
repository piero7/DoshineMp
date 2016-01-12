using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    /// <summary>
    /// 地区
    /// </summary>
    public class District
    {
        [Key]
        public int DistrictId { get; set; }

        public string Name { get; set; }

        public double? LocationX { get; set; }

        public double? LocationY { get; set; }

        public string Remarks { get; set; }
    }
}
