/*
Constant cho loại phân quyền là : Create , Modify ,Export , Import , View , None
*/
var PrivilegeType = new function () {
    this.None = "None";
    this.View = "View";
    this.Delete = "Delete";
    this.Create = "Create";
    this.Modify = "Modify";
    this.Approve = "Approve";
    this.Print = "Print";
    this.Export = "Export";
    this.Import = "Import";
    this.Attach = "Attach";
};  // close PrivilegeType
Object.freeze(PrivilegeType);



var Repository;
(function (Repository) {
    var GenericRepository = (function () {
        function GenericRepository(server, route) {
            this.Server = server;
            this.Route = route;
        }
        GenericRepository.prototype.Get = function (key, callback) {
            $.ajax({
                url: this.Server + this.Route,
                type: "GET",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "id=" + key,
            });
        };
        GenericRepository.prototype.Post = function (queryEntity, callback) {
            debugger;
            var dataReturn = null;
            var entity = JSON.stringify(queryStringToJSON(slpitQueryEntity(queryEntity)));
            $.ajax({
                url: this.Server + this.Route,
                crossDomain: true,
                type: "POST",
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: entity,
                success: function (data) {
                    debugger;

                    dataReturn = data;
                    if (typeof (data) == "string") {
                        callback(data);
                    }
                    else {
                        $("#ID").val(data.ID);
                        $("#Language_ID").val(data.ID);
                        $("#Computing_ID").val(data.ID);
                        $("#Qualification_ID").val(data.ID);
                        $("#Reward_ID").val(data.ID);
                        $("#Discipline_ID").val(data.ID);
                        $("#Accident_ID").val(data.ID);
                        $("#CandidateHistory_ID").val(data.ID);
                        $("#WorkHistory_ID").val(data.ID);
                        $("#Visa_ID").val(data.ID);
                        $("#Relative_ID").val(data.ID);
                        $("#PartyUnion_ID").val(data.ID);
                        $("#AppendixContract_ID").val(data.ID);
                        if (data.Contract_ID != '' && data.Contract_ID == 'undefined') {
                            $("#Contract_ID").val(data.Contract_ID);
                        }
                        if (data.ContractID != '' && data.ContractID == 'undefined') {
                            $("#Contract_ID").val(data.ContractID);
                        }
                       

                        callback(data.ActionStatus);
                    }
                },

                error: function (x, e) {
                    debugger;
                    if (x.status == 0) {
                        alert('You are offline!!\n Please Check Your Network.');
                    } else if (x.status == 404) {
                        alert('Requested URL not found.');
                    } else if (x.status == 550) {
                        alert(x.responseText);
                    } else if (x.status == 500) {
                        alert('Internel Server Error.');
                    } else if (e == 'parsererror') {
                        alert('Error.\nParsing JSON Request failed.');
                    } else if (e == 'timeout') {
                        alert('Request Time out.');
                    } else {
                        alert('Unknow Error.\n' + x.responseText);
                    }
                }
            });
            return dataReturn;
        };

        GenericRepository.prototype.PostForm = function (formName, callback) {
            $.ajax({
                url: this.Server + this.Route,
                type: "POST",
                dataType: "json",
                data: formName.serialize(),
                statusCode: {
                    200: function (entity) {
                        callback('success');
                    },
                    201: function (entity) {
                        //  alert('flase');
                        //callback(response);
                    },
                    400: function (entity) {
                        // alert('invalid!!!!');
                        //callback(response);
                    }
                }
            });
        };

        GenericRepository.prototype.Put = function (entity, callback) {
            //
            $.ajax({
                url: this.Server + this.Route,
                type: "PUT",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(entity),
                statusCode: {
                    200: function (model) {

                        //callback(response);
                    }
                }
            });
        };
        return GenericRepository;
    })();
    Repository.GenericRepository = GenericRepository;
})(Repository || (Repository = {}));


function onCreate(formValidate, url, route) {
    debugger;

    ShowHre_ProgressBar();

    var dataReturn = null;
    var repo = new Repository.GenericRepository(url, route);
    dataReturn = repo.Post(formValidate.serialize(), function (responseMessage) {
        debugger;
        if (responseMessage != "Change") {
            if (responseMessage == "ErrorDateOfBirth") {
                return dataReturn;
            }
            ShowNotificationProcessor(responseMessage);
        }
    });

    CloseHre_ProgressBar();

    return dataReturn;
}

function onCreateForm(formValidate, url, route) {

    ShowHre_ProgressBar();

    var repo = new Repository.GenericRepository(url, route);
    repo.PostForm(formValidate, function (responseMessage) {
        alert(reponseMessage);
    });

    CloseHre_ProgressBar();
}

//[Chuc.Nguyen] - Xử lý delete các dòng dữ liệu được chọn
function DeleteSelected(gridName, urlDelete, parameter, pageNamePermission) {

    ShowHre_ProgressBar();


    /// <signature>
    /// <summary>delete các dòng dữ liệu được chọn</summary>
    /// <param name="gridName" type="String">tên Grid</param>
    /// <param name="urlDelete" type="String">Url</param>
    /// <param name="parameter" type="String">Tham số</param>
    /// <returns type="Void"></returns>
    /// </signature>

    /// <signature>
    /// <summary>delete các dòng dữ liệu được chọn</summary>
    /// <param name="gridName" type="String">tên Grid</param>
    /// <param name="urlDelete" type="String">Url</param>
    /// <param name="parameter" type="String">Tham số</param>
    /// <param name="pageNamePermission" type="String">tên trang phân quyền</param>
    /// <returns type="Void"></returns>
    /// </signature>

    if (pageNamePermission != undefined) {
        var isAccess = checkPermission(PrivilegeType.Delete, pageNamePermission);
        if (!isAccess) {
            return;
        }
    }
    debugger;

    if (parameter != '' && parameter != null) {
        $.when(kendo.ui.ExtOkCancelDialog.show({
            title: "Thông Báo",
            message: "Bạn có chắc chắn muốn xóa?",
            icon: "k-ext-warning"
        })
        ).done(function (response) {
            debugger;
            if (response.button == "OK") {
                $.ajax({
                    traditional: true,
                    type: 'POST',
                    url: urlDelete,
                    dataType: 'html',
                    data: { selectedIds: parameter },
                    success: function (data) {

                        CloseHre_ProgressBar();

                        debugger;
                        selectedIds = [];
                        ReadGrid(gridName);
                        window.ShowNotificationProcessor("Success");
                    },
                    error: function (data) {
                        window.ShowNotificationProcessor(data.responseText);

                    }
                });
            }
        });
    } else {

        window.ShowNotificationProcessor("Không có dữ lệu nào được chọn.");
        CloseHre_ProgressBar();
    }



}

function onCreateOrEditGrid(formValidate, url, route, gridId) {

    ShowHre_ProgressBar();

    var form = $(formValidate);
    var repo = new Repository.GenericRepository(url, route);
    repo.Post(form.serialize(), function (responseMessage) {
        var grid = $("#" + gridId).data("kendoGrid");
        grid.dataSource.read();
    });

    CloseHre_ProgressBar();
}

var queryStringToJSON = function (url) {
    if (url === '')
        return '';
    var pairs = (url || location.search).split('&');
    var result = {};
    for (var idx in pairs) {
        var pair = pairs[idx].split('=');
        if (!!pair[0] && !result.hasOwnProperty(pair[0].toLowerCase())) {

            var strValue = decodeURIComponent(pair[1] || '').replace(/\+/g, " ");
            result[pair[0].toLowerCase()] = convertStringToDateTime(strValue);
        }
    }
    return result;
}

var convertStringToDateTime = function (_strDate) {

    // var _host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port : '') + "/";
    // var _url = "http://localhost:8000/Canteen_GetData/GetFormatDate/";
    var _url = window.location.protocol + "//" + window.location.hostname + ":5000/Sys_GetData/GetFormatDate/";
    //var _url = _host + "Home/GetFormatDate/";
    //10-05-2014
    //alert(_strDate);
    if (_strDate.length == 10 && (_strDate.indexOf('-') > 0 || _strDate.indexOf('/') > 0)) {
        //var pattern = /(\d{2})\-(\d{2})\-(\d{4})/;
        //var dt = new Date(_strDate.replace(pattern, '$3-$2-$1'));
        //return dt;
        _strDate = ConvertDatetime(_url, _strDate);
    }
        //10-05-2014 12:20
    else if (_strDate.length == 16 && (_strDate.indexOf('-') > 0 || _strDate.indexOf('/') > 0)) {
        //var pattern = /(\d{2})\-(\d{2})\-(\d{4}) (\d{2}):(\d{2})/;
        //var _strDateTime = _strDate.replace(pattern, '$3-$2-$1 $4:$5');
        //var dt = new Date(_strDateTime);
        //return dt;
        _strDate = ConvertDatetime(_url, _strDate);
    }
        //10-05-2014 12:20:45/(\d{4})-(\d{2})-(\d{2}) (\d{2}):(\d{2}):(\d{2})/g;
    else if (_strDate.length == 19 && (_strDate.indexOf('-') > 0 || _strDate.indexOf('/') > 0)) {
        //var pattern = /(\d{2})\-(\d{2})\-(\d{4}) (\d{2}):(\d{2}):(\d{2})/;
        //var dt = new Date(_strDate.replace(pattern, '$3-$2-$1 $4:$5:$6'));
        //return dt;
        _strDate = ConvertDatetime(_url, _strDate);
    }
    //alert(_strDate);
    return _strDate;
}

//[Hien.Nguyen] Convert định dạng dd/MM/yyy sang định dạng của Server hiện tại
function GetUserInfo(urlPost) {

    var result = "";
    $.ajax({
        type: 'POST',
        url: urlPost,
        dataType: 'json',
        async: false,
        success: function (returnValue) {
            result = returnValue;
        },
        error: function (data) {

        }
    });
    debugger;
    return result;
}


//[Tin.Nguyen] - 18/8/2014
function slpitQueryEntity(queryEntityInput) {

    var strHtml = "";
    var returnValue = "";
    var strSplit = "";
    strSplit = queryEntityInput.split('&');
    for (var i = 0; i < strSplit.length; i++) {
        if (strSplit[i].split('=')[0].indexOf('_') == -1) {
            strHtml += strSplit[i] + '&';
        }
        else {
            strHtml += strSplit[i].substring(strSplit[i].indexOf('_') + 1, strSplit[i].length) + '&';
        }
    }

    debugger;
    var _url = window.location.protocol + "//" + window.location.hostname + ":6100/Base/GetUserInfo/";
    strHtml += GetUserInfo(_url);
    returnValue = strHtml.substring(0, strHtml.length - 1);
    return returnValue;
}


//[Chuc.Nguyen] - 30/05/2014
//Lưu và tạo mới
function onSaveAndContinue(formValidate, url, route, gridId, popupId) {
    debugger;
    var repo = new Repository.GenericRepository(url, route);
    repo.Post(formValidate.serialize(), function (responseMessage) {
        var window = $("#" + popupId).data("kendoWindow");
        window.refresh();
        var grid = $("#" + gridId).data("kendoGrid");
        grid.dataSource.read();
        ShowNotificationProcessor(responseMessage);
    });
}

//[Chuc.Nguyen] - 30/05/2014
//Lưu
//function onSaveData(urlService) {
//        
//        if (urlService != '' && urlService != null) {
//            $.ajax({
//                url: urlService,
//                dataType: "json",
//                type: 'POST',
//                success: function (result) {
//                    window.ShowNotificationProcessor("Success");
//                },
//                error: function (result) {
//                    window.ShowNotificationProcessor(result.responseText);
//                }
//            });
//        }
//    }

function onSaveData(urlService, dataSave, objName) {

    if (dataSave != '' && dataSave != null) {
        showAjaxSaving();
        $.ajax({
            url: urlService,
            data: { data: JSON.stringify(dataSave), tableName: objName },
            dataType: "json",
            type: 'POST',
            success: function (result) {

                column = [];
                ShowNotificationProcessor(result);
                hideAjaxSaving();
            },
            error: function (result) {
                ShowNotificationProcessor(result.responseText);
                hideAjaxSaving();
            }
        });
    }
}

//Lưu và tạo mới có xử lý cây phòng ban
function onSaveAndContinueOrg(formValidate, url, route, gridId, popupId, listOrg) {
    var repo = new Repository.GenericRepository(url, route);
    repo.Post(formValidate.serialize(), function (responseMessage) {
        var window = $("#" + popupId).data("kendoWindow");
        window.refresh();
        var grid = $("#" + gridId).data("kendoGrid");
        grid.dataSource.read();

        ShowNotificationProcessor(responseMessage);
    });
}

//Lưu và đóng
function onSaveAndClose(formValidate, url, route, gridId, popupId) {
    var rs = false;
    var repo = new Repository.GenericRepository(url, route);
    repo.Post(formValidate.serialize(), function (responseMessage) {
        debugger;
        if (responseMessage.indexOf("Success") == 0) {

            var window = $("#" + popupId).data("kendoWindow");
            window.refresh().close();
            var grid = $("#" + gridId).data("kendoGrid");
            grid.dataSource.read();
            rs = true;
        }


        ShowNotificationProcessor(responseMessage);
    });
    return rs;
}

//Lưu
function onlySave(formValidate, url, route, gridId) {
    var rs = false;
    var repo = new Repository.GenericRepository(url, route);
    repo.Post(formValidate.serialize(), function (responseMessage) {
        debugger;
        if (responseMessage.indexOf("Success") == 0) {

            var grid = $("#" + gridId).data("kendoGrid");
            grid.dataSource.read();


            rs = true
        }
        ShowNotificationProcessor(responseMessage);
    });
    return rs;
}


function Login(formValidate, url, route) {
    var form = $(formValidate);
    var repo = new Repository.GenericRepository(url, route);
    repo.Post(form.serialize(), function (responseMessage) {
        alert(reponseMessage);
        if (responseMessage == "true") {
            alert("Success");
            //KendoRouteMapping('#Hrm_Att_Web/Att_Roster/Edit/' + id, 'Hrm_Att_Web', '@_hrm_Att_Web');
        }
        else {
            alert("Fail");
        }
    });
}



///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// DataType

/*    Data Type of javascript
 
Author : Tung.Ly
Modified Date : 2014Aug19
*/

function fnCheckTypeOf(variableName, dataType) {
    /// <summary>kiểm tra biến có thuộc 1 kiểu dữ liệu hay không </summary>
    /// <param name="variableName" type="String">variable Name</param>                
    /// <param name="dataType" type="String">Data Type</param>                
    /// <returns type="Boolean"></returns>    

    var _variableName = typeof (variableName);
    var _isEqual = false;
    if (_variableName == dataType.toLowerCase()) {
        _isEqual = true;
    }
    return _isEqual;
}

var DataType = new function () {
    this.String = "string";
    this.Number = "number";
    this.Boolean = "boolean";
    this.Null = null;
    this.Undefined = "undefined";
    this.Object = "object";
    this.Array = "array";
    this.Function = "function";

    this.isString = function (variable) {
        return fnCheckTypeOf(variable, DataType.String);
    };
    this.isNumber = function (variable) {
        return fnCheckTypeOf(variable, DataType.Number);
    };
    this.isBoolean = function (variable) {
        return fnCheckTypeOf(variable, DataType.Boolean);
    };
    this.isNull = function (variable) {
        return fnCheckTypeOf(variable, DataType.Null);
    };
    this.isUndefined = function (variable) {
        return fnCheckTypeOf(variable, DataType.Undefined);
    };
    this.isFunction = function (variable) {
        return fnCheckTypeOf(variable, DataType.Function);
    };
};
Object.freeze(DataType);

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function InsertSuccess() {
    var notificationWidget = $("#popupNotification").kendoNotification().data("kendoNotification");

    notificationWidget.show('Thêm Mới Thành Công');

}

function InsertFail() {
    var notificationWidget = $("#popupNotification").kendoNotification().data("kendoNotification");

    notificationWidget.show('Thêm Mới Thất Bại', 'error');

}

function UpdateSuccess() {
    var notificationWidget = $("#popupNotification").kendoNotification().data("kendoNotification");

    notificationWidget.show('Cập Nhập Thành Công');

}

function UpdateFail() {
    var notificationWidget = $("#popupNotification").kendoNotification().data("kendoNotification");

    notificationWidget.show('Cập Nhập Thất Bại', 'error');

}
function ExportFail() {
    var notificationWidget = $("#popupNotification").kendoNotification().data("kendoNotification");

    notificationWidget.show('Vui Lòng Chọn Template Để Xuất Báo Cáo', 'warning');

}
function DeleteFail() {
    var notificationWidget = $("#popupNotification").kendoNotification().data("kendoNotification");

    notificationWidget.show('Không Có Dữ Liệu Nào Được Chọn Để Xóa', 'warning');

}
function ExportSelectedFail() {
    var notificationWidget = $("#popupNotification").kendoNotification().data("kendoNotification");

    notificationWidget.show('Vui Lòng Chọn Dữ Liệu Để Xuất Excel ', 'error');

}
function ExportAllFail() {
    var notificationWidget = $("#popupNotification").kendoNotification().data("kendoNotification");

    notificationWidget.show('Không Có Dữ Liệu Để Xuất Excel ', 'error');

}
//[Hieu.van] - Hiển thị thông báo lỗi
function ErrorMessages(messages) {
    var notificationWidget = $("#popupNotification").kendoNotification().data("kendoNotification");
    notificationWidget.show(messages, 'error');
}

//[Hieu.van] - Hiển thị Cảnh Báo
function WarningMessages(messages) {
    var notificationWidget = $("#popupNotification").kendoNotification().data("kendoNotification");
    notificationWidget.show(messages, 'warning');
}

//[Chuc.Nguyen] - Hiện thị thông báo lỗi
function ShowNotification(messages) {
    var notificationWidget = $("#popupNotification").kendoNotification().data("kendoNotification");
    notificationWidget.show(messages);
}

//Lấy dữ liệu control DatePicker Và DateTimePicker
function getDatePicker(datePickerTimeId) {
    /// <summary>Lấy dữ liệu control DatePicker Và DateTimePicker</summary>        
    /// <param name="datePickerTimeId" type="String">Id của control kendoDatePicker hoặc kendoDateTimePicker</param>        
    /// <returns type="String">Trả về chuổi ngày tháng theo chuẩn GMT</returns>


    var datePicker = $("#" + datePickerTimeId).data("kendoDatePicker");
    if (datePicker == undefined) {
        var dateTimePickerValue = $("#" + datePickerTimeId).data("kendoDateTimePicker").value();
        if (dateTimePickerValue instanceof Date) {

            var dtDate = new Date(dateTimePickerValue.getFullYear(), dateTimePickerValue.getMonth(), dateTimePickerValue.getDate(), dateTimePickerValue.getHours(), dateTimePickerValue.getMinutes(), dateTimePickerValue.getSeconds());
            return dtDate.toGMTString();
        }
    }
    if (datePicker.value() instanceof Date) {
        var dtDatePicker = datePicker.value();

        var dtDate = new Date(dtDatePicker.getFullYear(), dtDatePicker.getMonth(), dtDatePicker.getDate(), dtDatePicker.getHours(), dtDatePicker.getMinutes(), dtDatePicker.getSeconds());
        return dtDate.toGMTString();
    }
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// VnrString

/*     String utilities of javascript
 
Author : Tung.Ly
Modified Date : 2014Feb07
*/
var VnrString = new function () {
    //accessor variable    

    this.format = fnFormat;
    this.isNullOrEmpty = fnIsNullOrEmpty;

    // function
    
    function fnFormat() {
        /// <summary>String.format('Name : {0} ;Age : {1}','Nguyen Van A ', 25) ==> Name :Nguyen Van A ; Age : 25</summary>

        var s = arguments[0];
        for (var i = 0; i < arguments.length - 1; i++) {
            var reg = new RegExp("\\{" + i + "\\}", "gm");
            s = s.replace(reg, arguments[i + 1]);
        }
        return s;
    }
    
    function fnIsNullOrEmpty(value) {
        /// <summary>Check value is Null or Empty.</summary>
        /// <param name="value" type="String"></param>                
        /// <returns type="Boolean"></returns>

        var isNullorEmpty = false;
        switch (value) {
            case "": isNullorEmpty = true; break;
            case null: isNullorEmpty = true; break;
            case undefined: isNullorEmpty = true; break;
            default: isNullorEmpty = false;
        }
        return isNullorEmpty;
    }
   
};     // close String
Object.freeze(String);

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//
function getJson(_host, _controller, _action, _jsonData, onSuccess) {
    /// <signature>
    /// <summary>get json data in order to get data or validation</summary>
    /// <param name="_host" type="String">tên host (vd:http://localhost:8000/)</param>
    /// <param name="_controller" type="String">tên controller</param>
    /// <param name="_action" type="String">tên action (phương thức POST)</param>
    /// <param name="onSuccess" type="Function">
    /// function(response){
    ///     xử lý code khi response thành công
    ///  }
    ///</param>        
    /// <returns type="Void"></returns>
    /// </signature>

    /// <signature>
    /// <summary>get json data in order to get data or validation</summary>
    /// <param name="_host" type="String">tên host (vd:http://localhost:8000/)</param>
    /// <param name="_controller" type="String">tên controller</param>
    /// <param name="_action" type="String">tên action (phương thức POST)</param>
    /// <param name="_jsonData" type="json">kiểu json {name:'Alan',age:25} ** Note: Nếu không truyền tham số thì ghi [] </param>
    /// <param name="onSuccess" type="Function">
    /// function(response){
    ///     xử lý code khi response thành công
    ///  }
    ///</param>        
    /// <returns type="Void"></returns>
    /// </signature>

    var _url = _host + _controller + "/" + _action;


    //set gia tri cho json
    var jsonDataValue = [];
    if (!DataType.isFunction(arguments[3])) {
        //neu _jsonData không phải là function
        jsonDataValue = arguments[3];
    }

    $.ajax({
        url: _url,
        type: 'POST',
        async: false,
        data: jsonDataValue,
        success: function (result, status) {
            if (onSuccess != null) {
                onSuccess(result);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Status: " + textStatus + "Error: " + errorThrown);
        }
    });
}

function checkDuplicate(_host, _controller, _action, _jsonData, _fieldId) {
    /// <signature>
    /// <summary>Kiểm tra trùng dữ liệu</summary>        
    /// <param name="_host" type="String">tên host</param>        
    /// <param name="_controller" type="String">tên controller</param>        
    /// <param name="_action" type="String">tên action (phương thức POST) </param>        
    /// <param name="_jsonData" type="json">kiểu json ({ loginName: 'admin',id : 1 };) ** Note: Nếu không truyền tham số thì ghi []</param>        
    /// <param name="_fieldId" type="string">tên cột kiểm tra trùng (dùng để xuất thông báo cột nào bị trùng dữ liệu)</param>        
    /// <returns type="bool">Trả về field có trùng dữ liệu không ?</returns>
    /// </signature>

    var _url = _host + _controller + "/" + _action;
    var success = false;
    $.ajax({
        url: _url,
        type: 'POST',
        async: false,
        data: _jsonData,
        cache: false,
        success: function (result, status) {
            debugger;
            if (result == true) {
                var fieldId = "#" + _fieldId;
                if (_fieldId.indexOf("lbl") == -1) {
                    fieldId = "#lbl" + _fieldId;
                }
                var fieldName = $(fieldId).text();
                ShowNotificationProcessor("Error, [" + fieldName + "] Đã Tồn Tại !");
                success = true;

            } else {
                success = false;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Status: " + textStatus + "Error: " + errorThrown);
            success = false;
        }
    });
    return success;
}

function checkDuplicate_4Condition(_host, _controller, _action, _jsonData, _fieldId) {
    /// <signature>
    /// <summary>Kiểm tra trùng dữ liệu</summary>        
    /// <param name="_host" type="String">tên host</param>        
    /// <param name="_controller" type="String">tên controller</param>        
    /// <param name="_action" type="String">tên action (phương thức POST) </param>        
    /// <param name="_jsonData" type="json">kiểu json ({ loginName: 'admin',id : 1 };) ** Note: Nếu không truyền tham số thì ghi []</param>        
    /// <param name="_fieldId" type="string">tên cột kiểm tra trùng (dùng để xuất thông báo cột nào bị trùng dữ liệu)</param>        
    /// <returns type="bool">Trả về field có trùng dữ liệu không ?</returns>
    /// </signature>
    debugger;
    var _url = _host + _controller + "/" + _action;
    var success = false;
    $.ajax({
        url: _url,
        type: 'POST',
        async: false,
        data: _jsonData,
        cache: false,
        success: function (result, status) {
            debugger;
            var listField = _fieldId.split(",");
            if (result == true) {
                var fieldId1 = "#" + listField[0];
                var fieldId2 = "#" + listField[1];
                var fieldId3 = "#" + listField[2];
                var fieldId4 = "#" + listField[3];
                if (listField[0].indexOf("lbl") == -1) {
                    fieldId1 = "#lbl" + listField[0];
                }
                if (listField[1].indexOf("lbl") == -1) {
                    fieldId2 = "#lbl" + listField[1];
                }
                if (listField[2].indexOf("lbl") == -1) {
                    fieldId3 = "#lbl" + listField[2];
                }
                if (listField[3].indexOf("lbl") == -1) {
                    fieldId4 = "#lbl" + listField[3];
                }
                var fieldName1 = $(fieldId1).text();
                var fieldName2 = $(fieldId2).text();
                var fieldName3 = $(fieldId3).text();
                var fieldName4 = $(fieldId4).text();
                ShowNotificationProcessor("Error, [" + fieldName1 + ", " + fieldName2 + ", " + fieldName3 + ", " + fieldName4 + "] Đã Tồn Tại !");
                success = true;

            } else {
                success = false;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Status: " + textStatus + "Error: " + errorThrown);
            success = false;
        }
    });
    return success;
}

function checkPermission(privilegeType, screen, isNotify) {
    /// <signature>
    /// <summary>Kiểm tra quyền user hiện tại có quyền trên trang không?</summary>     
    /// <param name="privilegeType" type="Privilege">Enum Privilege quyền (view,modify,...) **Note : có thể dùng enum Privilege.View ,Privilege.Import ... </param>        
    /// <param name="screen" type="String">tên màn hình cần phân quyền </param>        
    /// <returns type="bool">Trả về màn hình đấy có phân quyền hay chưa ?</returns>
    /// </signature>

    /// <signature>
    /// <summary>Kiểm tra quyền user hiện tại có quyền trên trang không?</summary>     
    /// <param name="privilegeType" type="Privilege">Enum Privilege quyền (view,modify,...) **Note : có thể dùng enum Privilege.View ,Privilege.Import ... </param>        
    /// <param name="screen" type="String">tên màn hình cần phân quyền </param>
    /// <param name="isNotify" type="Boolean">Cho phép xuất thông báo hay không ?</param>
    /// <returns type="bool">Trả về màn hình đấy có phân quyền hay chưa ?</returns>
    /// </signature>

    var isPrintNotify = true;

    if (isNotify != undefined) {
        isPrintNotify = isNotify;
    }

    var _host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port : '') + "/";
    var _url = _host + "Base/CheckPermission";
    var success = false;
    $.ajax({
        url: _url,
        type: 'POST',
        async: false,
        data: { privilegeType: privilegeType, permission: screen },
        success: function (result, status) {
            if (result == true) {
                success = true;
            } else {
                var privilegeTranslate = "Truy cập";
                if (privilegeType == PrivilegeType.Create) {
                    privilegeTranslate = "Tạo Mới";
                }
                else if (privilegeType == PrivilegeType.Modify) {
                    privilegeTranslate = "Chỉnh Sửa";
                }
                else if (privilegeType == PrivilegeType.Delete) {
                    privilegeTranslate = "Xoá";
                }
                else if (privilegeType == PrivilegeType.Import) {
                    privilegeTranslate = "Nhập Dữ Liệu";
                }
                else if (privilegeType == PrivilegeType.Export) {
                    privilegeTranslate = "Xuất Dữ Liệu";
                }
                var msg = VnrString.format("Bạn không có quyền {0} trang này .", privilegeTranslate);

                if (isPrintNotify) {
                    ShowNotificationProcessor(msg);
                }
                success = false;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Status: " + textStatus + "Error: " + errorThrown);
            success = false;
        }
    });
    return success;
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// VnrValidation

/*                      This is Javascript Validation Class using for validate

Author : Tung.Ly
Modified Date : 2014AUG16
*/

var VnrValidation = new function () {
    //variable
    var regIdentification = "^[0-9]{9}$";
    var regEmail = "^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@" + "[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";

    //properties
    this.validate_Email = fnValidateEmail;


    //function
    function fnValidateEmail(value) {
        /// <summary>ràng buộc Email</summary>        
        /// <param name="value" type="String"></param>        
        /// <returns type="Boolean"></returns>
        return validate(regEmail, value);
    }


    function validate(regPattern, value) {
        /// <summary>(Private) Ràng buộc dữ liệu theo 1 mẫu định sẵn</summary>        
        /// <param name="regPattern" type="String">Pattern </param>        
        /// <param name="value" type="String"></param>        
        /// <returns type="Boolean"></returns>
        var regex = new RegExp(regPattern);
        return regex.test(value);
    }

}; // close Validation class

//[Tung.Ly] :xuat thong bao confirm
function VnrConfirm(messages, selectedValues, onSuccess) {
    /// <summary>Xuat thong bao confirm</summary>
    /// <param name="messages" type="String">Thông báo</param>
    /// <param name="onSuccess" type="Function">
    /// function(response){
    ///     xử lý code khi response thành công
    ///  }
    ///</param>        
    /// <returns type="Void"></returns>

    $.when(kendo.ui.ExtOkCancelDialog.show({
        title: "Thông Báo",
        message: messages,
        icon: "k-ext-warning"
    })
        ).done(function (response) {
            if (response.button == "OK") {
                if (onSuccess != null) {
                    onSuccess(selectedValues);
                }
            }
        });
}
