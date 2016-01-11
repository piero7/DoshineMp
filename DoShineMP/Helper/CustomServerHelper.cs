using DoShineMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Helper
{
    class CustomServerHelper
    {
        /// <summary>
        /// 更改客服状态
        /// </summary>
        /// <param name="name">客服名称</param>
        /// <param name="status">目标状态</param>
        /// <returns></returns>
        public static CustomServer SetServerStatus(string name, CustomServerStatus status)
        {
            var db = new ModelContext();
            var cs = db.CustomServerSet.FirstOrDefault(item => item.Name == name);
            if (cs == null)
            {
                return null;
            }

            cs.Status = status;
            if (status == CustomServerStatus.Online)
            {
                cs.LastLoginDate = DateTime.Now;
            }

            db.SaveChanges();

            return cs;
        }



        /// <summary>
        /// 获取客服信息（主要用于获取状态）
        /// </summary>
        /// <param name="name">客服名称</param>
        /// <returns></returns>
        public static CustomServer GetServer(string name)
        {
            var db = new ModelContext();
            return db.CustomServerSet.FirstOrDefault(item => item.Name == name);
        }
    }
}
