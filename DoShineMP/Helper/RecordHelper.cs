﻿using DoShineMP.Models;
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
            var ret = db.RecordSet.OrderByDescending(item => item.Address).FirstOrDefault(item => item.Type == RecordType.MpRepair && item.Openid == openid);

            if (ret == null)
            {
                return new Record
                {
                    Openid = openid,
                    Type = RecordType.MpRepair,
                    RecordId = 0
                };
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

            var nrec = db.RecordSet.FirstOrDefault(item => item.RecordId == rec.RecordId);
            if (!(rec.Name == nrec.Name && rec.PhoneNumber == nrec.PhoneNumber && rec.Address == nrec.Address))
            {
                rec.RecordId = 0;
                db.RecordSet.Add(rec);
            }

            db.SaveChanges();
            return rec;
        }

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
