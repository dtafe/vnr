using System;


namespace HRM.Business.Training.Models
{
    public class Tra_TraineeScoreImport : HRM.Business.BaseModel.HRMBaseModel
    {

        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        public string Column5 { get; set; }
        public Double Column6 { get; set; }
        public string Column7 { get; set; }
        public int Stt {get;set;}
        /// <summary>
        /// Mã Lớp Học
        /// </summary>
        public string ClassCode{get;set;}
        /// <summary>
        /// Mã Nhân Viên
        /// </summary>
        public string CodeEmp{get;set;}
        /// <summary>
        /// Mã Môn Học
        /// </summary>
        public string TopicCode{get;set;}
        /// <summary>
        /// Mã Loại Điểm
        /// </summary>
        public string TypeScore{get;set;}
        /// <summary>
        /// Điểm Số
        /// </summary>
        public Double Score{get;set;}
        /// <summary>
        /// Loại Lỗi (Khi Import Bị Lỗi)
        /// </summary>
        public string TypeError{get;set;}
    }
}

