using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_TraineeScoreImportModel : BaseViewModel
    {

        public int Stt { get; set; }
        /// <summary>
        /// Mã Lớp Học
        /// </summary>
        public string ClassCode { get; set; }
        /// <summary>
        /// Mã Nhân Viên
        /// </summary>
        public string CodeEmp { get; set; }
        /// <summary>
        /// Mã Môn Học
        /// </summary>
        public string TopicCode { get; set; }
        /// <summary>
        /// Mã Loại Điểm
        /// </summary>
        public string TypeScore { get; set; }
        /// <summary>
        /// Điểm Số
        /// </summary>
        public Double Score { get; set; }
        /// <summary>
        /// Loại Lỗi (Khi Import Bị Lỗi)
        /// </summary>
        public string TypeError { get; set; }
        public partial class FieldNames
        {
            public const string Stt = "Stt";
            public const string ClassCode = "ClassCode";
            public const string CodeEmp = "CodeEmp";
            public const string TopicCode = "TopicCode";
            public const string TypeScore = "TypeScore";
            public const string Score = "Score";
            public const string TypeError = "TypeError";
        }
    }


}
