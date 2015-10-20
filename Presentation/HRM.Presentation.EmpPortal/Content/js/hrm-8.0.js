var dateFields = [];
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
function GridDynamicColum(gridName, gridData) {
    debugger;
    //var listFileName = gridData[0];
    //gridData.splice(0,1);
    var model = generateModel(gridData[0]);
    debugger;
    var parseFunction;
    if (dateFields.length > 0) {
        debugger;

        parseFunction = function (response) {
            debugger;
            for (var i = 0; i < response.length; i++) {
                debugger;
                for (var fieldIndex = 0; fieldIndex < dateFields.length; fieldIndex++) {
                    debugger;

                    var record = response[i];

                    record[dateFields[fieldIndex]] = kendo.parseDate(record[dateFields[fieldIndex]], "dd/MM/yyyy")


                    // kendo.toString(kendo.parseDate(record[dateFields[fieldIndex]]), "dd/MM/yyyy");
                }
            }
            return response;
        };
    }
    debugger;
    var grid = $("#" + gridName).kendoGrid({
        dataSource: {
            data: gridData,
            schema: {
                model: model,
                parse: parseFunction
            }
        },
        //editable: true,
        sortable: true,
        selectable: true,
        scrollable: true,
        resizable: true
    });
    debugger;

    for (var name in listFileName) {
        debugger;
        $("#" + gridName + " thead [data-field=" + name + "] .k-link").html(listFileName[name]);
    }
    //var newstring ='';
    //var listTitle = $("#" + gridName + " .k-grid-header .k-header");
    //for (var i = 0; i < listTitle.length; i++) {
    //    debugger;
    //    newstring += listTitle[i].innerText +',';
    //}
    //debugger;
    //$.ajax({
    //    url: "http://localhost:4000/Hre_GetData/Translate/",
    //    data: { field: newstring },
    //    type: 'POST',
    //    datatype: 'json',
    //    async: false,
    //    success: function (data) {
    //        debugger;
    //        for (var pro in data) {
    //            $("#" + gridName + " thead [data-field=" + pro + "] .k-link").html(data[pro]);
    //        }
    //    }
    //});


}
function generateModel(gridData) {
    debugger;
    var model = {};
    model.id = "ID";
    var fields = {};
    for (var property in gridData) {
        debugger;

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
    model.fields = fields;

    return model;
}

function SetRowColor(gridName, arrayConfigField) {
    debugger;
    if (gridName != "" && gridName != undefined && arrayConfigField != null && arrayConfigField != undefined && arrayConfigField.length > 0) {
        var grid = $('#' + gridName).data("kendoGrid");
        var dataSource = grid.dataSource.data();
        for (var i = 0; i < arrayConfigField.length; i++) {
            if (arrayConfigField[i].length == 4) {
                var field = arrayConfigField[i][0];
                var value = arrayConfigField[i][1];
                var background = arrayConfigField[i][2];
                var color = arrayConfigField[i][3];
                grid.tbody.find('>tr').each(function () {
                    var dataItem = grid.dataItem(this);
                    if (dataItem[field] == value) {
                        $(this).attr('style', 'background:' + background + ';color:' + color + ';');
                    }
                });
            }
        }
    }
}

function ApproveSelected(gridId, url, permissionName) {
    debugger;

    if (permissionName != undefined) {
        var permissionNotAllow_Msg = 'Bạn Không Có Quyền Duyệt Đối Tượng Này !';
        var isAccess = checkPermission(PrivilegeType.Create, permissionName, false);
        if (!isAccess) {
            ShowNotificationProcessor(permissionNotAllow_Msg);
            return;
        }
    }

    var error = 0, fistApproved = 0, approved = 0;
    if (selectedIds != null && selectedIds != undefined && selectedIds.length > 0) {
        if (gridId != "" && gridId != undefined) {
            var grid = $('#' + gridId).data("kendoGrid");
            grid.tbody.find('>tr').each(function () {
                debugger;
                var dataItem = grid.dataItem(this);
                var id = dataItem.ID;
                if (selectedIds.indexOf(id) > -1) {
                    var status = dataItem.Status;
                    if (status != "Từ Chối" || status != "E_REJECTED") {
                        if (status != "Duyệt" || status != "E_APPROVED") {
                            var userId = $("#UserLoginId").val();
                            var userApprove1 = dataItem.UserApproveID;
                            var userApprove2 = dataItem.UserApproveID2;
                            debugger;
                            //Trường hợp người duyệt khác nhau
                            if (userApprove1 != userApprove2) {
                                //TH1: Người duyệt đầu duyệt
                                if (userApprove1 == userId) {
                                    if (status != 'E_FIRST_APPROVED' || status != "Người Duyệt Đầu") {
                                        var st = SetStatus(gridId, url, id, "E_FIRST_APPROVED");
                                        if (st == true) {
                                            fistApproved++;
                                        }
                                        else {
                                            error++;
                                        }
                                    }
                                    else {
                                        error++;
                                    }
                                }
                                //TH2: Người duyệt cuối
                                if (userApprove2 == userId) {
                                    if (status == 'E_FIRST_APPROVED' || status == "Người Duyệt Đầu") {
                                        var st = SetStatus(gridId, url, id, "E_APPROVED");
                                        if (st == true) {
                                            approved++;
                                        }
                                        else {
                                            error++;
                                        }
                                    } else if (status != 'E_SUBMIT' || status != "Yêu Cầu") {
                                        var st = SetStatus(gridId, url, id, "E_APPROVED");
                                        if (st == true) {
                                            approved++;
                                        }
                                        else {
                                            error++;
                                        }
                                    }
                                    else {
                                        error++;
                                    }
                                }
                            }
                                //Trường hợp người duyệt giống nhau
                            else {
                                var st = SetStatus(gridId, url, id, "E_APPROVED");
                                if (st == true) {
                                    approved++;
                                }
                                else {
                                    error++;
                                }
                            }
                        }
                        else {
                            approved++;
                        }
                    } else {
                        error++;
                    }
                }
            });
            selectedIds = [];
            var strStatus = "";
            if (approved > 0) {
                strStatus += "Duyệt thành công: " + approved;
            }
            if (fistApproved > 0) {
                strStatus += " | Duyệt đầu thành công: " + fistApproved;
            }
            if (error > 0) {
                strStatus += " | Duyệt thất bại: " + error;
            }

            ShowNotificationProcessor(strStatus);
            ReadGrid(gridId);
        }
    }
    else {
        ShowNotificationProcessor("Không có dữ liệu nào được chọn.");
    }
    return false;
}

function RejectSelected(gridId, url,permissionName) {

    if (permissionName != undefined) {
        var permissionNotAllow_Msg = 'Bạn Không Có Quyền Từ Chối Đối Tượng Này !';
        var isAccess = checkPermission(PrivilegeType.Create, permissionName, false);
        if (!isAccess) {
            ShowNotificationProcessor(permissionNotAllow_Msg);
            return;
        }
    }
    
    debugger;
    var success = 0, error = 0;
    if (selectedIds != null && selectedIds != undefined && selectedIds.length > 0) {
        if (gridId != "" && gridId != undefined) {
            var grid = $('#' + gridId).data("kendoGrid");
            grid.tbody.find('>tr').each(function () {
                debugger;
                var dataItem = grid.dataItem(this);
                if (selectedIds.indexOf(dataItem.ID) > -1) {
                    var status = dataItem.Status;
                    if (status != "Từ Chối" && status != "E_REJECTED") {
                        var st = SetStatus(gridId, url, dataItem.ID, "E_REJECTED");
                        if (st == true) {
                            success++;
                        }
                        else {
                            error++;
                        }
                    }
                    else
                    {
                        success++;
                    }
                }
            });
            selectedIds = [];
            ReadGrid(gridId);
        }
        var strStatus = "";
        if (success > 0) {
            strStatus += "Xử lý thành công: " + success+ " dòng";
        }
        if (error > 0 && success > 0) {
            strStatus += "| Xử lý Có lỗi: " + error +" dòng.";
        }
        else if (error > 0) {
            strStatus += "Xử lý Có lỗi: " + error + " dòng.";
        }

        ShowNotificationProcessor(strStatus);
    }
    else {
        ShowNotificationProcessor("Không có dữ liệu nào được chọn.");
    }
    return false;
}

function RejectSelectedWithReson(gridId, url, reason, permissionName) {

    if (permissionName != undefined) {
        var permissionNotAllow_Msg = 'Bạn Không Có Quyền Từ Chối Đối Tượng Này !';
        var isAccess = checkPermission(PrivilegeType.Create, permissionName, false);
        if (!isAccess) {
            ShowNotificationProcessor(permissionNotAllow_Msg);
            return;
        }
    }

    debugger;
    var success = 0, error = 0;
    if (selectedIds != null && selectedIds != undefined && selectedIds.length > 0) {
        if (gridId != "" && gridId != undefined) {
            var grid = $('#' + gridId).data("kendoGrid");
            grid.tbody.find('>tr').each(function () {
                debugger;
                var dataItem = grid.dataItem(this);
                if (selectedIds.indexOf(dataItem.ID) > -1) {
                    var status = dataItem.Status;
                    if (status != "Từ Chối" && status != "E_REJECTED") {
                        var st = SetStatusWithReason(gridId, url, dataItem.ID, reason, "E_REJECTED");
                        if (st == true) {
                            success++;
                        }
                        else {
                            error++;
                        }
                    }
                    else {
                        success++;
                    }
                }
            });
            selectedIds = [];
            ReadGrid(gridId);
        }
        var strStatus = "";
        if (success > 0) {
            strStatus += "Xử lý thành công: " + success + " dòng";
        }
        if (error > 0 && success > 0) {
            strStatus += "| Xử lý Có lỗi: " + error + " dòng.";
        }
        else if (error > 0) {
            strStatus += "Xử lý Có lỗi: " + error + " dòng.";
        }

        ShowNotificationProcessor(strStatus);
    }
    else {
        ShowNotificationProcessor("Không có dữ liệu nào được chọn.");
    }
    return false;
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


    $.ajax({
        type: 'POST',
        url: urlGetData,
        data: dataParameter,
        dataType: 'json',
        success: function (returnValue) {

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

    $.ajax({
        traditional: true,
        type: 'POST',
        url: urlGetData,
        data: { selectedIds: listIdSelected, valueFields: listFields },
        dataType: 'json',
        success: function (returnValue) {

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
    debugger;
    var grid = $('#' + gridName).data("kendoGrid");
    if (grid != null && grid.dataSource != null && grid != undefined) {
        grid.dataSource.read();
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
    debugger;
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
    debugger;
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
//Hieu.Van - Chỉ set status, không thông báo với Lý Do 
function SetStatusWithReason(gridName, url, selectedIds, reason, status) {
    debugger;
    var st = false;
    if (selectedIds != '' && selectedIds != null) {
        $.ajax({
            traditional: true,
            url: url,
            data: { selectedIds: selectedIds, status: status, reason: reason },
            async: false,
            type: 'POST',
            success: function (data) {
                debugger;
                st = true;
            }
        });
    }
    return st;
}

//Chuc.Nguyen - Chỉ set status, không thông báo
function SetStatus(gridName, url, selectedIds, status) {
    debugger;
    var st = false;
    if (selectedIds != '' && selectedIds != null) {
        $.ajax({
            traditional: true,
            url: url,
            data: { selectedIds: selectedIds, status: status },
            async: false,
            type: 'POST',
            success: function (data) {
                debugger;
                st = true;
            }
        });
    }
    return st;
}
//[Tam.Le] - Xử lý update Status các dòng dữ liệu được chọn
function SetStatusSelected(gridName, urlDelete, selectedIds, status, notification) {
    debugger;
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
                ShowNotificationProcessor(notification);
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
    debugger;
    if (selectedIds != '' && selectedIds != null) {
        $.ajax({
            traditional: true,
            url: urlDelete,
            data: { selectedIds: selectedIds, status: status },
            type: 'POST',
            success: function (data) {
                //alert(data);
                selectedIds = [];
                debugger;
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
    debugger;
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

//[Hien.Nguyen] 
$(document).on("click", "input#mastercheckbox", function () {
    debugger;
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
    debugger;
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
    $("#loading111").css("display", "block");

}
function hideAjaxLoading() {
    $("#loading111").css("display", "none");

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
    debugger;
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
//xử lý sự kiện close popup
function onCloseModals() {
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

    debugger;
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

//HieuVan bỏ: do xử lý task 0048151
//[Hien.Nguyen]
//Validation datetimepicker
function ValidationDatetime(e) {
    debugger;
    if (kendo.parseDate(e.sender.value()) == null) {
        //ShowNotificationProcessor("Ngày tháng không hợp lệ !");
        e.sender.value("");
    }
};
