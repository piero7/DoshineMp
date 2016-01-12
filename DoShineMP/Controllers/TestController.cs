using DoShineMP.Helper;
using DoShineMP.Models;
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
        public IEnumerable<ImageDownloadLog> TestAddRep()
        {
            RepairHelper reph = new RepairHelper();
            var ret = WechatImageHelper.AddNewImageForRepair(new string[] { "VSUwflr4OP1wUDHIXxNfjjenfcVYUSKkLqzZr6KBFfFBgjh9HVkj-ZiMolPCViZ8", "cm4WWQ3yJ9area22VLnPbICnANAwMAExxaft00vkn1Qhf_7zaHXlOchgtB9Asql7", "dSprEDWr96HM0OhgLL8ArxzGV7474OHRkj9kAh7KJWJgpEGGpbYQy7EtORAD-aw9" }, 143, "aaa");
            return ret;
        }
    }
}
