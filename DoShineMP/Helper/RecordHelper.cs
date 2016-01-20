using DoShineMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Helper
{
    class RecordHelper
    {
        /// <summary>
        /// 获取用户记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static Record GetRecord(string openid)
        {
            var db = new ModelContext();
            var ret = db.RecordSet.OrderByDescending(item => item.CreateDate).FirstOrDefault(item => item.Type == RecordType.MpRepair && item.Openid == openid);

            if (ret == null)
            {

                var usr = WechatHelper.CheckOpenid(openid);
                usr = WechatHelper.CheckUser(usr);


                var rec = new Record
                {
                    Openid = openid,
                    Type = RecordType.MpRepair,
                    RecordId = 0
                };

                if (usr != null && usr.UserInfo != null)
                {
                    rec.PhoneNumber = usr.UserInfo.PhoneNumber;
                    rec.Name = usr.UserInfo.Name;
                }

                return rec;
            }
            return ret;
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public static Record UpdateRecord(Record rec)
        {
            var db = new ModelContext();
            if (rec.RecordId == 0)
            {
                rec.CreateDate = DateTime.Now;
                db.RecordSet.Add(rec);
            }
            else
            {
                var nrec = db.RecordSet.FirstOrDefault(item => item.RecordId == rec.RecordId);
                if (!(nrec != null && rec.Name == nrec.Name && rec.PhoneNumber == nrec.PhoneNumber && rec.Address == nrec.Address))
                {
                    rec.RecordId = 0;
                    db.RecordSet.Add(new Record
                    {
                        Name = rec.Name,
                        CreateDate = rec.CreateDate,
                        Address = rec.Address,
                        PhoneNumber = rec.PhoneNumber,
                        Remarks = rec.Remarks,
                        Type = rec.Type,
                        Openid = rec.Openid,
                        UserInfoId = rec.UserInfoId,
                        UserInfo = rec.UserInfo
                    });
                }
            }
            db.SaveChanges();
            return rec;

        }


        /// <summary>
        /// 更新各人记录
        /// </summary>
        /// <param name="id">原有记录id，0为新纪录</param>
        /// <param name="openid"></param>
        /// <param name="type">记录类型</param>
        /// <param name="phone">电话</param>
        /// <param name="name">姓名</param>
        /// <param name="address">地址（小区名称）</param>
        /// <returns></returns>
        public static Record UpdateRecord(int id, string openid, RecordType type, string phone, string name, string address)
        {
            if (id == 0)
            {
                return UpdateRecord(new Record
                {
                    RecordId = 0,
                    CreateDate = DateTime.Now,
                    Name = name,
                    PhoneNumber = phone,
                    Type = type,
                    Openid = openid,
                    Address = address,
                });
            }

            var db = new ModelContext();
            var rec = db.RecordSet.FirstOrDefault(item => item.RecordId == id);
            if (rec == null)
            {
                return null;
            }

            rec.Name = name;
            rec.Address = address;
            rec.PhoneNumber = phone;
            return UpdateRecord(rec);

        }
    }
}
