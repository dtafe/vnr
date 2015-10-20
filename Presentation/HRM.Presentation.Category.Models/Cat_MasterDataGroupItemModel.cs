using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.Category.Models
{
    public class Cat_MasterDataGroupItemModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_MasterDataGroupItem_MasterDataGroupID)]
        public Guid? MasterDataGroupID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_MasterDataGroupItem_ObjectName)]
        public string ObjectName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_MasterDataGroupItem_ObjectID)]
        public Guid? ObjectID { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_RptD02TS_STT)]
        public int? STT { get; set; }

        public List<Guid?> ObjectIDs { get; set; }
        public string ObjectIDStr { get; set; }

        public partial class FieldNames
        {
            public const string MasterDataGroupID = "MasterDataGroupID";
            public const string ObjectName = "ObjectName";
            public const string ObjectID = "ObjectID";
            public const string STT = "STT";
        }
    }

    public class Cat_MasterDataGroupItemSearchModel
    {
        public string ObjectName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

}
