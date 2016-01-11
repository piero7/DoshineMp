using DoShineMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Helper
{
    class SalesmanHelper
    {
        /// <summary>
        /// 获得所有的销售信息
        /// </summary>
        /// <returns></returns>
        public static List<Salesman> GetAllSalesman()
        {
            var db = new ModelContext();
            return (from sm in db.SalesmanSet
                    select sm).ToList();
        }
    }
}
