var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
    allSupply: [],
    ImportExcel: false,
    LoadFromDataBase : false
}
var homeController = {
    init: function () {
        //homeController.loadData();
        homeController.registerEvent();
        homeController.loadAllSuply();
    },
    registerEvent: function () {

       
        $('#btnDownload').off('click').on('click', function () {
            $('#hiddenForm').submit();

        })
        $('#btnDownload1').off('click').on('click', function () {
            homeController.loadFile();
        })
        $('#btnAddNew').off('click').on('click', function () {
            var newRow = $('#template-row').clone();
            $(newRow).addClass('data-row');
            console.log(newRow.html());
            var ddlData = "<select class='form-control ddlCode'>";
            ddlData += "<option value=''> --- Chọn vật tư --- </option>";
            $.each(homeconfig.allSupply, function (i, item) {
                ddlData += "<option value='" + item.SuppliesId + "' data-name='" + item.SuppliesName + "' data-unit='" + item.Unit + "'>" + item.SuppliesCode + "</option>"

            });           
            ddlData += "</select>";
            var codeColumn = $(newRow).find('.colCode').html(ddlData);
            $(newRow).insertAfter('#template-row');
            $(newRow).removeAttr('style');
            homeController.registerEventForChangeDropDown();
            
        });
        $('#btnView').off('click').on('click', function () {
            $('#lblPopupTitle').text('Danh sách phiếu xuất kho');
            homeController.resetForm();
            $('#myModal').modal('show');
        });
        $('#btnView1').off('click').on('click', function () {
            $('#lblPopupTitle').text('Danh sách phiếu kiểm kho');
            homeController.resetForm();
            $('#myModal').modal('show');
        });
        $('#btnSaveImport').off('click').on('click', function () {
            var allRows = $('.data-row');
            var tmpData = [];
            $.each(allRows, function (i, item) {
                var detail = {};
                detail.SuppliesId = $(item).find('.ddlCode').val();
                detail.Quantity = $(item).find('.txtQuantity').val();
                detail.Note = $(item).find('.txtNote').val();
                detail.Unit = $(item).find('.colUnit').text();
                tmpData.push(detail);
            });
            var data = {
                ExportPaperCode: $('#txtImportCode').val(),
                Note: $('#txtNote').val(),
                ExportPaperDetails: tmpData,
                Status: "False"
            };
            $.ajax({
                url: '/WareHouse/AddExportPaper',
                type: 'Post',
                dataType: 'json',
                data: data,
                async: false,
                success: function (res) {
                    if (!res.success) {
                        toastr.success("Tạo mới không thành công.");

                    }
                    else {
                        toastr.success("Tạo mới thành công.");
                        $('#myModal').modal('hide');
                        homeController.loadData();
                    }
                }
            })
        });
        $('#btnSaveInventory').off('click').on('click', function () {
            var allRows = $('.data-row');
            var tmpData = [];
            $.each(allRows, function (i, item) {
                var detail = {};
                detail.SuppliesId = $(item).find('.ddlCode').val();
                detail.Quantity = $(item).find('.txtQuantity').val();
                detail.Note = $(item).find('.txtNote').val();
                detail.Unit = $(item).find('.colUnit').text();
                tmpData.push(detail);
            });
            var data = {
                ExportPaperCode: $('#txtImportCode').val(),
                Note: $('#txtNote').val(),
                ExportPaperDetails: tmpData,
                Status: "True"
            };
            $.ajax({
                url: '/WareHouse/AddExportPaper',
                type: 'Post',
                dataType: 'json',
                data: data,
                async: false,
                success: function (res) {
                    if (!res.success) {
                        toastr.success("Tạo mới không thành công.");

                    }
                    else {
                        toastr.success("Tạo mới thành công.");
                        $('#myModal').modal('hide');
                        homeController.loadData();
                    }
                }
            })
        });
        $('#btnSearch').off('click').on('click', function () {
            homeController.loadData(true);
        });
        $('#btnSearchInventory').off('click').on('click', function () {
            homeController.loadDataInventory(true);
        });
        $('#btnReset').off('click').on('click', function () {
            $('#txtNameS').val('');
            $('#ddlStatusS').val('');
            homeController.loadData(true);
        });
        $('.btn-edit').off('click').on('click', function () {
            $('#myModal').modal('hide');
            var id = $(this).data('id');
            homeController.loadDetail(id); 
            $('#btnSaveImport').hide();
            $('#btnSaveInventory').hide();
            $('#btnAddNew').hide();
            $('#input').hide();
            $('#inputPKK').hide();
            
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');

                homeController.deleteExport(id);
          
        });

        $("#input").off('change').on("change", function () {
           var excelFile,
           fileReader = new FileReader();
            fileReader.onload = function (e) {
                var buffer = new Uint8Array(fileReader.result);
                $('.data-row').remove();
                $.ig.excel.Workbook.load(buffer, function (workbook) {
                    var column, row, newRow, cellValue, columnIndex, i,
                        worksheet = workbook.worksheets(0),
                        columnsNumber = 0,
                        gridColumns = [],
                        data = [],
                        worksheetRowsCount;

                    // Both the columns and rows in the worksheet are lazily created and because of this most of the time worksheet.columns().count() will return 0
                    // So to get the number of columns we read the values in the first row and count. When value is null we stop counting columns:
                    while (worksheet.rows(0).getCellValue(columnsNumber)) {
                        columnsNumber++;
                    }

                    // Iterating through cells in first row and use the cell text as key and header text for the grid columns
                    for (columnIndex = 0; columnIndex < columnsNumber; columnIndex++) {
                        column = worksheet.rows(0).getCellText(columnIndex);
                        switch (columnIndex) {
                            case 0:
                                column = "SuppliesCode";
                                break;
                            case 1:
                                column = "Quantity";
                                break;
                            default:
                                column = "Note";
                                break;
                        }
                        gridColumns.push({ headerText: column, key: column });
                    }

                    // We start iterating from 1, because we already read the first row to build the gridColumns array above
                    // We use each cell value and add it to json array, which will be used as dataSource for the grid
                    for (i = 1, worksheetRowsCount = worksheet.rows().count(); i < worksheetRowsCount; i++) {
                        newRow = {};
                        row = worksheet.rows(i);

                        for (columnIndex = 0; columnIndex < columnsNumber; columnIndex++) {
                            cellValue = row.getCellText(columnIndex);
                            newRow[gridColumns[columnIndex].key] = cellValue;
                        }

                        data.push(newRow);
                    }

                    

                    var ddlData = "<select class='form-control ddlCode'>";
                    $.each(homeconfig.allSupply, function (i, item) {
                        ddlData += "<option value='" + item.SuppliesId + "' data-name='" + item.SuppliesName + "' data-unit='" + item.Unit + "'>" + item.SuppliesCode + "</option>"

                    });
                    ddlData += "</select>";

                    homeconfig.ImportExcel = true;
                    $.each(data, function (i, item) {
                        for (var i = 0; i < homeconfig.allSupply.length; i++) {
                            if (homeconfig.allSupply[i].SuppliesCode == item.SuppliesCode) {
                                item.SuppliesId = homeconfig.allSupply[i].SuppliesId;
                                item.Unit = homeconfig.allSupply[i].Unit;
                                item.SuppliesName = homeconfig.allSupply[i].SuppliesName;
                                var valid = true;
                                if (homeconfig.allSupply[i].Quantity < item.Quantity) {
                                    item.Quantity = 0;
                                    item.Note = "Số lượng kho chỉ còn " + homeconfig.allSupply[i].Quantity;
                                    valid = false;
                                } 
                                break;
                            }
                        }
                        var newRow = $('#template-row').clone();
                        $(newRow).addClass('data-row');
                        var codeColumn = $(newRow).find('.colCode').html(ddlData);
                        $(newRow).find('.txtQuantity').val(item.Quantity);
                        $(newRow).find('.txtNote').val(item.Note);
                        $(newRow).find('.colName').text(item.SuppliesName);
                        $(newRow).find('.colUnit').text(item.Unit);
                        if (valid == false) {                           
                            $(newRow).addClass('alertQuanity');
                        }
                        $(newRow).css('background-color', 'red');
                        $(newRow).insertAfter('#template-row');

                        


                    });
                  
                    var allRows = $('.data-row');
                    
                    for (var i = 0; i < allRows.length; i++) {
                        var supplyId = data[allRows.length - 1 - i].SuppliesId;
                        $(allRows[i]).find('.ddlCode').val(supplyId).change();
                    }
                    $('.data-row').removeAttr('style');
                    homeController.registerEventForChangeDropDown();
                    homeconfig.ImportExcel = false;
                }, function (error) {
                    //$("#result").text("The excel file is corrupted.");
                    //$("#result").show(1000);
                });
            }

            if (this.files.length > 0) {
                excelFile = this.files[0];
                if (excelFile.type === "application/vnd.ms-excel" || excelFile.type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || (excelFile.type === "" && (excelFile.name.endsWith("xls") || excelFile.name.endsWith("xlsx")))) {
                    fileReader.readAsArrayBuffer(excelFile);
                } else {
                    toastr.error("Nhập file excel"); 
                }
            }

        });
          $("#inputPKK").off('change').on("change", function () {
            var excelFile,
            fileReader = new FileReader();
             fileReader.onload = function (e) {
                var buffer = new Uint8Array(fileReader.result);
                $('.data-row').remove();
                $.ig.excel.Workbook.load(buffer, function (workbook) {
                    var column, row, newRow, cellValue, columnIndex, i,
                        worksheet = workbook.worksheets(0),
                        columnsNumber = 0,
                        gridColumns = [],
                        data = [],
                        worksheetRowsCount;

                    // Both the columns and rows in the worksheet are lazily created and because of this most of the time worksheet.columns().count() will return 0
                    // So to get the number of columns we read the values in the first row and count. When value is null we stop counting columns:
                    while (worksheet.rows(2).getCellValue(columnsNumber)) {
                        columnsNumber++;
                    }

                    // Iterating through cells in first row and use the cell text as key and header text for the grid columns
                    for (columnIndex = 0; columnIndex < columnsNumber; columnIndex++) {
                        column = worksheet.rows(2).getCellText(columnIndex);
                        switch (columnIndex) {
                            case 1:
                                column = "SuppliesCode";
                                break;
                            case 5:
                                column = "Quantity";
                                break;
                            case 6:
                                column = "Note";
                                break;
                        }
                        gridColumns.push({ headerText: column, key: column });
                    }

                    // We start iterating from 1, because we already read the first row to build the gridColumns array above
                    // We use each cell value and add it to json array, which will be used as dataSource for the grid
                    for (i = 3, worksheetRowsCount = worksheet.rows().count(); i <= worksheetRowsCount; i++) {
                        newRow = {};
                        row = worksheet.rows(i);

                        for (columnIndex = 0; columnIndex < columnsNumber; columnIndex++) {
                            cellValue = row.getCellText(columnIndex);
                            newRow[gridColumns[columnIndex].key] = cellValue;
                        }

                        data.push(newRow);
                    }



                    var ddlData = "<select class='form-control ddlCode'>";
                    $.each(homeconfig.allSupply, function (i, item) {
                        ddlData += "<option value='" + item.SuppliesId + "' data-name='" + item.SuppliesName + "' data-unit='" + item.Unit + "'>" + item.SuppliesCode + "</option>"

                    });
                    ddlData += "</select>";

                    homeconfig.ImportExcel = true;
                    $.each(data, function (i, item) {
                        console.log(item);
                        for (var i = 0; i < homeconfig.allSupply.length; i++) {
                            if (homeconfig.allSupply[i].SuppliesCode == item.SuppliesCode) {
                                item.SuppliesId = homeconfig.allSupply[i].SuppliesId;
                                item.Unit = homeconfig.allSupply[i].Unit;
                                item.SuppliesName = homeconfig.allSupply[i].SuppliesName;
                                break;
                            }
                        }
                        var newRow = $('#template-row').clone();
                        $(newRow).addClass('data-row');
                        var codeColumn = $(newRow).find('.colCode').html(ddlData);
                        $(newRow).find('.txtQuantity').val(item.Quantity);
                        $(newRow).find('.txtNote').val(item.Note);
                        $(newRow).find('.colName').text(item.SuppliesName);
                        $(newRow).find('.colUnit').text(item.Unit);
                        $(newRow).insertAfter('#template-row');




                    });

                    var allRows = $('.data-row');

                    for (var i = 0; i < allRows.length; i++) {
                        var supplyId = data[allRows.length - 1 - i].SuppliesId;
                        $(allRows[i]).find('.ddlCode').val(supplyId).change();
                    }
                    $('.data-row').removeAttr('style');
                    homeController.registerEventForChangeDropDown();
                    homeconfig.ImportExcel = false;
                }, function (error) {
                    $("#result").text("The excel file is corrupted.");
                    $("#result").show(1000);
                });
            }

            if (this.files.length > 0) {
                excelFile = this.files[0];
                if (excelFile.type === "application/vnd.ms-excel" || excelFile.type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || (excelFile.type === "" && (excelFile.name.endsWith("xls") || excelFile.name.endsWith("xlsx")))) {
                    fileReader.readAsArrayBuffer(excelFile);
                } else {
                    toastr.error("Nhập file excel");
                    $("#result").show(1000);
                }
            }

        });
    
    },

    deleteExport: function (id) {
        $.ajax({
            url: '/WareHouse/DeleteExportPaper',
            data: {
                importId: id
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.success == true) {
                    toastr.success("Xóa thành công.");
                    homeController.loadData(true);
                }
                else {
                    toastr.error("Xóa không thành công.");
                }
            },
            error: function (err) {
                console.log(err);
            }
        });

    },


    
    loadDetail: function (id) {
        $.ajax({
            url: '/WareHouse/LoadPaperExportDetailId',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    $('#txtImportCode').val(data.ExportPaperCode);
                    $('#txtNote').val(data.Note);
                    $('#txtCreateDate').val(data.CreateDate);
                    $('#txtCreateDate').removeAttr('style');
                    var importPaperDetailDtos = data.ExportPaperDetailDtos;
                    $('.data-row').remove();
                    
                    var ddlData = "<select class='form-control ddlCode'>";
                    $.each(homeconfig.allSupply, function (i, item) {
                        ddlData += "<option value='" + item.SuppliesId + "' data-name='" + item.SuppliesName + "' data-unit='" + item.Unit + "'>" + item.SuppliesCode + "</option>"

                    });
                    ddlData += "</select>";
                    
                    homeconfig.LoadFromDataBase = true;
                    $.each(importPaperDetailDtos, function (i, item) {
                        var newRow = $('#template-row').clone();
                        $(newRow).addClass('data-row');
                        var codeColumn = $(newRow).find('.colCode').html(ddlData);
                        $(newRow).find('.txtQuantity').val(item.Quantity);
                        $(newRow).find('.txtNote').val(item.Note);
                        $(newRow).insertAfter('#template-row');
                        homeController.registerEventForChangeDropDown();
                        
                        
                    });
                    console.log(importPaperDetailDtos);
                    
                    var allRows = $('.data-row');
                    console.log(allRows);
                    for (var i = 0; i < allRows.length; i++) {
                        var supplyId = importPaperDetailDtos[allRows.length - 1 - i].SuppliesId;
                        $(allRows[i]).find('.ddlCode').val(supplyId).change();
                    }
                    $('.data-row').removeAttr('style');
                    homeconfig.LoadFromDataBase = false;
                }
               
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    saveData: function () {
        
    },
    resetForm: function () {
        $('#txtSupplyId').val('0');
        $('#txtCode').val('');
        $('#txtName').val('')
        $('#ddlSupplyType').val('').change();
        $('#ddlSupplyUnit').val('').change();
        $('#txtNote').val('')
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/Warehouse/GetAllExportPapers',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, createDate: $('#txtSearch').val() },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ImportPaperId: item.ExportPaperId,
                            ImportPaperCode: item.ExportPaperCode,
                            CreateDate: item.CreateDate,
                        });

                    });
                    console.log(html);
                    $('#tblData1').html(html);
                    homeController.paging(response.total, function () {
                        homeController.loadData();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        })
    },
    loadDataInventory: function (changePageSize) {
        $.ajax({
            url: '/Warehouse/GetAllInventorys',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, createDate: $('#txtSearch').val() },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ImportPaperId: item.ExportPaperId,
                            ImportPaperCode: item.ExportPaperCode,
                            CreateDate: item.CreateDate,
                        });

                    });
                    console.log(html);
                    $('#tblData1').html(html);
                    homeController.paging(response.total, function () {
                        homeController.loadDataInventory();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / homeconfig.pageSize);

        //Unbind pagination if it existed or click change pagesize
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "Đầu",
            next: "Tiếp",
            last: "Cuối",
            prev: "Trước",
            visiblePages: 10,
            onPageClick: function (event, page) {
                homeconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    },
    loadAllSuply: function () {
        $.ajax({
            url: '/WareHouse/GetAllSupply',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    homeconfig.allSupply = data;

                }
                else {
                    bootbox.alert(response.message);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },

    loadFile: function () {
        $("#hiddenFormExportFile").submit();
    },
    registerEventForChangeDropDown: function () {
        $('.ddlCode').off('change').on('change', function () {

            var value = $(this).find(':selected').val();
            console.log(value);
            var allRows = $('.data-row');
            console.log(homeconfig.LoadFromDataBase);
            console.log(homeconfig.ImportExcel);
            if (homeconfig.LoadFromDataBase == true || homeconfig.ImportExcel == true) {

            }
            else {
                for (var i = 0; i < allRows.length; i++)
                {
                    if (i != 0 && $(allRows[i]).find('.ddlCode').val() == value) {
                        toastr.error("Vật tư với mã " + $(this).find(':selected').text() + " đã được chọn ");
                        return;
                    }
                }
            }
           

            var name = $(this).find(':selected').data('name');
            var unit = $(this).find(':selected').data('unit');
            var curentRow = $(this).closest('tr');
            $(curentRow).find('.colName').text(name);
            $(curentRow).find('.colUnit').text(unit);
        });
    }
}
homeController.init();