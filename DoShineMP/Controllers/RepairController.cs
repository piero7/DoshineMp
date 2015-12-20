using DoShineMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoShineMP.Controllers
{
    public class RepairController : ApiController
    {
        public Repair AddRepair(string openid, string content)
        {
            var db = new ModelContext();
            var wuser = db.WechatUserSet.Include("UserInfo").FirstOrDefault(item => item.OpenId == openid);
            if (wuser == null)
            {
                return null;
            }
            if (wuser.UserInfo == null)
            {
                return null;
            }

            var rep = new Repair
            {
                Contenet = content,
                CreateDate = DateTime.Now,
                Status = RepairStatus.Apply,
                UserId = wuser.UserInfoId,
            };

            db.RepairSet.Add(rep);
            db.SaveChanges();
            LogController.AddLog("Apply a new repair", rep.RepairId.ToString(), openid);
            return rep;
        }
    }
}
