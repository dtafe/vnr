
function SetFieldRequiredByScreenName(screenName) {
    debugger;
    $.ajax({
        url: location.origin + "/Portal/GetFieldRequired",
        data: { tableName: screenName },
        type: 'POST',
        async: false,
        cache: false,
        success: function (dataResult) {
            debugger;
            if (dataResult != null) {
                for (var i = 0; i < dataResult.length; i++) {
                    if (!dataResult[i].Nullable) {
                        if (document.getElementsByClassName('required' + dataResult[i].Name + screenName + i).length == 0) {
                            $("#" + dataResult[i].Name).after('<span class="required' + dataResult[i].Name + screenName + i + '" style="color:red; margin-left:5px">*</span>');
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
    debugger;
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
