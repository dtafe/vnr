using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_ProcessApprovedSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_System_ProcessApproved_ProcessName)]
        public string ProcessName { get; set; }
    }

    public class Sys_ProcessApprovedMultiModel
    {
        public string ProcessName { get; set; }
        public Guid ID { get; set; }
    }

    public class Sys_ProcessApprovedModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_System_ProcessApproved_ProcessName)]
        public string ProcessName { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ProcessApproved_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ProcessApproved_Approved1)]
        public string Approved1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ProcessApproved_Approved2)]
        public string Approved2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ProcessApproved_Approved3)]
        public string Approved3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ProcessApproved_Approved4)]
        public string Approved4 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ProcessApproved_Approved5)]
        public string Approved5 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ProcessApproved_Approved6)]
        public string Approved6 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ProcessApproved_Approved7)]
        public string Approved7 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ProcessApproved_Approved8)]
        public string Approved8 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ProcessApproved_Approved9)]
        public string Approved9 { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ProcessApproved_Approved10)]
        public string Approved10 { get; set; }

        public partial class FieldNames
        {
            public const string ProcessName = "ProcessName";
            public const string Description = "Description";

            public const string Approved1 = "Approved1";
            public const string Approved2 = "Approved2";
            public const string Approved3 = "Approved3";
            public const string Approved4 = "Approved4";
            public const string Approved5 = "Approved5";
            public const string Approved6 = "Approved6";
            public const string Approved7 = "Approved7";
            public const string Approved8 = "Approved8";
            public const string Approved9 = "Approved9";
            public const string Approved10 = "Approved10";
        }
    }
}
