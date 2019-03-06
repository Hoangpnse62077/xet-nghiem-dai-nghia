var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
    allLabTestType: {}
}
var homeController = {
    init: function () {
        homeController.loadData();
        homeController.registerEvent();
        //homeController.loadAllLabTestType();
        //homeController.loadAllPatients();
    },
    registerEvent: function () {
        $('#btnClearForm').off('click').on('click', function () {
            $('#ddlPatientChoose').val('').change();
            homeController.loadData(true);
        });
        $('#btnSearch').off('click').on('click', function () {
            homeController.loadData(true);
        });
        $('#btnExportHuyetHoc').off('click').on('click', function () {
            var id = $('#txtLabTestResultId').val(); 
            if (id != 0) {
                $('#lbllabTestResultId').val(id);
                $('#exportHuyetHoc').submit();
            }
        });
        $('#btnExportNuocTieu').off('click').on('click', function () {
            var id = $('#txtLabTestResultId').val();
            if (id != 0) {
                $('#lbllabTestResultIdNuocTieu').val(id);
                $('#exportNuocTieu').submit();
            }
        });
        $('#btnExportSinhHoa').off('click').on('click', function () {
            var id = $('#txtLabTestResultId').val();
            if (id != 0) {
                $('#lbllabTestResultIdSinhHoa').val(id);
                $('#exportSinhHoa').submit();
            }
        });
        $('#ddlPatients').off('change').on('change', function () {
            var id = $(this).val();
            if (id != 0) {
                $.ajax({
                    url: '/Patient/PatientDetail',
                    type: 'GET',
                    dataType: 'json',
                    data: { id: id },
                    success: function (response) {
                        var patient = response.data;
                        console.log(patient);
                        $('#txtCode').val(patient.PatientCode);
                        $('#txtPhoneNumber').val(patient.PhoneNumber);
                        $('#txtHomeAddress').val(patient.HomeAddress);
                        $('#ddlGender').val(patient.Gender).change();
                        $('#txtDate').val(patient.Age);
                        $('#txtPatientName').val(patient.FullName);
                    }
                });
            } else {
                $('#txtCode').val('');
                $('#txtPhoneNumber').val('');
                $('#txtHomeAddress').val('');
                $('#ddlGender').val('');
            }
            
            
        });
        $("#txtName").autocomplete({
            source: function (request, response) {
                var customer = new Array();
                $.ajax({
                    async: false,
                    cache: false,
                    type: "POST",
                    url: "/Patient/AutoCompletet",
                    data: { "searchKey": $("#txtName").val() },
                    success: function (data) {
                        response($.map(data, function (val, item) {
                            return {
                                label: val.FullName,
                                value: val.PatientId,
                                customerId: val.PatientId
                            };
                        }));

                        //console.log(data);
                        //for (var i = 0; i < data.length; i++) {
                        //    customer[i] = { label: data[i].FullName, value: data[i].PatientId };
                        //}
                    }
                });
                console.log(customer);
                response(customer);
            },
            select: function (event, ui) {
                //fill selected customer details on form
                //$.ajax({
                //    cache: false,
                //    async: false,
                //    type: "POST",
                //    url: "@(Url.Action("GetDetail", " Home"))",
                //    data: { "id": ui.item.Id },

                //    success: function (data) {
                //        $('#VisitorDetail').show();
                //        $("#Name").html(data.VisitorName)
                //        $("#PatientName").html(data.PatientName)
                //        //alert(data.ArrivingTime.Hours)
                //        $("#VisitorId").val(data.VisitorId)
                //        $("#Date").html(data.Date)
                //        $("#ArrivingTime").html(data.ArrivingTime)
                //        $("#OverTime").html(data.OverTime)

                //        action = data.Action;
                //    },
                //    error: function (xhr, ajaxOptions, thrownError) {
                //        alert('Failed to retrieve states.');
                //    }
                //});
                console.log(ui);
            }
        });
        $('#btnHistory').off('click').on('click', function () {
            var patientId = parseInt($('#ddlPatients').val());
            $.ajax({
                url: '/LabTestResult/SaveRefPatientId',
                dataType: 'json',
                type: 'post',
                data: { patienId: patientId },
                success: function (res) {
                    window.open("/labtestresult/index", "_blank");
                }
            });

        });
        $('.btn-viewResult').off('click').on('click', function () {
            var code = $(this).data('id');
            $('#txtResultCodeView').val(code)
            $('#hiddenFormView').submit();

        });
        $('#btnClose').off('click').on('click', function () {

            $('#txtCode').val('');
        });
        $('#btnSaveResult').off('click').on('click', function () {
            var code = $('#txtAppCode').val();
            var con = $('#txtResult').val(); 
            var cmt = $('#txtCMTPT').froalaEditor('html.get');
            $.ajax({
                url: '/LabTest/UpdateResult',
                type: 'Post',
                dataType: 'json',
                data: { code: code, con: con,cmt:cmt },
                async: false,
                success: function (res) {
                    if (!res.success) {
                        toastr.success("Chẩn đoán không thành công.");

                    }
                    else {
                        toastr.success("Chẩn đoán thành công.");
                        homeController.loadDataLabTestingResult();
                    }
                }
            })
        });
        $('#btnSave').off('click').on('click', function () {
            
            var $this = $(this);
            $this.button('loading');
            var labTestDetails = [];
            var patientId = $('#ddlPatients').val();
            var allTextboxs = $('.txtLabTestDetail');
            $.each(allTextboxs, function (i, item) {
                var labTestItem = {};
                labTestItem.LabTestDetailId = $(item).data('id');
                labTestItem.Value = $(item).val();
                labTestItem.Name = $(item).data('name');
                labTestDetails.push(labTestItem);
            });
            console.log(labTestDetails);
            var labTestResult = {
                LabTestResultId: $('#txtLabTestResultId').val(),
                PatientId: patientId,
                Comment: $('#txtComment').val(),
                LabTestResultDetails: labTestDetails
            };
            if (labTestResult.LabTestResultId == 0) {
                $.ajax({
                    url: '/LabTestResult/AddLabTestResult',
                    type: 'Post',
                    dataType: 'json',
                    data: labTestResult,
                    success: function (res) {
                        if (!res.success) {
                            if (res.validation && res.validation.Errors) {
                                toastr.error(res.validation.Errors[0].ErrorMessage);
                            }

                        }
                        else {
                            toastr.success("Tạo mới thành công.");
                            $('.divExport').show();
                            $('#lblPopupTitle').text('Cập nhật thông tin bệnh nhân');
                            homeController.resetForm();
                            homeController.loadDetail(res.labTestResultId);
                            homeController.loadData();
                        }
                        $('#btnSave').button('reset');
                    }
                })
            } else {
                $.ajax({
                    url: '/LabTestResult/UpdateLabTestResult',
                    type: 'Post',
                    dataType: 'json',
                    data: labTestResult,
                    success: function (res) {
                        if (!res.success) {
                            toastr.error("Cập nhật không thành công");
                        }
                        else {
                            toastr.success("Cập nhật thành công.");
                            homeController.loadData();
                        }
                        $('#btnSave').button('reset');
                    }
                });
            }
            
          
        })
        $('.btn-printResult').off('click').on('click', function () {
            var code = $(this).data('id');
            $('#txtResultCode').val(code);
            $('#hiddenForm').submit();

        });
        $('#btnAddNew').off('click').on('click', function () {
            $('.divExport').hide();
            $('#lblPopupTitle').text('Thêm mới kết quả xét nghiệm');
            homeController.resetForm();
            $('#myModal').modal({ backdrop: 'static', keyboard: false });  
        });   
        $('.btn-edit').off('click').on('click', function () {
            $('.divExport').show();
            $('#lblPopupTitle').text('Cập nhật thông tin bệnh nhân');    
            $('#myModal').modal({ backdrop: 'static', keyboard: false });
            var id = $(this).data('id');
            homeController.resetForm();
            homeController.loadDetail(id);
        });
        $('.btn-editResult').off('click').on('click', function () {
            $('#lblPopupTitle').text('Cập nhật thông tin xét nghiệm');
            $('#myModalHistory').modal('hide');
            $('#myModal1').modal('show');
            var id = $(this).data('id');
            homeController.loadAppResult(id);
        });
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm("Bạn có chắc chắn muốn xóa không?", function (result) {
                if (result) {
                    homeController.deleteLabTestResult(id);
                }

            });
            
            
        });
        $("#txtSearchCode").off('change').on("change", function () {
            homeController.loadDataResultCode($('#txtSearchCode').val());
            //homeController.loadData(true);
        });
        $("#txtSearch").off('change').on("change", function () {
            homeController.loadData(true);
        })
    },
    deleteLabTestResult: function (id) {
        $.ajax({
            url: '/labtestResult/DeleteLabTest',
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
        });
    },
    loadDetail: function (id) {
        $.ajax({
            url: '/LabTestResult/GetLabTestResultDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    $('#patienDiv').hide();
                    $('#txtLabTestResultId').val(data.LabTestResultId);
                    $('#lbllabTestResultId').val(data.LabTestResultId);
                    console.log(data);
                    //$('#txtCode').val(data.PatientCode);
                    $('#ddlPatients').val(data.PatientId).change();                   
                    $('#ddlPatients').prop("disabled", true);
                    $('#ddlPatients').val(data.PatientId).change();
                    $('#ddlPatients').val(data.PatientId).change();
                    //$('#txtPhoneNumber').val(data.PhoneNumber);
                    var labTestResultDetails = data.LabTestResultDetails;
                    $('#txtComment').val(data.Comment);
                    $.each(labTestResultDetails, function (i, item) {
                        if (item.IsCombobox) {
                            $('.txtLabTestDetail[data-id="' + item.LabTestDetailId + '"]').val(item.Value).change();
                        }
                        else {
                            $('.txtLabTestDetail[data-id="' + item.LabTestDetailId + '"]').val(item.Value);
                        }
                        
                    });
                }
                else {
                    bootbox.alert(response.message);
                }
            },
        });
    },
    resetForm: function () {
        $('#txtLabTestResultId').val('0');
        $('#ddlGender').val('').change();
        $('#txtPhoneNumber').val('');
        $('#txtPatientName').val('');
        $('#txtHomeAddress').val('');
        $('.txtLabTestDetail').val('');
        $('#txtComment').val('');
        $('#ddlPatients').prop("disabled", false);
        $('#patienDiv').show();
        $('.txtLabTestDetail.dropdown').val('').change();
    },
    loadAppResult: function (id, changePageSize) {
        $.ajax({
            url: '/appointment/AppDetail',
            type: 'GET',
            dataType: 'json',
            data: { app: id },
            success: function (response) {
                if (response.sucess) {
                    var data = response.data;
                    $('#txtResultxx').val(data.Conclusion);
                    $('#txtAppCodexx').val(data.AppointmentCode);
                    $('#txtCMTPT').froalaEditor('html.set', data.DoctorComment);
                
                }
            }
        })
    },
    loadDataResult: function (id,changePageSize) {
        $.ajax({
            url: '/Patient/GetAllResultsNoPaging',
            type: 'GET',
            dataType: 'json',
            data: {
                id: id
            },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#dataLabTestingResult-template').html();
                    $.each(data, function (i, item) {
                        $('#txtResult').val(item.Conclusion);
                        $('#txtAppCode').val(item.AppointmentCode);
                        html += Mustache.render(template, {
                            LabTestingId: item.LabTestingId,
                            Name: item.DateResult,
                            Status: item.Status,
                            Getting: item.AppointmentCode,
                            Group: item.Conclusion,
                        });

                    });
                    $('#tblDataLabTestingResult').html(html);
                    homeController.paging(response.total, function () {
                        homeController.loadDataLabTesting();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        })
    },
    loadDataResultCode: function (id) {
        $.ajax({
            url: '/Patient/GetPatientByCode',
            type: 'GET',
            dataType: 'json',
            data: { code: id},
            success: function (response) {
                var data = response.data;
                $('#txtSearch').val(data.PatientName); homeController.loadData(true);
            
            }
        })
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/LabTestResult/GetAllLabTest',
            type: 'POST',
            dataType: 'json',
            data: {
                searchDto: {
                    PageIndex: homeconfig.pageIndex,
                    PageSize: homeconfig.pageSize,
                    PatientId: $('#ddlPatientChoose').val()
                }
            },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            LabTestResultId: item.LabTestResultId,
                            PatientId: item.PatientId,
                            PatientCode: item.PatientCode,
                            FullName: item.PatientName,
                            PhoneNumber: item.PhoneNumber,
                            HomeAddress: item.Address,
                            Comment: item.Comment,
                            CreatedDate: item.CreatedDate,
                            Age : item.Age
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
//    loadAllLabTestType: function () {
//        $.ajax({
//            url: '/LabTestResult/GetAllLabTestType',
//            type: 'GET',
//            dataType: 'json',
         
//            success: function (response) {
//                if (response.success) {
//                    var data = response.data;
//                    homeconfig.allLabTestType = data;
//                    $.each(data, function (i, item) {
//                        var newColumn = null;
//                        if (item.LabTestTypeId === 1) {
//                            for (var j = 0; j < item.LabTestDetails.length; j++) {
//                                newColumn = $('#template').clone();
//                                $(newColumn).removeAttr('style');
//                                $(newColumn).removeAttr('id');
//                                $(newColumn).addClass('customize-column');
//                                $(newColumn).find('.txtValue').data('id', item.LabTestDetails[j].LabTestDetailId);
//                                $(newColumn).find('.txtValue').data('name', item.LabTestDetails[j].Name);
//                                $(newColumn).find('.lblTitle').text(item.LabTestDetails[j].Name + ":");
//                                $(newColumn).find('.lblUnit').text(item.LabTestDetails[j].Unit);
//                                $(newColumn).insertBefore('#template');

//                            }
//                        } else if (item.LabTestTypeId === 2) {
//                            for (var j = 0; j < item.LabTestDetails.length; j++) {
//                                newColumn = $('#templateNuocTieu').clone();
//                                $(newColumn).removeAttr('style');
//                                $(newColumn).removeAttr('id');
//                                $(newColumn).addClass('customize-column');
//                                $(newColumn).find('.txtValue').data('id', item.LabTestDetails[j].LabTestDetailId);
//                                $(newColumn).find('.txtValue').data('name', item.LabTestDetails[j].Name);
//                                $(newColumn).find('.lblTitle').text(item.LabTestDetails[j].Name + ":");
//                                $(newColumn).find('.lblUnit').text(item.LabTestDetails[j].Unit);
//                                $(newColumn).insertBefore('#templateNuocTieu');
//                            }
//                        } else {
//                            for (var j = 0; j < item.LabTestDetails.length; j++) {
//                                newColumn = $('#templateSinhHoa').clone();
//                                $(newColumn).removeAttr('style');
//                                $(newColumn).removeAttr('id');
//                                $(newColumn).addClass('customize-column');
//                                $(newColumn).find('.txtValue').data('id', item.LabTestDetails[j].LabTestDetailId);
//                                $(newColumn).find('.txtValue').data('name', item.LabTestDetails[j].Name);
//                                $(newColumn).find('.lblTitle').text(item.LabTestDetails[j].Name + ":");
//                                $(newColumn).find('.lblUnit').text(item.LabTestDetails[j].Unit);
//                                $(newColumn).insertBefore('#templateSinhHoa');
//                            }
//}

//                    });
//                }
//            }
//        })
//    },
    loadAllPatients: function () {
        $.ajax({
            url: '/Patient/AutoCompletet',
            type: 'POST',
            dataType: 'json',

            success: function (response) {
                var data = response;
                var html = '<option data-subtext="Chọn bệnh nhân ">-- Chọn bệnh nhân --</option>';
                $.each(response, function (i, item) {
                    html += '<option data-subtext=" ' + item.HomeAddress  + ' " value="' + item.PatientId + '">' + item.FullName +  '</option>';
                });
                console.log(html);
                $('#ddlPatients').html(html);
            }
        });
    }
}   
homeController.init();