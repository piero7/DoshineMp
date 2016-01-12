using DoShineMP.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Services.Description;

namespace DoShineMP.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public Helper.WechatJsConfig TestConfig(string url)
        {
            return new Helper.WechatJsConfig("http://www.baidu.com");
        }

        [HttpGet]
        public string TestDownload(int id)
        {
            var db = new Models.ModelContext();
            var dlog = db.ImageDownloadLogSet.Find(id);


            return Helper.WechatHelper.DownloadImgFile(dlog.MediaNumber);
        }

        [HttpGet]
        public IEnumerable<Models.Repair> GetHistory(string openid)
        {
            Helper.RepairHelper rp = new Helper.RepairHelper();
            return rp.GetHistoryRepair(openid);
        }

        [HttpGet]
        public void TestAddRep()
        {
            RepairHelper reph = new RepairHelper();
            //reph.Add(aa,bb,)
        }
    }
}
