using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;
using HRM.Infrastructure.Utilities;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrTreeViewBuilder
    {
        
        public static TreeViewBuilder VnrTreeView<TModel>(this HtmlHelper<TModel> helper, TreeViewBuilderInfo builderInfo)
        {
            var dataSource = new Action<HierarchicalDataSourceBuilder<object>>(d =>
            {
                
                if (!string.IsNullOrWhiteSpace(builderInfo.Url))
                {
                    d.Read(read => read.Url(builderInfo.Url).Type(HttpVerbs.Post));
                }
                if (!string.IsNullOrEmpty(builderInfo.Id))
                {
                    d.Model(model =>
                    {
                        model.Id(builderInfo.Id);
                    });;
                }
                
            });

            var treeViewEventBuilder = new Action<TreeViewEventBuilder>(e =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.EventSelect))
                {
                    e.Select(builderInfo.EventSelect);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.EventCheck))
                {
                    e.Check(builderInfo.EventCheck);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.EventDataBound))
                {
                    e.DataBound(builderInfo.EventDataBound);
                }
            });

            var treeViewBuilder = helper.Kendo().TreeView()
                .Name(builderInfo.Name)
                .DataSource(dataSource)
                .LoadOnDemand(builderInfo.LoadOnDemand)
                .Events(treeViewEventBuilder)
                .AutoBind(builderInfo.AutoBind)
                .ExpandAll(builderInfo.ExpandAll).Deferred(false);

            if (builderInfo.ShowCheckbox)
            {
                treeViewBuilder.Checkboxes(checkboxes => 
                    {
                        if (builderInfo.CheckChildren!=null)
                        {
                            checkboxes.CheckChildren((bool)builderInfo.CheckChildren);
                        }
                       
                        if (!string.IsNullOrEmpty(builderInfo.Template))
                        {
                            checkboxes.Template(builderInfo.Template);
                        }
                        if (!string.IsNullOrEmpty(builderInfo.NameChecked))
                        {
                            checkboxes.Name(builderInfo.NameChecked);
                        }
                    });
            }
            if (builderInfo.HtmlAttributes != null)
            {
                treeViewBuilder.HtmlAttributes(builderInfo.HtmlAttributes);
            }
            if (builderInfo.BindTo != null)
            {
                treeViewBuilder.BindTo(builderInfo.BindTo);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Icon))
            {
                treeViewBuilder.DataImageUrlField(builderInfo.Icon);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.DataTextField))
            {
                treeViewBuilder.DataTextField(builderInfo.DataTextField);
            }
            return treeViewBuilder;
        }

        public static MvcHtmlString VnrOrgStructure(this HtmlHelper helper, VnrOrgStructureInfo buildInfo)
        {
            string str = "<div id=" + buildInfo.Name + "></div>" +
                         "<script>" +

                         "OrgStructure('" + buildInfo.Url + "'," +
                         "'" + buildInfo.Name + "'," +
                         buildInfo.ExpandAll.ToString().ToLower();
            str += ");</script>";
            return MvcHtmlString.Create(str.ToString(CultureInfo.InvariantCulture));
        }
    }

    public class VnrOrgStructureInfo
    {
        public VnrOrgStructureInfo()
        {
            Name = ConstantControlName.HRM_OrgStructureName;
            Url = ConstantPathWeb.Hrm_Hre_Service;
            ExpandAll = true;
        }
        public bool ExpandAll { get; set; }
        public string ESelect { get; set; }
        public string Name { get; set; }
        public string EDataBound { get; set; }
        public string Url { get; set; }
    }
    #region TreeView Infomation
    public class TreeViewBuilderInfo : VnrBaseBuilderInfo
    {
        public TreeViewBuilderInfo()
        {
            CheckChildren = true;
            ExpandAll = false;
            LoadOnDemand = true;
            ShowCheckbox = true;
            AutoBind = true;
        }
        private string _icon = string.Empty;
        public IEnumerable<TreeViewItemModel> BindTo { get; set; }

        /// <summary>
        /// Check chọn node con khi check vào node cha Default: true
        /// </summary>
        public bool? CheckChildren { get; set; }
        /// <summary>
        /// Show tất cả các node Default: false
        /// </summary>
        public bool ExpandAll { get; set; }
        public bool AutoBind { get; set; }
        /// <summary>
        /// Sự kiện khi select vào một node
        /// </summary>
        public string EventSelect { get; set; }
        public string EventCheck { get; set; }
        public string Template { get; set; }
        /// <summary>
        /// Sự kiện khi load node cho Treeview
        /// </summary>
        public string EventDataBound { get; set; }
        /// <summary>
        /// Url dùng để lấy dữ liệu
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Id cho mỗi node trong Treeview
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Field sẽ hiển thị ra treeview cho người dùng xem
        /// </summary>
        public string DataTextField { get; set; }
        /// <summary>
        /// Name cho node con
        /// </summary>
        public string NameChecked { get; set; }
        /// <summary>
        /// Dữ liệu của node con sẽ không load khi treeview được khởi tạo Default: true
        /// </summary>
        public bool LoadOnDemand { get; set; }
        public Dictionary<string, string> HtmlAttributes { get; set; }
        /// <summary>
        /// Hiển thị checkbox trước mỗi node của treeview Default: true
        /// </summary>
        public bool ShowCheckbox { get; set; }
        /// <summary>
        /// Hiển thị icon trước mỗi node
        /// </summary>
        public string Icon
        {
            get
            {
                if (string.IsNullOrEmpty(_icon))
                {
                    _icon = "~/Content/images/icons/icon1.png";
                }
                return _icon;
            }
            set { _icon = value; }
        }
    }

    public class TreeViewDropdDownBuilderInfo
    {
        private readonly static string _hrm_Cat_Service = ConstantPathWeb.Hrm_Hre_Service;
        public TreeViewDropdDownBuilderInfo()
        {
            IsShowCheckBox = true;
            Name = "OrgTreeViewDropDown";
            Url = _hrm_Cat_Service + "Cat_GetData/GetOrgStructureTree";
            DataTextField = "Name";
            DataValueField = "ID";
            Value = "";
            SetFieldID = "";
            Width = "250px";
            Placeholder = ConstantDisplay.ChooseOrgStructure.TranslateString();
            LoadOnDemand = false;
            WidthTreeView = "300px";
            IsCheckChildren = true;
            EvenCheck = string.Empty;
            Fixed = false;
            MaxHeight = "400px";
        }
        /// <summary>
        /// chiều dài của treeview
        /// </summary>
        public string WidthTreeView { get; set; }
        public string EvenSelect { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Placeholder { get; set; }
        /// <summary>
        /// Show Check Box (Default true)
        /// </summary>
        public bool IsShowCheckBox { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// (Default Cat_GetData/GetOrgStructureTree)
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// (Default Name)
        /// </summary>
        public string DataTextField { get; set; }
        /// <summary>
        /// (Default ID)
        /// </summary>
        public string DataValueField { get; set; }
        /// <summary>
        /// Gán giá trị cho textbox
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Xử dụng cho các màn hình create
        /// Field Name cần gán giá trị khi selected TreeNode
        /// </summary>
        public string SetFieldName { get; set; }
        /// <summary>
        /// Xử dụng cho các màn hình create
        /// Field ID dùng để select node khi edit
        /// </summary>
        public string SetFieldID { get; set; }
        /// <summary>
        /// Chiều dài (default:250)
        /// </summary>
        public string Width { get; set; }
        /// <summary>
        /// Dữ liệu của node con sẽ không load khi treeview được khởi tạo Default: false
        /// </summary>
        public bool LoadOnDemand { get; set; }
        /// <summary>
        /// Check các nodes con khi node cha đc check hay không
        /// </summary>
        public bool? IsCheckChildren { get; set; }
        public string EvenCheck { get; set; }
        /// <summary>
        /// Set Position Fixed , sử dụng cho các cây phòng ban trong popup
        /// </summary>
        public bool Fixed { get; set; }
        public string MaxHeight { get; set; }
        
    }
    #endregion
}
