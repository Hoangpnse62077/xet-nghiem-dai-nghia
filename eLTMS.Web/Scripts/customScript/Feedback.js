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
        $('#btnSave').off('click').on('click', function () {     
            var feedbackId = $('#txtFeedbackId').val();
            var employeeName = $('#txtEmployeeName').val();
            var patientName = $('#txtPatientName').val();
            var context = $('#txtContext').val();
            var day = $('#txtDay').val();
            var status = $('#ddlStatus').val();
            var isDeleted = "False";
            var feedback = {
                FeedbackId: feedbackId,          
                Employee: {
                    FullName: $('#txtEmployeeName').val()
                },
                Content: $('#txtContext').val(),
                ReceivedDateTime: $('#txtDay').val(),
                Status: $('#ddlStatus').val()
            }
            console.log(feedback);
            if (feedback.FeedbackId == 0) {
                $.ajax({
                    url: '/Feedback/AddFeedback',
                    type: 'Post',
                    dataType: 'json',
                    data: feedback,
                    success: function (res) {
                        if (!res.sucess) { //200
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
                    url: '/Feedback/UpdateFeedback',
                    type: 'Post',
                    dataType: 'json',
                    data: feedback,
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
        $('#btnAddNew').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thêm mới feedback');
            homeController.resetForm();
            $('#myModal').modal('show');
        });
        $('#btnSearch').off('click').on('click', function () {
            homeController.loadData(true);
        });
        $('#btnReset').off('click').on('click', function () {
            $('#txtNameS').val('');
            $('#ddlStatusS').val('');
            homeController.loadData(true);
        });
        $('.btn-edit').off('click').on('click', function () {
            $('#lblPopupTitle').text('Cập nhật Feedback');
            $('#myModal').modal('show');
            var id = $(this).data('id');
            homeController.loadDetail(id);
        });
        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            homeController.deleteFeedback(id);

        });
        $("#txtSearch").off('change').on("change", function () {
            homeController.loadData(true);
        })
    },
    deleteFeedback: function (id) {
        $.ajax({
            url: '/Feedback/DeleteFeedback',
            data: {
                FeedbackId: id
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
            url: '/Feedback/FeedbackDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.sucess) {
                    var data = response.data;
                    $('#txtFeedbackId').val(data.FeedbackId);     
                    $('#ddlStatus').val(data.Status).change();
                    $('#txtPatientName').val(data.PatientName);
                    $('#txtEmployeeName').val(data.EmployeeName);
                    $('#txtContext').val(data.Content);
                    $('#txtDay').val(data.ReceivedDateTime);         
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
    saveData: function () {

    },
    resetForm: function () {
        $('#txtFeedbackId').val(data.FeedbackId);
        $('#ddlStatus').val(data.Status).change();
        $('#txtPatientName').val(data.PatientName);
        $('#txtEmployeeName').val(data.EmployeeName);
        $('#txtContext').val(data.Content);
        $('#txtDay').val(data.ReceivedDateTime);   
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/Feedback/GetAllFeed',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, dateTime: $('#txtSearch').val() },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    console.log(data);
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            FeedbackId: item.FeedbackId,
                            EmployeeName: item.EmployeeName,
                            PatientName: item.PatientName,
                            Status: item.Status,
                            ReceiveDateTime: item.ReceivedDateTime

                        });

                    });
                    console.log(html);
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