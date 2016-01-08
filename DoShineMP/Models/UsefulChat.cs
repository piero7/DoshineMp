using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoShineMP.Models
{
    public class UsefulChat
    {
        [Key]
        public int UsefulChatId { get; set; }

        public int Level { get; set; }

        public string Contenet { get; set; }

        public bool IsEnable { get; set; }

        public string Remarks { get; set; }
    }
}