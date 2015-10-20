using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace HRM.Presentation.UI.Controls.Kendo
{
    /// <summary>
    /// [Chuc.Nguyen - Control hỗ trợ nhập các kiểu số]
    /// </summary>
    public static class VnrNumericTextBoxBuilder 
    {
        #region NumericTextBox Builder
        #region NumericTextBox
        public static NumericTextBoxBuilder<TValue> VnrNumericTextBox<TModel, TValue>(this HtmlHelper<TModel> helper, NumericTextBoxBuilderInfo<TValue> builderInfo) where TValue : struct
        {
            var numericTextBoxBuilder = helper.Kendo().NumericTextBox<TValue>().Min(builderInfo.Min).Max(builderInfo.Max);
            if (!string.IsNullOrWhiteSpace(builderInfo.Format))
            {
                numericTextBoxBuilder.Format(builderInfo.Format);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Placeholder))
            {
                numericTextBoxBuilder.Placeholder(builderInfo.Placeholder);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
            {
                numericTextBoxBuilder.HtmlAttributes(new { style = builderInfo.StyleCss });
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                numericTextBoxBuilder.Name(builderInfo.Name);
            }

            

            if (typeof(TValue) == typeof(Int32))
            {
                double value = Convert.ToDouble(builderInfo.Value);
                if (value != 0)
                {
                    numericTextBoxBuilder.Value(builderInfo.Value);
                }

                int step = Convert.ToInt32(builderInfo.Step);
                if (step != 0)
                {
                    numericTextBoxBuilder.Step(builderInfo.Step);
                }
            }
            if (typeof(TValue) == typeof(int))
            {
                int value = Convert.ToInt32(builderInfo.Value);
                if (value != 0)
                {
                    numericTextBoxBuilder.Value(builderInfo.Value);
                }

                int step = Convert.ToInt32(builderInfo.Step);
                if (step != 0)
                {
                    numericTextBoxBuilder.Step(builderInfo.Step);
                }
            }
            return numericTextBoxBuilder;
        }
        #endregion
        #region NumericTextBoxFor
        /// <summary>
        /// Dùng nhập các giá trị là số và không cho phép giá trị null
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="builderInfo"></param>
        /// <returns></returns>
        public static NumericTextBoxBuilder<TValue> VnrNumericTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, NumericTextBoxBuilderInfo<TValue> builderInfo) where TValue : struct
        {
            var numericTextBoxBuilder = helper.Kendo().NumericTextBoxFor(expression).Min(builderInfo.Min).Max(builderInfo.Max);
            if (!string.IsNullOrWhiteSpace(builderInfo.Format))
            {
                numericTextBoxBuilder.Format(builderInfo.Format);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Placeholder))
            {
                numericTextBoxBuilder.Placeholder(builderInfo.Placeholder);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
            {
                numericTextBoxBuilder.HtmlAttributes(new { style = builderInfo.StyleCss });
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                numericTextBoxBuilder.Name(builderInfo.Name);
            }
            if (typeof(TValue) == typeof(double))
            {
                double value = Convert.ToDouble(builderInfo.Value);
                if (value != 0)
                {
                    numericTextBoxBuilder.Value(builderInfo.Value);
                }

                double step = Convert.ToDouble(builderInfo.Step);
                if (step != 0)
                {
                    numericTextBoxBuilder.Step(builderInfo.Step);
                }
                if (builderInfo.Decimals > 0)
                {
                    numericTextBoxBuilder.Decimals(builderInfo.Decimals);
                }
            }
            if (typeof(TValue) == typeof(int))
            {
                int value = Convert.ToInt32(builderInfo.Value);
                if (value != 0)
                {
                    numericTextBoxBuilder.Value(builderInfo.Value);
                }

                int step = Convert.ToInt32(builderInfo.Step);
                if (step != 0)
                {
                    numericTextBoxBuilder.Step(builderInfo.Step);
                }
            }
            return numericTextBoxBuilder;
        }

        /// <summary>
        /// Dùng nhập các giá trị là số và cho phép giá trị có thể null
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="builderInfo"></param>
        /// <returns></returns>
        public static NumericTextBoxBuilder<TValue> VnrNumericTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> helper,
           Expression<Func<TModel, TValue?>> expression, NumericTextBoxBuilderInfo<TValue> builderInfo) where TValue : struct
        {
            var numericTextBoxBuilder = helper.Kendo().NumericTextBoxFor(expression).Min(builderInfo.Min).Max(builderInfo.Max).Enable(builderInfo.Enable);
            if (!string.IsNullOrWhiteSpace(builderInfo.Format))
            {
                numericTextBoxBuilder.Format(builderInfo.Format);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Placeholder))
            {
                numericTextBoxBuilder.Placeholder(builderInfo.Placeholder);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
            {
                numericTextBoxBuilder.HtmlAttributes(new { style = builderInfo.StyleCss });
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                numericTextBoxBuilder.Name(builderInfo.Name);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.EvenChange))
            {
                numericTextBoxBuilder.Events(m => m.Change(builderInfo.EvenChange));
            }

            if (typeof(TValue) == typeof(double))
            {
                double value = Convert.ToDouble(builderInfo.Value);
                if (value != 0)
                {
                    numericTextBoxBuilder.Value(builderInfo.Value);
                }

                double step = Convert.ToDouble(builderInfo.Step);
                if (step !=0)
                {
                    numericTextBoxBuilder.Step(builderInfo.Step);
                }
                if (builderInfo.Decimals > 0)
                {
                    numericTextBoxBuilder.Decimals(builderInfo.Decimals);
                }
            }
            if (typeof(TValue) == typeof(int))
            {
                int value = Convert.ToInt32(builderInfo.Value);
                if (value != 0)
                {
                    numericTextBoxBuilder.Value(builderInfo.Value);
                }

                int step = Convert.ToInt32(builderInfo.Step);
                if (step != 0)
                {
                    numericTextBoxBuilder.Step(builderInfo.Step);
                }
            }
            return numericTextBoxBuilder;
        }
        #endregion
        #endregion
    }

    #region NumericTextBox Information
    public class NumericTextBoxBuilderInfo<T> : VnrBaseBuilderInfo where T : struct
    {
        public NumericTextBoxBuilderInfo()
        {
            Enable = true;
           var  type = typeof(T);
            if (type == typeof(Double))
            {
                Min = (T)((object)Convert.ToDouble(0));
                Max = (T)((object)Convert.ToDouble(double.MaxValue));
            }
            if (type == typeof(float))
            {
                Min = (T)((object)float.Parse("0")); ;
                Max = (T)((object)float.Parse(float.MaxValue.ToString()));
            }
            if (type == typeof(Int16))
            {
                Min = (T)((object)Convert.ToInt16(0));
                Max = (T)((object)Convert.ToInt16(Int16.MaxValue));
            }
            if (type == typeof(Int32))
            {
                Min = (T)((object)Convert.ToInt32(0));
                Max = (T)((object)Convert.ToInt32(Int32.MaxValue));
            }
            if (type == typeof(Int64))
            {
                Min = (T)((object)Convert.ToInt64(0));
                Max = (T)((object)Convert.ToInt64(Int64.MaxValue));
            }
            if (type == typeof(Decimal))
            {
                Min = (T)((object)Convert.ToDecimal(0));
                Max = (T)((object)Convert.ToDecimal(decimal.MaxValue));
            }
        }
        #region Properties
        /// <summary>
        /// Tên của control dùng trong các xử lý
        /// </summary>
        public override string Name { get; set; }
        /// <summary>
        /// Hiển thị dòng chữ mờ nhắc nhở trong control
        /// </summary>
        public string Placeholder { get; set; }
        /// <summary>
        /// Thay đổi style cho Control, viết dạng Css bình thường
        /// </summary>
        public string StyleCss { get; set; }
        /// <summary>
        /// Định dàng hiển thị cho control, có nhiều loại: 'n' - Number ; 'c' - Currency...
        /// Xem thêm tại:http://docs.telerik.com/kendo-ui/getting-started/framework/globalization/numberformatting
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// Giá trị hiển thị trong control
        /// </summary>
        public T Value { get; set; }
        /// <summary>
        /// Giá trị tối thiểu có thể sử dụng
        /// </summary>
        public T Min { get; set; }
        /// <summary>
        /// Giá trị tối đa có thể sử dụng
        /// </summary>
        public T Max { get; set; }

        /// <summary>
        /// Giá trị tiếp theo có thể sử dụng
        /// </summary>
        public T Step { get; set; }

        /// <summary>
        /// Số thập phân sau dấu .
        /// </summary>
        public int Decimals { get; set; }

        public string EvenChange { get; set; }
        public bool Enable { get; set; }
        #endregion
    }
    #endregion
}
