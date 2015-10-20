using System;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.HrmSystem.Models
{
    public class BaseViewModel
    {
        public virtual Guid ID { get; set; }

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

        public virtual int? UserLockID { get; set; }

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

        [ScaffoldColumn(false)]
        public Guid UserCreateID { get; set; }

        [ScaffoldColumn(false)]
        public Guid UserUpdateID { get; set; }

        [Display(Name = "UserCreate")]
        public string UserNameCreate { get; set; }

        [Display(Name = "UserLastUpdate")]
        public string UserNameUpdate { get; set; }

        [ScaffoldColumn(false)]
        public bool CanCreate { get; set; }

        [ScaffoldColumn(false)]
        public bool CanEdit { get; set; }

        [ScaffoldColumn(false)]
        public bool CanDelete { get; set; }

        [ScaffoldColumn(false)]
        public bool CanAppend { get; set; }

        [ScaffoldColumn(false)]
        public bool CanAppendTo { get; set; }

        [ScaffoldColumn(false)]
        public bool CanChangeOwner { get; set; }

        [ScaffoldColumn(false)]
        public bool CanShare { get; set; }
    }
}