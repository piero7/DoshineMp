using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    class Salesman
    {
        [Key]
        public int SalesmanId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string InnerNumber { get; set; }

        public string Remarks { get; set; }

        public string NickName { get; set; }
    }
}
