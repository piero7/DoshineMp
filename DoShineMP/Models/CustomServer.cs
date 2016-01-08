using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    /// <summary>
    /// 客服
    /// </summary>
    public class CustomServer
    {
        [Key]
        public int CustomServerId { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public string Name { get; set; }

        public string Remarks { get; set; }

        public CustomServerStatus Status { get; set; }
    }

    /// <summary>
    /// 客服状态
    /// </summary>
    public enum CustomServerStatus
    {
        Unknown = 0,
        /// <summary>
        /// 在线
        /// </summary>
        Online = 1,
        /// <summary>
        /// 离线
        /// </summary>
        Offline = 20,
        /// <summary>
        /// 离开
        /// </summary>
        Leave = 30,
    }
}
