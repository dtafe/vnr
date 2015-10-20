using System;
using System.Linq;
using Kendo.Mvc.UI.Fluent;
using System.Collections.Generic;
using Kendo.Mvc.UI;
using System.Web.Mvc;
using VnResource.Helper.Data;


namespace HRM.Presentation.UI.Controls.Kendo.Grid
{
    #region VnrGridBuilder
    
    public static class VnrGridControl
    {
        public static GridBuilder<T> GridControl<T>(this HtmlHelper helper, GridInfomation grid) where T : class
        {
            grid.ObjectType = grid.ObjectType != null ? grid.ObjectType : typeof(T);//Kiểu dữ liệu của đối tượng đang xét trên lưới.
            Action<GridSelectionSettingsBuilder> selection = new Action<GridSelectionSettingsBuilder>(d => d.Mode(GridSelectionMode.Single));
            Action<PageableBuilder> pageable = new Action<PageableBuilder>(d => d.PageSizes(true).Refresh(true).ButtonCount(5));

            Action<GridColumnFactory<T>> columns = new Action<GridColumnFactory<T>>(column =>
            {

                if (grid.ValueFields != null)
                {
                    column.AutoGenerate(false);


                    foreach (var valueField in grid.ValueFields)
                    {
                        GridBoundColumnBuilder<T> bound = null;

                        Type fieldValueType = null;

                        if (grid.TypeFields != null && grid.TypeFields.Any(d => d.Key == valueField))
                        {
                            fieldValueType = grid.TypeFields.Where(d => d.Key == valueField).Select(d => d.Value).FirstOrDefault();
                        }
                        else if (grid.ObjectType != null && grid.ObjectType.HasProperty(valueField))
                        {
                            fieldValueType = grid.ObjectType.GetPropertyType(valueField);
                        }

                        if (fieldValueType != null)
                        {
                            bound = column.Bound(fieldValueType, valueField);
                        }
                        else
                        {
                            bound = column.Bound(valueField);
                        }

                        if (grid.HasDisplayFields())
                        {
                            string displayField = grid.DisplayFields.Where(d => d.Key
                                == valueField).Select(d => d.Value).FirstOrDefault();

                            if (!string.IsNullOrWhiteSpace(displayField))
                            {
                                bound.Title(displayField);
                            }
                        }

                        if (grid.InvisibleFields != null)
                        {
                            bound.Visible(!grid.InvisibleFields.Contains(valueField));
                        }

                        if (grid.HasFormatFields())
                        {
                            string formatField = grid.FormatFields.Where(d => d.Key
                                == valueField).Select(d => d.Value).FirstOrDefault();

                            if (!string.IsNullOrWhiteSpace(formatField))
                            {
                                bound.Format(formatField);
                            }
                        }
                    }
                    //1. Tạo command muốn hiển thị
                    //column.Command(cmd =>
                    //{
                    //    cmd.Edit();

                    ////    //cmd.Custom("Change").Click("showpage");
                    ////    //cmd.Custom("Edit").Click("ShowInfoCatBankEdit");
                    //    cmd.Destroy();
                    //}).Width(300);
                }
            });

            Action<DataSourceBuilder<T>> dataSource = new Action<DataSourceBuilder<T>>(d =>
            {
                AjaxDataSourceBuilder<T> ajaxDataSource = d.Ajax();
                //2. Xác định được id tương ứng khi click vào một record trên lưới

                ajaxDataSource.Model(m => m.Id(grid.ValueFields[0]));
                if (grid.PageSize > 0)
                {
                    ajaxDataSource.PageSize(grid.PageSize);
                }

                if (!string.IsNullOrWhiteSpace(grid.IdPropertyName))
                {
                    ajaxDataSource.Model(model => model.Id(grid.IdPropertyName));
                }

                if (!string.IsNullOrWhiteSpace(grid.Url))
                {
                    ajaxDataSource.Read(read => read.Url(grid.Url).Type(HttpVerbs.Post));
                    //ajaxDataSource.Update(read => read.Url(grid.EditUrl).Type(HttpVerbs.Post));
                    ////ajaxDataSource.Create(read => read.Url(grid.InsertUrl).Type(HttpVerbs.Post));
                }
                if (!string.IsNullOrWhiteSpace(grid.InsertActionName))
                {
                    ajaxDataSource.Update(grid.EditActionName, grid.EditControllerName);
                    //ajaxDataSource.Read(grid.DataActionName, grid.ControllerName);
                    ajaxDataSource.Create(grid.InsertActionName, grid.InsertControllerName);

                    //3. Phải xác định dược controller và action áp dụng cho nút xóa
                    ajaxDataSource.Destroy(grid.DeleteActionName, grid.DeleteControllerName);
                }
            });
            var gride = new Action<GridEventBuilder>(e =>
            {
                if (!string.IsNullOrWhiteSpace(grid.Change))
                {
                    e.Change(grid.Change);
                }

            });

            return helper.Kendo().Grid<T>()
                .Name(grid.GridName)
                .Sortable()
                .Scrollable()
                //.ToolBar(t =>
                //{
                //    t.Create();
                //    //t.Custom().Text("Add").Url("#").HtmlAttributes(new { onclick = "NewPageInsert()" });
                //    //t.Custom().Text("Add new").Url("#").HtmlAttributes(new { onclick = "ShowDivInsert()" });

                //})
                .Groupable()
                .Editable(editable => editable.Mode(GridEditMode.PopUp).TemplateName(grid.TemplateName))
                .Selectable(selection)
                .Pageable(pageable)
                .Columns(columns)
                .DataSource(dataSource)                
                .Events(gride);
        }
    }

    #endregion

    #region GridInfomation

    public class GridInfomation
    {
        #region Properties

        private Type objectType;
        private int pageSize = 20;
        private string detailScreen;
        private string[] pinFields;
        private string[] valueFields;
        private string[] queryFields;
        private string[] invisibleFields;
        private string[] disableFields;
        private string gridName = string.Empty;
        private string orderField = string.Empty;
        private string idPropertyName = string.Empty;
        private string dataActionName = string.Empty;
        private string controllerName = string.Empty;
        private string editControllerName = string.Empty;
        private string editActionName = string.Empty;
        private string insertControllerName = string.Empty;
        private string insertActionName = string.Empty;
        private string deleteControllerName = string.Empty;
        private string deleteActionName = string.Empty;
        private string templateName = string.Empty;
        private string gridEditMode = string.Empty;

        private string url = string.Empty;
        private string editurl = string.Empty;
        private string inserturl = string.Empty;

        Dictionary<string, string> displayFields;
        Dictionary<string, string> manualFields;
        Dictionary<string, string> groupFields;
        Dictionary<string, string> formatFields;
        Dictionary<string, string> linkFields;
        Dictionary<string, Type> typeFields;
        Dictionary<string, int> sizeFields;

        /// <summary>
        /// Sự kiện Change
        /// </summary>
        public string Change { get; set; }

        /// <summary>
        /// GridEditMode của grid
        /// </summary>
        public string GridEditMode
        {
            get { return gridEditMode; }
            set { gridEditMode = value; }
        }
        /// <summary>
        /// Template của grid
        /// </summary>
        public string TemplateName
        {
            get { return templateName; }
            set { templateName = value; }
        }
        /// <summary>
        /// Tên grid control được tạo ra.
        /// </summary>
        public string EditUrl
        {
            get
            {
                return editurl;
            }
            set { editurl = value; }
        }
        /// <summary>
        /// Tên grid control được tạo ra.
        /// </summary>
        public string InsertUrl
        {
            get
            {
                return inserturl;
            }
            set { inserturl = value; }
        }

        /// <summary>
        /// Tên grid control được tạo ra.
        /// </summary>
        public string GridName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(gridName))
                {
                    gridName = DateTime.Now.ToOADate().ToString();
                }
                return gridName;
            }
            set { gridName = value; }
        }

        /// <summary>
        /// Tên cột làm ID cho dữ liệu hiện tại.
        /// </summary>
        public string IdPropertyName
        {
            get { return idPropertyName; }
            set { idPropertyName = value; }
        }

        /// <summary>
        /// Tên controller dữ liệu.
        /// </summary>
        public string ControllerName
        {
            get { return controllerName; }
            set { controllerName = value; }
        }

        /// <summary>
        /// Tên method load dữ liệu.
        /// </summary>
        public string DataActionName
        {
            get { return dataActionName; }
            set { dataActionName = value; }
        }

        /// <summary>
        /// Tên controller insert dữ liệu.
        /// </summary>
        public string InsertControllerName
        {
            get { return insertControllerName; }
            set { insertControllerName = value; }
        }

        /// <summary>
        /// Tên method insert dữ liệu.
        /// </summary>
        public string InsertActionName
        {
            get { return insertActionName; }
            set { insertActionName = value; }
        }

        /// <summary>
        /// Tên controller delete dữ liệu.
        /// </summary>
        public string DeleteControllerName
        {
            get { return deleteControllerName; }
            set { deleteControllerName = value; }
        }

        /// <summary>
        /// Tên method delete dữ liệu.
        /// </summary>
        public string DeleteActionName
        {
            get { return deleteActionName; }
            set { deleteActionName = value; }
        }

        /// <summary>
        /// Tên controller edit dữ liệu.
        /// </summary>
        public string EditControllerName
        {
            get { return editControllerName; }
            set { editControllerName = value; }
        }

        /// <summary>
        /// Tên method edit load dữ liệu.
        /// </summary>
        public string EditActionName
        {
            get { return editActionName; }
            set { editActionName = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        /// <summary>
        /// Tên màn hình edit.
        /// </summary>
        public string DetailScreen
        {
            get { return detailScreen; }
            set { detailScreen = value; }
        }

        /// <summary>
        /// Loại dự liệu hiển thị trên lưới.
        /// </summary>
        public Type ObjectType
        {
            get { return objectType; }
            set { objectType = value; }
        }

        public string OrderField
        {
            get { return orderField; }
            set { orderField = value; }
        }

        /// <summary>
        /// Danh sách field dữ liệu được truy vấn.
        /// </summary>
        public string[] PinFields
        {
            get { return pinFields; }
            set { pinFields = value; }
        }

        /// <summary>
        /// Danh sách field dữ liệu được truy vấn.
        /// </summary>
        public string[] QueryFields
        {
            get { return queryFields; }
            set { queryFields = value; }
        }

        /// <summary>
        /// Danh sách field dữ liệu hiển thị trên lưới.
        /// </summary>
        public string[] ValueFields
        {
            get { return valueFields; }
            set { valueFields = value; }
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
            internal set
            {
                displayFields = value;
            }
        }

        /// <summary>
        /// Danh sách field do người dùng tự thiết lập.
        /// Key là tên field, value có thể là value type.
        /// </summary>
        public Dictionary<string, string> ManualFields
        {
            get
            {
                if (manualFields == null)
                {
                    manualFields = new Dictionary<string, string>();
                }

                return manualFields;
            }
            internal set
            {
                manualFields = value;
            }
        }

        /// <summary>
        /// Danh sách field dữ liệu bị ẩn trên lưới.
        /// </summary>
        public string[] InvisibleFields
        {
            get { return invisibleFields; }
            set { invisibleFields = value; }
        }

        /// <summary>
        /// Danh sach field dữ liệu không cho chỉnh sửa.
        /// </summary>
        public string[] DisableFields
        {
            get { return disableFields; }
            set { disableFields = value; }
        }

        public Dictionary<string, string> LinkFields
        {
            get
            {
                if (linkFields == null)
                {
                    linkFields = new Dictionary<string, string>();
                }

                return linkFields;
            }
            internal set
            {
                linkFields = value;
            }
        }

        /// <summary>
        /// Danh sách field dữ liệu được group mặc định.
        /// </summary>
        public Dictionary<string, string> GroupFields
        {
            get
            {
                if (groupFields == null)
                {
                    groupFields = new Dictionary<string, string>();
                }

                return groupFields;
            }
            internal set
            {
                groupFields = value;
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
            internal set
            {
                formatFields = value;
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
            internal set
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

        /// <summary>
        /// Sử dụng hàm này để không vô tình khởi tạo danh sách.
        /// </summary>
        /// <returns></returns>
        internal bool HasManualFields()
        {
            return manualFields != null && manualFields.Count > 0;
        }

        internal bool HasTypeFields()
        {
            return typeFields != null && typeFields.Count > 0;
        }

        internal bool HasLinkFields()
        {
            return linkFields != null && linkFields.Count > 0;
        }

        internal bool HasGroupFields()
        {
            return groupFields != null && groupFields.Count > 0;
        }

        internal bool HasFormatFields()
        {
            return formatFields != null && formatFields.Count > 0;
        }

        internal bool HasSizeFields()
        {
            return sizeFields != null && sizeFields.Count > 0;
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

        public void SetFormat1(string fieldName, Type fieldType, string format)
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
