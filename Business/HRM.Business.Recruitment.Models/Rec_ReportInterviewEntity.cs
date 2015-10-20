using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_ReportInterviewEntity : HRMBaseModel
    {
        public string CodeCandidate { get; set; }
        public string CandidateName { get; set; }
        public string Gender { get; set; }
        public string GenderView { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TAddress { get; set; }
        public Nullable<System.DateTime> YearGraduation { get; set; }
        public string GraduateSchool { get; set; }
        public string Specialisation { get; set; }
        public Nullable<System.DateTime> DateExam { get; set; }
        public string Description { get; set; }
        public Nullable<double> Score1 { get; set; }
        public Nullable<double> Score3 { get; set; }
        public string IdentifyNumber { get; set; }
        public string PositionName { get; set; }
    }
}
