using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoShineMP.Models
{
    public class Repair
    {
        [Key]
        public int RepairId { get; set; }

        public DateTime CreateDate { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserInfo User { get; set; }

        public string Contenet { get; set; }

        public RepairStatus Status { get; set; }

        public int? ImageFileId { get; set; }

        [ForeignKey("ImageFileId")]
        public virtual ImageFile Image { get; set; }

        public DateTime? AccepDate { get; set; }

        public string InnerNumber { get; set; }

        /// <summary>
        /// 处理完成时间
        /// </summary>
        public DateTime? FinishHandlendDate { get; set; }

        /// <summary>
        /// 用户反馈
        /// </summary>
        public string Response { get; set; }

        public DateTime? ResponeDate { get; set; }

        public double Score { get; set; } = 0;

        public string Remarks { get; set; }

        /// <summary>
        /// 与客户商议后的上门时间
        /// </summary>
        public DateTime? ExceptHandleDate { get; set; }

        /// <summary>
        /// 用于在显示的时候标识为用户自身记录还是公共显示记录
        /// </summary>
        [NotMapped]
        public bool? IsUserself { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public int? VillageId { get; set; }

        [ForeignKey("VillageId")]
        public virtual Village Village { get; set; }

        /// <summary>
        /// 报修图片,id字符串，用逗号分割
        /// </summary>
        public string ImageFilesStr { get; set; }

        /// <summary>
        /// 报修完成图片，id字符串，用逗号分割
        /// 
        /// </summary>
        public string FinishImageFilesStr { get; set; }

        public RepairFinishType FinishType { get; set; }

        public string Name { get; set; }

        public string Describe { get; set; }

        /// <summary>
        /// 报修文件列表
        /// </summary>
        [NotMapped]
        public List<ImageFile> ImageFiles
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImageFilesStr))
                {
                    return new List<ImageFile>();
                }

                var db = new ModelContext();
                var ret = new List<ImageFile>();
                var tmpArr = this.ImageFilesStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var id in tmpArr)
                {
                    var file = db.ImageFileSet.FirstOrDefault(item => item.ImageFileId.ToString() == (id));
                    if (file != null)
                    {
                        ret.Add(file);
                    }
                }
                return ret.Distinct().ToList(); ;
            }
            set
            {
                this.ImageFilesStr = "";
                if (value != null && value.Count() != 0)
                {
                    foreach (var file in value)
                    {
                        if (file != null && file.ImageFileId > 0)
                        {
                            this.ImageFilesStr += (file.ImageFileId + ",");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 完成报修文件列表
        /// </summary>
        [NotMapped]
        public List<ImageFile> FinishImageFiles
        {
            get
            {
                if (string.IsNullOrEmpty(this.FinishImageFilesStr))
                {
                    return new List<ImageFile>();
                }

                var db = new ModelContext();
                var ret = new List<ImageFile>();
                var tmpArr = this.FinishImageFilesStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var id in tmpArr)
                {
                    var file = db.ImageFileSet.FirstOrDefault(item => item.ImageFileId.ToString() == id);
                    if (file != null)
                    {
                        ret.Add(file);
                    }
                }
                return ret.Distinct().ToList();
            }
            set
            {
                this.FinishImageFilesStr = "";
                if (value != null && value.Count() != 0)
                {
                    foreach (var file in value)
                    {
                        if (file != null && file.ImageFileId > 0)
                        {
                            this.FinishImageFilesStr += (file.ImageFileId + ",");
                        }
                    }
                }
            }
        }
    }


    public enum RepairStatus
    {
        Unknow = 0,
        /// <summary>
        /// 提出申请
        /// </summary>
        Apply = 5,
        /// <summary>
        /// 受理
        /// </summary>
        Accept = 10,
        /// <summary>
        /// 处理中
        /// </summary>
        Handle = 15,
        /// <summary>
        /// 处理完成
        /// </summary>
        FinishHandle = 20,
        /// <summary>
        /// 完成
        /// </summary>
        Finish = 99,
        /// <summary>
        /// 已取消
        /// </summary>
        Cancel = -1,
    }


    /// <summary>
    /// 报修完成类型
    /// </summary>
    public enum RepairFinishType
    {
        Unknown = 0,
        /// <summary>
        /// 已修复
        /// </summary>
        Fixed = 1,
        /// <summary>
        /// 未修复
        /// </summary>
        Unfixed = 2,
        /// <summary>
        /// 超出范围
        /// </summary>
        OutOfRange = 10,

    }
}
