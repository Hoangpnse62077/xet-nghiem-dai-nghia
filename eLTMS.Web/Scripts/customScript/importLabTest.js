var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
    allLabTesting: [],
    ImportExcel: false, 
    LoadFromDataBase: false
}
var homeController = {
    init: function () {
        homeController.loadDataLabTestingAdd();
        homeController.loadData();
        homeController.registerEvent();
        homeController.loadDataLabTestingResult();
    },
    registerEvent: function () {

        $('#btnSave').off('click').on('click', function () {
            var code = $('#txtCode').val();
            var name = $('#txtName').val();
            var type = parseInt($('#ddlType').val());
            var labTestId = parseInt($('#txtLabTestId').val());
            var price = parseInt($('#txtPrice').val());
            var description = $('#txtDescription').val();
            var isDeleted = "False";
            var labTest = {
                LabTestId: labTestId,
                LabTestCode: code,
                LabTestName: name,
                SampleId: type,
                Price: price,
                Description: description,
                IsDeleted: isDeleted
            }
            if (labTest.LabTestId == 0) {
                $.ajax({
                    url: '/LabTest/AddLabTest',
                    type: 'Post',
                    dataType: 'json',
                    data: labTest,
                    success: function (res) {
                        if (!res.sucess) {
                            if (res.validation && res.validation.Errors) {
                                toastr.error(res.validation.Errors[0].ErrorMessage);
                            }

                        }
                        else {
                            toastr.success("Tạo mới thành công.");
                            $('#myModal').modal('hide');
                            homeController.loadData();
                        }
                    }
                })
            } else {
                $.ajax({
                    url: '/LabTest/UpdateLabTest',
                    type: 'Post',
                    dataType: 'json',
                    data: labTest,
                    success: function (res) {
                        if (!res.sucess) {
                           
                                toastr.error("Cập nhật không thành công");
                            

                        }
                        else {
                            toastr.success("Cập nhật thành công.");
                            $('#myModal').modal('hide');
                            homeController.loadData();
                        }
                    }
                })
            }
          
        })      
        $('#btnSaveLabTesting').off('click').on('click', function () {
            var allRows = $('.data-row-lab-testing-import');
            var allData = [];
            $.each(allRows, function (i, item) {
               
                var labTestingId = $(item).find('.ddlCode').val();
                var machineSlot = $(item).find('.txtSlot').val();
                var status = "Waiting";
                var data = {
                    LabTestingId:labTestingId,
                    MachineSlot: machineSlot,
                    Status: status
                }
                if (data.LabTestingId != null && data.LabTestingId != '' )
                allData.push(data);


             
            });
            $.ajax({
                url: '/LabTest/UpdateLabTesting',
                type: 'Post',
                dataType: 'json',
                data: { labTesting: allData },
                async: false,
                success: function (res) {
                    if (!res.success) {
                        toastr.success("Tạo mới không thành công.");

                    }
                    else {
                        toastr.success("Tạo mới thành công.");
                        $('#input').show();
                    }
                }
            })
            homeController.loadDataLabTestingAdd();
            $('#myModalLabTestingData').modal('hide');
        })
        $('#btnSaveLabTestingIndex').off('click').on('click', function () {          
            var allRows = $('.data-row');
            var allData = [];
            var allData1 = [];
            $.each(allRows, function (i, item) {

                var labTestingId = $(item).find('.colId').text();               
                var name = $(item).find('.colName').text();
                var value = $(item).find('.colValue').text();
                var labTestingIndexStatus = $(item).find('.colStatus').text();
                var nomal = $(item).find('.colNomal').text();
                var unit = $(item).find('.colUnit').text();
                var data = {
                    LabTestingId: labTestingId ,
                    MachineSlot: 0,
                    Status: "LabtestDone"
                }
                var data1 = {
                    LabTestingId: labTestingId,
                    IndexName: name,
                    IndexValue: value,
                    LowNormalHigh: labTestingIndexStatus,
                    NormalRange: nomal,
                    IsDeleted: false,
                    Unit: unit
                }
                allData1.push(data1);
                allData.push(data);
               


            });
           
            $.ajax({
                url: '/labtest/addlabtestingindex',
                type: 'post',
                datatype: 'json',
                data: { labtestingindex: allData1 },
                async: false,
                success: function (res) {
                    if (!res.success) {
                        toastr.success("tạo mới không thành công.");

                    }
                    else {
                        $.ajax({
                url: '/LabTest/UpdateLabTesting',
                type: 'Post',
                dataType: 'json',
                data: { labTesting: allData },
                async: false,
                success: function (res) {
                    if (!res.success) {
                        toastr.success("Tạo mới không thành công.");

                    }
                    else {
                        toastr.success("Tạo mới thành công.");

                    }
                }
            })
                        $('#btnSaveLabTestingIndex').hide();
                        $('#input').hide();
                    }
                }
            })

        })
        $('#btnSaveSample').off('click').on('click', function () {
            var name = $('#txtSampleName').val();
            var type = parseInt($('#ddlSampleType').val());
            var description = $('#txtSampleDescription').val();
            var isDeleted = "False";
            var sample = {
                SampleName: name,
                SampleGroupId: type,
                Description: description,
                IsDeleted: isDeleted
            }
            
                $.ajax({
                    url: '/LabTest/AddSample',
                    type: 'Post',
                    dataType: 'json',
                    data: sample,
                    success: function (res) {
                        if (!res.sucess) {
                            if (res.validation && res.validation.Errors) {
                                toastr.error(res.validation.Errors[0].ErrorMessage);
                            }

                        }
                        else {
                            toastr.success("Tạo mới thành công.");
                            $('#myModal1').modal('hide');
                            homeController.loadDataSample();
                            $('#myModal3').modal('show');
                        }
                    }
                })
            

        })
        $('#btnSaveSampleGroup').off('click').on('click', function () {
            var name = $('#txtGroupName').val();
            var duration = parseInt($('#txtTime').val());
            var start = parseInt($('#txtStart').val());
            var close = parseInt($('#txtClose').val());
            var isDeleted = "False";
            var sampleGroup = {
                GroupName: name,
                GettingDuration: duration,
                OpenTime: start,
                CloseTime: close,
                IsDeleted: isDeleted
            }

            $.ajax({
                url: '/LabTest/AddSampleGroup',
                type: 'Post',
                dataType: 'json',
                data: sampleGroup,
                success: function (res) {
                    if (!res.sucess) {
                        if (res.validation && res.validation.Errors) {
                            toastr.error(res.validation.Errors[0].ErrorMessage);
                        }

                    }
                    else {
                        toastr.success("Tạo mới thành công.");
                        $('#myModal2').modal('hide');
                        homeController.loadDataSampleGroup();
                        $('#myModal4').modal('show');
                    }
                }
            })


        })
        $('#btnAddNew').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thêm mới dịch vụ xét nghiệm');
            homeController.resetForm();
            $('#myModal').modal('show');
        });
        $('#btnAddNewSampleGroup').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thêm mới nhóm xét nghiệm');
            $('#myModal4').modal('hide');
            $('#myModal2').modal('show');
        });
        $('#btnAddNewLabTesting').off('click').on('click', function () {
            $('#lblPopupTitle').text('Tiến hành xử lý mẫu');
            $('#myModalLabTesting').modal('hide');
            $('#myModalLabTestingData').modal('show');
            homeController.loadDataLabTestingAdd();
           
           
        });
        $('#btnAddNewSample').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thêm mới xét nghiệm');
            $('#myModal3').modal('hide');
            $('#myModal1').modal('show');
        });
        $('#btnViewSample').off('click').on('click', function () {
            $('#lblPopupTitle').text('Danh sách các loại xét nghiệm');
            $('#myModal3').modal('show');
            homeController.loadDataSample(true);
        });
        $('#btnViewSampleGroup').off('click').on('click', function () {
            $('#lblPopupTitle').text('Danh sách các nhóm xét nghiệm');
            $('#myModal4').modal('show');
           homeController.loadDataSampleGroup(true);
        });
        $('#btnViewLabTesting').off('click').on('click', function () {
            $('#lblPopupTitle').text('Danh sách các yêu cầu xét nghiệm');
            $('#myModalLabTesting').modal('show');
            homeController.loadDataLabTesting(true);
            
        });
        $('#btnReset').off('click').on('click', function () {
            $('#txtNameS').val('');
            $('#ddlStatusS').val('');
            homeController.loadData(true);
        });
        $('#btnClose').off('click').on('click', function () {
            $('#txtResult').val('');
            homeController.loadDataLabTestingResult();
        });
        $('.btn-edit').off('click').on('click', function () {
            $('#lblPopupTitle').text('Cập nhật xét nghiệm');
            $('#myModal').modal('show');
            var id = $(this).data('id');
            homeController.loadDetail(id);
        });
        //$('.btn-delete').off('click').on('click', function () {
        //    var id = $(this).data('id');
        //    homeController.delete(id);
            
        //});
        $('.btn-deleteSample').off('click').on('click', function () {
            var id = $(this).data('id');
            homeController.deleteSample(id);
            homeController.loadDataSample(true);

        });
        $('.btn-deleteSampleGroup').off('click').on('click', function () {
            var id = $(this).data('id');
            homeController.deleteSampleGroup(id);
        });
        $('.btn-editLabTestingResult').off('click').on('click', function () {
            var id = $(this).data('id');
            homeController.loadDataLabTestingResultHaveCode(id);
            $('#lblPopupTitle').text('Danh sách các yêu cầu xét nghiệm');
            $('#myModalLabTestingResult').modal('show');
           
        });
        $('.btn-viewLabTestingIndex').off('click').on('click', function () {
            var id = $(this).data('id');
            homeController.loadDataLabTestingIndexHaveLabtestingId(id);
            $('#lblPopupTitle').text('Danh sách các yêu cầu xét nghiệm');
            $('#myModalLabTestingIndexResult1').modal('show');
            $('#txtLabtestingFailID').val(id);
        });
        $('#btnAgain').off('click').on('click', function () {
            var id = $('#txtLabtestingFailID').val();  
            $.ajax({
                url: '/LabTest/GetAllLabTestingIndexHaveLabtestingId',
                type: 'GET',
                dataType: 'json',
                data: { id: id },
                success: function (response) {
                    if (response.success) {
                        var data = response.data;
                        $.ajax({
                            url: '/LabTest/DeleteAll',
                            type: 'Post',
                            dataType: 'json',
                            data: { labTestingIndex: data },
                            async: false,
                            success: function (res) {
                                if (!res.success) {
                                    toastr.success("Xóa không thành công.");

                                }
                                else {
                                    toastr.success("Thành công.");

                                }
                            }
                        })
                    }
                }
            })
            $.ajax({
                url: '/LabTest/UpdateLabTestingFail',
                type: 'Post',
                dataType: 'json',
                data: { id:id },
                success: function (res) {
                    if (!res.sucess) {
                        if (res.validation && res.validation.Errors) {
                            toastr.error(res.validation.Errors[0].ErrorMessage);
                        }

                    } 

                    $('#myModalLabTestingResult').modal('hide');
                    location.reload();

                }
            })
        })
        $('#btnAddNewResult').off('click').on('click', function () {
            var ids = "" + $(this).data('ids') + "" ;
            var listId = ids.split(',');
            var code = $('#txtCode').val();
            var con = $('#txtResult').val(); 
            var cmt = $('#txtCMT').froalaEditor('html.get');
            var allData = [];
            $.each(listId, function (i, item) {
                
                var data = {
                    LabTestingId: item,
                    MachineSlot: 0,
                    Status: "DOCTORDONE"
                }
                allData.push(data);
            });          
            $.ajax({
                url: '/LabTest/UpdateResult',
                type: 'Post',
                dataType: 'json',
                data: { code:code,con:con,cmt:cmt },
                success: function (res) {
                    if (!res.sucess) {
                        if (res.validation && res.validation.Errors) {
                            toastr.error(res.validation.Errors[0].ErrorMessage);
                        }

                    } toastr.success("Chẩn đoán thành công.");
                  
                    $('#myModalLabTestingResult').modal('hide');
                    location.reload();
                    
                }
            })
        })
        $("#txtResult").off('change').on("change", function () {
            $('#btnAddNewResult').show();
        })
        $("#txtSearch").off('change').on("change", function () {
            homeController.loadData(true);
        })
        $("#input").off('change').on("change", function () {
            var excelFile,
                fileReader = new FileReader();

            $("#result").hide();

            fileReader.onload = function (e) {
                var buffer = new Uint8Array(fileReader.result);
                $('.data-row').remove();
                $.ig.excel.Workbook.load(buffer, function (workbook) {
                    for (var a = 0; a < 10; a++) {
                        var column, row, newRow, cellValue, columnIndex, i,
                            worksheet = workbook.worksheets(a),
                            columnsNumber = 0,
                            gridColumns = [],
                            data = [];

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
                                column = "IndexName";
                                break;
                            case 1:
                                column = "IndexValue";
                                break;
                            case 2:
                                column = "Status";
                                break;
                            case 3:
                                column = "Normal";
                                break;
                            case 4:
                                column = "Unit";
                                break;
                        }
                        gridColumns.push({ headerText: column, key: column });
                    }

                    // We start iterating from 1, because we already read the first row to build the gridColumns array above
                    // We use each cell value and add it to json array, which will be used as dataSource for the grid
                    for (i = ((worksheet.rows().count())-1); i >0; i--) {
                        newRow = {};
                        row = worksheet.rows(i);

                        for (columnIndex = 0; columnIndex < columnsNumber; columnIndex++) {
                            cellValue = row.getCellText(columnIndex);
                            newRow[gridColumns[columnIndex].key] = cellValue;
                        }

                        data.push(newRow);
                    }

                    homeconfig.ImportExcel = true;
                    $.each(data, function (i, item) {
                     
                        for (var i = 0; i < homeconfig.allLabTesting.length; i++) {
                           
                            if (homeconfig.allLabTesting[i].MachineSlot == (a+1)) {
                                item.LabTestingId = homeconfig.allLabTesting[i].LabTestingId;
                              
                                break;
                            }
                        }
                        var newRow = $('#template-row').clone();
                        $(newRow).insertAfter('#template-row');
                        $(newRow).addClass('data-row');
                        $(newRow).find('.colId').text(item.LabTestingId);
                        $(newRow).find('.colName').text(item.IndexName);
                        $(newRow).find('.colValue').text(item.IndexValue);
                        $(newRow).find('.colStatus').text(item.Status);
                        $(newRow).find('.colNomal').text(item.Normal);
                        $(newRow).find('.colUnit').text(item.Unit);
                        if (item.Status == 'L' || item.Status == 'H') {
                            $(newRow).addClass('alertQuanity');
                        }
                    });

                    var allRows = $('.data-row');

                    for (var i = 0; i < allRows.length; i++) {
                        $(allRows[i]).change();
                    }
                    $('.data-row').removeAttr('style');
                    homeconfig.ImportExcel = false;
                        $("#result").show();
                        $("#btnSaveLabTestingIndex").show();
                }
                }, function (error) {
                    $("#result").text("The excel file is corrupted.");
                   
                });
            }

            if (this.files.length > 0) {
                excelFile = this.files[0];
                if (excelFile.type === "application/vnd.ms-excel" || excelFile.type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || (excelFile.type === "" && (excelFile.name.endsWith("xls") || excelFile.name.endsWith("xlsx")))) {
                    fileReader.readAsArrayBuffer(excelFile);
                } else {
                    $("#result").text("The format of the file you have selected is not supported. Please select a valid Excel file ('.xls, *.xlsx').");
                    $("#result").show(1000);
                }
            }

        });
        $('#btnSearchDate').off('click').on('click', function () {
            homeController.loadDataLabTestingAddDate();
        });

    },
    delete: function (id) {
        $.ajax({
            url: '/LabTest/DeleteLabTest',
            data: {
                id: id
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
    deleteSample: function (id) {
        try {
            $.ajax({
                url: '/LabTest/DeleteSample',
                data: {
                    id: id
                },
                async: false,
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.success == true) {
                        toastr.success("Xóa thành công.");
                    }
                    else {
                        toastr.error("Xóa không thành công.");
                    }
                }
            });
           
        }

        catch (err) {
            alert(err);
        }
       
    },
    deleteSampleGroup: function (id) {
        try {
            $.ajax({
                url: '/LabTest/DeleteSampleGroup',
                data: {
                    id: id
                },
                async: false,
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.success == true) {
                        toastr.success("Xóa thành công.");
                    }
                    else {
                        toastr.error("Xóa không thành công.");
                    }
                }
            });

        }

        catch (err) {
            alert(err);
        }

    },
    deleteLabtestingIndex: function (id) {
        try {
            $.ajax({
                url: '/LabTest/DeleteLabTestingIndex',
                data: {
                    id: id
                },
                async: false,
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.success == true) {
                    }
                    else {
                        toastr.error("Xóa không thành công.");
                    }
                }
            });

        }

        catch (err) {
            alert(err);
        }

    },
    loadDetail: function (id) {
        $.ajax({
            url: '/LabTest/LabTestDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.sucess) {
                    var data = response.data;
                    $('#txtLabTestId').val(data.LabTestId);
                    $('#txtCode').val(data.LabTestCode);
                    $('#txtName').val(data.LabTestName);
                    $('#ddlType').val(data.SampleId).change();
                    $('#txtPrice').val(data.Price);
                    $('#txtDescription').val(data.Description);
                   
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
    resetForm: function () {
        $('#txtLabTestId').val('0');
        $('#txtCode').val('');
        $('#txtName').val('');
        $('#ddlType').val('').change();
        $('#txtPrice').val('');
        $('#txtDescription').val('');
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/LabTest/GetAllLabTests',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, code: $('#txtSearch').val() },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            LabTestId: item.LabTestId,
                            Code: item.LabTestCode,
                            Name: item.LabTestName,
                            Type: item.SampleName,
                            Price: item.Price,
                            Description: item.Description,
                        });

                    });
                    $('#tblData').html(html);
                    homeController.paging(response.total, function () {
                        homeController.loadData();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        })
    },
    loadDataSample: function () {
        $.ajax({
            url: '/LabTest/GetAllSamples',
            type: 'GET',
            dataType: 'json',
            data: {  },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#dataSample-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            SampleId: item.SampleId,
                            Name: item.SampleName,
                            Type: item.SampleGroupName,
                            Description: item.Description,
                        });

                    });
                    $('#tblDataSample').html(html);
                    
                    homeController.registerEvent();
                }
            }
        })
    },
    loadDataSampleGroup: function () {
        $.ajax({
            url: '/LabTest/GetAllSampleGroups',
            type: 'GET',
            dataType: 'json',
            data: {},
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#dataSampleGroup-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            SampleGroupId: item.SampleGroupId,
                            Name: item.GroupName,
                            Start: item.OpenTime,
                            Duration: item.GettingDuration,
                            Close: item.CloseTime,
                        });

                    });
                    $('#tblDataSampleGroup').html(html);

                    homeController.registerEvent();
                }
            }
        })
    },
    loadDataLabTestingAdd: function (changePageSize) {
        $.ajax({
            url: '/LabTest/GetAllLabTestings',
            type: 'GET',
            dataType: 'json',
            data: {},
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    homeconfig.allLabTesting = data;   
                    $('.data-row-lab-testing-import').remove();
                    homeController.loadAddLabTesting();          
                    homeController.registerEvent();
                }
            }
        })
    },
    loadDataLabTestingAddDate: function (changePageSize) {
        $.ajax({
            url: '/LabTest/GetAllLabTestingDate',
            type: 'GET',
            dataType: 'json',
            data: { date: $('#txtSearchDate').val()},
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    homeconfig.allLabTesting = data;
                    $('.data-row-lab-testing-import').remove();
                    homeController.loadAddLabTesting();
                    homeController.registerEvent();
                }
            }
        })
    },
    loadDataLabTesting: function (changePageSize) {
        $.ajax({
            url: '/LabTest/GetAllLabTesting',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#dataLabTesting-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            LabTestingId: item.LabTestingId,
                            Name: item.LabTestName,
                            Status: item.Status,
                            Getting: item.AppointmentCode,
                            Slot: item.MachineSlot,
                            Group: item.SampleName,
                        });

                    });
                    $('#tblDataLabTesting').html(html);
                    homeController.paging(response.total, function () {
                        homeController.loadDataLabTesting();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        })
    },
    loadDataLabTestingResult: function (changePageSize) {
        $.ajax({
            url: '/LabTest/GetAllLabTestingResult',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#dataLabTestingResult-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            LabTestingId: item.LabTestingId,
                            Name: item.PatientName,
                            Status: item.Status,
                            Getting: item.AppointmentCode,
                            Group: item.PDOB,
                        });

                    });
                    $('#tblDataLabTestingResult').html(html);
                    homeController.paging(response.total, function () {
                        homeController.loadDataLabTestingResult();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        })
    },
    loadDataLabTestingResultHaveCode: function (code) {
        $.ajax({
            url: '/LabTest/GetAllLabTestingHaveAppointmentCode',
            type: 'GET',
            dataType: 'json',
            data: { code:code },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#dataLabTestingResult1-template').html();
                    var allIds = [];
                    $.each(data, function (i, item) {
                        allIds.push(item.LabTestingId);
                        html += Mustache.render(template, {
                            LabTestingId: item.LabTestingId,
                            Name: item.LabTestName,
                            Status: item.Status,
                            Getting: item.AppointmentCode,
                            Group: item.SampleName,

                        });


                    });
                    var allIdsString = JSON.stringify(allIds); 
                    console.log(allIdsString);

                    $('#btnAddNewResult').attr('data-ids', allIdsString);
                    console.log(html);
                    $('#tblDataLabTestingResult1').html(html);
                    $('#txtCode').val(code);
                    homeController.registerEvent();
                } $('#txtResult').val(''); /*$('#btnAddNewResult').hide();/**/
            }
        })
    },
    loadDataLabTestingIndexHaveLabtestingId: function (id) {
        $.ajax({
            url: '/LabTest/GetAllLabTestingIndexHaveLabtestingId',
            type: 'GET',
            dataType: 'json',
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#dataLabTestingIndexResult1-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            LabTestingId: item.LabTestingId,
                            Name: item.IndexName,
                            Value: item.IndexValue,
                            Status: item.LowNormalHigh,
                            Nomal: item.NormalRange,
                            Unit: item.Unit,
                            changeColor: homeController.getColorByStatus(item.LowNormalHigh)
                        });
                        
                    });
                    $('#tblDataLabTestingIndexResult1').html(html);

                    homeController.registerEvent();
                }
            }
        })
    },
    loadAddLabTesting: function () {
        var i = 0;
        for (i; i < 10; i++) {

            var newRow = $('#template1-row').clone();
            $(newRow).addClass('data-row-lab-testing-import');
            console.log(newRow.html());
            var ddlData = "<select class='form-control ddlCode'>";
            ddlData += "<option value=''> --- Chọn mẫu xét nghiệm --- </option>";
            $.each(homeconfig.allLabTesting, function (i, item) {

                ddlData += "<option value='" + item.LabTestingId + "' data-name='" + item.LabTestName + "'>" + item.AppointmentCode + " - " + item.LabTestName + "</option>"
                

            });
            ddlData += "</select>";
            $(newRow).find('.txtSlot').val(10 - i);
            $(newRow).find('.colCode').html(ddlData);
            $(newRow).insertAfter('#template1-row');
            $(newRow).removeAttr('style');
            homeController.registerEventForChangeDropDown();
            
        }

    },
    registerEventForChangeDropDown: function () {
        $('.ddlCode').off('change').on('change', function () {

            var value = $(this).find(':selected').val();       
            var allRows = $('.data-row-lab-testing-import');         
            var name = $(this).find(':selected').data('name');
            var curentRow = $(this).closest('tr');
            $(curentRow).find('.colName').text(name);
        });
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
    checkIsDelete: function (SampleGettingId) {

        var modalConfirm = function (callback) {

            $("#mi-modal").modal('show');

            $("#modal-btn-si").off('click').on("click", function () {
                callback(true);
                $("#mi-modal").modal('hide');
            });

            $("#modal-btn-no").off('click').on("click", function () {
                callback(false);
                $("#mi-modal").modal('hide');
            });
        };

        modalConfirm(function (confirm) {
            if (confirm) {
                homeController.delete(SampleGettingId);
            }
            else {
                homeController.loadData();
            }
        });

    },
    getColorByStatus: function (status) {
        if (status == 'L') {
            return 'background-color: yellow;'
        } else if (status == 'H') {
            return 'background-color: red;'
        } else {
            return "";
        }
                
    }
}

homeController.init();