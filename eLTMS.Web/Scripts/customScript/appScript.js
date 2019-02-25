var homeconfig = {
    pageSize: 10,
    pageIndex: 1,
}
var homeController = {
    init: function () {
        //homeController.loadData();
        var dateNow = homeController.formatDate(new Date());
        document.getElementById("select-date").value = dateNow;
        homeController.loadData();
        homeController.registerEvent();
    },

    checkIsPaid: function (SampleGettingId) {
        homeController.loadPrice(SampleGettingId);
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
                homeController.ChangeIsPaid(SampleGettingId);
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

        $('#btnSave').off('click').on('click', function () {
            var name = $('#txtName').val();
            var address = $('#txtAddress').val();
            var phone = $('#txtPhone').val();
            var dateOfBirth = $('#txtDateOfBirth').val();
            //var cmnd = $('#txtCMND').val();
            var gender = $('#ddlGender').val();
            var mau = false;
            var nuocTieu = false;
            var teBaoHoc = false;
            var phan = false;
            var dich = false;

     
            var labTests = [];
            $('#mauCheckGroup input[type=checkbox]:checked').each(function (index, element) {
                var dataLT = $(element).data('labtest-id');
                if (dataLT == '') {
                    dataLT = -1;
                }
                var dataSP = $(element).data('sample-id');
                if (dataSP == '') {
                    dataSP = -1;
                }
                var labtestId = parseInt(dataLT);
                var sampleId = parseInt(dataSP);
                labTests.push({
                    LabTestId: labtestId,
                    SampleId: sampleId
                });
            });
            $('#nuocTieuCheckGroup input[type=checkbox]:checked').each(function (index, element) {
                var dataLT = $(element).data('labtest-id');
                if (dataLT == '') {
                    dataLT = -1;
                }
                var dataSP = $(element).data('sample-id');
                if (dataSP == '') {
                    dataSP = -1;
                }
                var labtestId = parseInt(dataLT);
                var sampleId = parseInt(dataSP);
                labTests.push({
                    LabTestId: labtestId,
                    SampleId: sampleId
                });
            });
            $('#teBaoHocCheckGroup input[type=checkbox]:checked').each(function (index, element) {
                var dataLT = $(element).data('labtest-id');
                if (dataLT == '') {
                    dataLT = -1;
                }
                var dataSP = $(element).data('sample-id');
                if (dataSP == '') {
                    dataSP = -1;
                }
                var labtestId = parseInt(dataLT);
                var sampleId = parseInt(dataSP);
                labTests.push({
                    LabTestId: labtestId,
                    SampleId: sampleId
                });
            });
            $('#phanCheckGroup input[type=checkbox]:checked').each(function (index, element) {
                var dataLT = $(element).data('labtest-id');
                if (dataLT == '') {
                    dataLT = -1;
                }
                var dataSP = $(element).data('sample-id');
                if (dataSP == '') {
                    dataSP = -1;
                }
                var labtestId = parseInt(dataLT);
                var sampleId = parseInt(dataSP);
                labTests.push({
                    LabTestId: labtestId,
                    SampleId: sampleId
                });
            });
            $('#dichCheckGroup input[type=checkbox]:checked').each(function (index, element) {
                var dataLT = $(element).data('labtest-id');
                if (dataLT == '') {
                    dataLT = -1;
                }
                var dataSP = $(element).data('sample-id');
                if (dataSP == '') {
                    dataSP = -1;
                }
                var labtestId = parseInt(dataLT);
                var sampleId = parseInt(dataSP);
                labTests.push({
                    LabTestId: labtestId,
                    SampleId: sampleId
                });
            });
            console.log(labTests);

            if ($('#checkBox_loaiXetNghiem1').prop('checked') === true || $('#checkBox_loaiXetNghiem2').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem3').prop('checked') === true
                || $('#checkBox_loaiXetNghiem4').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem5').prop('checked') === true || $('#checkBox_loaiXetNghiem6').prop('checked') === true
                || $('#checkBox_loaiXetNghiem7').prop('checked') === true || $('#checkBox_loaiXetNghiem8').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem9').prop('checked') === true
                || $('#checkBox_loaiXetNghiem10').prop('checked') === true || $('#checkBox_loaiXetNghiem11').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem12').prop('checked') === true
                || $('#checkBox_loaiXetNghiem13').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem14').prop('checked') === true || $('#checkBox_loaiXetNghiem15').prop('checked') === true
                || $('#checkBox_loaiXetNghiem16').prop('checked') === true || $('#checkBox_loaiXetNghiem17').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem18').prop('checked') === true
                || $('#checkBox_loaiXetNghiem19').prop('checked') === true || $('#checkBox_loaiXetNghiem20').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem21').prop('checked') === true
                || $('#checkBox_loaiXetNghiem22').prop('checked') === true)
                mau = true;
            if ($('#checkBox_loaiXetNghiem23').prop('checked') === true || $('#checkBox_loaiXetNghiem24').prop('checked') === true)
                nuocTieu = true;
            if ($('#checkBox_loaiXetNghiem25').prop('checked') === true || $('#checkBox_loaiXetNghiem26').prop('checked') === true)
                teBaoHoc = true;
            if ($('#checkBox_loaiXetNghiem27').prop('checked') === true || $('#checkBox_loaiXetNghiem28').prop('checked') === true)
                phan = true;
            if ($('#checkBox_loaiXetNghiem29').prop('checked') === true || $('#checkBox_loaiXetNghiem30').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem31').prop('checked') === true
                || $('#checkBox_loaiXetNghiem32').prop('checked') === true || $('#checkBox_loaiXetNghiem33').prop('checked') === true ||
                $('#checkBox_loaiXetNghiem34').prop('checked') === true)
                dich = true;
            //validate
            var valid = true;

            var $name = $('#step-0 [name=txtName] ~ .valid-mess');
            $name.html("");
            if (name === "") {
                $name.html('Vui lòng điền đầy đủ họ tên');
                valid = false;
                if (name.length > 100) {
                    $name.html('Họ tên dài tối đa 100 ký tự');
                    valid = false;
                }
            }

            var $dob = $('#step-0 [name=txtDateOfBirth] ~ .valid-mess');
            $dob.html("");
            if (dateOfBirth === "") {
                $dob.html('Vui lòng điền ngày sinh');
                valid = false;
            } 

            var $phone = $('#step-0 [name=txtPhone] ~ .valid-mess');
            $phone.html("");
            if (phone === "") {
                $phone.html('Vui lòng điền đầy đủ số điện thoại');
                valid = false;
            }
            else if (phone.length > 11) {
                $phone.html('Số điện thoại có độ dài tối đa 11 ký tự');
                valid = false;
            }

            var $address = $('#step-0 [name=txtAddress] ~ .valid-mess');
            $address.html("");
            if (address === "") {
                $address.html('Vui lòng điền đầy đủ địa chỉ');
                valid = false;
                if (address.length > 100) {
                    $address.html('Địa chỉ dài tối đa 100 ký tự');
                    valid = false;
                }
            } 
            
            if (valid) {
                var item = {
                    Name: name,
                    Phone: phone,
                    Address: address,
                    DateOfBirth: dateOfBirth,
                    Gender: gender,
                    Mau: mau,
                    NuocTieu: nuocTieu,
                    TeBaoHoc: teBaoHoc,
                    Phan: phan,
                    Dich: dich
                };
                $.ajax({
                    url: '/cashier/AddApp',
                    type: 'Post',
                    dataType: 'json',
                    data: {
                        data: item,
                        labTests: labTests
                    },
                    success: function (res) {
                        if (!res.success) {
                            toastr.error("Tạo mới thất bại.");
                            $('#myModal').modal('hide');
                        }
                        else {
                            toastr.success("Tạo mới thành công.");
                            $('#myModal').modal('hide');
                            homeController.loadData();
                        }
                    }
                });
            } else {
                console.log("invalid");
            }
        });


        $('#btnAddNew').off('click').on('click', function () {
            $('#lblPopupTitle').text('Thêm mới cuộc hẹn');
            homeController.resetForm();
            $('#myModal').modal('show');
            var selectedSample = $(".Sample").children("option:selected").val();
            if (selectedSample == 1) {
                $('#mauCheckGroup').show();
                $('#nuocTieuCheckGroup').hide();
                $('#teBaoHocCheckGroup').hide();
                $('#phanCheckGroup').hide();
                $('#dichCheckGroup').hide();
            }
            else if (selectedSample == 2) {
                $('#mauCheckGroup').hide();
                $('#nuocTieuCheckGroup').show();
                $('#teBaoHocCheckGroup').hide();
                $('#phanCheckGroup').hide();
                $('#dichCheckGroup').hide();
            }
            else if (selectedSample == 3) {
                $('#mauCheckGroup').hide();
                $('#nuocTieuCheckGroup').hide();
                $('#teBaoHocCheckGroup').show();
                $('#phanCheckGroup').hide();
                $('#dichCheckGroup').hide();
            }
            else if (selectedSample == 4) {
                $('#mauCheckGroup').hide();
                $('#nuocTieuCheckGroup').hide();
                $('#teBaoHocCheckGroup').hide();
                $('#phanCheckGroup').show();
                $('#dichCheckGroup').hide();
            }
            else if (selectedSample == 5) {
                $('#mauCheckGroup').hide();
                $('#nuocTieuCheckGroup').hide();
                $('#teBaoHocCheckGroup').hide();
                $('#phanCheckGroup').hide();
                $('#dichCheckGroup').show();
            }
        });


        //$('#btnSearch').off('click').on('click', function () {
        //    homeController.loadData(true);
        //});

        $("#txtSearch").off('change').on("change", function () {
            homeController.loadData(true);
        })
        /*
        console.log('register switch')
        $('label.switch input').off('change').on('change', function () {
            var sampleGettingId = $(this).data('id');
            console.log(sampleGettingId);
            homeController.checkIsPaid(sampleGettingId);
        });/**/


    },
    
    
    resetForm: function () {
        $('#txtDateOfBirth').val('');
        $('#txtName').val('');
        $('#ddlGender').val('Male');
        $('#txtPhone').val('');
        $('#txtAddress').val('');
        $('mauCheckGroup').val('');

    },
    
    //cashier load cuộc hẹn trong ngày
    loadData: function (changePageSize) {
        //chi lấy dữ liệu mà select
        var selectedSample = $("#select-sample").children("option:selected").val();
        var selectDate = $("#select-date").val();
        var searchData = $("#txtSearch").val();
        $.ajax({
            url: '/cashier/GetAppBySample',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, sampleId: selectedSample, date: selectDate, search: searchData },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    //searchData = data.PatientName;
                    //do du lieu qua html
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            AppCode: item.AppointmentCode,
                            FullName: item.PatientName,
                            DOB: item.DateOfBirth,
                            Phone: item.Phone,
                            Address: item.Address,
                            StartTime: item.StartTime,
                            OrderNumber: i+1,
                            Date: item.Date,
                            Table: item.Table,
                            SampleGettingId: item.SampleGettingId,
                            IsPaid: item.IsPaid,
                            ReadOnly: (item.IsPaid === true) ? "return false;" : "", 
                            Checked: (item.IsPaid === true) ?  "checked" : ""
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


    ChangeIsPaid: function (SampleGettingId) {
        $.ajax({
            url: '/cashier/IsPaid',
            type: 'POST',
            dataType: 'json',
            data: { sampleGettingId: SampleGettingId },
            success: function (response) {
//                homeController.loadPrice(SampleGettingId);
                if (response.success === true) {
                    //toastr.success('Thanh toán hoàn tất');
                    homeController.loadData();
                }
                
            }
        })
    },

    loadPrice: function (id, changePageSize) {
        $.ajax({
            url: '/cashier/GetPriceByPatient',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, sampleGettingId: id },
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