using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.Category.Models
{
    public class Cat_MasterDataGroupDashBoardModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_MasterDataGroup_Code)]
        public Guid? UserID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_MasterDataGroup_MasterDataGroupName)]
        public string MasterDataGroupName { get; set; }
        public List<Guid?> MasterDataGroupIDs { get; set; }
    }


}
