using HRM.Presentation.UI.Controls.Kendo.ControlBase;
using System;
using System.Linq;
using System.Text;
using Kendo.Mvc.UI.Fluent;
using System.Collections.Generic;
using Kendo.Mvc.UI;
using System.Web.Mvc;
using VnResource.Helper.Data;

namespace HRM.Presentation.UI.Controls.Kendo.NumericTextBox
{
    #region VnrNumericTextboxBuilder

    public static class VnrNumericTextboxControl
    {
        public static NumericTextBoxBuilder<T> VnrNumericTextbox<T>(this HtmlHelper helper, NumericTextboxInfomation numericInfo) where T : struct
        {
            return helper.Kendo().NumericTextBox<T>()
                .Name(numericInfo.Name)
                .Format(numericInfo.Format)
                .Placeholder(numericInfo.Placeholder);
        }
    }
    #endregion

    #region NumericTextboxInfomation
    public class NumericTextboxInfomation : VnrPropertiesBase
    {
        #region Properties

        private double step;
        private string placeholder;
        private double min;
        private double max;

        private string format;

        /// <summary>
        /// Giá trị cho mỗi lần tăng hoặc giảm 
        /// </summary>
        public Double Step
        {
            get { return step; }
            set { step = value; }
        }

        /// <summary>
        /// Thông tin hổ trợ cho người dùng lựa chọn . Example: "Please!Select A Value"
        /// </summary>
        public String Placeholder
        {
            get { return placeholder; }
            set { placeholder = value; }
        }

        /// <summary>
        /// Giá trị nhỏ nhất 
        /// </summary>
        public Double Min
        {
            get { return min; }
            set { min = value; }
        }

        /// <summary>
        /// Giá trị lớn nhất
        /// </summary>
        public Double  Max
        {
            get { return max; }
            set { max = value; }
        }

        /// <summary>
        /// Định dạng lại
        /// </summary>
        public string Format
        {
            get { return format; }
            set { format = value; }
        }

        #endregion

        #region Methods

        #endregion

        #region Even

        #endregion
    }
    #endregion
}