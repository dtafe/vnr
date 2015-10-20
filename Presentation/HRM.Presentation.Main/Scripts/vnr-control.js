//Start Control VnrDropDownGrid
//buildInfo[idControl] = "example"
//buildInfo[defaultValueID] = [1,2,3]
//buildInfo[valueField] = "OrderID"
//buildInfo[textField] = "ShipName"
//buildInfo[urlGetData] = "http://demos.kendoui.com/service/Northwind.svc/Orders"
//buildInfo[placeHolderNoItem] = "Không có dữ liệu được chọn"
//buildInfo[placeHolderSelectedItem] = "dòng dữ liệu được chọn"

function VnrDropDownGrid(buildInfo) {
     
    //Các thông tin cấu hình
    //Default select theo ID
    var selectedIdentifiers = ko.observableArray([]);
    //Số item hiển thị tên khi được chọn: Default: 0
    var numberOfSelectedItemsToShow = buildInfo["NumberSelectedShowName"];
    //Chiều dài tối đa của dữ liệu khi được hiển thị Default: 15
    var maxLabelLength = buildInfo["MaxLengthShowName"];
    //Field lấy value
    var idFieldName = buildInfo["ValueField"];
    //Text column hiển thị theo template
    var labelFieldName = '';
    //Url lấy dữ liệu
    var dataSourceUrl = buildInfo["UrlGetData"];
    //Biến lưu dữ liệu khi sau khi ajax qua server
    var dataSourceNew = null;
    //Id của control
    var elementId = '#' + buildInfo["ControlID"];
    //Text hiển thị trước input tìm kiếm: Default: Tìm Kiếm
    var textFilter = buildInfo["TextFilter"];
    //Cấu hình column hiển thị
    var fieldCol = buildInfo["ListFields"];
    var titleCol = buildInfo["DisplayFields"];
    var columnGrid = [];
    columnGrid.push({ field: "Selected", title: "#", width: 30 })
    for (var i = 0; i < titleCol.length; i++) {
        columnGrid.push({ title: titleCol[i], field: fieldCol[i] });
        labelFieldName += '<td>#: ' + fieldCol[i] + ' #</td>';
    }
    //Dòng text khi không có dữ liệu nào được chọn
    var textNoData = buildInfo["TextNoData"];
    //Dòng text khi có hơn số lượng dữ liệu được hiển thị tên
    var textSelectData = buildInfo["TextSelectData"];
    //Column tìm kiếm dữ liệu
    var colfilter = buildInfo["ColumnFilter"];

    var element = $(elementId);

    var button = $('<div class="btn-group"><a class="btn dropdown-toggle" href="#"><span>Toggle</span> <span class="caret"></span></a></div>').appendTo(element).find('a');

    var selectedItems = {};
    var selectedIdentifiersValue = selectedIdentifiers();
    for (var i = 0, l = selectedIdentifiersValue.length; i < l; i++) {
        selectedItems[selectedIdentifiersValue[i]] = undefined;
    }
    updateSelection();
    $.ajax({
        url: dataSourceUrl,
        type: 'POST',
        async: false,
        success: function (data) {
            dataSourceNew = data.Data;
            var gridEl = $('<div />').kendoGrid({
                dataSource: {
                    //type: "POST",
                    //serverPaging: true,
                    //serverSorting: true,
                    //serverFiltering: true,
                    pageSize: 10,
                    data: dataSourceNew,
                    //transport: {
                    //    read: {
                    //        type: "POST",
                    //        url: dataSourceUrl,
                    //        dataType: "json"
                    //        //success: function (data) {
                    //        //     
                    //        //    alert(data);
                    //        //},
                    //        //error: function (xhr, error) {
                    //        //    alert(1);
                    //        //}
                    //        //data: dataParameter
                    //    }
                    //},
                    requestEnd: function (e) {
                         
                        var response = e.response;
                        var type = e.type;
                        if (type == 'read' && response && response.d && response.d.results) {
                            var results = response.d.results;
                            for (var i = 0, l = results.length; i < l; i++) {
                                results[i].Selected = false;
                            }
                        }
                    }
                },
                toolbar: kendo.template('<label>' + textFilter + ': <input type="search" class="k-textbox" style="width: 200px;max-height: 25px;"/></label>'),
                rowTemplate: kendo.template('<tr data-uid="#= ' + idFieldName + ' #" #if (Selected) {# class="k-state-selected" #}#><td><input type="checkbox" #if (Selected) {# checked="checked" #}# /></td>' + labelFieldName + '</tr>'),
                /*scrollable: {
                    virtual: true
                },*/
                scrollable: false,
                pageable: {
                    buttonCount: 7
                },
                selectable: false,
                columns: columnGrid,
                dataBinding: function (e) {
                    if (e.action == 'rebind' && e.items) {
                        for (var i = 0, l = e.items.length; i < l; i++) {
                            e.items[i].Selected = e.items[i][idFieldName] in selectedItems;
                        }
                    }
                }
            });

            grid = gridEl.data('kendoGrid');
             
            var dropDown = gridEl.find(".k-toolbar label input[type=search]").bind('propertychange keyup input paste', function () {
                var value = $(this).val();
                if (value) {
                    grid.dataSource.filter({ field: colfilter, operator: "contains", value: value });
                } else {
                    grid.dataSource.filter({});
                }
            });

            gridEl.appendTo($("<div />").appendTo(document.body).kendoPopup({
                toggleTarget: button,
                anchor: button,
                open: function () {
                    this.options.toggleTarget.parent().addClass('open');
                    if (grid.virtualScrollable) {
                        setTimeout(function () { grid.virtualScrollable.refresh(); }, 0);
                    }
                },
                close: function () {
                    this.options.toggleTarget.parent().removeClass('open');
                },
                animation: {}
            }));

            grid.tbody.on('click', 'tr td input[type=checkbox]', function (e) {
                var tr = $(this).closest('tr'), id = tr.data('uid');
                tr.toggleClass('k-state-selected');
                if (id in selectedItems) {
                    delete selectedItems[id];
                    selectedIdentifiers.remove(id);
                } else {
                    selectedItems[id] = tr.children('td:eq(1)').text();
                    selectedIdentifiers.push(id);
                }
                updateSelection();

            });
        },
        error: function (data) {
            alert('error');
        }
    });
    

    function updateSelection() {
         

        var count = 0, displayingItemIdentifiers = [], identifiersWithoutDisplayName = [];
        for (var k in selectedItems) {
             
            if (count <= numberOfSelectedItemsToShow) {
                //displayingItemIdentifiers.push(parseInt(k));
                displayingItemIdentifiers.push(k);
                if (!selectedItems[k]) {
                    //identifiersWithoutDisplayName.push(parseInt(k));
                    identifiersWithoutDisplayName.push(k);
                }
            }
            count++;
        }

        if (!count) {
            updateButtonText(textNoData);
            return;
        }

        if (count > numberOfSelectedItemsToShow) {
            updateButtonText(count +" "+ textSelectData);
            return;
        }

        if (identifiersWithoutDisplayName.length) {
            requestDisplayNamesAndUpdateButtonText(displayingItemIdentifiers, identifiersWithoutDisplayName);
        } else {
            generateAndUpdateButtonText(displayingItemIdentifiers);
        }
    }

    function requestDisplayNamesAndUpdateButtonText(displayingItemIdentifiers, identifiersWithoutDisplayName) {
         

        var filters = [];
        for (var i = 0, l = identifiersWithoutDisplayName.length; i < l; i++) {
            filters.push({ field: idFieldName, operator: 'eq', value: identifiersWithoutDisplayName[i] });
        }
         
        var dataSource = new kendo.data.DataSource({
            //type: "POST",
            serverFiltering: true,
            //transport: {
            //    read: {
            //        type: "POST",
            //        url: dataSourceUrl,
            //        dataType: "json"
            //        //success: function (data) {
            //        //     
            //        //    alert(data);
            //        //},
            //        //error: function (xhr, error) {
            //        //    alert(1);
            //        //}
            //        //data: dataParameter
            //    }
            //},
            data:dataSourceNew,
            filter: {
                logic: 'or',
                filters: filters
            }
        });
        dataSource.fetch(function () {
            var view = dataSource.view();
            for (var i = 0, l = view.length; i < l; i++) {
                selectedItems[view[i][idFieldName]] = view[i][labelFieldName];
            }
            generateAndUpdateButtonText(displayingItemIdentifiers);
        });
    }

    function generateAndUpdateButtonText(identifiers) {
         
        var text = '', itemText;
        for (var i = 0, l = identifiers.length; i < l; i++) {
             
            if (text.length) {
                text += ', ';
            }
            itemText = selectedItems[identifiers[i]];
            if (itemText.length > maxLabelLength) {
                itemText = itemText.slice(0, maxLabelLength - 1).trim() + '<i>&#8230;</i>';
            }
            text += itemText;
        }
        updateButtonText(text);
    }

    function updateButtonText(text) {
        button.find('span:eq(0)').html(text);
    }
}

//End Control VnrDropDownGrid


//Start Control Upload

function LimitCharacter(str, len) {
     
    var newString = str;
    if (str.length > len)
    {
        newString = str.substring(0, len) + "...";
    }

    return newString;
}

function BytesToKb(size) {
    
    if (size == "0")
    {
        size = "Not Found";
    }
    else
    {
        //size = parseFloat(size / 1024).toFixed(3) + ' Kb'
        size = parseFloat(size / 1024).toFixed(2) + ' Kb'
    }

    return size;
}

function GetHref(path, name,  size) {
    if (size == "0") {
        path = "#";
    }
    else {
        path = path + name;
    }

    return path;
}
function SuFixDate() {
    var strDate = '';
    var date = new Date();
    var d = date.getDate();
    var MM = date.getMonth() + 1;
    var y = date.getFullYear();
    var H = date.getHours();
    var mm = date.getMinutes();
    var s = date.getSeconds();
    strDate = d + "" + MM + "" + y + "" + H + "" + mm + "" + s;

    return strDate;
}
function CreateSufix(fileName, format) {
     
    var index = fileName.lastIndexOf(".");
    var name = fileName.substring(0, index);
    var ext = fileName.substring(index, fileName.length);
    fileName = name + format + ext;

    return fileName;
}


function GetInfoFile(urlGetInfo, url, strFiles) {
     
    var dataReturn = '';
    $.ajax({
        url: urlGetInfo,
        data: { path: url, fileNames: strFiles },
        type: 'POST',
        datatype:'json',
        async: false,
        success: function (data) {
             
            dataReturn = data;
        }
    });
    
    return dataReturn;
}
function VnrUpload(vnrUploadInfo) {
     
    var host = vnrUploadInfo["Host"];
    var style = vnrUploadInfo["Style"];
    var id = vnrUploadInfo["Id"];
    var files = vnrUploadInfo["Files"];
    var path = vnrUploadInfo["PathFile"];
    var fieldName = vnrUploadInfo["Field"];
    var multiple = vnrUploadInfo["Multiple"];
    var autoReNameFile = vnrUploadInfo["AutoReNameFile"];
    var autoUpload = vnrUploadInfo["AutoUpload"];
    var fileType = vnrUploadInfo["FileType"];
    var enable = vnrUploadInfo["Enable"];
    //var readOnly = true;
    //Chuổi định dang khi chọn auto rename
    var strFormat = '';
     
    var urlSave = host + "Home/SaveFileUpload/";
    var urlDelete = host + "Home/DeleteFile";
    var urlGetInfo = host + "Home/GetFileInfo";
    var urlTemplate = path;

    var strTextSelect = "Click chọn hoặc kéo thả file để tải lên";
    if (!enable) {
        strTextSelect = "Danh sách tập tin";
    }
    //----
    var fileInfo = [];
    if (files != '' && files != undefined && files.length > 0) {
        fileInfo = GetInfoFile(urlGetInfo, path, files);
    }
     
    //----

    //if (style == '' || style == undefined) {
    //    style = "style='width:285px;'";
    //}
    //style = style.replace('"', '\'');
    //alert(style);
        $("#" + id).kendoUpload({
            async: {
                saveUrl: urlSave,
                removeUrl: urlDelete,
                autoUpload: autoUpload,
            },
            multiple: multiple,
            files: fileInfo,
            localization: {
                select: strTextSelect,
                dropFilesHere: "thả",
                headerStatusUploaded: "Xong",
                headerStatusUploading: "...",
                uploadSelectedFiles: "Tải lên"
            },
            enabled: enable,
            upload: onUploadDedault,
            success: onSuccessDefault,
            remove: onRemoveDefault,
            select: onSelectDefault,
            progress: onProgressDefault,
            template: "<div id='upload-file' class='container' style='" + style + "; min-width: 290px !important;'>" +
                "<div class='row' style='" + style + "'><div>" +
              "<div class='col-lg-8 no-padding'><a class='file-upload-name' title='#=name#' href='#=GetHref('" + urlTemplate + "',name, size)#'>#=LimitCharacter(name,30)#</a></div>" +
                      "<div class='col-lg-3 no-padding' style='text-align:center;'>#=BytesToKb(size)#</div>" +
                      "<div class='col-lg-1 no-padding'><button type='button' class='k-upload-action' style='position: absolute; top: 0; right: 0;'></button></div>" +
                      "</div></div></div>"
        });
    
    // 
    //if (!readOnly) {
    //     
    //    var upload = $("#" + id).data("kendoUpload");
    //    upload.async(function () {
    //        return {
    //            saveUrl: urlSave,
    //            removeUrl: urlDelete,
    //            autoUpload: autoUpload,
    //        }
    //    });
    //    //upload.bind("change", multiselect_change);
    //}

    //if (fileInfo != '' && fileInfo != undefined) {
    //    var uploadControl = $("#" + id).data("kendoUpload");
    //    uploadControl.files = fileInfo;
    //}
     
    function onProgressDefault(e) {
    }
    function onSelectDefault(e) {
         
        var fileTypes = fileType.split("*");
        $.each(e.files, function (index, value) {
            var ext = value.extension;
            var i = fileTypes.indexOf(ext);
            if (i == -1) {
                e.preventDefault();
                ShowNotificationProcessor("Error, Chỉ cho phép tải lên các loại file sau: " + fileType);
            }
        });
        if (autoReNameFile == true) {
            if (e != undefined && autoReNameFile == true) {
                var listFile = e.files;
                for (var i = 0; i < listFile.length; i++) {
                    var name = listFile[i].name;
                    strFormat = SuFixDate();
                    name = CreateSufix(name, strFormat);
                    listFile[i].name = name;
                }
            }
        }

    }
    function onUploadDedault(e) {
         

        
        e.data = {
            formatString: strFormat
        };
    }
    function onRemoveDefault(e) {
        if (!enable) {
            e.preventDefault();
        }
        else {
             
            var str = $("#" + fieldName).val();
            for (var i = 0; i < e.files.length; i++) {
                var file = e.files[i].name;
                var strSplit = str.split(',');
                for (var i = 0; i < strSplit.length; i++) {
                    if (strSplit[i] == "" || strSplit[i] == file) {
                        strSplit.splice(i, 1);
                    }
                }
                str = strSplit.join(',');
            }

            $("#" + fieldName).val(str);
        }
    }
    function onSuccessDefault(e) {
        
        var responseTxt = jQuery.parseJSON(e.XMLHttpRequest.responseText);
         
        var char = ",";
        var fileNames = $("#" + fieldName).val();
        if (fileNames == '') {
            char = '';
        }
        var fileReturn = responseTxt.FileName;
        fileNames += char + fileReturn;
        $("#" + fieldName).val(fileNames);
    }
    function addExtensionClass(extension) {
        switch (extension) {
            case '.jpg':
            case '.img':
            case '.png':
            case '.gif':
                return "img-file";
            case '.doc':
            case '.docx':
                return "doc-file";
            case '.xls':
            case '.xlsx':
                return "xls-file";
            case '.pdf':
                return "pdf-file";
            case '.zip':
            case '.rar':
                return "zip-file";
            default:
                return "default-file";
        }
    }
}
//End Control Upload


