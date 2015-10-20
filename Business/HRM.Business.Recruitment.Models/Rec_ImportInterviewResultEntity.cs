using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_ImportInterviewResultEntity
    {
        public Guid ID { get; set; }
        public string CandidateCode { get; set; }
        public string CandidateName { get; set; }
        public string JobVacancyCode { get; set; }
        public string CandidateEmail { get; set; }
        public string LanguageCode { get; set; }

        public string GroupCondition1 { get; set; }
        public string GroupCondition2 { get; set; }
        public string GroupCondition3 { get; set; }
        public string GroupCondition4 { get; set; }
        public string GroupCondition5 { get; set; }
        public string GroupCondition6 { get; set; }
        public string GroupCondition7 { get; set; }

        public string GroupResult1 { get; set; }
        public string GroupResult2 { get; set; }
        public string GroupResult3 { get; set; }
        public string GroupResult4 { get; set; }
        public string GroupResult5 { get; set; }
        public string GroupResult6 { get; set; }
        public string GroupResult7 { get; set; }

        public double Group1Score1 { get; set; }
        public double Group1Score2 { get; set; }
        public double Group1Score3 { get; set; }

        public double Group2Score1 { get; set; }
        public double Group2Score2 { get; set; }
        public double Group2Score3 { get; set; }

        public double Group3Score1 { get; set; }
        public double Group3Score2 { get; set; }
        public double Group3Score3 { get; set; }

        public double Group4Score1 { get; set; }
        public double Group4Score2 { get; set; }
        public double Group4Score3 { get; set; }

        public double Group5Score1 { get; set; }
        public double Group5Score2 { get; set; }
        public double Group5Score3 { get; set; }

        public double Group6Score1 { get; set; }
        public double Group6Score2 { get; set; }
        public double Group6Score3 { get; set; }

        public double Group7Score1 { get; set; }
        public double Group7Score2 { get; set; }
        public double Group7Score3 { get; set; }
    }
}
