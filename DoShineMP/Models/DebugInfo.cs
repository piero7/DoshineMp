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

        public DateTime CreateDate { get; set; }

        public string Remarks { get; set; }

        public DebugInfo(string info, string remarks)
        {
            this.CreateDate = DateTime.Now;
            this.Info = info;
            this.Remarks = remarks;
            var db = new ModelContext();
            db.DebugInfoSet.Add(this);
            db.SaveChanges();
        }
    }
}
