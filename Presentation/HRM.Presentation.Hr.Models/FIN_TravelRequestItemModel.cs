using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class FIN_TravelRequestItemModel : BaseViewModel
    {
        public string PositionName { get; set; }
        public string AccountCode { get; set; }
        public string TravelRequestCode { get; set; }
        public string Status { get; set; }
        public Nullable<bool> IsClaim { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> DateFromEstimate { get; set; }
        public Nullable<System.DateTime> DateToEstimate { get; set; }
        public string Attachment { get; set; }
        public Nullable<double> CostEstimate { get; set; }
        public Nullable<double> CashAdvance { get; set; }
        public Nullable<double> CostActual { get; set; }
        public Nullable<double> Balance { get; set; }
        public Nullable<double> PaidAmount { get; set; }
        public Nullable<System.DateTime> DateRequest { get; set; }
        public Nullable<System.DateTime> DateFromActual { get; set; }
        public Nullable<System.DateTime> DateToActual { get; set; }

        public string Channel { get; set; }
               public string Region { get; set; }
           public string Area { get; set; }
           public string Branch { get; set; }
        public string OrgStructureCode { get; set; }
        public string OrgStructureName { get; set; }
        public string CurrencyName { get; set; }
        public string ProfileName { get; set; }

        public string StatusView { get; set; }



        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_TravelRequestID)]
        public Nullable<System.Guid> TravelRequestID { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_TravelRequestID)]
        public string TravelRequestName { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_Date)]
        public Nullable<System.DateTime> Date { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_Description2)]
        public string Description2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_Description3)]
        public string Description3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_Description4)]
        public string Description4 { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_Amount)]
        public Nullable<double> Amount { get; set; }
        private Guid _id = Guid.Empty;
        public double? Total { get; set; }
        public Guid? Claim_TravelRequestID { get; set; }
        public Guid TravelRequestItem_ID
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
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_TravelRequestItemName)]
        public string TravelRequestItemName { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_TravelRequestItemCode)]
        public string TravelRequestItemCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_IsBookingHotel)]
        public Nullable<bool> IsBookingHotel { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_Departure)]
        public string Departure { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_DepartureTime)]
        public Nullable<System.DateTime> DepartureTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_Arrival)]
        public string Arrival { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_ArrivalTime)]
        public Nullable<System.DateTime> ArrivalTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_Mean)]
        public string Mean { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_Purpose)]
        public string Purpose { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_Mean)]
        public string MeanView { get; set; }
        [DisplayName(ConstantDisplay.HRM_FIN_TravelRequestItem_Purpose)]
        public string PurposeView { get; set; }

        [DisplayName(ConstantDisplay.HRM_FIN_CashAdvance_TotalAmount)]
        public int? TotalAmount { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Total = "Total";
            public const string TravelRequestID = "TravelRequestID";
            public const string TravelRequestName = "TravelRequestName";
            public const string Date = "Date";
            public const string Description = "Description";
            public const string Description2 = "Description2";
            public const string Description3 = "Description3";
            public const string Description4 = "Description4";
            public const string Amount = "Amount";
            public const string TravelRequestItemName = "TravelRequestItemName";
            public const string TravelRequestItemCode = "TravelRequestItemCode";
            public const string IsBookingHotel = "IsBookingHotel";
            public const string Departure = "Departure";
            public const string DepartureTime = "DepartureTime";
            public const string Arrival = "Arrival";
            public const string ArrivalTime = "ArrivalTime";
            public const string Mean = "Mean";
            public const string Purpose = "Purpose";
            public const string PurposeView = "PurposeView";
            public const string MeanView = "MeanView";
            public const string StatusView = "StatusView";

        }
    }
  
}
