//Enum các loại control kendo
var KendoControlType = {
    get Dropdownlist() {
        return "kendoDropDownList"
    },
    get Combobox() {
        return "kendoCombobox"
    },
    get MultiSelect() {
        return "kendoMultiSelect";
    },
    get DatePicker() {
        return "kendoDatePicker";
    },
    get DateTimePicker() {
        return "kendoDateTimePicker";
    },
    get NumericTextBox() {
        return "kendoNumericTextBox";
    },
    get Grid() {
        return "kendoGrid";
    },
    get TabStrip() {
        return "kendoTabStrip";
    },
    get Window() {
        return "kendoWindow";
    }
};

//-----------------------BaseControl---------------------->
/*
Khởi tạo Control 
controlID : ID
controlType : Loại control
*/
function BaseControl(controlID, kendoControlType) {
    this.data = $("#" + controlID).data(kendoControlType);
};

/*
Các phương thức của lớp Base
*/
BaseControl.prototype = {
    /*
    Get Value
    */
    GetSelectValue: function () {
        return this.data.value();
    },
    /*
    Set Data Source
      <param name="dataSource" type="Array">IsEnable</param>
    */
    SetDataSource: function (dataSource) {
        this.data.setDataSource(dataSource);
    },
    /*
    Get Data Source
    */
    GetDataSource: function () {
        return this.data.dataSource.data();
    },
    /*
    Refresh Control
    */
    Refresh: function () {
        this.data.refresh();
    },
    /*
     Enable or Disable
     <param name="IsEnable" type="boolean">IsEnable</param>
    */
    Enable: function (IsEnable) {
        this.data.enable(IsEnable);
    },
    /*
     Readonly Dropdownlist
     <param name="IsReadonly" type="boolean">IsReadonly</param>
    */
    Readonly: function (IsReadonly) {
        if (IsReadonly) {
            this.data.readonly();
        }
        else {
            this.data.readonly(IsReadonly);
        }
    },
    /*
    Giải phóng đối tượng  
    */
    Destroy: function () {
        this.data.destroy();
    },
};
//-----------------------/BaseControl---------------------->

//-----------------------Dropdownlist---------------------->
/*
Khởi tạo control Dropdownlist
*/
function VnrDropdownlist(controlID) {
    this.data = $("#" + controlID).data(KendoControlType.Dropdownlist);
};
//Kế thừa lớp BaseControl
VnrDropdownlist.prototype = new BaseControl();
VnrDropdownlist.prototype.constructor = VnrDropdownlist;

/*
 Select item by value or by text
 <param name="Value" type="string">Value</param>
 <param name="DataValueField" type="string">DataValueField or DataTextField</param>
*/
VnrDropdownlist.prototype.SetSelectValue = function (value, valueField) {
    this.data.select(function (dataItem) {
        return dataItem[valueField] === value;
    });
};

/*
 Get Text Is Select
*/
VnrDropdownlist.prototype.GetSelectText = function () {
    return this.data.text();
};

/*
 Select First
*/
VnrDropdownlist.prototype.SelectFirst = function () {
    this.data.select(this.data.ul.children().eq(0));
};

/*
Get Select Index
*/
VnrDropdownlist.prototype.GetSelectIndex = function () {
    return this.data.select();
};

/*
 Select Index
 <param name="index" type="int">Index</param>
*/
VnrDropdownlist.prototype.SetSelectIndex = function (index) {
    this.data.select(index);
};

/*
 Search by value
 <param name="searchValue" type="string">searchValue</param>
*/
VnrDropdownlist.prototype.Search = function (searchValue) {
    this.data.search(searchValue);
};

/*
 Close
*/
VnrDropdownlist.prototype.Close = function () {
    this.data.close();
};

//-----------------------/Dropdownlist---------------------->

//-----------------------ComboBox---------------------->
/*
Khởi tạo control Combobox
*/
function VnrCombobox(controlID) {
    this.data = $("#" + controlID).data(KendoControlType.Combobox);
};
//Kế thừa lớp BaseControl
VnrCombobox.prototype = new BaseControl();
VnrCombobox.prototype.constructor = VnrCombobox;

/*
 Select item by value or by text
 <param name="Value" type="string">Value</param>
 <param name="DataValueField" type="string">DataValueField or DataTextField</param>
*/
VnrCombobox.prototype.SetSelectValue = function (value, valueField) {
    alert('asdasd');
    this.data.select(function (dataItem) {
        return dataItem[valueField] === value;
    });
};

/*
 Get Text Is Select
*/
VnrCombobox.prototype.GetSelectText = function () {
    return this.data.text();
};

/*
 Select First
*/
VnrCombobox.prototype.SelectFirst = function () {
    this.data.select(this.data.ul.children().eq(0));
};

/*
Get Select Index
*/
VnrCombobox.prototype.GetSelectIndex = function () {
    return this.data.select();
};

/*
 Select Index
 <param name="index" type="int">Index</param>
*/
VnrCombobox.prototype.SetSelectIndex = function (index) {
    this.data.select(index);
};

/*
 Search by value
 <param name="searchValue" type="string">searchValue</param>
*/
VnrCombobox.prototype.Search = function (searchValue) {
    this.data.search(searchValue);
};

/*
 Close
*/
VnrCombobox.prototype.Close = function () {
    this.data.close();
};

//-----------------------/ComboBox---------------------->

//-----------------------Multiple Select---------------------->
/*
Khởi tạo control MultiSelect 
*/
function VnrMultiSelect(controlID) {
    this.data = $("#" + controlID).data(KendoControlType.MultiSelect);
};

/*
 Close
*/
VnrMultiSelect.prototype.Close = function () {
    this.data.close();
};

/*
 Giải phóng đối tượng
*/
VnrMultiSelect.prototype.Destroy = function () {
    this.data.destroy();
};

/*
  Enable or Disable
 <param name="IsEnable" type="boolean">IsEnable</param>
*/
VnrMultiSelect.prototype.Enable = function (IsEnable) {
    this.data.enable(IsEnable);
};

/*
  Readonly
 <param name="IsReadonly" type="boolean">IsReadonly</param>
*/
VnrMultiSelect.prototype.Readonly = function (IsReadonly) {
    this.data.readonly(IsReadonly);
};

/*
  Refresh
*/
VnrMultiSelect.prototype.Refresh = function () {
    this.data.refresh();
};

/*
 Search by value
 <param name="searchValue" type="string">searchValue</param>
*/
VnrMultiSelect.prototype.Search = function (searchValue) {
    this.data.search(searchValue);
};

/*
 Set Data Source
 <param name="dataSource" type="Array">dataSource</param>
*/
VnrMultiSelect.prototype.SetDataSource = function (dataSource) {
    this.data.setDataSource(dataSource);
    this.data.refresh();
};


/*
 Get Data Source
*/
VnrMultiSelect.prototype.GetDataSource = function () {
    return this.data.dataSource.data();
};

/*
 Xóa các giá trị đang tìm kiếm
*/
VnrMultiSelect.prototype.ClearFilter = function () {
    return this.data.dataSource.filter({});
};

/*
 Select value
 <param name="value" type="Array">Value (exp: ["1", "2"])</param>
*/
VnrMultiSelect.prototype.SetSelectValue = function (value) {
    this.data.value(value);
};

/*
 Get Value is select
*/
VnrMultiSelect.prototype.GetSelectValue = function () {
    return this.data.value();
};

/*
 Get All
*/
VnrMultiSelect.prototype.SelectAll = function (ValueField) {
    if (this.data != undefined && this.data.dataSource != undefined && this.data.dataSource.data() != undefined) {
        var dataSrouce = this.data.dataSource.data();
        var source = [];
        debugger;
        for (var i = 0; i < dataSrouce.length; i++) {
            source.push(dataSrouce[i][ValueField]);
        }
        this.data.value(source);
    }
};

/*
 Get Value is select
 Hàm trả về một Array (['text1','text2','text3'])         
  <param name="valueField" type="String">valueField</param>
   <param name="textField" type="String">textField</param>
*/
VnrMultiSelect.prototype.GetSelectText = function (valueField, textField) {
    var ArrResult = [];
    var valueSelect = this.data.value();
    var Source = this.data.dataSource.data();
    for (var i = 0; i < Source.length; i++) {
        if (valueSelect.indexOf(Source[i][valueField]) != -1) {
            debugger;
            ArrResult.push(Source[i][textField]);
        }
    }
    return ArrResult;
};

//-----------------------/Multiple Select---------------------->

//-----------------------Date Picker---------------------->

/*
Khởi tạo control Date Picker
*/
function VnrDatePicker(controlID) {
    this.data = $("#" + controlID).data(KendoControlType.DatePicker);
};

/*
 Open
*/
VnrDatePicker.prototype.Open = function () {
    this.data.open();
};

/*
 Close
*/
VnrDatePicker.prototype.Close = function () {
    this.data.close();
};

/*
 Giải phòng control
*/
VnrDatePicker.prototype.Destroy = function () {
    this.data.destroy();
};

/*
  Enable or Disable
 <param name="IsEnable" type="boolean">IsEnable</param>
*/
VnrDatePicker.prototype.Enable = function (IsEnable) {
    this.data.enable(IsEnable);
};

/*
  Readonly
 <param name="IsReadonly" type="boolean">IsReadonly</param>
*/
VnrDatePicker.prototype.Readonly = function (IsReadonly) {
    this.data.readonly(IsReadonly);
};

/*
  Set Min DateTime Value
 <param name="dateTime" type="Datetime">dateTime (exp: new Date(2010, 5, 6) )</param>
*/
VnrDatePicker.prototype.SetMinValue = function (dateTime) {
    this.data.setOptions({
        min: dateTime
    });
};

/*
  Set Max DateTime Value
 <param name="dateTime" type="Datetime">dateTime (exp: new Date(2010, 5, 6) )</param>
*/
VnrDatePicker.prototype.SetMaxValue = function (dateTime) {
    this.data.setOptions({
        max: dateTime
    });
};

/*
  Get value is select
*/
VnrDatePicker.prototype.GetSelectValue = function () {
    return this.data.value();
};

/*
  Get value is select
   <param name="value" type="Datetime">value (exp: 01/01/2015)</param>
*/
VnrDatePicker.prototype.SetSelectValue = function (value) {
    this.data.value(value);
};

//-----------------------/Date Picker---------------------->

//-----------------------Datetime Picker--------------------->

/*
Khởi tạo control Date Picker
*/
function VnrDateTimePicker(controlID) {
    this.data = $("#" + controlID).data(KendoControlType.DateTimePicker);
};

/*
 Open
*/
VnrDateTimePicker.prototype.Open = function () {
    this.data.open();
};

/*
 Close
*/
VnrDateTimePicker.prototype.Close = function () {
    this.data.close();
};

/*
 Giải phòng control
*/
VnrDateTimePicker.prototype.Destroy = function () {
    this.data.destroy();
};

/*
  Enable or Disable
 <param name="IsEnable" type="boolean">IsEnable</param>
*/
VnrDateTimePicker.prototype.Enable = function (IsEnable) {
    this.data.enable(IsEnable);
};

/*
  Readonly
 <param name="IsReadonly" type="boolean">IsReadonly</param>
*/
VnrDateTimePicker.prototype.Readonly = function (IsReadonly) {
    this.data.readonly(IsReadonly);
};

/*
  Set Min DateTime Value
 <param name="dateTime" type="Datetime">dateTime (exp: new Date(2010, 5, 6) )</param>
*/
VnrDateTimePicker.prototype.SetMinValue = function (dateTime) {
    this.data.setOptions({
        min: dateTime
    });
};

/*
  Set Max DateTime Value
 <param name="dateTime" type="Datetime">dateTime (exp: new Date(2010, 5, 6) )</param>
*/
VnrDateTimePicker.prototype.SetMaxValue = function (dateTime) {
    this.data.setOptions({
        max: dateTime
    });
};

/*
  Get value is select
*/
VnrDateTimePicker.prototype.GetSelectValue = function () {
    return this.data.value();
};

/*
  Get value is select
  <param name="value" type="Datetime">value (exp: 01/01/2015)</param>
*/
VnrDateTimePicker.prototype.SetSelectValue = function (value) {
    this.data.value(value);
};

/*
   Toggle
   <param name="option" type="string">option (exp: date or time)</param>
*/
VnrDateTimePicker.prototype.Toggle = function (option) {
    this.data.toggle(option);
};

//-----------------------/Datetime Picker---------------------->

//-----------------------Numberric Textbox---------------------->

/*
Khởi tạo control Numberric Textbox
*/
function VnrNumericTextBox(controlID) {
    this.data = $("#" + controlID).data(KendoControlType.NumericTextBox);
};

/*
 Giải phòng control
*/
VnrNumericTextBox.prototype.Destroy = function () {
    this.data.destroy();
};

/*
  Enable or Disable
 <param name="IsEnable" type="boolean">IsEnable</param>
*/
VnrNumericTextBox.prototype.Enable = function (IsEnable) {
    this.data.enable(IsEnable);
};

/*
  Readonly
 <param name="IsReadonly" type="boolean">IsReadonly</param>
*/
VnrNumericTextBox.prototype.Readonly = function (IsReadonly) {
    this.data.readonly(IsReadonly);
};

/*
  Focus tới control
*/
VnrNumericTextBox.prototype.Focus = function () {
    this.data.focus();
};

/*
  Set max value
   <param name="maxValue" type="Double">maxValue</param>
*/
VnrNumericTextBox.prototype.SetMaxValue = function (maxValue) {
    this.data.max(maxValue);
};

/*
  Set min value
     <param name="minValue" type="Double">minValue</param>
*/
VnrNumericTextBox.prototype.SetMinValue = function (minValue) {
    this.data.min(minValue);
};

/*
  Mỗi lần tăng lên bao nhiêu ?
       <param name="stepValue" type="Double">stepValue</param>
*/
VnrNumericTextBox.prototype.Step = function (stepValue) {
    this.data.step(minValue);
};

/*
    Get Value
*/
VnrNumericTextBox.prototype.GetValue = function () {
    return this.data.value();
};

/*
  Set Value
  <param name="value" type="Double">value</param>
*/
VnrNumericTextBox.prototype.SetValue = function (value) {
    this.data.value(value);
};

//-----------------------/Numberric Textbox---------------------->

//-----------------------Tree View---------------------->



//-----------------------/Tree View---------------------->

//-----------------------Gird---------------------->

/*
Khởi tạo control Gird
*/
function VnrGrid(controlID) {
    this.data = $("#" + controlID).data(KendoControlType.Grid);
};

/*
  Refresh Grid
*/
VnrGrid.prototype.Refresh = function () {
    this.data.Refresh();
};

/*
  Read Grid
*/
VnrGrid.prototype.Read = function () {
    this.data.dataSource.read();
};

/*
  Get Select IDs
*/
VnrGrid.prototype.GetSelectIDs = function (gridID) {
    var ArrayResult = [];
    $("#" + gridID + " .k-grid-content tbody tr").each(function () {
        var $row = $(this);
        var checked = $row.find('.checkboxGroups').attr('checked');
        var id = $row.find('#ID').text();
        ArrayResult.push(id);
    });
    return ArrayResult;
};

/*
  Set data source Grid
    <param name="dataSource" type="Array">dataSource</param>
*/
VnrGrid.prototype.SetDataSource = function (dataSource) {
    this.data.setDataSource(dataSource);
    this.data.Refresh();
};

/*
  Get data source Grid
*/
VnrGrid.prototype.GetDataSource = function () {
    return this.data.dataSource.data();
};

/*
  Get row is select
*/
VnrGrid.prototype.GetSelectRow = function () {
    return this.data.dataItem(this.data.select());
};


//-----------------------/Gird--------------------->

//-----------------------TabStrip---------------------->

/*
Khởi tạo control TabStrip
*/
function VnrTabStrip(controlID) {
    this.data = $("#" + controlID).data(KendoControlType.TabStrip);
};

/*
    Active tab by tab id
    <param name="tabID" type="String">tabID</param>
*/
VnrTabStrip.prototype.ActiveTab = function (tabID) {
    var tabToActivate = $("#" + tabID);
    this.data.activateTab(tabToActivate);
};

/*
    DeActive tab by tab id
    <param name="tabID" type="String">tabID</param>
*/
VnrTabStrip.prototype.DeActiveTab = function (tabID) {
    var tabToDeActivate = $("#" + tabID);
    this.data.deactivateTab(tabToDeActivate);
};

/*
    Append thêm tabstrip
    <param name="tabName" type="String">Tên tab</param>
    <param name="url" type="String">Url tới content</param>
*/
//VnrTabStrip.prototype.AppendTab = function (tabName, url) {
//    this.data.append([{
//        text: tabName,
//        url: url
//    }]);
//};

/*
 Giải phòng control
*/
VnrTabStrip.prototype.Destroy = function () {
    this.data.destroy();
};

/*
  Enable or Disable by TabID
  <param name="tabID" type="String">tabID</param>
  <param name="IsEnable" type="boolean">IsEnable</param>
*/
VnrTabStrip.prototype.Enable = function (tabID, IsEnable) {
    var tabEnable = $("#" + tabID);
    this.data.enable(tabEnable, IsEnable);
};

/*
  Reload tab by TabID
  <param name="tabID" type="String">tabID</param>
*/
VnrTabStrip.prototype.Reload = function (tabID) {
    var tabReload = $("#" + tabID);
    this.data.reload(tabReload);
};

/*
  Remove tab by TabID
  <param name="tabID" type="String">tabID</param>
*/
VnrTabStrip.prototype.Remove = function (tabID) {
    var tabRemove = $("#" + tabID);
    this.data.remove(tabRemove);
};

/*
  Remove tab by TabID
  <param name="tabID" type="String or Int">tabID hoặc Index của tab</param>
*/
VnrTabStrip.prototype.SelectTab = function (tabID) {
    if (typeof tabID === "string") {
        var tabSelect = $("#" + tabID);
        this.data.select(tabSelect);
    }
    else if (typeof tabID === "int") {
        this.data.select(tabID);
    }
};

//-----------------------/TabStrip--------------------->

//-----------------------Windows---------------------->

/*
Khởi tạo control Windows Popup
*/
function VnrWindow(controlID) {
    this.data = $("#" + controlID).data(KendoControlType.Window);
};

/*
    Centers the window
*/
VnrWindow.prototype.Center = function () {
    this.data.center();
};

/*
    Close Window
*/
VnrWindow.prototype.Close = function () {
    this.data.close();
};

/*
    Open Window
*/
VnrWindow.prototype.Open = function () {
    this.data.open();
};

/*
    Restore Window
*/
VnrWindow.prototype.Restore = function () {
    this.data.restore();
};

/*
    Set Width Window
      <param name="widthValue" type="Int">width Value</param>
*/
VnrWindow.prototype.SetWidth = function (widthValue) {
    this.data.setOptions({
        width: widthValue,
    });
};

/*
    Set Height Window
      <param name="heightValue" type="Int">width Value</param>
*/
VnrWindow.prototype.SetHeight = function (heightValue) {
    this.data.setOptions({
        height: heightValue,
    });
};

/*
    Get Title Window
*/
VnrWindow.prototype.GetTitle = function () {
    this.data.title();
};

/*
    Set Title Window
      <param name="titleValue" type="String">title Value</param>
*/
VnrWindow.prototype.SetTitle = function (titleValue) {
    this.data.title(titleValue);
};
//-----------------------/Windows--------------------->

//-----------------------OrgStructure TreeView---------------------->



//-----------------------/OrgStructure TreeView--------------------->

//-----------------------Common---------------------->

var VnrCommon = (function () {
    return {
        /*
            Format định dạng ngày tháng kiểu Javascript sang kiểu dd/MM/yyyy
        */
        FormatDate: function (date) {
            if (date != undefined) {
                var hours = date.getHours();
                var minutes = date.getMinutes();
                var ampm = hours >= 12 ? 'pm' : 'am';
                hours = hours % 12;
                hours = hours ? hours : 12; // the hour '0' should be '12'
                minutes = minutes < 10 ? '0' + minutes : minutes;
                var strTime = hours + ':' + minutes + ' ' + ampm;
                return date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear() + " " + strTime;
            }
            return null;
        },
        /*
            Load lại dữ liệu của Grid
        */
        ReadGrid: function (gridName) {
            var grid = $('#' + gridName).data("kendoGrid");
            if (grid != null && grid.dataSource != null && grid != undefined) {
                grid.dataSource.read();
                selectedIds = [];
            }
        },
        /*
           Hiện thông báo
           <param name="messages" type="String">Messages</param>
           <param name="type" type="String">Loại thông báo</param>
       */
        Notification: function (messages, type) {
            var notificationWidget = $("#popupNotification").kendoNotification().data("kendoNotification");
            notificationWidget.show(messages, type);
        },
        /*
         Lấy array id được check chọn trên lưới
         <param name="gridName" type="String">gridName</param>
        */
        GetSelectIdsByGrid: function (gridName) {
            if (gridName != undefined && gridName != '') {
                if (IsSelectAllPage() == 1) {
                    var grid = $('#' + gridName).data("kendoGrid");
                    var DataSource = grid.dataSource.data();
                    var selectIds = [];

                    for (var i = 0; i < DataSource.length; i++) {
                        if (DataSource[i].id != undefined) {
                            selectIds.push(DataSource[i].id);
                        }
                    }
                    return selectIds;
                }
                else {
                    return selectedIds;
                }
            }
            return [];
        },
    }
}());

//-----------------------/Common--------------------->




