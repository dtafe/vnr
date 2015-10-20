using System;
using System.ComponentModel.DataAnnotations;
using HRM.Infrastructure.Utilities;
using System.ComponentModel;

namespace HRM.Presentation.Service
{
    public interface IHRMBaseModel
    {
    }
    /// <summary>
    /// [Chuc.Nguyen] - Chứa các thuộc tính chung cho tất cả các model
    /// </summary>
    public class BaseViewModel : IHRMBaseModel
    {
        public virtual Guid ID { get; set; }
        public virtual Guid UserID { get; set; }

        [MaxLength(32)]
        public virtual string UserCreate { get; set; }


        [MaxLength(32)]
        public virtual string UserUpdate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime? DateCreate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime? DateUpdate { get; set; }

        public virtual Guid? UserLockID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime? DateLock { get; set; }
        public virtual bool? IsDelete { get; set; }

        [MaxLength(50)]
        public virtual string ServerUpdate { get; set; }

        [MaxLength(50)]
        public virtual string ServerCreate { get; set; }

        [MaxLength(50)]
        public virtual string IPCreate { get; set; }

        [MaxLength(50)]
        public virtual string IPUpdate { get; set; }
        /// <summary>
        /// Xử lý trạng thái khi thao tác
        /// </summary>
        public string ActionStatus { get; set; }

        public int TotalRow { get; set; }

        public ExportFileType ExportType { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        private Guid _id = Guid.Empty;
        public Guid Topic_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }
    }
}
