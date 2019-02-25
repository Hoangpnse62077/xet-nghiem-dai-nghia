var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
}
var homeController = {
    init: function () {
        homeController.loadData();
        homeController.registerEvent();
    },
    registerEvent: function () {
        $('#btnHistory').off('click').on('click', function () {
            var patientId = parseInt($('#txtPatientId').val());
            $.ajax({
                url: '/LabTestResult/SaveRefPatientId',
                dataType: 'json',
                type: 'post',
                data: { patienId: patientId},
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
            var code = $('#txtCode').val();
            var accountId = parseInt($('#txtAccountId').val());
            var name = $('#txtName').val();
            var gender = $('#ddlGender').val();
            var patientId = parseInt($('#txtPatientId').val());
            var phone = $('#txtPhoneNumber').val();
            var homeAddress = $('#txtHomeAddress').val();
            var companyAddress = $('#txtCompanyAddress').val();
            var cmnd = $('#txtCmnd').val();
            var date = $('#txtDate').val();
            var isDeleted = "False";
            var patient = {
                PatientId: patientId,
                PatientCode: code,
                AccountId: accountId,
                FullName: name,
                Gender: gender,
                DateOfBirth: date,
                PhoneNumber: phone,
                HomeAddress: homeAddress,
                CompanyAddress: companyAddress,
                IdentityCardNumber: cmnd,
                IsDeleted: isDeleted,
                AvatarUrl: $('#avatar').attr('src'),
                Age: date
            };
            if (patient.PatientId == 0) {
                $.ajax({
                    url: '/Patient/AddPatient',
                    type: 'Post',
                    dataType: 'json',
                    data: patient,
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
                            location.reload();
                        }
                    }
                })
            } else {
                $.ajax({
                    url: '/Patient/UpdatePatient',
                    type: 'Post',
                    dataType: 'json',
                    data: patient,
                    success: function (res) {
                        if (!res.sucess) {
                           
                                toastr.error("Cập nhật không thành công");
                            

                        }
                        else {
                            toastr.success("Cập nhật thành công.");
                            $('#myModal').modal('hide');                            
                            location.reload();
                            homeController.loadData();
                        }
                    }
                })
            }
          
        })
        $('.btn-printResult').off('click').on('click', function () {
            var code = $(this).data('id');
            $('#txtResultCode').val(code)
            $('#hiddenForm').submit();

        });
        $('#btnAddNew').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thêm mới bệnh nhân');
            homeController.resetForm();
            $('#myModal').modal('show');
        });   
        $('.btn-edit').off('click').on('click', function () {
            $('#lblPopupTitle').text('Cập nhật thông tin bệnh nhân');    
            $('#myModal').modal('show');
            var id = $(this).data('id');
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
            homeController.deletePatient(id);
            
        });
        $("#txtSearchCode").off('change').on("change", function () {
            homeController.loadDataResultCode($('#txtSearchCode').val());
            //homeController.loadData(true);
        });
        $("#txtSearch").off('change').on("change", function () {
            homeController.loadData(true);
        })
    },
    deletePatient: function (id) {
        $.ajax({
            url: '/Patient/DeletePatient',
            data: {
                patientId: id
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
            url: '/Patient/PatientDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.sucess) {
                    var data = response.data;
                    $('#txtPatientId').val(data.PatientId);
                    $('#txtAccountId').val(data.AccountId);
                    $('#txtCode').val(data.PatientCode);
                    $('#txtName').val(data.FullName);
                    $('#txtDate').val(data.Age);
                    $('#ddlGender').val(data.Gender).change();
                    $('#txtPhoneNumber').val(data.PhoneNumber.trim());
                    $('#txtHomeAddress').val(data.HomeAddress);
                    $('#txtCompanyAddress').val(data.CompanyAddress);
                    $('#txtCmnd').val(data.IdentityCardNumber);
                    $('#avatar').attr('src', data.Avatar);
                }
                else {
                    bootbox.alert(response.message);
                }
            },
        });
    },
    resetForm: function () {
        $('#txtPatientId').val('0');
        $('#txtAccountId').val('');
        $('#txtCmnd').val('');
        $('#txtName').val('');
        $('#txtDate').val('');
        $('#ddlGender').val('').change();
        $('#txtPhoneNumber').val('');
        $('#txtHomeAddress').val('');
        $('#txtCompanyAddress').val('');
        $('#avatar').attr('src', '');
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
            url: '/Patient/GetAllPatients',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, phoneNumber: $('#txtSearch').val() },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            PatientId: item.PatientId,
                            PatientCode: item.PatientCode,
                            FullName: item.FullName,
                            PhoneNumber: item.PhoneNumber,
                            HomeAddress: item.HomeAddress,
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
    }
}
homeController.init();