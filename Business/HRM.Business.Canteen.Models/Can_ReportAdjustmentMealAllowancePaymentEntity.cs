using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Canteen.Models
{
    public class Can_ReportAdjustmentMealAllowancePaymentEntity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string OrgName { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string TeamName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }   
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string UserPrint { get; set; }
        public DateTime? DatePrint { get; set; }
        public List<Guid?> OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public int? CountCard { get; set; }//Tong lan quet the
        public int? CountEatNotStandar { get; set; }// so bua an ko dat chuan
        public double? AmountEatNotStandar { get; set; }// so tien an o ko dat chuan
        public int? CountForgetMore { get; set; }// tong so lan quen quet the nhieu lan
        public double? AmountForgetMore { get; set; }// tong so tien quen quet the
        public double? AmountSubtractCardMore { get; set; }// so tien tru do quet the nhieu lan
        public int? CountNotWorkHasEat { get; set; }// so lan ko quet the nhung co an
        public double? AmountNotWorkHasEat { get; set; }// so tiền trừ ko quet the nhung co an
        public int? CountHDTJob { get; set; }// so lan quet the co HDTJOb
        public double? AmounSubtractHDTJob { get; set; }// Số tiền trừ do quẹt thẻ HDT 
        public int? CountCardMore { get; set; }// tong so lan do quet the nhieu lan
        public double? SumCardMore { get; set; }//Số tiền trừ do quẹt thẻ nhiều lần
        public int? CountCardWorngStandar { get; set; }// so lan quet the sai tieu chuan
        public double? AmountSubtractWorngStandar { get; set; }// so tien tru the sai tieu chuan
        public double? Amount3OnMonth { get; set; }// so tien tru do quet the >= 3 lan/thang
        public double? TotalMealAllowance { get; set; }//Tong so bua an duoc ho tro
        public double? SumAmount { get; set; }// tong so tien ho tro
        public double? AmountNotWorkButHasHDT { get; set; }// Số tiền trừ không đi làm nhưng quẹt thẻ HDT
        public double? CountSubtractWorngStandarHDT { get; set; }// Số lần quẹt thẻ HDT sai tiêu chuẩn
        public double? AmountSubtractWorngStandarHDT { get; set; }// Số tiền trừ do quẹt thẻ HDT sai tiêu chuẩn
    }
}
