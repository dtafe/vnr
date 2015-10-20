
var selectedIds = [];
//Xử lý load nội dung động cho một div nào đó
//urlGetData: đường dẫn đến
//partialName: tham số, ví dụ load thông tin của 1 nhân viên
//idLoadContent: id của div, để hiển thị nội dung


//activeTabHre|Hre_Profile/Hre_ProfilePersonalView,Hre_Profile/Hre_ProfileContactView
//activeTabHre => TabActive
//Hre_Profile/Hre_ProfilePersonalView => strActionControlers
//Hre_Profile => ControlerName
//Hre_ProfilePersonalView => ActionName

function Translate(str, url) {
    $.ajax({
        url: url,
        data: { text: str },
        type: 'POST',
        async: false,
        success: function (data) {
            str = data;
        },
        error: function (data) {
            alert('error Translate');
        }
    });

    return str;
}


//Control VnrGridDynamic
var VnrGridDynamic = {

    gridName: '',
    dtSource: '',
    dtBound: '',
    lockedFields: '',
    sortable: true,
    selectable: true,
    scrollable: true,
    resizable: true,
    showcheckbox: false,
    editable: false,
    pageSize: 20,
    buttonCount: 20,
    get GridName() { return this.gridName },
    set GridName(val) { return this.gridName = val },
    get DataSource() { return this.dtSource },
    set DataSource(val) { return this.dtSource = val },
    get DataBound() { return this.dtBound },
    set DataBound(val) { return this.dtBound = val },
    get LockedFields() { return this.lockedFields },
    set LockedFields(val) { return this.lockedFields = val },
    get Sortable() { return this.sortable },
    set Sortable(val) { return this.sortable = val },
    get Selectable() { return this.selectable },
    set Selectable(val) { return this.selectable = val },
    get Scrollable() { return this.scrollable },
    set Scrollable(val) { return this.scrollable = val },
    get Resizable() { return this.resizable },
    set Resizable(val) { return this.resizable = val },
    get Editable() { return this.editable },
    set Editable(val) { return this.editable = val },
    get PageSize() { return this.pageSize },
    set PageSize(val) { return this.pageSize = val },
    get ButtonCount() { return this.buttonCount },
    set ButtonCount(val) { return this.buttonCount = val },
    get ShowCheckbox() { return this.showcheckbox },
    set ShowCheckbox(val) { return this.showcheckbox = val }
}

//Field hỗ trợ xử lý lưới động
var dateFields = [];
var gridDynamicColumns = [];
var gridDynamicModelFields = [];

//[Chuc.Nguyen] - Xử lý lý động
function VnrGridDynamicControl(VnrGridDynamic) {
    debugger;
    dateFields = [];
    gridDynamicColumns = [];
    gridDynamicModelFields = [];

    //Tên lưới
    var gridName = VnrGridDynamic["GridName"];
    //Dữ liệu cho lưới
    var dtSource = VnrGridDynamic["DataSource"];
    var dtBound = VnrGridDynamic["DataBound"];
    //Các cột bị khóa khi kéo thanh scroll bên dưới lưới
    var lockedFields = VnrGridDynamic["LockedFields"];
    //Các thuộc tính cấu hình khác
    var sortable = VnrGridDynamic["Sortable"];
    var selectable = VnrGridDynamic["Selectable"];
    var scrollable = VnrGridDynamic["Scrollable"];
    var resizable = VnrGridDynamic["Resizable"];
    var editable = VnrGridDynamic["Editable"];
    var pageSize = VnrGridDynamic["PageSize"];
    var buttonCount = VnrGridDynamic["ButtonCount"];

    GenerateColumnAndModel(dtSource[0], VnrGridDynamic);

    var configuration = {
        dataSource: {
            data: dtSource,
            schema: {
                model: {
                    id: "ID",
                    fields: gridDynamicModelFields[0]
                }
            },
            pageSize: pageSize
        },
        dataBound: dtBound,
        editable: editable,
        sortable: sortable,
        selectable: selectable,
        scrollable: scrollable,
        resizable: resizable,
        filterable: true,
        pageable: {
            buttonCount: buttonCount
        },
        columns: gridDynamicColumns
    }

    //Tạo lưới động
    debugger;
    var grid = $("#" + gridName).data("kendoGrid");
    if (grid != null && grid != undefined) {
        grid.destroy();
        $("#" + gridName).empty().kendoGrid(configuration);
    }
    else {
        $("#" + gridName).kendoGrid(configuration).data("kendoGrid");
    }

    dateFields = []
    gridDynamicColumns = [];
    gridDynamicModelFields = [];
}
String.prototype.replaceAll = function (find, replace) {
    var str = this;
    return str.replace(new RegExp(find.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'), 'g'), replace);
};

function GenerateColumnAndModel(gridData, VnrGridDynamic) {
    var str = gridData["TempColumn"];
    var showcb = VnrGridDynamic["ShowCheckbox"];
    if (str != undefined && str != "") {
        var strSplit = str.split("|");

        var strColumns = strSplit[0].replaceAll('True', 'true');
        strColumns = strColumns.replaceAll('False', 'false');
        if (showcb == true) {
            strColumns = "{\"title\" : \"<input type=\'checkbox\' id='mastercheckbox' />\",\"template\" : \"<input type=\'checkbox\' value='#=ID#' class='checkboxGroups'/> \", \"width\": 30}," + strColumns;
        }
        gridDynamicColumns = $.parseJSON('[' + strColumns + ']');
        gridDynamicModelFields = $.parseJSON('[' + strSplit[1] + ']');
    }
}

function GridDynamicColum(gridName, gridData) {


    //   var coutData = gridData.length;
    //   var listFileName = gridData[coutData-1];
    //  gridData.splice(coutData-1, 1);
    var listFileName = gridData;
    var model = generateModel(gridData[0]);

    var parseFunction;
    if (dateFields.length > 0) {


        parseFunction = function (response) {

            for (var i = 0; i < response.length; i++) {

                for (var fieldIndex = 0; fieldIndex < dateFields.length; fieldIndex++) {

                    var record = response[i];
                    record[dateFields[fieldIndex]] = kendo.parseDate(record[dateFields[fieldIndex]], "dd/MM/yyyy")
                }
            }
            return response;
        };
    }

    var grid = $("#" + gridName).kendoGrid({
        dataSource: {
            data: gridData,
            schema: {
                model: model,
                parse: parseFunction
            },
            pageSize: 20
        },
        //editable: true,
        sortable: true,
        selectable: true,
        scrollable: true,
        resizable: true,
        pageable: {
            buttonCount: 20
        },
    }).data("kendoGrid");


    //Lấy đường dẫn hiện tại trên trình duyệt
    var currentLocation = window.location.href;
    //Tách link thành 2 phần: trước và sau dấu #
    var strArray = currentLocation.split('#');
    ///tạm thời dịch nhiều lần sau này sẻ chuyển listFileName dịch 1 lần rồi trả về

    for (var name in listFileName) {

        var fieldName = Translate(listFileName[name], strArray[0] + "Home/Translate");
        $("#" + gridName + " thead [data-field=" + name + "] .k-link").html(fieldName);
        //var width1 = $("#" + gridName + " .k-grid-header-wrap").find("th:contains(" + name + ")").width();

    }
    $("#" + gridName + " .k-grid-header-wrap") //header
            .find("colgroup col")
            .css({ width: 150 });

    $("#" + gridName + " .k-grid-content") //content
       .find("colgroup col")
       .css({ width: 150 });

}
function generateModel(gridData) {
    var model = {};
    model.id = "ID";
    var fields = {};
    for (var property in gridData) {

        var propType = typeof gridData[property];

        if (propType == "number") {
            fields[property] = {
                type: "number",
                validation: {
                    required: false
                }
            };
        } else if (propType == "boolean") {
            fields[property] = {
                type: "boolean",
                validation: {
                    required: false
                }
            };
        } else if (propType == "string") {
            var parsedDate = kendo.parseDate(gridData[property]);
            if (parsedDate) {
                fields[property] = {
                    type: "date",
                    validation: {
                        required: false
                    }
                };
                dateFields.push(property);
            } else {
                fields[property] = {
                    validation: {
                        required: false
                    }
                };
            }
        } else {
            fields[property] = {
                validation: {
                    required: false
                }
            };
        }
    }
    debugger;
    model.fields = fields;

    return model;
}

//[Hien.Nguyen] Lưu ID khi checkbox
$(document).on("click", "input[class=checkboxGroups]", function () {
    debugger;
    if (this.checked) {
        if (selectedIds.indexOf(this.defaultValue) == -1) {
            selectedIds.push(this.defaultValue);
        }
    }
    else {
        if (selectedIds.indexOf(this.defaultValue) != -1) {
            selectedIds.splice(selectedIds.indexOf(this.defaultValue), 1);
        }
    }
});


//Kết thúc control VnrGridDynamic

function SetRowColor(gridName, arrayConfigField) {

    if (gridName != "" && gridName != undefined && arrayConfigField != null && arrayConfigField != undefined && arrayConfigField.length > 0) {
        var grid = $('#' + gridName).data("kendoGrid");
        var dataSource = grid.dataSource.data();
        for (var i = 0; i < arrayConfigField.length; i++) {
            if (arrayConfigField[i].length == 5) {
                var field = arrayConfigField[i][0];
                var value = arrayConfigField[i][1];
                var background = arrayConfigField[i][2];
                var color = arrayConfigField[i][3];
                var operator = arrayConfigField[i][4];
                grid.tbody.find('>tr').each(function () {

                    var dataItem = grid.dataItem(this);
                    if (operator == "=") {
                        if (dataItem[field] == value) {
                            $(this).attr('style', 'background:' + background + ';color:' + color + ';');
                        }
                    }
                    else if (operator == ">=") {
                        if (dataItem[field] >= value) {
                            $(this).attr('style', 'background:' + background + ';color:' + color + ';');
                        }
                    }
                    else if (operator == "<=") {
                        if (dataItem[field] <= value) {
                            $(this).attr('style', 'background:' + background + ';color:' + color + ';');
                        }
                    }
                    else if (operator == "<") {
                        if (dataItem[field] < value) {
                            $(this).attr('style', 'background:' + background + ';color:' + color + ';');
                        }
                    }
                    else if (operator == ">") {
                        if (dataItem[field] > value) {
                            $(this).attr('style', 'background:' + background + ';color:' + color + ';');
                        }
                    }
                });
            }
        }
    }
}

function SetRowColorNoCondition(gridName, arrayConfigField) {
    debugger;

    if (gridName != "" && gridName != undefined && arrayConfigField != null && arrayConfigField != undefined && arrayConfigField.length > 0) {
        var grid = $('#' + gridName).data("kendoGrid");
        var dataSource = grid.dataSource.data();
        for (var i = 0; i < arrayConfigField.length; i++) {
            if (arrayConfigField[i].length == 5) {
                var field = arrayConfigField[i][0];
                var value = arrayConfigField[i][1];
                var background = arrayConfigField[i][2];
                var color = arrayConfigField[i][3];
                var operator = arrayConfigField[i][4];
                grid.tbody.find('>tr').each(function () {

                    var dataItem = grid.dataItem(this);
                    if (operator == "=") {
                        $(this).attr('style', 'background:' + background + ';color:' + dataItem[field] + ';');
                    }
                });
            }
        }
    }
}

function SetStyleGrid(gridName) {


    if (gridName != "" && gridName != undefined) {
        var grid = $('#' + gridName).data("kendoGrid");

        grid.tbody.find('>tr').each(function () {

            var dataItem = grid.dataItem(this);
            var value = dataItem['Description4'];
            var valueReturn = dataItem['Value'];
            var grossAmount = dataItem['GrossAmount'];
            var valueResult = 0;


            if (!isNaN(valueReturn) && valueReturn != 0) {
                valueResult = parseFloat(parseFloat(valueReturn).toFixed(2)).toLocaleString();
            }
            else {
                valueResult = valueReturn;
            }

            //Xử Lý cột GrossAmount của màn hình lương căn bản trong chi tiết nv
            //if (grossAmount != '' && grossAmount != undefined) {

            //    $(this.cells[8]).context.innerHTML = parseFloat(grossAmount).toFixed(2).toLocaleString();
            //    $(this.cells[8]).attr('style', 'text-align:right;');
            //}

            if (value != '' && value != undefined) {
                var values = value.split(",");
                if (values != '' && values != undefined && value.length > 0) {
                    for (var i = 0; i < values.length; i++) {
                        switch (values[i]) {
                            case "Double":
                                $(this.cells[2])[0].innerHTML = valueResult;
                                $(this.cells[2]).attr('style', 'text-align:right;');
                                break;
                            case "E_BOLD":
                                $(this).attr('style', 'font-weight: bold!important;');
                                break;
                            case "E_LEVEL_0":
                                $(this.cells[1]).attr('style', 'padding-left: 30px!important;');
                                break;
                            case "E_LEVEL_1":
                                $(this.cells[1]).attr('style', 'padding-left: 60px!important;');
                                break;
                            case "E_LEVEL_2":
                                $(this.cells[1]).attr('style', 'padding-left: 90px!important;');
                                break;
                            case "E_LEVEL_3":
                                $(this.cells[1]).attr('style', 'padding-left: 120px!important;');
                                break;
                            case "E_LEVEL_4":
                                $(this.cells[1]).attr('style', 'padding-left: 150px!important;');
                                break;

                        }
                    }
                }
            }
        });
    }
}


function SetStyleStringGrid(gridName, colName, colIndex) {


    if (gridName != "" && gridName != undefined) {
        var grid = $('#' + gridName).data("kendoGrid");

        grid.tbody.find('>tr').each(function () {

            var dataItem = grid.dataItem(this);

            var valueField = dataItem[colName];

            //Xử Lý cột GrossAmount của màn hình lương căn bản trong chi tiết nv
            if (valueField != '' && valueField != undefined) {

                $(this.cells[colIndex]).context.innerHTML = parseFloat(parseFloat(valueField).toFixed(2)).toLocaleString();
                $(this.cells[colIndex]).attr('style', 'text-align:right;');
            }

        });
    }
}

var LoadTabContent = function (strTabInfo) {

    //Get TabActive


    var tabActiveid = GetTabActive(strTabInfo);

    var tabActive = $('#' + tabActiveid);
    tabActive.html("");
    var tabCreates = GetTabCreate(strTabInfo);

    var tabStrip = CreateTabStrip(tabActiveid)

    //Tạo ra tab
    for (var _tabIndex in tabCreates) {
        var strActionControler = tabCreates[_tabIndex];
        var actionName = GetActionName(strActionControler);
        var controlerName = GetControlerName(strActionControler);
        if (!tabActive.hasOwnProperty(actionName)) {
            tabStrip.append({
                text: actionName,
                content: "<div style='height:430px'></div>"
            });
        }
    }
    tabStrip.select(0);
}
//Crate TabStrip

var CreateTabStrip = function (tabActive) {
    var tabActive = $('#' + tabActive);
    var tabStrip = tabActive.kendoTabStrip({
        activate: onActivate,
    }).data("kendoTabStrip");
    return tabStrip;
}

//Get GetTabCreate
var GetTabCreate = function (strTabInfo) {
    var tabActive = GetTabActive(strTabInfo);
    var strActionControlerRemoveTabActive = strTabInfo.substring(tabActive.length + 1);
    if (strActionControlerRemoveTabActive == '' && strActionControlerRemoveTabActive.indexOf(',') <= 0)
        return strActionControlerRemoveTabActive;
    var strActionControlers = strActionControlerRemoveTabActive.split(',');
    return strActionControlers;
}

//Get TabActive
var GetTabActive = function (strTabInfo) {
    if (strTabInfo == '' && strTabInfo.indexOf('|') <= 0)
        return strTabInfo;
    var tabActiveId = strTabInfo.split('|')[0];
    return tabActiveId;
}

//GetAction bởi strActionControlers

var GetActionName = function (strActionControlers) {
    if (strActionControlers == '' && strActionControlers.indexOf('/') <= 0)
        return strActionControlers
    var strAction = strActionControlers.split('/')[1];
    return strAction;
}

//GetControler bởi strActionControlers

var GetControlerName = function (strActionControlers) {
    if (strActionControlers == '' && strActionControlers.indexOf('/') <= 0)
        return strActionControlers
    var strControlerName = strActionControlers.split('/')[0];
    return strControlerName;

}

//Get
function ApproveSelected(gridId, url) {
    var grid = $('#' + gridId).data("kendoGrid");
    var selectedItem = grid.dataItem(grid.select());
    var status = selectedItem.Status;
    if (status != "Từ Chối" && status != "E_REJECTED") {
        if (status != "Duyệt" && status != "E_APPROVED") {
            var userId = $("#UserLoginId").val();
            var userApprove1 = selectedItem.UserApproveID;
            var userApprove2 = selectedItem.UserApproveID2;
            //Trường hợp người duyệt khác nhau
            if (userApprove1 != userApprove2) {
                //TH1: Người duyệt đầu duyệt
                if (userApprove1 == userId) {
                    if (status != 'E_FIRST_APPROVED' && status != "Người Duyệt Đầu") {
                        SetStatusSelected(gridId, url, selectedIds, "E_FIRST_APPROVED", 'Người duyệt đầu duyệt thành công!');
                    }
                    else {
                        ErrorMessages("Bạn đã duyệt đăng ký này!");
                    }
                }
                //TH2: Người duyệt cuối
                if (userApprove2 == userId) {
                    if (status == 'E_FIRST_APPROVED' || status == "Người Duyệt Đầu") {
                        SetStatusSelected(gridId, url, selectedIds, "E_APPROVED", 'Duyệt đăng ký tăng thành công!');
                    }
                    else {
                        ErrorMessages("Người duyệt đầu chưa duyệt đăng ký này!");
                    }
                }
                selectedIds = [];
            }
                //Trường hợp người duyệt giống nhau
            else {
                SetStatusSelected(gridId, url, selectedIds, "E_APPROVED", 'Thay Đổi Trạng Thái Thành Công!');
                selectedIds = [];
            }
        }
        else {
            ErrorMessages("Đăng Ký Đã Được Duyệt!");
        }
        return false;
    } else {
        ErrorMessages("Đăng Ký Đã Từ Chối Không Được Phép Duyệt!");
    }
}

function VnrContentDiv(urlGetData, partialName, idLoadContent) {
    $.ajax({
        url: urlGetData,
        data: { "partialName": partialName },
        type: 'GET',
        success: function (data) {
            $('#' + idLoadContent + '').html(data);
        }
    });
}

//Tạo mới một tabstrip
function VnrTabStrip(vnrIdTab, vnrActionTab, vnrValueHiddenField) {
    var tabStrip = $('#' + vnrIdTab + '').kendoTabStrip({
        activate: onActivaold,
    }).data("kendoTabStrip");

    for (var t in vnrActionTab) {
        if (vnrActionTab.hasOwnProperty(t)) {
            tabStrip.append({
                text: t,
                content: "<br>"
            });
        }
    }
    tabStrip.select(0);
    function onActivaold(e) {
        //Lấy index của tabd đang activate
        var index = this.select().index();
        //Tạo ra id cho hidden field theo index của tab
        var idHidden = "hidden-" + index;

        //Kiểm tra xem hidden field đã được tạo ra chưa
        var myElem = document.getElementById(idHidden);
        if (myElem == null) {
            e.item.innerHTML += "<input type=\"text\" style =\"display:none\" value=\"0\" id=\"" + idHidden + "\">";
        }
        //Nếu id !=null (trường hợp chạy lần đầu tiên) và giá trị id khác với value của hidden field thì get dữ liệu mới
        if (vnrValueHiddenField != null && parseFloat(document.getElementById(idHidden).value) != vnrValueHiddenField) {

            document.getElementById(idHidden).setAttribute("value", vnrValueHiddenField);

            var tabid = (vnrIdTab + "-" + (index + 1));

            //Tham khảo: http://stackoverflow.com/questions/11734417/javascript-equivalent-of-pythons-values-dictionary-method
            var actionLink = Object.keys(vnrActionTab).map(function (key) {
                return vnrActionTab[key];
            });
            //danh sách các action tương ừng cho tường tab
            $.ajax({
                url: actionLink[index],

                data: { id: vnrValueHiddenField },
                type: 'POST',
                success: function (data) {
                    //Gán dữ liệu vào tab tương ứng khi xử lý lấy dữ liệu xong
                    $("#" + tabid).html(data);
                },
                error: function (data) {
                    alert('error');
                }
            });
        }
    }

    return tabStrip;
}



//[Chuc.Nguyen] - Nhập dữ liệu
function Import(urlGetData, dataParameter) {

    $.ajax({
        type: 'POST',
        url: urlGetData,
        data: dataParameter,
        dataType: 'json',
        success: function (returnValue) {

            //alert("Thành công");
            //
            //var n = returnValue.indexOf(","), status = '', fullPath = '';
            //if (n > 0) {
            //    status = returnValue.substring(0, n);
            //    if (status == "Success") {
            //        fullPath = returnValue.substring(n + 1, returnValue.length);
            //        window.location = urlDownloadFileExport + fullPath;
            //        ShowNotificationProcessor(status);
            //    }
            //    else {
            //        status = returnValue;
            //        ShowNotificationProcessor(status);
            //    }
            //}
        },
        error: function (data) {

            ShowNotificationProcessor(data.responseText);
        }
    });
}

//[Chuc.Nguyen] - Xuất tất cả dữ liệu ra excel, tối đa 10000 dòng.
function ExportAll(urlGetData, dataParameter, urlDownloadFileExport, pageNamePermission) {

    debugger;
    /// <signature>
    /// <summary> Xuất tất cả dữ liệu ra excel, tối đa 10000 dòng.</summary>
    /// <param name="urlGetData" type="String">Url lấy dữ liệu</param>
    /// <param name="dataParameter" type="Json">Các tham số</param>
    /// <param name="urlDownloadFileExport" type="String">url download</param>
    /// <returns type="Void"></returns>
    /// </signature>

    /// <signature>
    /// <summary> Xuất tất cả dữ liệu ra excel, tối đa 10000 dòng.</summary>
    /// <param name="urlGetData" type="String">Url lấy dữ liệu</param>
    /// <param name="dataParameter" type="Json">Các tham số</param>
    /// <param name="urlDownloadFileExport" type="String">url download</param>
    /// <param name="pageNamePermission" type="String">tên trang phân quyền (có thể dùng '@ConstantPermission.')</param>
    /// <returns type="Void"></returns>
    /// </signature>

    if (pageNamePermission != undefined) {
        var isAccess = checkPermission(PrivilegeType.Export, pageNamePermission);
        if (!isAccess) {
            return;
        }
    }

    ShowHre_ProgressBar();
    $.ajax({
        type: 'POST',
        url: urlGetData,
        data: dataParameter,
        dataType: 'json',
        //async: false,
        success: function (returnValue) {

            //if (returnValue.Data.length > 0) {
            //    alert(1);
            //    return;
            //}
            debugger;

            var n = returnValue.indexOf(","), status = '', fullPath = '';
            if (n > 0) {
                status = returnValue.substring(0, n);
                if (status == "Success") {
                    fullPath = returnValue.substring(n + 1, returnValue.length);
                    ShowNotificationProcessor(status);
                    window.location.href = urlDownloadFileExport + fullPath;
                }
            }
            else if (returnValue == "-1") {
                ShowNotificationProcessor("Không tìm thấy template");
            }
            else {
                ShowNotificationProcessor(returnValue);
            }
            CloseHre_ProgressBar();
        },
        error: function (data) {

            ShowNotificationProcessor(data.responseText);
            CloseHre_ProgressBar();
        }
    });
}

//[Chuc.Nguyen] - Xuất dữ liệu ra excel theo Id đã chọn
function ExportSelected(urlGetData, listIdSelected, listFields, urlDownloadFileExport, pageNamePermission) {
    /// <signature>
    /// <summary> Xuất dữ liệu ra excel theo Id đã chọn.</summary>
    /// <param name="urlGetData" type="String">Url lấy dữ liệu</param>
    /// <param name="listIdSelected" type="String">Danh sách Ids</param>
    /// <param name="listFields" type="String">Danh sách Các field hiển thị ra excel</param>
    /// <param name="urlDownloadFileExport" type="String">Url Download</param>
    /// <returns type="Void"></returns>
    /// </signature>

    /// <signature>
    /// <summary> Xuất dữ liệu ra excel theo Id đã chọn.</summary>
    /// <param name="urlGetData" type="String">Url lấy dữ liệu</param>
    /// <param name="listIdSelected" type="String">Danh sách Ids</param>
    /// <param name="listFields" type="String">Danh sách Các field hiển thị ra excel</param>
    /// <param name="urlDownloadFileExport" type="String">Url Download</param>
    /// <param name="pageNamePermission" type="String">Tên trang phân quyền</param>
    /// <returns type="Void"></returns>
    /// </signature>

    if (pageNamePermission != undefined) {
        var isAccess = checkPermission(PrivilegeType.Export, pageNamePermission);
        if (!isAccess) {
            return;
        }
    }
    ShowHre_ProgressBar();
    $.ajax({
        traditional: true,
        type: 'POST',
        url: urlGetData,
        data: { selectedIds: listIdSelected, valueFields: listFields },
        dataType: 'json',
        success: function (returnValue) {
            if (returnValue == "Error") {
                CloseHre_ProgressBar();
                ShowNotificationProcessor("Bạn Chưa Cấu Hình Template");
                return;
            }
            else if (returnValue == "NotTemplate") {
                CloseHre_ProgressBar();
                ShowNotificationProcessor("Không Có Template.");
                return;
            }
            var n = returnValue.indexOf(","), status = '', fullPath = '';
            if (n > 0) {
                status = returnValue.substring(0, n);
                if (status == "Success") {
                    fullPath = returnValue.substring(n + 1, returnValue.length);
                    window.location = urlDownloadFileExport + fullPath;
                    ShowNotificationProcessor(status);
                }
            }
            else if (returnValue == "-1") {
                ShowNotificationProcessor("Không tìm thấy template");
            }
            else {
                ShowNotificationProcessor(returnValue);
            }
            CloseHre_ProgressBar();
        },
        error: function (data) {
            ShowNotificationProcessor(data.responseText);
            CloseHre_ProgressBar();
        }
    });
}

function ExportWordByTemplateSelected(urlGetData, listIdSelected, listFields, urlDownloadFileExport, pageNamePermission) {

    /// <signature>
    /// <summary> Xuất dữ liệu ra excel theo Id đã chọn.</summary>
    /// <param name="urlGetData" type="String">Url lấy dữ liệu</param>
    /// <param name="listIdSelected" type="String">Danh sách Ids</param>
    /// <param name="listFields" type="String">Danh sách Các field hiển thị ra excel</param>
    /// <param name="urlDownloadFileExport" type="String">Url Download</param>
    /// <returns type="Void"></returns>
    /// </signature>

    /// <signature>
    /// <summary> Xuất dữ liệu ra excel theo Id đã chọn.</summary>
    /// <param name="urlGetData" type="String">Url lấy dữ liệu</param>
    /// <param name="listIdSelected" type="String">Danh sách Ids</param>
    /// <param name="listFields" type="String">Danh sách Các field hiển thị ra excel</param>
    /// <param name="urlDownloadFileExport" type="String">Url Download</param>
    /// <param name="pageNamePermission" type="String">Tên trang phân quyền</param>
    /// <returns type="Void"></returns>
    /// </signature>

    if (pageNamePermission != undefined) {
        var isAccess = checkPermission(PrivilegeType.Export, pageNamePermission);
        if (!isAccess) {
            return;
        }
    }

    $.ajax({
        traditional: true,
        type: 'POST',
        url: urlGetData,
        data: { selectedIds: listIdSelected, valueFields: listFields },
        dataType: 'json',
        success: function (returnValue) {

            if (returnValue == "Error") {

                ShowNotificationProcessor("Thông Báo", "Bạn Chưa Cấu Hình Template", "k-block k-info-colored");
                return;
            }

            var n = returnValue.indexOf(","), status = '', fullPath = '';
            if (n > 0) {
                status = returnValue.substring(0, n);
                if (status == "Success") {
                    fullPath = returnValue.substring(n + 1, returnValue.length);
                    window.location = urlDownloadFileExport + fullPath;
                    ShowNotificationProcessor(status);
                }
            }
            else {
                ShowNotificationProcessor(returnValue);
            }
        },
        error: function (data) {
            ShowNotificationProcessor(data.responseText);
        }
    });
}

//[Chuc.Nguyen] - Đọc lại dữ liệu cho lưới
function ReadGrid(gridName) {

    var grid = $('#' + gridName).data("kendoGrid");
    if (grid != null && grid.dataSource != null && grid != undefined) {
        grid.dataSource.read();
        selectedIds = [];
    }
}

//[Chuc.Nguyen] - Xử lý lỗi khi lấy dữ liệu trên lưới
function ErrorHandler(e) {
    if (e.errors) {
        var message = "";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    if (key == "Id") {
                        message = this + "\n" + message;
                    }
                    else {
                        message += this + "\n";
                    }
                });
            }
        });
        ShowNotificationProcessor(message);
    }
}


function PopUpCreate(idWindow, titlePopUp, pageNamePermission) {
    /// <signature>
    /// <summary>Hiển thị popup cho Create</summary>
    /// <param name="idWindow" type="String">tên popup</param>
    /// <param name="titlePopUp" type="String">Tiêu đề</param>
    /// <returns type="Void"></returns>
    /// </signature>

    /// <signature>
    /// <summary>Hiển thị popup cho Create</summary>
    /// <param name="idWindow" type="String">tên popup</param>
    /// <param name="titlePopUp" type="String">Tiêu đề</param>
    /// <param name="pageNamePermission" type="String">tên trang phân quyền</param>
    /// <returns type="Void"></returns>
    /// </signature>

    if (pageNamePermission != undefined) {
        var isAccess = checkPermission(PrivilegeType.Create, pageNamePermission);
        if (!isAccess) {
            return;
        }
    }
    var window = $("#" + idWindow).data("kendoWindow");
    window.refresh().center().open().title(titlePopUp);
}

//Hiển thị poup cho Edit
function PopUpEdit(idWindow, url, parameter, titlePopUp) {
    debugger;
    var window = $("#" + idWindow).data("kendoWindow");
    window.refresh({
        url: url,
        data: { id: parameter }
    }).center().open().title(titlePopUp);
}

//exapand
$(".panel-wrap").on("click", "span.k-i-arrowhead-s", function (e) {
    var contentElement = $(e.target).closest(".widget").find(">div");
    $(e.target)
        .removeClass("k-i-arrowhead-s")
        .addClass("k-i-arrowhead-n");

    kendo.fx(contentElement).expand("vertical").stop().play();
});

//collapse
$(".panel-wrap").on("click", "span.k-i-arrowhead-n", function (e) {
    var contentElement = $(e.target).closest(".widget").find(">div");
    $(e.target)
        .removeClass("k-i-arrowhead-n")
        .addClass("k-i-arrowhead-s");

    kendo.fx(contentElement).expand("vertical").stop().reverse();
});

/// <signature>
/// <summary>Mở edit Popup</summary>
/// <param name="gridName" type="String">gridName</param>
/// <param name="popUpName" type="String">popUpName</param>
/// <param name="urlGetView" type="String">urlGetView</param>
/// <returns type="Void"></returns>
/// </signature>
//[Chuc.Nguyen] - Dùng cho edit bằng popup

function EditGridPopUp(gridName, popUpName, urlGetView, pageNamePermission, titlePopUp) {


    /// <signature>
    /// <summary>Mở edit Popup</summary>
    /// <param name="gridName" type="String">gridName</param>
    /// <param name="popUpName" type="String">popUpName</param>
    /// <param name="urlGetView" type="String">urlGetView</param>
    /// <returns type="Void"></returns>
    /// </signature>

    /// <signature>
    /// <summary>Mở edit Popup</summary>        
    /// <param name="gridName" type="String">gridName</param>
    /// <param name="popUpName" type="String">popUpName</param>
    /// <param name="urlGetView" type="String">urlGetView</param>
    /// <param name="pageNamePermission" type="String">tên trang phân quyền</param>
    /// <returns type="Void"></returns>
    /// </signature>

    /// <signature>
    /// <summary>Mở edit Popup</summary>        
    /// <param name="gridName" type="String">gridName</param>
    /// <param name="popUpName" type="String">popUpName</param>
    /// <param name="urlGetView" type="String">urlGetView</param>
    /// <param name="pageNamePermission" type="String">tên trang phân quyền</param>
    /// <param name="titlePopUp" type="String">tên tiêu đề popup</param>
    /// <returns type="Void"></returns>
    /// </signature>


    debugger;
    $("#" + gridName).on("click", ".gridEditImgButton", function (e) {
        if (pageNamePermission != undefined) {
            var isAccess = checkPermission(PrivilegeType.Modify, pageNamePermission);
            if (!isAccess) {
                return;
            }
        }
        var grid = $("#" + gridName).data("kendoGrid");
        e.preventDefault();
        var row = $(this).closest("tr");
        var dataItem = grid.dataItem(row);
        PopUpEdit(popUpName, urlGetView, dataItem.ID, titlePopUp);
        ReadGrid(gridName);
    });
}

//[Chuc.Nguyen] - Chỉnh sửa dữ liệu trên lưới dùng cho edit qua trang
function EditGridNewPage(gridName, url, hrmWebSite, alias, pageNamePermission) {
    /// <signature>
    /// <summary> Chỉnh sửa dữ liệu trên lưới dùng cho edit qua trang.</summary>
    /// <param name="gridName" type="String">tên Grid</param>
    /// <param name="url" type="Json">đường dẫn (vd:#Hrm_Cat_Web/Cat_Position/Edit/)</param>
    /// <param name="hrmWebSite" type="String">tên web (vd:Hrm_Cat_Web)</param>
    /// <param name="alias" type="String">địa chỉ web với port (vd:'@_hrm_Cat_Web')</param>
    /// <returns type="Void"></returns>
    /// </signature>

    /// <signature>
    /// <summary> Chỉnh sửa dữ liệu trên lưới dùng cho edit qua trang.</summary>
    /// <param name="gridName" type="String">tên Grid</param>
    /// <param name="url" type="Json">đường dẫn (vd:#Hrm_Cat_Web/Cat_Position/Edit/)</param>
    /// <param name="hrmWebSite" type="String">tên web (vd:Hrm_Cat_Web)</param>
    /// <param name="alias" type="String">địa chỉ web với port (vd:'@_hrm_Cat_Web')</param>
    /// <param name="pageNamePermission" type="String">tên trang phân quyền (có thể dùng '@ConstantPermission.')</param>
    /// <returns type="Void"></returns>
    /// </signature>

    //if (pageNamePermission != undefined) {
    //    var isAccess = checkPermission(PrivilegeType.Modify, pageNamePermission);
    //    if (!isAccess) {
    //        return;
    //    }
    //}


    $("#" + gridName).on("click", ".gridEditImgButton", function (e) {
        //var hrefs = location.href.split("#");
        //if (hrefs.length > 1) {
        //    var str = "#" + hrefs[1] + "|" + this.name + "|" + this.lang;
        //    $("#CurrentLinkForClickBack").val(str);
        //}
        var grid = $("#" + gridName).data("kendoGrid");
        e.preventDefault();
        var row = $(this).closest("tr");
        var dataItem = grid.dataItem(row);
        KendoRouteMapping(url + dataItem.ID, hrmWebSite, alias);
    });
}

//[Chuc.Nguyen] - Delete trên lưới
function DeleteGrid(gridName, urlDelete, pageNamePermission) {
    /// <signature>
    /// <summary> Delete trên lưới.</summary>
    /// <param name="gridName" type="String">tên Grid</param>
    /// <param name="urlDelete" type="Json">đường dẫn </param>
    /// <returns type="Void"></returns>
    /// </signature>

    /// <signature>
    /// <summary> Delete trên lưới.</summary>
    /// <param name="gridName" type="String">tên Grid</param>
    /// <param name="urlDelete" type="Json">đường dẫn </param>
    /// <param name="pageNamePermission" type="String">tên trang phân quyền (có thể dùng '@ConstantPermission.')</param>
    /// <returns type="Void"></returns>
    /// </signature>

    if (pageNamePermission != undefined) {
        var isAccess = checkPermission(PrivilegeType.Delete, pageNamePermission);
        if (!isAccess) {
            return;
        }
    }


    $("#" + gridName).on("click", ".gridDeleteImgButton", function (e) {
        $.when(kendo.ui.ExtOkCancelDialog.show({
            title: "Thông Báo",
            message: "Bạn có chắc chắn muốn xóa?",
            icon: "k-ext-warning"
        })
        ).done(function (response) {
            var grid = $("#" + gridName).data("kendoGrid");
            e.preventDefault();
            var row = $(this).closest("tr");
            var dataItem = grid.dataItem(row);
            $.ajax({
                url: urlDelete,
                data: { id: dataItem.Id },
                type: 'POST',
                success: function (data) {
                    ReadGrid(gridName);
                    ShowNotificationProcessor("Success");
                },
                error: function (data) {
                    ShowNotificationProcessor(data.responseText);
                }
            });
        });
    });
}

//[Hien.Nguyen] - Xử lý check chọn nút trên treeview, lấy OrderNumber
function checkedNodeOrderNumbers(nodes, checkedNodes) {

    if (arguments.length == 3) {
        checkedNodeOrderNumbersAndName(nodes, checkedNodes, arguments[2]);
    } else {
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].checked) {
                checkedNodes.push(nodes[i].OrderNumber);
            }
            if (nodes[i].hasChildren) {
                checkedNodeOrderNumbers(nodes[i].children.view(), checkedNodes);
            }
        }
    }
}

//[Hien.Nguyen] - Xử lý check chọn nút trên treeview, lấy OrderNumber và Name
function checkedNodeOrderNumbersAndName(nodes, checkedNodes, checkedNodesName) {

    for (var i = 0; i < nodes.length; i++) {
        if (nodes[i].checked) {
            checkedNodes.push(nodes[i].OrderNumber);
            //checkedNodesName.push(nodes[i].Name);
            checkedNodesName.push(nodes[i].Name);
        }
        if (nodes[i].hasChildren) {
            checkedNodeOrderNumbersAndName(nodes[i].children.view(), checkedNodes, checkedNodesName);
        }
    }
}

//[Chuc.Nguyen] - Xử lý check chọn nút trên treeview
function checkedNodeIds(nodes, checkedNodes) {

    if (arguments.length == 3) {
        checkedNodeIdAndName(nodes, checkedNodes, arguments[2]);
    } else {
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].checked) {
                checkedNodes.push(nodes[i].ID);
            }
            if (nodes[i].hasChildren) {
                checkedNodeIds(nodes[i].children.view(), checkedNodes);
            }
        }
    }

}

//[Chuc.Nguyen] - Xử lý check chọn nút trên treeview
function checkedNodeIdAndName(nodes, checkedNodes, checkedNodesName) {

    for (var i = 0; i < nodes.length; i++) {
        if (nodes[i].checked) {
            checkedNodes.push(nodes[i].ID);
            //checkedNodesName.push(nodes[i].Name);
            checkedNodesName.push(nodes[i].Name);
        }
        if (nodes[i].hasChildren) {
            checkedNodeIdAndName(nodes[i].children.view(), checkedNodes, checkedNodesName);
        }
    }
}

//[Hieu.Van] - Xử lý update Status các dòng dữ liệu được chọn
function SetStatusSelectedWithPermission(gridName, urlDelete, selectedIds, status, userApproved, notification) {

    if (selectedIds != '' && selectedIds != null) {
        $.ajax({
            traditional: true,
            url: urlDelete,
            data: { selectedIds: selectedIds, status: status, userApproved: userApproved },
            type: 'POST',
            success: function (data) {
                checkedNodes = [];
                selectedIds = [];
                ReadGrid(gridName);
                if (data.ActionStatus == "NoPermission") {
                    ShowNotificationProcessor("Bạn Không Có Quyền Xử Lý Bản Ghi Này");
                }
                else if (data.ActionStatus == "NoApproveOTMySelf") {
                    ShowNotificationProcessor("Không Cho Phép Duyệt Tăng Ca Cho Bản Thân ");
                } else {
                    ShowNotificationProcessor(notification);
                }
            },
            error: function (data) {
                alert('Error');
            }
        });
        selectedIds = [];
    }
    else {
        window.ShowNotificationProcessor("Không có dữ lệu nào được chọn.");
    }
}

//// set language cho từng project
//function SetLanguageHRM(lang, _hrm_Web) {
//    
//    $.ajax({
//        url: _hrm_Web + 'Setting/setLanguage/',
//        data: { Language: lang },
//        type: 'POST',
//        datatype: 'json',
//        success: function (result) { }
//    });
//}
// set language cho từng project
function SetLanguageHRM(lang, _hrm_Main_Web, _hrm_Sys_Web) {



    $.ajax({
        url: _hrm_Main_Web + 'Setting/setLanguage/',
        data: { Language: lang },
        type: 'POST',
        datatype: 'json',
        success: function (result) { }
    });

    $.ajax({
        url: _hrm_Sys_Web + 'Setting/setLanguage/',
        data: { Language: lang },
        type: 'POST',
        datatype: 'json',
        success: function (result) { }
    });

}


//[Tam.Le] - Xử lý update Status các dòng dữ liệu được chọn
function SetStatusSelected(gridName, urlDelete, selectedIds, status, notification) {

    if (selectedIds != '' && selectedIds != null) {
        $.ajax({
            traditional: true,
            url: urlDelete,
            data: { selectedIds: selectedIds, status: status },
            type: 'POST',
            success: function (data) {
                checkedNodes = [];
                selectedIds = [];
                ReadGrid(gridName);
                if (data.ActionStatus == "NoPermission") {
                    ShowNotificationProcessor("Bạn Không Có Quyền Xử Lý Bản Ghi Này");
                } else {
                    ShowNotificationProcessor(notification);
                }
            },
            error: function (data) {
                alert('Error');
            }
        });
        selectedIds = [];
    }
    else {
        window.ShowNotificationProcessor("Không có dữ lệu nào được chọn.");
    }
}

function SetMethodPaymentSelected(gridName, urlDelete, selectedIds, status) {

    if (selectedIds != '' && selectedIds != null) {
        $.ajax({
            traditional: true,
            url: urlDelete,
            data: { selectedIds: selectedIds, status: status },
            type: 'POST',
            success: function (data) {
                //alert(data);
                selectedIds = [];

                if (data.ActionStatus == "NoPermission") {
                    ShowNotificationProcessor("Bạn Không Có Quyền Xử Lý Bản Ghi Này");
                } else {
                    ShowNotificationProcessor(data.ActionStatus);
                }
                ReadGrid(gridName);
            },
            error: function (data) {
                alert('Error');
            }
        });
        selectedIds = [];
    }
    else {
        window.ShowNotificationProcessor("Không có dữ lệu nào được chọn.");
    }
}

//[Chuc.Nguyen] - Xử lý ẩn hiện các input trong khung tìm kiếm
function ShowHidePanel() {
    //$(".contendSearchWidge").hide();
    $(".panel-wrap").on("click", "span.k-i-arrowhead-s", function (e) {
        $(".contendSearchWidge").show();
        var contentElement = $(e.target).closest(".widget").find(">div");
        $(e.target)
            .removeClass("k-i-arrowhead-s")
            .addClass("k-i-arrowhead-n");

        kendo.fx(contentElement).expand("vertical").stop().play();

    });

    //collapse
    $(".panel-wrap").on("click", "span.k-i-arrowhead-n", function (e) {
        $(".contendSearchWidge").show();
        var contentElement = $(e.target).closest(".widget").find(">div");
        $(e.target)
            .removeClass("k-i-arrowhead-n")
            .addClass("k-i-arrowhead-s");

        kendo.fx(contentElement).expand("vertical").stop().reverse();

    });
}

function Search(btnSeach, treeViewName, gridName) {
    $("#" + btnSeach).click(function () {

        var treeView = $("#" + treeViewName).data("kendoTreeView");
        checkedNodeIds(treeView.dataSource.view(), checkedNodes);

        var grid = $('#' + gridName).data("kendoGrid");
        grid.dataSource.read();
        checkedNodes = [];
    });
}

//[Chuc.Nguyen] - Xử lý dùng chung cho checkbox trên lưới
function GridCheckbox(gridName) {

    //$('#' + gridName).on('change', 'input[type=checkbox][id!=mastercheckbox]', function (e) {
    //    var $check = $(this);
    //    if ($check.is(":checked") == true) {
    //        var checked = jQuery.inArray($check.val(), selectedIds);
    //        if (checked == -1) {
    //            //add id to selectedIds.  
    //            selectedIds.push($check.val());
    //        }
    //    }
    //    else {
    //        var checked = jQuery.inArray($check.val(), selectedIds);
    //        if (checked > -1) {
    //            //remove id from selectedIds.  
    //            selectedIds = $.grep(selectedIds, function (item, index) {
    //                return item != $check.val();
    //            });
    //        }
    //    }
    //});
    //var chk = false;
    //$('input#mastercheckbox').click(function () {
    //    
    //    //Show Select Page
    //    //$("div#MultipleSelectForPage").show(50);
    //    //alert(this.checked);
    //    //$(".checkboxGroups").prop("checked", chk);
    //    ////alert(2);
    //    //$('.checkboxGroups').attr('checked', $(this).is(':checked')).change();
    //    //if ($(this).attr('checked')) {
    //    if (chk == false) {
    //        $('.checkboxGroups').prop('checked', true);
    //        chk = true;
    //    }
    //    else {
    //        $('.checkboxGroups').prop('checked',false);
    //        chk = false;
    //    }
    //    $("div#MultipleSelectForPage").show(50);
    //    //} else {
    //    //    $('.checkboxGroups').removeAttr('checked');
    //    //}
    //});

    //[Hien.Nguyen]Xử lý ẩn hiện select Page



};
$('html').click(function () {
    $("div#MultipleSelectForPage").hide(50);
});
$('#MultipleSelectForPage').click(function (event) {
    event.stopPropagation();
});



//[Hien.Nguyen] 
$(document).on("click", "input#mastercheckbox", function () {

    var t = $('input[type=checkbox][class=checkboxGroups]');
    $('input.checkboxGroups').prop('checked', this.checked);//Xử lý check hoặc uncheck các checkbox con

    if (this.checked == true) {
        $("div#MultipleSelectForPage").show(50);
        for (var i = 0; i < t.length; i++) {
            if (t[i].defaultValue != '00000000-0000-0000-0000-000000000000') {
                if (selectedIds.indexOf(t[i].defaultValue) == -1) {
                    selectedIds.push(t[i].defaultValue);
                }
            }
        }
    }
    else {//Nếu là uncheck thì bỏ các giá trị đã lưu trong selectedIds
        //for (var i = 0; i < t.length; i++) {
        //    if (t[i].defaultValue != '00000000-0000-0000-0000-000000000000') {
        //        if (selectedIds.indexOf(t[i].defaultValue) != -1) {
        //            selectedIds.splice(selectedIds.indexOf(t[i].defaultValue), 1);
        //        }
        //    }
        //}
        selectedIds = [];
        var radio = document.getElementsByName('selectOnePageOrAllPage');
        radio[0].checked = true;//Cho select page trở về mặt định
    }
});

//[Hien.Nguyen] Xử lý check lại các checkbox đã chọn ở trang trước
function changePageGird() {

    $('#mastercheckbox').prop('checked', false);
    var result = $("input[type=checkbox][id!=mastercheckbox][class=checkboxGroups]");
    for (var i = 0; i < result.length; i++) {
        if (selectedIds.indexOf(result[i].defaultValue) != -1) {
            result[i].checked = true;
        }
    }
};



//[Chuc.Nguyen] - Lấy thông tin của file khi upload
function getFileInfo(e) {
    return $.map(e.files, function (file) {
        var info = file.name;

        if (file.size > 0) {
            info += " (" + Math.ceil(file.size / 1024) + " KB)";
        }
        return info;
    }).join(", ");
}


function CheckedGrid(gridName, checkedIds) {
    //$('#' + gridName + ' input[type=checkbox][id!=mastercheckbox]').each(function () {
    //    var currentId = $(this).val();
    //    var checked = jQuery.inArray(currentId, selectedIds);
    //    $(this).attr('checked', checked > -1);
    //});
}

//Jump anchor
function jumpAnchor(anchor) {
    //var url = location.href;
    //location.href = "#" + anchor;
    //history.replaceState(null, null, url)
}

//[hien.pham] function resizecontrol
//param id => giá trị vào id của div hay control
//param value => giá trị cần trừ lại nếu có:
//vd: màn hình hiển thị có chiều cao: 500, value trừ lại 50px => giá trị chiều cao là: 500 - 50 => 450px

function resizeControl(id, value) {
    //
    var _height = $(window).innerHeight();
    $("#" + id).height(_height - value);
}
// Show thông báo loading khi search...
function showAjaxLoading() {
    $("#loading").css("display", "block");

}
function hideAjaxLoading() {
    $("#loading").css("display", "none");

}
// show thông báo saving khi save...
function showAjaxSaving() {
    $("#saving").css("display", "block");

}
function hideAjaxSaving() {
    $("#saving").css("display", "none");

}

//[Hien.Nguyen] Convert định dạng dd/MM/yyy sang định dạng của Server hiện tại
function ConvertDatetime(urlPost, date, Isto) {

    Isto = typeof Isto !== 'undefined' ? Isto : false;
    var result = "";
    if (date === undefined || date == null)
        return "";
    $.ajax({
        type: 'POST',
        url: urlPost,
        data: { value: date, IsTo: Isto },
        dataType: 'json',
        async: false,
        success: function (returnValue) {
            result = returnValue;
            return result;
        },
        error: function (data) {

        }
    });
    return result;
};

//[Hieu.Van] Nối chuỗi string time vào dateTime
function JoinTimeInDate(urlPost, date, time) {

    if (date === undefined || date == null)
        return "";
    if (time === undefined || time == null)
        return "";
    $.ajax({
        type: 'POST',
        url: urlPost,
        data: { date: date, time: time },
        dataType: 'json',
        async: false,
        success: function (returnValue) {
            result = returnValue;
            return result;
        },
        error: function (data) {

        }
    });
    return result;
};


//Init Popup Loading...
function CreateProgress(value) {
    $("#hrm_main_content").css("display", "none");
    $("#ModalsProgress").data("kendoWindow").open();
    if (!isNaN(value))
        $("#progressBar").data("kendoProgressBar").value(value);
};
////Update value cho ProgreessBar
//function UpdateValueProgress(value) {
//    $("#progressBar").data("kendoProgressBar").value(value);
//};
//xử lý khi chạy xong Progress
function onCompleteProgress() {
    $("#hrm_main_content").css("display", "block");
    $("#ModalsProgress").data("kendoWindow").close();
    setTimeout(function () {
        $("#progressBar").data("kendoProgressBar").value(0);
    }, 100);
};
var IsActiveProgress = true;//Biến để cho phép progress đang run hay không
//xử lý sự kiện close popup
function onCloseModals() {
    IsActiveProgress = false;
    $("#hrm_main_content").css("display", "block");
    // $("#ModalsProgress").data("kendoWindow").close();
    setTimeout(function () {
        $("#progressBar").data("kendoProgressBar").value(0);
    }, 100);
};


//Xử lý các khóa ngoại khi giá trị null
function onEdit(e) {

    ////Xử lý dblclick
    //var myGrid = $('#' + e.sender._cellId.replace("_active_cell", "")).data("kendoGrid");

    //myGrid.element.on('click', 'tbody>tr>td:not(.k-edit-cell)', function (e) {
    //    myGrid.closeCell();
    //    return;
    //});

    //myGrid.element.on("dblclick", "tbody>tr>td:not(.k-edit-cell)", "dblclick", function (e) {
    //    myGrid.editCell($(this));
    //});

    var dropDown = e.container.find("[data-role=dropdownlist]").data("kendoDropDownList");

    if (dropDown) {
        dropDown.bind("change", function (e) {
            var fieldName = dropDown.element.attr("name");
            var grid = e.sender.wrapper.closest(".k-grid").data("kendoGrid");
            var dataItem = grid.dataItem(e.sender.wrapper.closest("tr"));
            //If current value is null
            //change the model value
            //dataItem.EmployeeID = 0;
            //get the currently selected value from the DDL
            var currentlySelectedValue = e.sender.value();
            //set the value to the model
            dataItem.set(fieldName, currentlySelectedValue);
        })
    }
};


//Xử lý ẩn hiện tabbar khi click btn
function EvenClickBtn(id) {

    $("#" + isSelectTabId).hide();
    $("#" + id).show();
    isSelectTabId = id;


    // alert(tabID + "---" + isSelectTabId + "----" + $("#" + isSelectTabId).kendoTabStrip().data("kendoTabStrip").select().index());

    var tabIndex = $("#" + isSelectTabId).kendoTabStrip().data("kendoTabStrip").select().index();
    if (tabIndex != -1) {
        tabIndex++;
        tabID = isSelectTabId + "-" + tabIndex;
        LoadForm();
    }
};


function ShowHre_ProgressBar() {
    document.getElementById("hrm_main_content").style.opacity = "0.5";
    $("#Hre_ProgressBar").show();
};

function CloseHre_ProgressBar() {
    document.getElementById("hrm_main_content").style.opacity = "1";
    $("#Hre_ProgressBar").hide();
};

function ShowHre_ProgressBarForElement(idDiv) {
    document.getElementById(idDiv).style.opacity = "0.5";
    $("#Hre_ProgressBar").show();
};

function CloseHre_ProgressBarForElement(idDiv) {
    document.getElementById(idDiv).style.opacity = "1";
    $("#Hre_ProgressBar").hide();
};

//[Hien.Nguyen]
//Validation datetimepicker
function ValidationDatetime(e) {

    if (e.sender.value() != null) {
        debugger;
        if (kendo.parseDate(e.sender.value()) == null) {
            ShowNotificationProcessor("Ngày tháng không hợp lệ !");
            e.sender.value("");
        }
    }
};

//Reload Control Dropdownlist
function ReloadDropDownList(e) {
    $('#' + e).data('kendoDropDownList').dataSource.read();
};

//xử lý ẩn checkbox cho cây phòng ban có check box
function EventDataBoundTreeViewOrg(e) {
    var IdTreeView = e.sender.element[0].id;
    var tree = $('#' + IdTreeView).data('kendoTreeView');

    var listNode = e.node;
    for (var i = 0; i < listNode.length; i++) {
        var dataItem = tree.dataItem(listNode[i]);
        if (!dataItem.IsShow) {
            var Node = $("input[type='checkbox'][value='" + dataItem.id + "']");
            Node.parent().css('opacity', '0.5').css('pointer-events', 'none');
            Node.parent().next().css('opacity', '0.5').css('pointer-events', 'none');
        }
    }
};

//xử lý ẩn checkbox cho cây phòng ban ko check box
function EventDataBoundTreeViewOrgNoCheckBox(e) {
    var IdTreeView = e.sender.element[0].id;
    var tree = $('#' + IdTreeView).data('kendoTreeView');

    var listNode = e.node;
    for (var i = 0; i < listNode.length; i++) {
        var dataItem = tree.dataItem(listNode[i]);
        if (!dataItem.IsShow) {
            listNode[i].childNodes[0].children[1].style.opacity = 0.5;
            listNode[i].childNodes[0].children[1].style.pointerEvents = "none";
        }
    }
};


