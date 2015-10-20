using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrHtmlExtensions
    {
        /// <summary>
        /// Lấy id của feld hiện tại, hỗ trợ cho xử lý javascript 
        /// Vd: $('#@Html.FieldIdFor(model => model.Definition)').val() 
        ///  tương đương với $('#Definition').val()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string FieldIdFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            return id.Replace('[', '_').Replace(']', '_');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="multiInfo"></param>
        /// <param name="urlGetData">GetProfileByOrgId</param>
        /// <returns></returns>
        public static IHtmlString ChooseEmpOrOrg(this HtmlHelper html, MultiSelectBuilderInfo multiInfo, string urlGetData)
        {
            if (multiInfo != null && !string.IsNullOrEmpty(urlGetData))
            {
                var nameMulti = multiInfo.Name;
                if (!string.IsNullOrEmpty(nameMulti))
                {
                    var strHtml = "<div id='div-main-control'>" +
                                        "<div class='div-radio'>" +
                                           " <input type='radio' name='input-emp-org' id='radio-emp' value='radio-emp'>Chọn Nhân Viên &nbsp&nbsp" + ConstantDisplay.HRM_HR_ChooseEmp.TranslateString() +
                                            "<input type='radio' name='input-emp-org' value='radio-org'>" + ConstantDisplay.HRM_HR_ChooseOrg.TranslateString() +
                                        "</div>" +
                                       " <div id='div-treeview'>" +
                                           html.Partial("EditorTemplates/OrgStructureTreeViewDropdown") +
                                           //"<img src='/Content/images/icons/submenu/menu/HR_WorkHistory.png' id='refresh-emp' class='refresh-emp' />" +
                                           " <div>" +
                                           "     <input type='checkbox' id='exclude-emp'>Loại Trừ Nhân Viên" +
                                          "  </div>" +
                                        "</div>" +
                                       " <div id='div-multiSelect'>" +
                                        html.VnrMultiSelect(multiInfo) +
                                         html.Hidden("ListIdSelected") +
                                       " </div>" +
                                   " </div>" +
                                    "<script>$(document).ready(function () {" +
                                            "function display(id, status) {document.getElementById(id).style.display = status;}" +
                                            "function checked(id, isChecked) {document.getElementById(id).checked = isChecked;}" +
                                           " function ischecked(id) {return document.getElementById(id).checked;}" +
                                         "   var DivMultiSelect = 'div-multiSelect', DivTreeview = 'div-treeview', RadioEmp = 'radio-emp'," +
                                               " RadioOrg = 'radio-org', ExcludeEmp = 'exclude-emp', InputEmpOrg = 'input-emp-org';" +

                                            "checked(RadioEmp, true); display(DivTreeview, 'none'); var multiSelect = $('#" + nameMulti + "').data('kendoMultiSelect');" +
                                           " $('input[type=radio][name=' + InputEmpOrg + ']').change(function () {" +
                                             "   if (this.value == RadioEmp) { display(DivMultiSelect, '');multiSelect.value(''); display(DivTreeview, 'none');$('#TreeViewDropDownInput').val('');RemoveTreeItemSelect('TreeViewDropDownInput');checkedNodes = [];" +
                                              "  } else if (this.value == RadioOrg) { multiSelect.value(''); display(DivTreeview, ''); checked(ExcludeEmp, false);display(DivMultiSelect, 'none');}" +
                                            "});" +
                                           " $('input[type=checkbox][id=' + ExcludeEmp + ']').change(function () {" +
                                                "if (ischecked(ExcludeEmp)) {display(DivMultiSelect, '');var checkedNodes = [];var treeView = $('#treeviewOrg').data('kendoTreeView');" +
                                                   " checkedNodeOrderNumbers(treeView.dataSource.view(), checkedNodes);" +
                                                "} else {display(DivMultiSelect, 'none');multiSelect.value('');checkedNodes = [];}" +
                                            "});});</script>";

                                        //"$('#refresh-emp').click(function () {" +
                                        //        "var checkedNodes = [], treeView = $('#treeviewOrg').data('kendoTreeView'), multiSelect = $('#" + nameMulti + "').data('kendoMultiSelect');" +
                                        //       " checkedNodeOrderNumbers(treeView.dataSource.view(), checkedNodes);" +
                                        //        "$.ajax({" +
                                        //           " type: 'POST', url:'" + urlGetData + "'," +
                                        //            "data: { orgs: checkedNodes.join(','), parameter: multiSelect.value().join(',') }," +
                                        //            "dataType: 'json',success: function (result) {" +
                                        //                "if (typeof result == 'object') {alert('Có ' + result[0] + ' nhân viên được chọn');$('#ListIdSelected').val(result[1]);" +
                                        //                "}else {alert('Có lỗi trong quá trình xử lý');}" +
                                        //            "},error: function (result) {alert(result.responseText);}" +
                                        //        "});checkedNodes = []});</script>"

                                  ;

                    return html.Raw(strHtml);
                }
            }
            return null;
        }

        public static IHtmlString ChooseEmpOrOrgFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, MultiSelectBuilderInfo multiInfo, string urlGetData)
        {
            if (multiInfo != null && !string.IsNullOrEmpty(urlGetData))
            {
                var fieldName = html.FieldIdFor(expression);
                var strHtml = "<div id='div-main-control'>" +
                                        "<div class='div-radio'>" +
                                           " <input type='radio' name='input-emp-org' id='radio-emp' value='radio-emp'> Chọn Nhân Viên &nbsp&nbsp"  +
                                            "<input type='radio' name='input-emp-org' value='radio-org' id='radio-org'>Chọn Phòng Ban" +
                                        "</div>" +
                                       " <div id='div-treeview'>" +
                                           html.Partial("EditorTemplates/OrgStructureTreeViewDropdown") +
                                           //"<img src='/Content/images/icons/submenu/menu/HR_WorkHistory.png' id='refresh-emp' class='refresh-emp' />" +
                                           " <div>" +
                                           "     <label><input type='checkbox' id='exclude-emp'>Loại Trừ Nhân Viên </label>" +
                                          "  </div>" +
                                        "</div>" +
                                       " <div id='div-multiSelect'>" +
                                        html.VnrMultiSelectFor(expression, multiInfo) +
                                         html.Hidden("ListIdSelected") +
                                       " </div>" +
                                   " </div>" +
                                    "<script>$(document).ready(function () {" +
                                            "function display(id, status) {document.getElementById(id).style.display = status;}" +
                                            "function checked(id, isChecked) {document.getElementById(id).checked = isChecked;}" +
                                           " function ischecked(id) {return document.getElementById(id).checked;}" +
                                         "   var DivMultiSelect = 'div-multiSelect', DivTreeview = 'div-treeview', RadioEmp = 'radio-emp'," +
                                               " RadioOrg = 'radio-org', ExcludeEmp = 'exclude-emp', InputEmpOrg = 'input-emp-org';" +

                                            "checked(RadioEmp, true); display(DivTreeview, 'none'); var multiSelect = $('#" + fieldName + "').data('kendoMultiSelect');" +
                                           " $('input[type=radio][name=' + InputEmpOrg + ']').change(function () {" +
                                             "   if (this.value == RadioEmp) { display(DivMultiSelect, '');multiSelect.value(''); display(DivTreeview, 'none');$('#TreeViewDropDownInput').val('');RemoveTreeItemSelect('TreeViewDropDownInput');checkedNodes = [];" +
                                              "  } else if (this.value == RadioOrg) { multiSelect.value(''); display(DivTreeview, ''); checked(ExcludeEmp, false);display(DivMultiSelect, 'none');}" +
                                            "});" +
                                           " $('input[type=checkbox][id=' + ExcludeEmp + ']').change(function () {" +
                                                "if (ischecked(ExcludeEmp)) {display(DivMultiSelect, '');var checkedNodes = [];var treeView = $('#treeviewOrg').data('kendoTreeView');" +
                                                   " checkedNodeOrderNumbers(treeView.dataSource.view(), checkedNodes);" +
                                                "} else {display(DivMultiSelect, 'none');multiSelect.value('');checkedNodes = [];}" +
                                                "});});</script>";
                // +

                //                        "$('#refresh-emp').click(function () {" +
                //                                "var checkedNodes = [], treeView = $('#treeviewOrg').data('kendoTreeView'), multiSelect = $('#" + fieldName + "').data('kendoMultiSelect');" +
                //                               " checkedNodeOrderNumbers(treeView.dataSource.view(), checkedNodes);" +
                //                                "$.ajax({" +
                //                                   " type: 'POST', url:'" + urlGetData + "'," +
                //                                    "data: { orgs: checkedNodes.join(','), parameter: multiSelect.value().join(',') }," +
                //                                    "dataType: 'json',success: function (result) {" +
                //                                        "if (typeof result == 'object') {alert('Có ' + result[0] + ' nhân viên được chọn');$('#ListIdSelected').val(result[1]);" +
                //                                        "}else {alert('Có lỗi trong quá trình xử lý');}" +
                //                                    "},error: function (result) {alert(result.responseText);}" +
                //                                "});checkedNodes = []});</script>";

                return html.Raw(strHtml);
            }
            return null;
        }

        public static MvcHtmlString VnrDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return VnrDisplayFor<TModel, TProperty>(htmlHelper, expression, null);
        }

        public static MvcHtmlString VnrDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (metadata.ModelType == typeof(DateTime) || metadata.ModelType == typeof(DateTime?))
            {
                metadata.DisplayFormatString = "{0:dd/MM/yyyy}";
            }

            //var ddd = htmlHelper.DisplayFor(expression, string.Format(metadata.DisplayFormatString, metadata.Model), htmlAttributes);
            //return ddd;
            if (metadata.Model != null && metadata.DisplayFormatString != null)
            {
                MvcHtmlString str = new MvcHtmlString(string.Format(metadata.DisplayFormatString, metadata.Model));
                return str;
            }

            return htmlHelper.DisplayFor(expression);
        }
    }

}
