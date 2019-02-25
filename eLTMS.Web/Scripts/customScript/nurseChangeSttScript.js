var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
}
var homeController = {
    init: function () {
        var dateNow = homeController.formatDate(new Date());
        document.getElementById("select-date").value = dateNow;
        homeController.loadData();
        homeController.registerEvent();
    },

    checkIsGot: function (SampleGettingId) {

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
                homeController.ChangeIsGot(SampleGettingId);
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

        $("#select-sample").change(function () {
            homeController.loadData();
        });
        
        $("#select-date").change(function () {
            homeController.loadData();
        });

        $('#btnSearch').off('click').on('click', function () {
            homeController.loadData(true);
        });

        $("#txtSearch").off('change').on("change", function () {
            homeController.loadData(true);
        })
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
            url: '/nurse/GetAppBySample',
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
                            SgId: item.SampleGettingId,
                            DOB: item.DateOfBirth,
                            LtIds: item.LabTestingIds,
                            FullName: item.PatientName,
                            SampleName: item.SampleName,
                            StartTime: item.StartTime,
                            Date: item.Date,
                            IsGot: item.IsGot,
                            ReadOnly: (item.IsGot === true) ? "return false;" : "",
                            Checked: (item.IsGot === true) ?  "checked" : ""
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

    //change IsGot
    ChangeIsGot: function (SampleGettingId) {
        $.ajax({
            url: '/nurse/IsGot',
            type: 'POST',
            dataType: 'json',
            data: { sampleGettingId: SampleGettingId },
            success: function (response) {
                if (response.success === true) {
                    //toastr.success('Đổi trạng thái thành công');
                    homeController.loadData();
                }
                
            }
        })
    }
}
homeController.init();