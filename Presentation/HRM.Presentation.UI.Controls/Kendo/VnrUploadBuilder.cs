using Kendo.Mvc.UI.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using System.Web.Mvc;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrUploadBuilder
    {
        public static UploadBuilder VnrUpload(this HtmlHelper helper, UploadBuilderInfo builderInfo)
        {
            var async = new Action<UploadAsyncSettingsBuilder>(a =>
            {
                a.AutoUpload(builderInfo.AutoUpload);
                if (!string.IsNullOrWhiteSpace(builderInfo.SaveUrl))
                {
                    a.SaveUrl(builderInfo.SaveUrl);
                }
            });

            var events = new Action<UploadEventBuilder>(e =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.EventSelect))
                {
                    e.Select(builderInfo.EventSelect);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.EventSuccess))
                {
                    e.Success(builderInfo.EventSuccess);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.EventError))
                {
                    e.Error(builderInfo.EventError);
                }
            });

            var uploadBuilder = helper.Kendo().Upload()
                .Async(async)
                .Multiple(builderInfo.Multiple)
                .ShowFileList(builderInfo.ShowFileList)
                .Events(events)
                .HtmlAttributes(new {style="width:"+builderInfo.Width+"px;height:"+builderInfo.Height+";" });
            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                uploadBuilder.Name(builderInfo.Name);
            }
            //if (builderInfo.ShowFileList)
            //{
            //    uploadBuilder.TemplateId("template-kendo-upload-file").ToClientTemplate();
            //}
            return uploadBuilder;
        }
    }
    #region Upload Infomation
    public class UploadBuilderInfo : VnrBaseBuilderInfo
    {
        public UploadBuilderInfo()
        {
            AutoUpload = true;
            Multiple = false;
            ShowFileList = false;
            Width = 400;
            Height = 100;
        }

        /// <summary>
        /// Tên của control upload file
        /// </summary>
        public override string Name { get; set; }
        /// <summary>
        /// Tự động Upload file sau khi chọn  Default : true
        /// </summary>
        public bool AutoUpload { get; set; }
        /// <summary>
        /// Đường dẫn đến hàm xử lý lưu file
        /// </summary>
        public string SaveUrl { get; set; }
        /// <summary>
        /// Sự kiện khi select file
        /// </summary>
        public string EventSelect { get; set; }
        /// <summary>
        /// Sự kiện khi upload thành công
        /// </summary>
        public string EventSuccess { get; set; }
        public string EventError { get; set; }
        /// <summary>
        /// Cho phép chọn upload nhiều file Default : false
        /// </summary>
        public bool Multiple { get; set; }
        /// <summary>
        /// Hiển thị danh sách file đã chọn và upload Defaul : false
        /// </summary>
        public bool ShowFileList { get; set; }
        /// <summary>
        /// Chiều rộng của control upload
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Chiều cao của control upload
        /// </summary>
        public int Height { get; set; }
    }
    #endregion
}
