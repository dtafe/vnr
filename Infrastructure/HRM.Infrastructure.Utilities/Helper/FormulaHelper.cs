using CalcEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRM.Infrastructure.Utilities.Helper
{
    public static class FormulaHelper
    {
        public class FormulaHelperModel
        {
            public FormulaHelperModel()
            {
                Value = 0;
                ErrorMessage = string.Empty;
            }
            public object Value { get; set; }
            public string ErrorMessage { get; set; }
        }
        /// <summary>
        /// Hàm tính toán công thức
        /// </summary>
        /// <param name="formula">Chuổi công thức - Vd: "basicsalary * workdaycount"</param>
        /// <param name="listElementFormula">
        /// Danh sách đối tượng phần tử 
        /// Vd: List<ElementFormula> listElementFormula = new List<ElementFormula>();
        /// lstElementFormula.Add(new ElementFormula("workdaycount", 82, 1));
        /// lstElementFormula.Add(new ElementFormula("basicsalary", 5200000, 2));
        /// <returns></returns>
        //public static object ParseFormula(string formula, List<ElementFormula> listElementFormula)
        //{
        //    object result = 0;
        //    listElementFormula = listElementFormula.OrderBy(v => v.OrderNumber).ToList();
        //    var ce = new CalcEngine.CalcEngine();
        //    foreach (var item in listElementFormula)
        //    {
        //        if (!ce.Variables.Any(m => m.Key == item.VariableName))
        //        {
        //            ce.Variables.Add(item.VariableName, item.Value);
        //        }
        //    }
        //    var x = ce.Parse(formula);
        //    result = x.Evaluate();
        //    if (result.ToString() == "NaN")
        //    { result = 0; }
        //    return result;
        //}

        public static FormulaHelperModel ParseFormula(string formula, List<ElementFormula> listElementFormula)
        {
            FormulaHelperModel result = new FormulaHelperModel();
            try
            {
                listElementFormula = listElementFormula.OrderBy(v => v.OrderNumber).ToList();
                var ce = new CalcEngine.CalcEngine();


                foreach (var item in listElementFormula)
                {
                    if(formula.Contains(item.VariableName))
                    {
                        if (!ce.Variables.Any(m => m.Key.ToUpper() == item.VariableName.ToUpper()))
                        {
                            try
                            {
                                ce.Variables.Add(item.VariableName, item.Value);
                            }
                            catch
                            {
                            }
                        }
                    }

                    //////if (!ce.Variables.Any(m => m.Key.ToUpper() == item.VariableName.ToUpper()))
                    //////{
                    //////    //try
                    //////    //{
                    //////    ce.Variables.Add(item.VariableName, item.Value);
                    //////    //}
                    //////    //catch
                    //////    //{
                    //////    //    string ttt = item.VariableName;
                    //////    //}

                    //////}
                }

                var x = ce.Parse(formula);
                object value = x.Evaluate();
                if (value.ToString() == "NaN")
                { value = 0; }
                result.Value = value;
                return result;
            }
            catch (Exception ex)
            {
                result.Value = 0;
                result.ErrorMessage = ex.Message;
                return result;
            }
        }
    }

    public class ElementFormula
    {
        /// <summary>
        /// Hàm khởi tạo phần tử
        /// </summary>
        /// <param name="_Name"></param>
        /// <param name="_value"></param>
        /// <param name="_orderNumber"></param>
        public ElementFormula()
        {

        }
        /// <summary>
        /// Hàm khởi tạo phần tử
        /// </summary>
        /// <param name="_Name"></param>
        /// <param name="_value"></param>
        /// <param name="_orderNumber"></param>
        public ElementFormula(string _Name, object _value, int _orderNumber)
        {
            VariableName = _Name;
            Value = _value;
            OrderNumber = _orderNumber;
            if (_value == null)
            {
                ErrorMessage = "Null";
            }
            else
            {
                ErrorMessage = string.Empty;
            }

        }
        public ElementFormula(string _Name, object _value, int _orderNumber, string _errorMessage)
        {
            VariableName = _Name;
            Value = _value;
            OrderNumber = _orderNumber;
            ErrorMessage = _errorMessage;
        }


        /// <summary>
        /// Tên biến của phần tử
        /// </summary>
        public string VariableName { get; set; }

        /// <summary>
        /// Giá trị của phần tử
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Số thứ tự tính toán trước sau của các phần tử
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Thông báo lỗi
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
