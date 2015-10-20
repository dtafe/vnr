using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Insurance.Models
{
    /// <summary>Danh Mục Trạng Thái D02</summary>
    public class Ins_TypeD02Entity : HRMBaseModel
    {
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string Comment { get; set; }
        public string Note { get; set; }
        public string CommentCode { get; set; }
    }

    public class Ins_TypeD02MultiEntity
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int TotalRow { get; set; }
        public string StatusSyn { get; set; }
    }

}
