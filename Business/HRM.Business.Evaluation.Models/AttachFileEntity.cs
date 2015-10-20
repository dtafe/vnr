using System;
using System.Collections.Generic;
using HRM.Business.BaseModel;

namespace HRM.Business.Evaluation.Models
{
    public class AttachFileEntity 
    {
        public AttachFileEntity(string fullName, int fileSize)
        {
            FullName = fullName;

            if (fullName != null && fullName.LastIndexOf(".") != -1)
            {
                FileName = fullName.Substring(0, fullName.LastIndexOf("."));
            }
            if (fullName != null && fullName.LastIndexOf(".") != -1)
            {
                FileExtension = fullName.Substring(fullName.LastIndexOf(".") + 1);
            }
            FileSize = fileSize;
        }

        public AttachFileEntity(string fullName)
        {
            FullName = fullName;

            if (fullName != null && fullName.LastIndexOf(".") != -1)
            {
                FileName = fullName.Substring(0, fullName.LastIndexOf("."));
            }
            if (fullName != null && fullName.LastIndexOf(".") != -1)
            {
                FileExtension = fullName.Substring(fullName.LastIndexOf(".") + 1);
            }
            FileSize = 0;
        }

        public partial class FieldNames
        {
            public const string FileName = "FileName";
            public const string FullName = "FullName";
            public const string FileExtension = "FileExtension";
            public const string FileSize = "FileSize";
        }

        public string FileName { get; set; }
        public string FullName { get; set; }
        public string FileExtension { get; set; }
        public int FileSize { get; set; }
    }

 

}
