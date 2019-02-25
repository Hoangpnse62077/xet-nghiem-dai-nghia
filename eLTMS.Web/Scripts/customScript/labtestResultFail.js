var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
    allLabTesting: [],
    ImportExcel: false, 
    LoadFromDataBase: false
}
var homeController = {
    init: function () {
        homeController.registerEvent();
        homeController.loadDataLabTestingResultFail();
    },
    formatDate: function (date) {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [year, month, day].join('-');
    },
    registerEvent: function () {
        $('.btn-closeLabTestingResultFail').off('click').on('click', function () {
            var id = $(this).data('id');
            homeController.deleteLabtesting(id);

        });
        $('.btn-viewResult').off('click').on('click', function () {
            var code = $(this).data('id');
            $('#txtResultCodeView').val(code)
            $('#hiddenFormView').submit();

        });
        $('#btnSaveResultLT').off('click').on('click', function () {
            var code = $('#txtAppCodeLT').val();
            var con = $('#txtResultLT').val();
            var cmt = $('#txtCMTLT').froalaEditor('html.get');
            $.ajax({
                url: '/LabTest/UpdateResult',
                type: 'Post',
                dataType: 'json',
                data: { code: code, con: con, cmt: cmt },
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
        $('.btn-printResult').off('click').on('click', function () {
            var code = $(this).data('id');
            $('#txtResultCode').val(code)
            $('#hiddenForm').submit();

        });
        $("#txtSearchDateResult").change(function () {
            homeController.loadData();
        });
        $('.btn-editResult').off('click').on('click', function () {
            $('#lblPopupTitle').text('Cập nhật thông tin xét nghiệm');
             $('#myModal1').modal('show');
            var id = $(this).data('id');
            homeController.loadDataResult(id);
        });
    },
    deleteLabtesting: function (id) {
        try {
            $.ajax({
                url: '/LabTest/DeleteLabtesting',
                data: {
                    id: id
                },
                async: false,
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.success == true) {
                        toastr.success("Xóa thành công.");
                        location.reload();
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
    checkIsGot: function (SampleGettingId) {

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
                homeController.ChangeIsPaid(SampleGettingId);
            }
            else {
                homeController.loadDataLabTestingResultFail();
            }
        });

    },
    loadDataResult: function (id, changePageSize) {
        $.ajax({
            url: '/appointment/AppDetail',
            type: 'GET',
            dataType: 'json',
            data: { app: id},
            success: function (response) {
                if (response.sucess) {
                    var data = response.data;
                    $('#txtResultLT').val(data.Conclusion);
                    $('#txtAppCodeLT').val(data.AppointmentCode);
                    //$('#txtCMTLT').val(data.DoctorComment);
                    $('#txtCMTLT').froalaEditor('html.set', data.DoctorComment);
                }
            }
        })
    },
    ChangeIsPaid: function (SampleGettingId) {
        $.ajax({
            url: '/cashier/IsPaid',
            type: 'POST',
            dataType: 'json',
            data: { sampleGettingId: SampleGettingId },
            success: function (response) {
               if (response.success === true) {
                    toastr.success('Đổi trạng thái thành công');
                }

            }
        })
    },
    loadDataLabTestingResultFail: function (changePageSize) {
        $.ajax({
            url: '/LabTest/GetAllLabTestingsFail',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#dataLabTestingResultFail-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            LabtestingID: item.LabTestingId,
                            SampleGettingID: item.SampleGettingId,
                            Name: item.PatientName,
                            Phone: item.PatientPhone,
                            Code: item.AppointmentCode,
                            Sample: item.LabTestName,
                            Price: item.Price,
                        });

                    });
                    $('#tblDataLabTestingResultFail').html(html);
                    homeController.registerEvent();
                }
            }
        })
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/LabTest/GetAllResult',
            type: 'GET',
            dataType: 'json',
            data: { s: $('#txtSearchDateResult').val(), page: homeconfig.pageIndex, pageSize: homeconfig.pageSize },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#dataLabTestingResult-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            LabTestingId: item.LabTestingId,
                            DOB: item.PDOB,
                            Pname: item.PatientName,
                            Phone: item.PatientPhone,
                            GetA: item.GetApp,
                            ReR: item.ReturnRe,
                            Getting: item.AppointmentCode,
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
 
}

homeController.init();