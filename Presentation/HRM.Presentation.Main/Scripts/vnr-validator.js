//Lưu trữ các field validate không hợp lệ
var validateField = new Array();

//Dịch list field validate theo ngôn ngữ hiện tại dựa vào lable for
function GetFieldAfterTranslate(strField) {
     
    var arrStr = [];
    var str = '', strTemp = '';
    var indexArrStr = 0;
    for (var i = 0; i < strField.length - 1; i++) {
        var hasLabel = $('label[for="' + strField[i] + '"]').length > 0 ? true:false;
        if (strField[i].indexOf('_') != -1) {
            strField[i] = strField[i].split('_')[1];
        }
        if (/Firefox[\/\s](\d+\.\d+)/.test(navigator.userAgent)) {
            if (hasLabel) {
                strTemp = $('label[for="' + strField[i] + '"]').get()[0].innerHTML;
            }
        } else {
            var fieldExists = $("#lbl" + strField[i]).get();
            if (fieldExists.length > 0) {
                strTemp = $("#lbl" + strField[i]).get()[0].innerText;
            }
        }
        if (strTemp == '') {
            var lblValue = arrStr.indexOf($("#lbl" + strField[i]).get()[0].innerText);
            if (lblValue == -1) {
                arrStr[indexArrStr++] = lblValue;
            }

          //  str += $("#lbl" + strField[i]).get()[0].innerText;
        } else {
            if (arrStr.indexOf(strTemp) == -1) {
                arrStr[indexArrStr++] = strTemp;
            }
          //  str += strTemp;
        }
        //if (i != strField.length - 2) {
        //    str += ', ';
        //}
    }
    //return str;
    return arrStr.join(',');
}

//[Chuc.Nguyen] - Ràn buộc thông tin bắt buộc nhập
function ValidatorRequired(listField, resultBool) {
    
    var listRequired ='';
    for (var i = 0; i < listField.length; i++) {
        var checkId = document.getElementById(listField[i]);
        if (checkId != undefined) {
            if ($("#" + listField[i]).val() == '' || $("#" + listField[i]).val() == null)
            {
                listRequired += listField[i] + ',';
                resultBool = false;
            }
        }
    }
    if (listRequired!='') {
        validateField['Required'] = listRequired;
    }
    return resultBool;
}

//[Chuc.Nguyen] - Kiểm tra ngày hợp lệ không
function ValidatorDate(listField) {
    for (var i = 0; i < listField.length; i++) {
        $("#" + listField[i]).attr('data-type', 'date');
    }
}

////[Chuc.Nguyen] - Kiểm tra chiều dài
//function ValidatorLength(listField) {
//    for (var i = 0; i < listField.length; i++) {
//        var item = listField[i].split('_');
//        $("#" + item[0]).attr('maxlength', item[1]);
//    }
//}

//[Chuc.Nguyen] - Kiểm tra email hợp lệ không
function ValidatorEmail(listField) {
    for (var i = 0; i < listField.length; i++) {
        $("#" + listField[i]).attr('data-email-msg', '{0} không hợp lệ');
        $("#" + listField[i]).attr('type', 'email');
    }
}
//[Chuc.Nguyen] - Validate số điện thoại
function ValidatorPhone(listField) {
    for (var i = 0; i < listField.length; i++) {
        $("#" + listField[i]).attr('validationMessage', '{0} phải là số có 6 - 11 chữ số');
        $("#" + listField[i]).attr('pattern', '\\d{6,11}');
    }
}
//[Chuc.Nguyen] - Validate chỉ cho nhập số
function ValidatorNumber(listField) {
    
    for (var i = 0; i < listField.length; i++) {
        $("#" + listField[i]).attr('validationMessage', '{0} phải là số');
        $("#" + listField[i]).attr('pattern', '\\d{0,}');
    }
}
//[Chuc.Nguyen] - Validate số SMND
function ValidatorIDNo(listField) {
    for (var i = 0; i < listField.length; i++) {
        $("#" + listField[i]).attr('validationMessage', '{0} phải là số có 9 chữ số');
        $("#" + listField[i]).attr('pattern', '\\d{9}');
    }
}

////[Chuc.Nguyen] - Kiểm tra ngày bắt đầu phải nhỏ hơn ngày kết thúc
//function ValidatorDateStartDateEnd(DateStart, DateEnd) {
//    $("#" + DateEnd).attr('data-greaterdate-msg', DateEnd + ' phải lớn hơn ' + DateStart);
//    $("#" + DateEnd).attr('data-greaterdate-field', DateStart);
//    $(function () {
//        var container = $(document.body);
//        kendo.init(container);
//        container.kendoValidator({
//            rules: {
//                greaterdate: function (input) {
//                    if (input.is("[data-greaterdate-msg]") && input.val() != "") {
//                        var date = input.data("kendoDatePicker").value(),
//                            otherDate = $("[name='" + input.data("greaterdateField") + "']").data("kendoDatePicker").value();
//                        return otherDate == null || otherDate.getTime() <= date.getTime();
//                    }
//                    return true;
//                }
//            }
//        });
//    });
//}

//So sánh ngày
//1.CompareDate="DateFrom" : So sánh ngày được chọn với ngày hiện tại, có thể có nhiều ngày được so sánh, mỗi ngày cách nhau bằng dấu phẩy(,)
//2.CompareDate="1-2_DateFrom_DateTo" : So sánh 2 ngày được chọn với nhau theo quy tắc đặt phía trước,  có thể có nhiều cặp ngày được so sánh, mỗi cặp ngày cách nhau bằng dấu phẩy(,)
//3.CompareDate="1-2;2-3_Date1_Date2_Date3": So sánh nhiều cặp ngày với nhau theo quy tắc phía trước, mỗi quy tắc cách nhau bằng dấu chấm phẩy (;), có thể có nhiều item mỗi item cách nhau dấu phẩy(,)
//4.CompareDate="DateFrom,1-2_DateFrom_DateTo,1-2;2-3_Date1_Date2_Date3" : So sánh tổng hợp các loại trên
function CompareDate(listField, resultBool) {
    var listCompareDate = '';
    for (var i = 0; i < listField.length; i++) {
        var listItem = listField[i].split('_');
        if (listItem.length > 0) {
            if (listItem.length == 1) {
                $("#" + listField[i]).val()
                if (date1.getTime() > date2.getTime()) {
                    alert("The first date is after the second date!");
                }
            }
        }
    }

    if (listRequired != '') {
        validateField['CompareDate'] = listCompareDate;
    }
    return resultBool;
}

//[Chuc.Nguyen]-Xử lý validate
function CheckValidate() {
    
    var listFieldValidate = GetValidateInfoInXml();
    validateField = new Array();
    var resultBool = true;
    if (listFieldValidate != null && listFieldValidate != '') {
        for (var item in listFieldValidate) {
            if (listFieldValidate.hasOwnProperty(item)) {
                var listField = listFieldValidate[item].split(',');
                switch (item) {
                    case "Required":
                        
                        resultBool = ValidatorRequired(listField, resultBool);
                        break;
                }
            }
        }
    }
    if (resultBool==false) {
        window.ShowNotificationValidate(validateField);
    }
    return resultBool;
}

function SetFieldRequiredByScreenName(screenName) {
  
    //Lấy đường dẫn hiện tại trên trình duyệt
    var currentLocation = window.location.href;
    //Tách link thành 2 phần: trước và sau dấu #
    var strArray = currentLocation.split('#');
    $.ajax({
        url: strArray[0] + "Home/GetFieldRequired",
        data: { tableName: screenName },
        type: 'POST',
        async: false,
        cache: false,
        success: function (dataResult) {
             
            if (dataResult != null) {
                for (var i = 0; i < dataResult.length; i++) {
                    debugger;
                    if (!dataResult[i].Nullable) {
                        if ($("#required" + dataResult[i].Name + screenName) == undefined || $("#required" + dataResult[i].Name + screenName).text() == "") {
                            //$("#lbl" + dataResult[i].Name).addClass("setNotifyFieldRequired");//.after('<span class="required' + dataResult[i].Name + screenName + i + '" style="color:red; margin-left:3px">(*)</span>');
                            $("#lbl" + dataResult[i].Name).after('<span id= "required' + dataResult[i].Name + screenName + '" class="required' + dataResult[i].Name + screenName + i + '" style="color:red; margin-left:2px">*</span>');
                        }
                    }
                }
            }
        },
        error: function (dataResult) {
            ErrorMessages(dataResult.responseText);
        }
    });
}
function SetFieldRequired() {
     
    //Lấy đường dẫn hiện tại trên trình duyệt
    var currentLocation = window.location.href;
    //Tách link thành 2 phần: trước và sau dấu #
    var strArray = currentLocation.split('#');
    var strArray2 = strArray[1].split('/');

    //Ẩn hết tất cả các validate default của kendo
    $(document.body).find("span.k-tooltip-validation").hide();

    var controllerName = strArray2[1];
    SetFieldRequiredByScreenName(controllerName)
}


//[Chuc.Nguyen]-Gán ký tự bắt buộc cho các field
function SetCharacteFieldRequired() {
    
    //Lấy đường dẫn hiện tại trên trình duyệt
    var currentLocation = window.location.href;
    //Tách link thành 2 phần: trước và sau dấu #
    var strArray = currentLocation.split('#');
    var strArray2 = strArray[1].split('/');
    //Lấy Controller Name và Action Name để tạo ra tên màn hình
    var screenName = strArray2[1] + strArray2[2];
    //Ẩn hết tất cả các validate default của kendo
    $(document.body).find("span.k-tooltip-validation").hide();
    //Lấy dữ liệu từ file XML
    var data = GetValidateInfoInXml();
    if (data!=null  && data != '') {
        for (var item in data) {
            if (data.hasOwnProperty(item)) {
                var listField = data[item].split(',');
                if (item == "Required") {
                    for (var i = 0; i < listField.length; i++) {
                        debugger;
                        //if ($('span').hasClass('requiredCode' + screenName + i)) {
                        //    var theString = "<span class='required'" +listField[i] + screenName + i + "'\" style=\"color:red; margin-left:5px\">*</span>";
                        //    var theResult = $("span", theString).remove().end().html();
                        //}
                        if ($("#required" + listField[i] + screenName) == undefined || $("#required" + listField[i] + screenName).text() == "") {
                           // $("#lbl" + listField[i]).addClass("setNotifyFieldRequired");// = "red";//.after('<span class="required' + +listField[i] + screenName + i + '" style="color:red; margin-left:3px">(*)</span>');
                            $("#lbl" + listField[i]).after('<span id= "required' + listField[i] + screenName + '"  class="required' + listField[i] + screenName + i + '" style="color:red; margin-left:0px">*</span>');
                        }
                    }
                }
            }
        }
    }
    return data;
}

//[Chuc.Nguyen]-Lấy thông tin validate từ file XML
function GetValidateInfoInXml() {
    

    //Lấy đường dẫn hiện tại trên trình duyệt
    var currentLocation = window.location.href;
    //Tách link thành 2 phần: trước và sau dấu #
    var strArray = currentLocation.split('#');
    var strArray2 = strArray[1].split('/');
    //Lấy Controller Name và Action Name để tạo ra tên màn hình
    var screenName = strArray2[1] + '/' + strArray2[2];
    var dataReturn = null;
    $.ajax({
        url: strArray[0] + "Home/VrnValidator",
        data: { screenName: screenName },
        type: 'POST',
        async: false,
        cache: false,
        success: function (data) {
            dataReturn = data;
        },
        error: function (data) {
            ErrorMessages(data.responseText);
        }
    });
    return dataReturn;
}
////[Chuc.Nguyen] - Kiểm tra validate trong form
//function CheckValidate() {
//    RemoveValidatorId();
//    //Lấy đường dẫn hiện tại trên trình duyệt
//    var currentLocation = window.location.href;
//    //Tách link thành 2 phần: trước và sau dấu #
//    var strArray = currentLocation.split('#');
//    var strArray2 = strArray[1].split('/');
//    //Lấy Controller Name và Action Name để tạo ra tên màn hình
//    var screenName = strArray2[1] + '/' + strArray2[2];
//    var validator = null;
//    $.ajax({
//        url: strArray[0] + "Home/VrnValidator",
//        data: { screenName: screenName },
//        type: 'POST',
//        async: false,
//        success: function (data) {
//            
//            if (data != '') {
//                for (var item in data) {
//                    if (data.hasOwnProperty(item)) {
//                        var listField = data[item].split(',');
//                        switch (item) {
//                            case "Email":
//                                ValidatorEmail(listField);
//                                break;
//                            case "Date":
//                                ValidatorDate(listField);
//                                break;
//                            case "Length":
//                                ValidatorLength(listField);
//                                break;
//                            case "Phone":
//                                ValidatorPhone(listField);
//                                break;
//                            case "IDNo":
//                                ValidatorIDNo(listField);
//                                break;
//                            case "Required":
//                                ValidatorRequired(listField);
//                                break;
//                            case "Number":
//                                ValidatorNumber(listField);
//                                break;
//                            case "CompareDate":
//                                //ValidatorDateStartDateEnd(listField[0], listField[1]);
//                                
//                                CompareDate(listField);
//                                break;
//                        }
//                    }
//                }
//            }
//        },
//        error: function (data) {
//            ErrorMessages(data.responseText);
//        }
//    });
//    validator = $(document.body).data("kendoValidator");
//    return validator;
//}


//[Chuc.Nguyen] - Kiểm tra ngày ngày lớn hơn ngày hiện tại
//function ValidatorCurrentDate(dateInput) {
//    RemoveValidatorId();
//    $("#" + dateInput).attr('data-custom-msg', dateInput + ' phải lớn hơn ngày hiện tại ');

//    $(function () {
//        var container = $(document.body);
//        kendo.init(container);
//        container.kendoValidator({
//            rules: {
//                custom: function (input) {
//                    
//                    if (input.is("[data-custom-msg]") && input.val() != "") {
//                        var date = $("#" + dateInput).data("kendoDatePicker").value()
//                        var todayDate = new Date();
//                        return todayDate.getTime() < date.getTime();
//                    }
//                    return true;
//                }
//            }
//        });
//    });
//}

//function ClearValidator() {
//    ////$('#form-create input[type=text]').each(function () {
//    ////    $(this).removeAttr('data-val-date');
//    ////});
//    //$(':input[data-kendoDatePicker=true]').each(function () {
//    //    $(this).removeAttr('data-val-date');
//    //});
//    ////alert(1);

//    //$('#MonthOfEffect').removeAttr('data-val-date');
//    //$('#DateOfBirth').removeAttr('data-val-date');
//    //$('#MonthOfExpiry').removeAttr('data-val-date');
//    $(document.body).kendoValidator({
//        rules: {
//            date: function (input) {
//                var d = kendo.parseDate(input.val(), "yyyy-MM-dd");
//                return d instanceof Date;
//            }
//        }
//    });
//}



//////[Chuc.Nguyen] - Kiểm tra validate trong form
//function CheckValidator(initKendoValidator) {
//    validator = null;
//    if (initKendoValidator) {
//        validator = $(document.body).data("kendoValidator");
//    }
//    else {
//        validator = $(document.body).kendoValidator().data("kendoValidator");
//    }
//    return validator;
//}
//function CheckValidate123() {
//    //Xóa validate field Id vì mặc định Id không được Null
//    RemoveValidatorId();
//    //Lấy đường dẫn hiện tại trên trình duyệt
//    var currentLocation = window.location.href;
//    //Tách link thành 2 phần: trước và sau dấu #
//    var strArray = currentLocation.split('#');
//    var strArray2 = strArray[1].split('/');
//    //Lấy Controller Name và Action Name để tạo ra tên màn hình
//    var screenName = strArray2[1] + '/' + strArray2[2];
//    var resultReturn = false, validator = null;
//    $.ajax({
//        url: strArray[0] + "Home/VrnValidator",
//        data: { screenName: screenName },
//        type: 'POST',
//        async: false,
//        success: function (data) {
//            
//            if (data != '') {
//                for (var item in data) {
//                    if (data.hasOwnProperty(item)) {
//                        var listField = data[item].split(',');
//                        switch (item) {
//                            case "Email":
//                                ValidatorEmail(listField);
//                                resultReturn = false;
//                                break;
//                            case "Date":
//                                ValidatorDate(listField);
//                                resultReturn = false;
//                                break;
//                            case "Length":
//                                ValidatorLength(listField);
//                                resultReturn = false;
//                                break;
//                            case "Phone":
//                                ValidatorPhone(listField);
//                                resultReturn = false;
//                                break;
//                            case "IDNo":
//                                ValidatorIDNo(listField);
//                                resultReturn = false;
//                                break;
//                            case "Required":
//                                ValidatorRequired(listField);
//                                resultReturn = false;
//                                break;
//                            case "Number":
//                                ValidatorNumber(listField);
//                                resultReturn = false;
//                                break;
//                            case "CompareDate":
//                                ValidatorDateStartDateEnd(listField[0], listField[1]);
//                                resultReturn = true;
//                                break;
//                        }
//                    }
//                }
//            }
//        },
//        error: function (data) {
//            ErrorMessages(data.responseText);
//        }
//    });
//    if (resultReturn) {
//        validator = $(document.body).data("kendoValidator")
//    }
//    else {
//        validator = $(document.body).kendoValidator().data("kendoValidator")
//    }
//    return validator;
//}