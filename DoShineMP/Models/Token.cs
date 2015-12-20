using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    public class Token
    {
        [Key]
        public int TokenId { get; set; }

        public DateTime? GetTime { get; set; }

        public string TokenStr { get; set; }

        public AccountType Type { get; set; }
    }


    public enum AccountType
    {
        Unknown = 0,
        Service = 1,
        Company = 2,
        JsTicket = 3,

    }
}
