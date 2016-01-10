using DoShineMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Helper
{
    class UsefulChatHelper
    {
        /// <summary>
        /// 返回所有的常用输入
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllUsefulChat()
        {
            var db = new ModelContext();
            return (from uc in db.UsefulChatSet
                    where uc.Level == 1
                    select uc.Contenet).ToList();
        }
    }
}
