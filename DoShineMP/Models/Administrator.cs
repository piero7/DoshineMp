using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    class Administrator
    {
        public int AdminId { get; set; }

        public string RealName { get; set; }

        public string LoginName { get; set; }

        /// <summary>
        /// Md5的密码
        /// </summary>
        public string Password { get; set; }

        public DateTime LastLoginDate { get; set; }

        public bool IsEnable { get; set; }

        public string Remarks { get; set; }

        public Administrator()
        {
            this.LastLoginDate = DateTime.Now;
            this.IsEnable = true;
        }

        /// <summary>
        /// 有参构造函数，初始化信息
        /// </summary>
        /// <param name="realname"></param>
        /// <param name="logingName"></param>
        /// <param name="pwd"></param>
        public Administrator(string realname, string logingName, string pwd) : this()
        {
            var enpwd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "MD5");
            this.RealName = realname;
            this.Password = enpwd;
            this.LoginName = logingName;

        }

    }
}
