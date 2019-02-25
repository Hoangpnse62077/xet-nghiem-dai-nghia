var CONFIG = {
    pageSize: 10,
    pageIndex: 1,
    //PatientId: 145
};

var Controller = {
    init: function () {
        Controller.loadData();
        Controller.registerEvent();
    },
    registerEvent: function () {

        toastr.options = {
            "debug": false,
            "positionClass": "toast-bottom-left",
            "onclick": null,
            "fadeIn": 300,
            "fadeOut": 1000,
            "timeOut": 5000,
            "extendedTimeOut": 1000
        }

        $('#btn-search').off('click').on('click', function () {
            Controller.loadData();
        })
        
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            bootbox.confirm({
                message: "Bạn có chắc chắn muốn xóa cuộc hẹn này?",
                buttons: {
                    confirm: {
                        label: "Có"
                    },
                    cancel: {
                        label: "Không"
                    }
                },
                callback: function (result) {
                    if (result) {
                        Controller.deleteAppointment(id);
                    }
                }
            });
        });

        $("#chkNew").off('change').on("change", function () {
            Controller.loadData(true);
        })
        $("#chkProcess").off('change').on("change", function () {
            Controller.loadData(true);
        })
        $("#chkDone").off('change').on("change", function () {
            Controller.loadData(true);
        })

    },
    deleteAppointment: function (id) {
        $.ajax({
            url: '/api/appointment/delete-appointment?appointmentId=' + id,
            type: 'DELETE',
            success: function (response) {
                if (response.Success == true) {
                    toastr["success"]("Hủy lịch thành công.");
                    Controller.loadData(true);
                }
                else {
                    toastr["error"](response.Message);
                }
            },
            error: function (err) {
                toastr["error"](err);
            }
        });
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
        var sttNew = $("#chkNew").prop("checked");
        var sttProcess = $("#chkProcess").prop("checked");
        var sttDone = $("#chkDone").prop("checked");
        if (sttNew == false && sttProcess == false && sttDone == false) {
            sttNew = sttProcess = sttDone = true;
        }
        $.ajax({
            //url: '/api/appointment/get-appointments-by-patient-id?patientId=' + CONFIG.PatientId,
            url: '/UserWeb/Appointment/GetAppointmentsByAccountId',
            type: 'GET',
            dataType: 'json',
            data: {
                accountId: accountId,
                page: CONFIG.pageIndex,
                pageSize: CONFIG.pageSize,
                sttNew: sttNew,
                sttProcess: sttProcess,
                sttDone: sttDone,
            },
            success: function (response) {
                //console.log(response);
                if (response.success) {
                    var hasData = response.data.length > 0;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(response.data, function (index, item) {
                        var totalPrice = 0;
                        var labTests = [];
                        for (var i = 0; i < item.SampleGettingDtos.length; i++) {
                            var sampleGetting = item.SampleGettingDtos[i];
                            //var sDate = sampleGetting.GettingDate;
                            //sampleGetting.FmGettingDate = moment(sDate, 'yyyy-MM-dd').format('dd-MM-yyyy');
                            for (var j = 0; j < sampleGetting.LabTests.length; j++) {
                                var labTest = sampleGetting.LabTests[j];
                                labTest.sPrice = labTest.Price.toLocaleString("VN");
                                labTests.push(labTest);
                                totalPrice += parseInt(labTest.Price);
                            }
                        }
                        html += Mustache.render(template, {
                            Index: (index + 1),
                            PatientName: item.PatientName,
                            AppointmentId: item.AppointmentId,
                            AppointmentCode: item.AppointmentCode,
                            SampleGettings: item.SampleGettingDtos,
                            LabTests: labTests,
                            TotalPrice: totalPrice.toLocaleString("VN"),
                            IsNew: item.Status == "NEW",
                            IsProcess: item.Status != "NEW" && item.Status != "DOCTORDONE",
                            IsDone: item.Status == "DOCTORDONE"
                        });
                    });
                    //console.log(html);
                    $('#tblData').html(html);

                    var patient = response.patient;
                    //$('#patient-name').html(patient.FullName);
                    //$('#patient-dob').html(response.patientDob);
                    //$('#patient-phone').html(patient.PhoneNumber);

                    if (hasData) {
                        $('#content-result').show(0);
                        $('#content-nodata').hide(0);
                    } else {
                        $('#content-result').hide(0);
                        $('#content-nodata').show(0);
                    }

                    Controller.paging(response.total, function () {
                        Controller.loadData();
                    }, changePageSize);
                    Controller.registerEvent();
                } // end if success
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / CONFIG.pageSize);

        //Unbind pagination if it existed or click change pagesize
        if ($('.div-pagination a').length === 0 || changePageSize === true) {
            $('.div-pagination').empty();
            $('.div-pagination').removeData("twbs-pagination");
            $('.div-pagination').unbind("page");
        }

        $('.div-pagination').twbsPagination({
            totalPages: totalPage,
            first: "Đầu",
            next: "Tiếp",
            last: "Cuối",
            prev: "Trước",
            visiblePages: 10,
            onPageClick: function (event, page) {
                CONFIG.pageIndex = page;
                setTimeout(callback, 200);
            }
        });
    }
}

Controller.init();