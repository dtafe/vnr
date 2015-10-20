using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System.Collections;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrProgressBarBuilder
    {
        public static ProgressBarBuilder VnrProgressBar(this HtmlHelper helper, ProgressBarBuilderInfo builderInfo)
        {
            var progressBar = helper.Kendo().ProgressBar()
                .ChunkCount(builderInfo.ChunkCount)
                .Enable(builderInfo.Enable)
                .Max(builderInfo.MaxValue)
                .Min(builderInfo.MinValue)
                .Name(builderInfo.Name)
                .Orientation(builderInfo.Orientation)
                .Reverse(builderInfo.Reverse)
                .ShowStatus(builderInfo.ShowStatus)
                .Type(builderInfo.ProgressType)
                .Value(builderInfo.DefaultValue);

            if (builderInfo.EvenChange != null && builderInfo.EvenChange != string.Empty)
                progressBar.Events(m => m.Change(builderInfo.EvenChange));
            if (builderInfo.EvenComplete != null && builderInfo.EvenComplete != string.Empty)
                progressBar.Events(m => m.Complete(builderInfo.EvenComplete));

            return progressBar;
        }
    }

    public class ProgressBarBuilderInfo : VnrBaseBuilderInfo
    {
        public ProgressBarBuilderInfo()
        {
            Enable = true;
            MinValue = 0;
            MaxValue = 100;
            DefaultValue = 0;
            Duration = 5000;
            Animation = true;
            Orientation = ProgressBarOrientation.Horizontal;
            Reverse = false;
            ShowStatus = true;
            ProgressType = ProgressBarType.Percent;
            ChunkCount = 5;
        }
        
        #region Methods
        //Default true
        public bool Animation { get; set; }
        //Tốc độ chạy của ProgressBar, tính theo ms (số càng lớn càng chậm) Default 5000
        public int Duration { get; set; }
        //Enable (default true)
        public bool Enable { get; set; }
        //Min value (default 0)
        public int MinValue { get; set; }
        //Max value (default 100)
        public int MaxValue { get; set; }
        //Default Value (default 0)
        public int DefaultValue { get; set; }
        //Nằm đứng hay nẳm ngang (Default horizontal) vertical
        public ProgressBarOrientation Orientation { get; set; }
        //Hiển thị số % bên phải hay bên trái (Defalut false) true=trái,false=phải
        public bool Reverse { get; set; }
        //Hiển thị thông báo (Default true)
        public bool ShowStatus { get; set; }
        //percent and chunk (Default percent)
        public ProgressBarType ProgressType { get; set; }
        //Default 5
        public int ChunkCount { get; set; }
        #endregion

        #region Even
        //Even Change ProgressBar
        public string EvenChange { get; set; }
        //Even ProgressBar Complete
        public string EvenComplete { get; set; }
        #endregion
    }
}
