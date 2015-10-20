using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_HDTJobEntity : HRMBaseModel
    {
        public string StatusData { get; set; }
        public string StatusDataView { get; set; }
        public string StatusDataOut { get; set; }
        public string StatusDataOutView { get; set; }
        public string StatusOutView { get; set; }
        // Lấy thông tin mã cv,tên cv, mã vị trí cũ
        public string StatusOut { get; set; }
        public string HDTJobTypeCodeOld { get; set; }
        public string HDTJobTypeNameOld { get; set; }
        public string HDTJobGroupNameOld { get; set; }
        public string HDTJobGroupCodeOld { get; set; }
        public double? PriceOld { get; set; }

        public int TotalRow { get; set; }

        public Guid? ProfileID { get; set; }

        public string CodeEmp { get; set; }

        public DateTime? DateFrom { get; set; } //smalldatetime

        public DateTime? DateTo { get; set; } //smalldatetime

        public Guid? HDTJobTypeID { get; set; }

        public string HDTJobTypeName { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public string Note { get; set; }

        public string ProfileName { get; set; }

        public Guid? PositionID { get; set; }

        public string PositionName { get; set; }

        public Guid? JobTitleID { get; set; }

        public string JobTitleName { get; set; }

        public Guid? OrgStructureID { get; set; }

        public string OrgStructureName { get; set; }

        public string Status { get; set; }

        public string StatusView { get; set; }

        public Nullable<System.Guid> HDTJobGroupID { get; set; }

        public string HDTJobGroupName { get; set; }
        public string HDTJobGroupCode { get; set; }

        private Guid _id = Guid.Empty;
        public Guid HDTJob_ID
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
        public string Unit { get; set; }
        public string Dept { get; set; }
        public string Part { get; set; }
        public string Line { get; set; }
        public string Session { get; set; }
        public string HDTJobTypeCode { get; set; }
        public double? Price { get; set; }
        public string HDTJobTypeNameHVN { get; set; }
        public string EncryptJob { get; set; }
        public string GroupType { get; set; }
        public string StandardElement { get; set; }
        public DateTime? DateOfEffect { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
    }
}
