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
    }
}
