var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
}
var homeController = {
    init: function () {
        var paId = 0;
        var dateNow = homeController.formatDate(new Date());
        document.getElementById("select-date").value = dateNow;
        homeController.loadData();
        homeController.registerEvent();
    },

    checkIsPaid: function (PatientId) {
        homeController.loadPrice(PatientId);
        var modalConfirm = function (callback) {

            $("#mi-modal").modal('show');

            $("#modal-btn-si").on("click", function () {
                callback(true);
                $("#mi-modal").modal('hide');
            });

            $("#modal-btn-no").on("click", function () {
                callback(false);
                $("#mi-modal").modal('hide');
            });
        };

        modalConfirm(function (confirm) {
            if (confirm) {
                homeController.ChangeIsPaid(PatientId);
            }
            else {
                homeController.loadData();
            }
        });
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

        $("#select-sample").off('change').on('change', function () {
            homeController.loadData();
        });
        
        $("#select-date").off('change').on('change', function () {
            homeController.loadData();
        });

        $('#btnSearch').off('click').on('click', function () {
            homeController.loadData(true);
        });

        $("#txtSearch").off('change').on("change", function () {
            homeController.loadData(true);
        })
        
        $('.btn-View').off('click').on('click', function () {
            var id = $(this).data('id');
            paId = id;
            homeController.loadDataByPatientId(id);
            $('#lblPopupTitle').text('Thông tin các cuộc hẹn');
            $('#myModal1').modal('show');
        });

        $('.btn-deleteSG').off('click').on('click', function () {
            var id = $(this).data('id');
            homeController.deleteSampleGetting(id);
            

        });
    },
    
    //change page
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

   
    loadData: function (changePageSize) {
        var selectedSample = $("#select-sample").children("option:selected").val();
        var selectDate = $("#select-date").val();
        var searchData = $("#txtSearch").val();
        $.ajax({
            url: '/receptionist/GetPatientByDate',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, sampleId: selectedSample, date: selectDate, search: searchData },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    searchData = data.PatientName;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            OrderNumber: i+1,
                            FullName: item.PatientName,
                            PatientID: item.PatientID,
                            //ID: item.IdentityCardNumber,
                            Phone: item.Phone,
                            Address: item.Address,
                            DOBirth: item.DateOfBirth,
                            IsPaid: item.IsPaid,
                            ReadOnly: (item.IsPaid === true) ? "return false;" : "",
                            Checked: (item.IsPaid === true) ? "checked" : ""
                        });

                    });
                    console.log(html);
                    $('#tblData').html(html);
                    homeController.paging(response.total, function () {
                        //homeController.loadDataBySample();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        })
    },

    //ispaid
    ChangeIsPaid: function (PatientId) {
        var selectDate = $("#select-date").val();
        $.ajax({
            url: '/receptionist/IsPaid',
            type: 'POST',
            dataType: 'json',
            data: { patientId: PatientId, date: selectDate  },
            success: function (response) {
                //                homeController.loadPrice(SampleGettingId);
                if (response.success === true) {
                    //toastr.success('Thanh toán hoàn tất');
                    //homeController.loadDataBySample();
                    homeController.loadData();
                }
            }
        })
    },

    //load data in modal
    loadDataByPatientId: function (id, changePageSize) {
        var selectDate = $("#select-date").val();
        $.ajax({
            url: '/receptionist/GetAppByPatientId',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, patientId: id, date: selectDate },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template1').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            OrderNum: item.OrderNumber,
                            SgId: item.SampleGettingId,
                            SampleName: item.SampleName,
                            //LabTestName: item.LabTestName,
                            StartTime: item.StartTime,
                            EnterDate: item.EnterDate,
                            EnterTime: item.EnterTime,
                            Status: item.Status,
                        });

                    });
                    console.log(html);
                    $('#tblData1').html(html);
                    homeController.paging(response.total, function () {
                        //homeController.loadDataBySample();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        })
    },

    //load giá
    loadPrice: function (id, changePageSize) {
        var selectDate = $("#select-date").val();
        $.ajax({
            url: '/receptionist/GetPriceByPatient',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, patientId: id, date: selectDate },
            success: function (response) {
                //debugger;
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template2 = $('#data-template2').html();
                    var template3 = $('#data-template3').html();
                    $.each(data.PriceListItemDto, function (i, item) {
                        html += Mustache.render(template2, {
                            OrderNumber: i + 1,
                            LabtestName: item.LabtestName,
                            Price: item.Price,
                        });
                    });
                    html += Mustache.render(template3, {
                        TotalPrice: data.TotalPrice
                    });
                    console.log(html);
                    $('#tblPriceData').html(html);
                    homeController.paging(response.total, function () {
                        //homeController.loadDataBySample();
                    }, changePageSize);
                    homeController.registerEvent();
                }
            }
        })
    },

    
    deleteSampleGetting: function (id) {
        try {
            $.ajax({
                url: '/receptionist/DeleteSampleGetting',
                data: { sgId: id },
                async: false,
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.success == true) {
                        //toastr.success("Xóa thành công.");
                        homeController.loadDataByPatientId(paId);
                    }
                    else {
                        toastr.error("Không thể xóa cuộc hẹn đã thanh toán");
                    }
                }
            });
        }
        catch (err) {
            alert(err);
        }
    },
}
homeController.init();