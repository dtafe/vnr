using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HRM.Business.Finance.Models
{
    public class FIN_TravelRequestItemEntity : HRMBaseModel
    {
        public Nullable<System.Guid> TravelRequestID { get; set; }
        public string TravelRequestName { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string StatusView { get; set; }
        public string Description3 { get; set; }
        public string Description4 { get; set; }
        public Nullable<double> Amount { get; set; }
        public double? Total { get; set; }
        public string TravelRequestItemName { get; set; }
        public string TravelRequestItemCode { get; set; }
        public Nullable<bool> IsBookingHotel { get; set; }
        public string Departure { get; set; }
        public Nullable<System.DateTime> DepartureTime { get; set; }
        public string Arrival { get; set; }
        public Nullable<System.DateTime> ArrivalTime { get; set; }
        public string Mean { get; set; }
        public string Purpose { get; set; }
        public string MeanView { get; set; }
        public string PurposeView { get; set; }

        public int? TotalAmount { get; set; }

    }
}
