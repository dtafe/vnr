using System;
using HRM.Infrastructure.Utilities;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System.Collections.Generic;
using System.Linq;
using VnResource.Helper.Data;
using System.Web.Mvc;
using VnResource.Helper.Utility;
using System.Collections;


namespace HRM.Presentation.UI.Controls.Kendo
{
    #region VnrGridBuilder

    public static class VnrGridBuilder
    {


        public static GridBuilder<T> VnrGrid<T>(this HtmlHelper helper,
            GridBuilderInfo builderInfo) where T : class
        {
            #region PageableBuilder

            Action<PageableBuilder> pageable = new Action<PageableBuilder>(d => d.PageSizes(builderInfo.ShowPageSize).Input(builderInfo.ShowInputPageNumber).PreviousNext(builderInfo.ShowNextPrevious).Numeric(builderInfo.ShowPageNumber).Refresh(true).ButtonCount(5));
            builderInfo.ObjectType = builderInfo.ObjectType != null ? builderInfo.ObjectType : typeof(T);

            Action<GridSelectionSettingsBuilder> selection = new Action<GridSelectionSettingsBuilder>(d =>
                d.Mode(builderInfo.SelectionMode).Type(builderInfo.SelectionType));

            Action<GridToolBarCommandFactory<T>> toolBar = new Action<GridToolBarCommandFactory<T>>(d =>
            {
                if (builderInfo.ShowCreateButton && !string.IsNullOrWhiteSpace(builderInfo.CreateActionUrl))
                {
                    d.Create().Text(DefaultConstants.Create.TranslateString());
                }
                if (builderInfo.ShowSaveButton && !string.IsNullOrWhiteSpace(builderInfo.EditActionUrl))
                {
                    d.Save().Text(DefaultConstants.Create.TranslateString());
                }
                if (builderInfo.ShowCustomCreate && !string.IsNullOrWhiteSpace(builderInfo.EventCustomAdd))
                {
                    d.Custom().Text(DefaultConstants.Create.TranslateString()).Url("#").HtmlAttributes(new { onclick = builderInfo.EventCustomAdd });
                }

                if (builderInfo.ShowCustomSaveChanges && !string.IsNullOrWhiteSpace(builderInfo.EventCustomSaveChanges) && !string.IsNullOrWhiteSpace(builderInfo.EventCustomCancelChanges))
                {
                    d.Custom().Text(builderInfo.CustomSaveChangesText).Url("#").HtmlAttributes(new { onclick = builderInfo.EventCustomSaveChanges });
                    d.Custom().Text(builderInfo.CustomCancelChangesText).Url("#").HtmlAttributes(new { onclick = builderInfo.EventCustomCancelChanges });
                }

            });

            Action<GridEditingSettingsBuilder<T>> editing = new Action<GridEditingSettingsBuilder<T>>(d =>
            {
                d.Mode(builderInfo.EditMode);
                if (!string.IsNullOrWhiteSpace(builderInfo.EditTemplate))
                {
                    d.TemplateName(builderInfo.EditTemplate);
                }
            });

            Action<GridEventBuilder> events = new Action<GridEventBuilder>(d =>
            {
                //if (builderInfo.ForeignKey.Count>0)
                //    d.Save("onSave");
                if (!string.IsNullOrWhiteSpace(builderInfo.SelectionHandler))
                {
                    d.Change(builderInfo.SelectionHandler);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.EventSaveChanges))
                {
                    d.SaveChanges(builderInfo.EventSaveChanges);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.EventDataBound))
                {
                    d.DataBound(builderInfo.EventDataBound);
                }
                else
                {
                    //[Tung.Ly 20140908 ] : dùng mặc định (viết ở _layout.cshtml ở main)
                    d.DataBound("dataBound");
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.EvenEdit))
                {
                    d.Edit(builderInfo.EvenEdit);
                }


            });

            #endregion

            #region GridColumnFactory


            var lockedCol = false;
            if (builderInfo.LockedFields != null && builderInfo.LockedFields.Any())
            {
                lockedCol = true;
            }

            Action<GridColumnFactory<T>> columns = new Action<GridColumnFactory<T>>(column =>
            {
                if (builderInfo.ValueFields != null)
                {
                    column.AutoGenerate(builderInfo.ValueFields.Count() == 0);

                    if (builderInfo.ShowCheckbox)
                    {
                        if (builderInfo.ShowCheckAll)
                        {
                            column.Bound("").Title("").HeaderTemplate("<input type='checkbox' id='mastercheckbox' /><div style='border: solid 1px #c5c5c5; margin-top: -3px; border-radius: initial; display: none; width: 60px; position: fixed; z-index: 99; background-color: #F8F8F8; ' class='hrm_treeview' id='MultipleSelectForPage'> <label><input type='radio' value='0' name='selectOnePageOrAllPage' checked /> Page</label><br /> <label><input value='1' type='radio' name='selectOnePageOrAllPage' /> All</label></div>").ClientTemplate("<input type='checkbox' value='#=ID#' class='checkboxGroups'/>").Width(25).Locked(lockedCol).Sortable(false);
                        }
                        else
                        {
                            column.Bound("").Title("").ClientTemplate("<input type='checkbox' value='#=ID#' class='checkboxGroups'/>").Width(25).Locked(lockedCol).Sortable(false);
                        }

                    }
                    if (builderInfo.ShowEditIcon)
                    {
                        column.Bound("").Title("").ClientTemplate("<img src='/Content/images/icons/edit-file-icon.png' title='Edit' alt='Edit' name='gridEditImgButton' class='gridEditImgButton' />").Width(30).Locked(lockedCol).Sortable(false);
                    }
                    if (builderInfo.ShowCommand)
                    {
                        column.Command(d =>
                        {
                            if (builderInfo.ShowEditButton && !string.IsNullOrWhiteSpace(builderInfo.EditActionUrl))
                            {
                                d.Edit()
                                    .Text(string.Empty)
                                    .UpdateText(DefaultConstants.Save.TranslateString())
                                    .CancelText(string.Empty);
                                // d.Edit().HtmlAttributes(new{@class="abdd"})

                            }

                            if (builderInfo.ShowDeleteButton &&
                                !string.IsNullOrWhiteSpace(builderInfo.DeleteActionUrl))
                            {
                                d.Destroy().Text(DefaultConstants.Delete.TranslateString());
                            }
                            if (builderInfo.ShowCustomEdit && !string.IsNullOrWhiteSpace(builderInfo.EventCustomEdit))
                            {
                                d.Custom(builderInfo.EditText).HtmlAttributes(new { @class = "k-icon k-i-pencil" }).Click(builderInfo.EventCustomEdit);
                            }
                        }).Width(170);
                    }


                    foreach (var valueField in builderInfo.ValueFields)
                    {
                        var locked = false;
                        if (builderInfo.LockedFields != null)
                        {
                            if (builderInfo.LockedFields.Contains(valueField))
                            {
                                locked = true;
                            }
                        }
                        GridBoundColumnBuilder<T> bound = null;

                        string displayField = string.Empty, formatField = string.Empty, hyperlinkField = string.Empty, template = string.Empty;
                        bool filterable = false;
                        Type fieldValueType = GetFieldValueType(builderInfo, valueField);
                        if (fieldValueType != null)
                        {
                            //Biến phân biệt đã add foreignKey hay chưa
                            bool _tmp = false;

                            foreach (var foreignKey in builderInfo.ForeignKey)
                            {
                                if (foreignKey.Key.ToString() == valueField)
                                {
                                    if (foreignKey.Key != null && foreignKey.Value != null && foreignKey.Value.Count() > 3)
                                    {
                                        IList lstFk = (IList)foreignKey.Value[2];
                                        if (lstFk != null)
                                        {
                                            if (foreignKey.Value[4].ToString() != string.Empty)
                                            {
                                                bound = column.ForeignKey(foreignKey.Key.ToString(), lstFk, foreignKey.Value[0].ToString(), foreignKey.Value[1].ToString()).Width(150).Title(foreignKey.Value[3].ToString().TranslateString()).EditorTemplateName(foreignKey.Value[4].ToString());
                                            }
                                            else
                                            {
                                                bound = column.ForeignKey(foreignKey.Key.ToString(), lstFk, foreignKey.Value[0].ToString(), foreignKey.Value[1].ToString()).Width(150).Title(foreignKey.Value[3].ToString().TranslateString());
                                            }
                                        }
                                        _tmp = true;
                                        break;
                                    }
                                }
                            }
                            if (!_tmp)
                            {
                                bound = column.Bound(fieldValueType, valueField).Locked(locked);
                                if (fieldValueType.IsBoolean())
                                {
                                    bound.ClientTemplate("<input type='checkbox' disabled='" + builderInfo.DisabledCheckbox + "' #= " + valueField + " ? 'checked=checked':'' # class='chkbx' />");
                                }
                                else if (fieldValueType.IsDateTime() || fieldValueType.IsFloat() || fieldValueType.IsNumeric() || fieldValueType.IsDouble() || fieldValueType.IsInteger() || fieldValueType.IsDecimal())
                                {
                                    bound.HtmlAttributes(new { style = "text-align:right" });
                                }
                                if (fieldValueType.IsDouble())
                                {
                                    bound.EditorTemplateName("Number");
                                }
                                if (fieldValueType.IsFloat())
                                {
                                    bound.EditorTemplateName("Percent");
                                }
                            }
                        }
                        else
                        {
                            bound = column.Bound(valueField).Locked(locked);
                        }

                        if (builderInfo.HasDisplayFields())
                        {
                            displayField = builderInfo.DisplayFields.Where(d => d.Key
                                == valueField).Select(d => d.Value).FirstOrDefault();
                        }

                        if (string.IsNullOrWhiteSpace(displayField))
                        {
                            displayField = valueField;
                        }
                        bound.Title(displayField.TranslateString());
                        #region Format Column
                        if (builderInfo.HasFormatFields())
                        {
                            formatField = builderInfo.FormatFields.Where(d => d.Key
                                == valueField).Select(d => d.Value).FirstOrDefault();
                        }

                        if (builderInfo.HasHyperlink())
                        {
                            hyperlinkField = builderInfo.HyperlinkFields.Where(d => d.Key
                                == valueField).Select(d => d.Value).FirstOrDefault();
                        }
                        if (builderInfo.HasFilterable())
                        {
                            filterable = builderInfo.Filterable.Where(d => d.Key
                                == valueField).Select(d => d.Value).FirstOrDefault();
                        }

                        //bound.Filterable(filterable);
                        bound.Filterable(filterable).Filterable(_filterable => _filterable
                                           .Extra(false)
                                           .Operators(ops =>
                                               ops.ForString(str => str.Clear()
                                                                       .Contains("Contains")
                                                                       .StartsWith("Starts with")
                                                                       .EndsWith("Ends with")
                                                                       .IsEqualTo("Is equal to"))
                                           ));

                        if (builderInfo.HasTemplate())
                        {
                            template = builderInfo.Template.Where(d => d.Key
                                == valueField).Select(d => d.Value).FirstOrDefault();
                        }

                        if (builderInfo.HasSizeFields())
                        {
                            int fieldWidth = builderInfo.SizeFields.Where(d => d.Key
                                == valueField).Select(d => d.Value).FirstOrDefault();

                            if (fieldWidth >= 10)
                            {
                                bound.Width(fieldWidth);
                            }
                        }

                        if (string.IsNullOrWhiteSpace(formatField))
                        {
                            formatField = GetDefaultFormat(fieldValueType);
                        }

                        if (!string.IsNullOrWhiteSpace(hyperlinkField))
                        {
                            var itemHyperlink = hyperlinkField.Split(',');
                            if (itemHyperlink.Count() > 1)
                            {
                                bound.ClientTemplate("<a onClick=\"" + itemHyperlink[0] + "('#=" + itemHyperlink[1] + "#')\">#=" + valueField + "#</a>");
                            }
                            else
                            {
                                bound.ClientTemplate("<a onClick=\"" + hyperlinkField + "('#=ID#')\">#=" + valueField + "#</a>");
                            }
                        }
                        if (builderInfo.SumFields != null && builderInfo.SumFields.Any())
                        {
                            foreach (var field in builderInfo.SumFields)
                            {
                                if (field == valueField)
                                {
                                    var str = "#=sum#";
                                    if (!string.IsNullOrEmpty(builderInfo.FormatSum))
                                    {
                                        str = "#=kendo.toString(sum,'" + builderInfo.FormatSum + "')#";
                                    }
                                    bound.ClientFooterTemplate(str);
                                    break;
                                }
                            }
                        }

                        if (builderInfo.GroupHeaderTemplate != null && builderInfo.GroupFields != null && builderInfo.GroupFields.Any())
                        {
                            var header = builderInfo.GroupHeaderTemplate;
                            foreach (var field in builderInfo.GroupFields)
                            {
                                if (header.ContainsKey(field) && !string.IsNullOrEmpty(header[field].ToString()))
                                {
                                    bound.ClientGroupHeaderTemplate(header[field]);
                                    break;
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(formatField))
                        {
                            if (!formatField.Contains("{") || !formatField.Contains("}"))
                            {
                                formatField = "{0:" + formatField + "}";
                            }

                            bound.Format(formatField);
                        }

                        if (builderInfo.HiddenFields != null && builderInfo.HiddenFields.Contains(valueField))
                        {
                            bound.Hidden(true);
                        }


                        #endregion

                    }

                    //foreach (var foreignKey in builderInfo.ForeignKey)
                    //{
                    //    if (foreignKey.Key != null && foreignKey.Value != null && foreignKey.Value.Count() > 3)
                    //    {
                    //        IList lstFk = (IList)foreignKey.Value[2];
                    //        if (lstFk != null)
                    //        {
                    //            if (foreignKey.Value[4].ToString() != string.Empty)
                    //            {
                    //                column.ForeignKey(foreignKey.Key.ToString(), lstFk, foreignKey.Value[0].ToString(), foreignKey.Value[1].ToString()).Width(150).Title(foreignKey.Value[3].ToString().TranslateString()).EditorTemplateName(foreignKey.Value[4].ToString());
                    //            }
                    //            else
                    //            {
                    //                column.ForeignKey(foreignKey.Key.ToString(), lstFk, foreignKey.Value[0].ToString(), foreignKey.Value[1].ToString()).Width(150).Title(foreignKey.Value[3].ToString().TranslateString());
                    //            }
                    //        }
                    //    }
                    //}
                    //column.Bound("").Title("").Sortable(false).Filterable(false).CompareByOperator(;
                    column.Bound("").Title("").Sortable(false).Filterable(filterable => filterable
                                           .Extra(false)
                                           .Operators(ops =>
                                               ops.ForString(str => str.Clear()
                                                                       .Contains("Contains")
                                                                       .StartsWith("Starts with")
                                                                       .EndsWith("Ends with")
                                                                       .IsEqualTo("Is equal to"))
                                           ));

                }
            });

            #endregion

            #region DataSourceBuilder
            Action<DataSourceBuilder<T>> dataSource = new Action<DataSourceBuilder<T>>(data =>
            {
                AjaxDataSourceBuilder<T> ajaxDataSource = data.Ajax();
                ajaxDataSource.Events(e => e.Error("ErrorHandler"));
                ajaxDataSource.Batch(builderInfo.Batch);

                int pos = Array.IndexOf(builderInfo.ValueFields, builderInfo.DefaultSortField);
                if (pos > -1)
                {
                    ajaxDataSource.Sort(sort => sort.Add(builderInfo.DefaultSortField).Descending());
                }

                if (!string.IsNullOrWhiteSpace(builderInfo.EventError))
                {
                    ajaxDataSource.Events(e => e.Error(builderInfo.EventError));
                }

                if (builderInfo.GroupFields != null && builderInfo.GroupFields.Count() > 0)
                {
                    foreach (var groupField in builderInfo.GroupFields)
                    {
                        if (!string.IsNullOrWhiteSpace(groupField))
                        {
                            ajaxDataSource.Group(group =>
                            {
                                group.Add(groupField, GetFieldValueType(builderInfo, groupField));
                            });
                        }
                    }
                }

                if (builderInfo.SumFields != null && builderInfo.SumFields.Count() > 0)
                {
                    ajaxDataSource.Aggregates(agg =>
                    {
                        foreach (var sumField in builderInfo.SumFields)
                        {
                            if (!string.IsNullOrWhiteSpace(sumField))
                            {
                                agg.Add(sumField, GetFieldValueType(builderInfo, sumField)).Sum();
                            }
                        }
                    });
                }

                if (builderInfo.PageSize > 0)
                {
                    ajaxDataSource.PageSize(builderInfo.PageSize);
                }
                if (!builderInfo.ServerOperation)
                {
                    ajaxDataSource.ServerOperation(builderInfo.ServerOperation);
                }

                ajaxDataSource.Model(model =>
                {
                    if (!string.IsNullOrWhiteSpace(builderInfo.IdPropertyName))
                    {
                        model.Id(builderInfo.IdPropertyName);
                    }

                    foreach (var valueField in builderInfo.ValueFields)
                    {
                        Type fieldValueType = GetFieldValueType(builderInfo, valueField);
                        bool editable = true;

                        if (builderInfo.DisableFields != null)
                        {
                            editable = !builderInfo.DisableFields.Contains(valueField);
                        }

                        if (fieldValueType != null)
                        {
                            model.Field(valueField, fieldValueType).Editable(editable);
                        }
                        else
                        {
                            model.Field(valueField, typeof(string)).Editable(editable);
                        }
                    }
                });

                if (!string.IsNullOrWhiteSpace(builderInfo.ReadActionUrl))
                {
                    if (builderInfo.ReadActionUrl.Contains("/"))
                    {
                        if (!string.IsNullOrWhiteSpace(builderInfo.ReadData))
                        {
                            //ajaxDataSource.Read(action => action.Url(builderInfo.ReadActionUrl).Data("OverWriteReadDataOnGrid(" + builderInfo.ReadData + ")"));
                            ajaxDataSource.Read(action => action.Url(builderInfo.ReadActionUrl).Data(builderInfo.ReadData));
                        }
                        else
                        {
                            //Trường hợp ReadActionUrl là một đường dẫn url
                            ajaxDataSource.Read(action => action.Url(builderInfo.ReadActionUrl));
                        }
                    }
                    else
                    {
                        //Trường hợp ReadActionUrl là một action name trong một controller
                        if (builderInfo.ReadParameter != null)
                        {
                            ajaxDataSource.Read(builderInfo.ReadActionUrl, builderInfo.Controller, builderInfo.ReadParameter);

                        }
                        if (!string.IsNullOrWhiteSpace(builderInfo.ReadData))
                        {
                            ajaxDataSource.Read(r => r.Action(builderInfo.ReadActionUrl, builderInfo.Controller).Data(builderInfo.ReadData));
                        }
                        //ajaxDataSource.Read(builderInfo.ReadActionUrl, builderInfo.Controller);
                    }
                }

                if (!string.IsNullOrWhiteSpace(builderInfo.CreateActionUrl))
                {
                    if (builderInfo.CreateActionUrl.Contains("/"))
                    {
                        //Trường hợp ReadActionUrl là một đường dẫn url
                        ajaxDataSource.Create(action => action.Url(builderInfo.CreateActionUrl));
                    }
                    else
                    {
                        //Trường hợp ReadActionUrl là một action name trong một controller
                        ajaxDataSource.Create(builderInfo.CreateActionUrl, builderInfo.Controller);
                    }
                }

                if (!string.IsNullOrWhiteSpace(builderInfo.EditActionUrl))
                {
                    if (builderInfo.EditActionUrl.Contains("/"))
                    {
                        //Trường hợp ReadActionUrl là một đường dẫn url
                        ajaxDataSource.Update(action => action.Url(builderInfo.EditActionUrl));
                    }
                    else
                    {
                        //Trường hợp ReadActionUrl là một action name trong một controller
                        ajaxDataSource.Update(builderInfo.EditActionUrl, builderInfo.Controller);
                    }
                }

                if (!string.IsNullOrWhiteSpace(builderInfo.DeleteActionUrl))
                {
                    if (builderInfo.DeleteActionUrl.Contains("/"))
                    {
                        //Trường hợp ReadActionUrl là một đường dẫn url
                        ajaxDataSource.Destroy(action => action.Url(builderInfo.DeleteActionUrl));
                    }
                    else
                    {
                        //Trường hợp ReadActionUrl là một action name trong một controller
                        ajaxDataSource.Destroy(builderInfo.DeleteActionUrl, builderInfo.Controller);
                    }
                }
            });

            #endregion

            #region GridBuilder

            var gridbuilder = helper.Kendo().Grid<T>()
                .Name(builderInfo.Name)
                .Groupable(d => d.Enabled(builderInfo.ShowGroupPanel))
                .AutoBind(builderInfo.AutoBind)
                .Resizable(d => d.Columns(true))
                .Reorderable(d => d.Columns(true))
                .Sortable(d => d.SortMode(GridSortMode.MultipleColumn))
                .ToolBar(toolBar)
                .Selectable(selection)
                .Pageable(pageable)
                .Columns(columns)
                .ColumnMenu()
                .Filterable()
                .Editable(editing)
                .DataSource(dataSource)
                .Events(events)
                //.ToolBar(tb =>
                //{
                //    tb.Custom().Name("ChangeColumnMode").Text("<img alt='icon' class='k-image' src='../Content/images/icons/submenu/menu/Sys_table_edit.png'>ChangeColumnMode").Url("#").HtmlAttributes(new { onclick = "ClickButtonChangeColum2('" + builderInfo.ObjectType.Name + "')" });
                //    tb.Custom().Name("update-inventory").Text("Update Inventory").HtmlAttributes(
                //        new {onclick = "onUpdateInventory()", title = "Update the system inventory from the OMS", @class="k-refresh"});
                //} )
                ;

            if (!string.IsNullOrEmpty(builderInfo.ClientDetailTemplateId))
            {
                gridbuilder.ClientDetailTemplateId(builderInfo.ClientDetailTemplateId);
            }
            IDictionary<string, object> attributes = new Dictionary<string, object>();
            var strStyle = string.Empty;
            if (builderInfo.GridHeight > 0)
            {
                strStyle += "height:" + builderInfo.GridHeight + "px; ";
            }
            if (builderInfo.GridWidth > 0)
            {
                strStyle += "width:" + builderInfo.GridWidth + "px; ";
            }
            if (!string.IsNullOrEmpty(strStyle))
            {
                attributes.Add("style", strStyle);
            }
            if (attributes.Any())
            {
                gridbuilder.HtmlAttributes(attributes);
            }
            if (builderInfo.ColumnFilterable)
            {
                gridbuilder.Filterable();
            }

            if (builderInfo.ColumnMenu)
            {
                gridbuilder.ColumnMenu();
            }
            if (builderInfo.Scrollable)
            {
                if (builderInfo.ScrollableHeight > 0)
                {
                    gridbuilder.Scrollable(s => s.Height("auto"));
                }
                else
                {
                    gridbuilder.Scrollable();
                }
            }
            if (builderInfo.Navigatable)
            {
                gridbuilder.Navigatable();
            }
            if (builderInfo.ToClientTemplate)
            {
                gridbuilder.ToClientTemplate();
            }
            return gridbuilder;

            #endregion
        }

        #region Methods

        private static Type GetFieldValueType(GridBuilderInfo builderInfo, string fieldName)
        {
            Type fieldValueType = typeof(string);

            if (builderInfo.TypeFields != null && builderInfo.TypeFields.Any(d => d.Key == fieldName))
            {
                fieldValueType = builderInfo.TypeFields.Where(d => d.Key == fieldName).Select(d => d.Value).FirstOrDefault();
            }
            else if (builderInfo.ObjectType != null && builderInfo.ObjectType.HasProperty(fieldName))
            {
                fieldValueType = builderInfo.ObjectType.GetPropertyType(fieldName);
            }

            return fieldValueType;
        }

        private static string GetDefaultFormat(Type fieldValueType)
        {
            string result = string.Empty;

            if (fieldValueType.IsDateTime())
            {
                result = "dd/MM/yyyy";
            }
            else if (fieldValueType.IsInteger() || fieldValueType.IsFloat()
                || fieldValueType.IsDouble() || fieldValueType.IsDecimal())
            {
                result = fieldValueType.IsInteger() ? "N0" : "N2";
            }

            return result;
        }

        #endregion
    }

    #endregion

    #region GridBuilderInfo

    public class GridBuilderInfo : VnrBaseBuilderInfo
    {
        #region Properties
        /// <summary>
        /// Hàm khởi tạo tham số default cho các thuộc tính
        /// </summary>
        public GridBuilderInfo()
        {
            ShowCheckbox = true;
            ShowEditIcon = true;
            ShowPageSize = false;
            ShowNextPrevious = true;
            ShowPageNumber = true;
            Scrollable = true;
            ShowCustomEdit = false;
            ShowCreateButton = false;
            ShowEditButton = false;
            ShowSaveButton = false;
            ShowCustomCreate = false;
            ShowDeleteButton = false;
            ShowCommand = false;
            DisabledCheckbox = false;
            AutoBind = false;
            Navigatable = false;
            ShowInputPageNumber = false;
            Batch = false;
            ColumnMenu = false;
            ColumnFilterable = false;
            ServerOperation = true;
            PageSize = 50;
            GridHeight = 650;
            FormatSum = "{0:n}";
            ScrollableHeight = 600;
            EditMode = GridEditMode.PopUp;
            DefaultSortField = "DateUpdate";
            EditText = "Chỉnh Sửa";
            ShowCheckAll = true;
            CustomSaveChangesText = "Lưu Thay Đổi";
            CustomCancelChangesText = "Hủy Thay Đổi";
            ToClientTemplate = false;
        }

        #region ForeignKey

        #endregion

        public IDictionary<string, object> HtmlAttributes { get; set; }

        public string EditText { get; set; }
        /// <summary>
        /// [Hien.Nguyen] Hiển thị checkbox chọn tất cả trên lưới (defaule true)
        /// </summary>
        public bool ShowCheckAll { get; set; }
        public string DefaultSortField { get; set; }
        /// <summary>
        /// Hiển thị checkbox trên lưới Default: true
        /// </summary>
        public bool ShowCheckbox { get; set; }
        /// <summary>
        /// Hiển thị nút Create trên toolbar của lưới  Default: false
        /// </summary>
        public bool ShowCreateButton { get; set; }
        /// <summary>
        /// Hiển thị icon edit trên lưới Default: true
        /// </summary>
        public bool ShowEditIcon { get; set; }
        /// <summary>
        /// Hiển thị nút edit trên lưới Default: false
        /// </summary>
        public bool ShowEditButton { get; set; }
        /// <summary>
        /// Hiển thị nút lưu trên lưới Default: false
        /// </summary>
        public bool ShowSaveButton { get; set; }
        /// <summary>
        /// Hiển thị nút delete trên lưới Default: false
        /// </summary>
        public bool ShowDeleteButton { get; set; }
        /// <summary>
        /// Hiển thị nút edit do người dùng tự định nghĩa  Default : false
        /// </summary>
        public bool ShowCustomEdit { get; set; }
        /// <summary>
        /// Hiển thị nút Create do người dùng tự định nghĩa Default: false
        /// </summary>
        public bool ShowCustomCreate { get; set; }
        /// <summary>
        /// Hiển thị nút Lưu tất cả thay đổi trên lưới
        /// </summary>
        public bool ShowCustomSaveChanges { get; set; }
        /// <summary>
        /// Hiển thị cột chứa các nút chỉnh sửa, xóa ... Default: false
        /// </summary>
        public bool ShowCommand { get; set; }
        /// <summary>
        /// Tự động load dữ liệu cho lưới Default: false
        /// </summary>
        public bool AutoBind { get; set; }
        /// <summary>
        /// Cho phép sử dụng phím tắt trên lưới Default: false
        /// </summary>
        public bool Navigatable { get; set; }
        /// <summary>
        /// Cho phép người dùng thay đổi số dòng dữ liệu hiển thị trên một trang của lưới Default: true
        /// </summary>
        public bool ShowPageSize { get; set; }
        /// <summary>
        /// Hiển thị phân trang của lưới Default : true
        /// </summary>
        public bool ShowPageNumber { get; set; }
        /// <summary>
        /// Hiển thị nút chuyển trang tới và lùi Default : true
        /// </summary>
        public bool ShowNextPrevious { get; set; }
        /// <summary>
        /// Hiển thị textbox cho nhập trang muốn xem của lưới Default: false
        /// </summary>
        public bool ShowInputPageNumber { get; set; }
        /// <summary>
        /// Quy định số dòng dữ liệu hiển thị trên 1 trang của lưới Default: 20
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Sử dụng trong trường hợp muốn lưu nhiều dữ liệu bị thay đổi trên lưới cùng lúc Default: false
        /// </summary>
        public bool Batch { get; set; }
        /// <summary>
        /// Kiểu edit hoặc create trên lưới (Inline, Incell, PopUp) Default: PopUp
        /// </summary>
        public GridEditMode EditMode { get; set; }
        /// <summary>
        /// Cho phép sắp xếp theo cột trên lưới Default : false
        /// </summary>
        public bool Scrollable { get; set; }
        /// <summary>
        /// Cho phép lọc dữ liệu theo cột trên lưới Default: false
        /// </summary>
        public bool ColumnFilterable { get; set; }

        /// <summary>
        /// Cho phép Lấy dữ liệu phân trang từ server: Default:true
        /// </summary>
        public bool ServerOperation { get; set; }
        /// <summary>
        /// Hiển thị menu trên tiêu đề của column cho phép chọn các thao tác khác  Default : false
        /// </summary>
        public bool ColumnMenu { get; set; }
        /// <summary>
        /// Chiều cao của lưới tính theo px
        /// </summary>
        public int GridHeight { get; set; }
        /// <summary>
        /// Chiều rồng của lưới theo px
        /// </summary>
        public int GridWidth { get; set; }
        /// <summary>
        /// Chiều cao của lưới sẻ áp dụng thuộc tính Scroll Default: 400
        /// </summary>
        public int ScrollableHeight { get; set; }

        private bool showGroupPanel = false;
        private Dictionary<object, object[]> foreignKey;

        Dictionary<string, string> displayFields;
        Dictionary<string, string> formatFields;
        Dictionary<string, string> hyperlinkFields;
        Dictionary<string, Type> typeFields;
        Dictionary<string, bool> filterable;
        Dictionary<string, string> template;
        Dictionary<string, int> sizeFields;
        private string idPropertyName;

        public object ReadParameter { get; set; }
        public string EventCheckBox { get; set; }
        public string EventCustomEdit { get; set; }
        public string EventCustomAdd { get; set; }
        public string EventCustomSaveChanges { get; set; }
        public string CustomSaveChangesText { get; set; }
        public string EventCustomCancelChanges { get; set; }
        public string CustomCancelChangesText { get; set; }
        public string EventDataBound { get; set; }
        public string EventSaveChanges { get; set; }
        public string EventError { get; set; }
        public string ReadData { get; set; }
        public string EvenEdit { get; set; }
        public bool DisabledCheckbox { get; set; }

        public bool ShowGroupPanel
        {
            get { return showGroupPanel; }
            set { showGroupPanel = value; }
        }

        public GridSelectionMode SelectionMode { get; set; }

        public GridSelectionType SelectionType { get; set; }

        /// <summary>
        /// Tên sự kiện chọn trên lưới.
        /// </summary>
        public string SelectionHandler { get; set; }

        /// <summary>
        /// Tên template để mở màn hình edit.
        /// </summary>
        public string EditTemplate { get; set; }
        /// <summary>
        /// Template chi tiết cho row
        /// </summary>
        public string ClientDetailTemplateId { get; set; }
        public bool ToClientTemplate { get; set; }

        /// <summary>
        /// Tên cột làm ID cho dữ liệu hiện tại.
        /// </summary>
        public string IdPropertyName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(idPropertyName))
                {
                    idPropertyName = DefaultConstants.ID;
                }
                return idPropertyName;
            }
            set { idPropertyName = value; }
        }

        /// <summary>
        /// Tên method load dữ liệu.
        /// Có thể dùng Url hoặc action name.
        /// </summary>
        public string ReadActionUrl { get; set; }

        /// <summary>
        /// Tên method tạo mới dữ liệu.
        /// Có thể dùng Url hoặc action name.
        /// </summary>
        public string CreateActionUrl { get; set; }

        /// <summary>
        /// Tên method chỉnh sửa dữ liệu.
        /// Có thể dùng Url hoặc action name.
        /// </summary>
        public string EditActionUrl { get; set; }

        /// <summary>
        /// Tên method xóa dữ liệu.
        /// Có thể dùng Url hoặc action name.
        /// </summary>
        public string DeleteActionUrl { get; set; }


        /// <summary>
        /// Loại dự liệu hiển thị trên lưới.
        /// </summary>
        public Type ObjectType { get; set; }

        public string OrderField { get; set; }

        /// <summary>
        /// Danh sach field dữ liệu không cho chỉnh sửa.
        /// </summary>
        public string[] DisableFields { get; set; }

        /// <summary>
        /// Danh sách field dữ liệu được group mặc định.
        /// </summary>
        public string[] GroupFields { get; set; }
        public Dictionary<string, string> GroupHeaderTemplate { get; set; }

        /// <summary>
        /// Danh sách field dữ liệu được sum dữ liệu.
        /// </summary>
        public string[] SumFields { get; set; }
        /// <summary>
        /// Định dạng giá trị sum
        /// </summary>
        public string FormatSum { get; set; }

        /// <summary>
        /// Danh sách field dữ liệu hiển thị trên lưới.
        /// </summary>
        public string[] ValueFields { get; set; }

        /// <summary>
        /// Danh sách field bị khóa trên lưới.
        /// </summary>
        public string[] LockedFields { get; set; }
        /// <summary>
        /// Danh sách các field ẩn trên lưới
        /// </summary>
        public string[] HiddenFields { get; set; }

        public Dictionary<object, object[]> ForeignKey
        {
            get
            {
                if (foreignKey == null)
                {
                    foreignKey = new Dictionary<object, object[]>();
                }

                return foreignKey;
            }
            set
            {
                foreignKey = value;
            }
        }



        /// <summary>
        /// Tên hiển thị tương ứng theo valuefield.
        /// </summary>
        public Dictionary<string, string> DisplayFields
        {
            get
            {
                if (displayFields == null)
                {
                    displayFields = new Dictionary<string, string>();
                }

                return displayFields;
            }
            set
            {
                displayFields = value;
            }
        }

        /// <summary>
        /// Danh sách chuỗi format theo valuefield.
        /// </summary>
        public Dictionary<string, string> FormatFields
        {
            get
            {
                if (formatFields == null)
                {
                    formatFields = new Dictionary<string, string>();
                }

                return formatFields;
            }
            set
            {
                formatFields = value;
            }
        }

        public Dictionary<string, string> HyperlinkFields
        {
            get
            {
                if (hyperlinkFields == null)
                {
                    hyperlinkFields = new Dictionary<string, string>();
                }

                return hyperlinkFields;
            }
            set
            {
                hyperlinkFields = value;
            }
        }

        public Dictionary<string, bool> Filterable
        {
            get
            {
                if (filterable == null)
                {
                    filterable = new Dictionary<string, bool>();
                }

                return filterable;
            }
            set
            {
                filterable = value;
            }
        }

        public Dictionary<string, string> Template
        {
            get
            {
                if (template == null)
                {
                    template = new Dictionary<string, string>();
                }

                return template;
            }
            set
            {
                template = value;
            }
        }

        /// <summary>
        /// Danh sách loại dữ liệu theo valuefield.
        /// </summary>
        public Dictionary<string, Type> TypeFields
        {
            get
            {
                if (typeFields == null)
                {
                    typeFields = new Dictionary<string, Type>();
                }

                return typeFields;
            }
            internal set { typeFields = value; }
        }

        /// <summary>
        /// Danh sách độ rộng theo valuefield.
        /// </summary>
        public Dictionary<string, int> SizeFields
        {
            get
            {
                if (sizeFields == null)
                {
                    sizeFields = new Dictionary<string, int>();
                }

                return sizeFields;
            }
            set
            {
                sizeFields = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sử dụng hàm này để không vô tình khởi tạo danh sách.
        /// </summary>
        /// <returns></returns>
        internal bool HasDisplayFields()
        {
            return displayFields != null && displayFields.Count > 0;
        }

        internal bool HasTypeFields()
        {
            return typeFields != null && typeFields.Count > 0;
        }

        internal bool HasHyperlink()
        {
            return hyperlinkFields != null && hyperlinkFields.Count > 0;
        }

        internal bool HasFormatFields()
        {
            return formatFields != null && formatFields.Count > 0;
        }

        internal bool HasSizeFields()
        {
            return sizeFields != null && sizeFields.Count > 0;
        }
        internal bool HasFilterable()
        {
            return filterable != null && filterable.Count > 0;
        }
        internal bool HasTemplate()
        {
            return template != null && template.Count > 0;
        }

        public void SetFormat(string fieldName, Type fieldType, string format, int width)
        {
            if (!string.IsNullOrWhiteSpace(fieldName))
            {
                if (!string.IsNullOrWhiteSpace(format))
                {
                    if (!FormatFields.ContainsKey(fieldName))
                    {
                        FormatFields.Add(fieldName, format);
                    }
                }

                if (fieldType != null)
                {
                    if (!TypeFields.ContainsKey(fieldName))
                    {
                        TypeFields.Add(fieldName, fieldType);
                    }
                }

                if (width > 0)
                {
                    if (!SizeFields.ContainsKey(fieldName))
                    {
                        SizeFields.Add(fieldName, width);
                    }
                }
            }
        }

        public void SetFormat(string fieldName, Type fieldType, string format)
        {
            SetFormat(fieldName, fieldType, format, -1);
        }

        public void SetFormat(string fieldName, string format, int width)
        {
            SetFormat(fieldName, null, format, width);
        }

        public void SetFormat(string fieldName, string format)
        {
            SetFormat(fieldName, format, -1);
        }

        #endregion
    }

    #endregion
}
