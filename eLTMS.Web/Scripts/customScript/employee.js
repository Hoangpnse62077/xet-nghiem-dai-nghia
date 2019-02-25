var homeconfig = {
    pageSize: 20,
    pageIndex: 1,
}
var homeController = {
    init: function () {
        homeController.loadData();
        homeController.registerEvent();
    },
    registerEvent: function () {

        $('#btnSave').off('click').on('click', function () {
            var code = $('#txtCode').val();
            var accountId = parseInt($('#txtAccountId').val());
            var id = $('#txtID').val();
            var accountId = parseInt($('#txtAccountId').val());
            var name = $('#txtName').val();
            var gender = $('#ddlGender').val();         
            var dateBirth = $('#txtDB').val();     
            var phone = $('#txtPhoneNumber').val();
            var homeAddress = $('#txtHomeAddress').val();   
            var dateStart = $('#txtSD').val();     
            var date = '06-10-2018';
            var isDeleted = "False";
            var employee = {
                EmployeeId: id,      
                FullName: name,
                Gender: gender,
                DateOfBirth: dateBirth,
                StartDate: dateStart,
                PhoneNumber: phone,
                HomeAddress: homeAddress,
                IsDeleted: isDeleted,
                Account: {
                    Role: $('#ddlRole').val(),
                    AvatarUrl: $('#avatar').attr('src'),
                    Email: $('#txtEmail').val(),
                },
                Status: $('#ddlStatus').val()
            }
            console.log(employee);
            if (employee.EmployeeId == 0) {
                $.ajax({
                    url: '/Employee/AddEmployee',
                    type: 'Post',
                    dataType: 'json',
                    data: employee,
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
                    url: '/Employee/UpdateEmployee',
                    type: 'Post',
                    dataType: 'json',
                    data: employee,
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
            $('#lblPopupTitle').text('Thêm mới nhân viên');
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
            $('#lblPopupTitle').text('Cập nhật nhân viên');
            $('#myModal').modal('show');
            var id = $(this).data('id');
            homeController.loadDetail(id);
        });

        $('.btn-delete').off('click').on('click', function () {
            var id = $(this).data('id');
            homeController.deleteEmployee(id);
            
        });
        $("#txtSearch").off('change').on("change", function () {
            homeController.loadData(true);
        })
    },
    deleteEmployee: function (id) {
        $.ajax({
            url: '/Employee/DeleteEmployee',
            data: {
                employeId: id
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
            url: '/Employee/EmployeeDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.sucess) {
                    var data = response.data;          
                    $('#txtID').val(data.EmployeeID);
                    $('#txtAccountId').val(data.AccountId);
                    $('#ddlStatus').val(data.Status).change();
                    $('#txtName').val(data.FullName); 
                    $('#ddlGender').val(data.Gender.trim()).change();
                    $('#txtDB').val(data.DateOfBirth);
                    $('#txtPhoneNumber').val(data.PhoneNumber);
                    $('#ddlRole').val(data.Role);
                    $('#txtHomeAddress').val(data.HomeAddress);
                    $('#txtSD').val(data.DateOfStart);
                    $('#txtEmail').val(data.Email);
                    $('#avatar').attr('src',data.Avatar);
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
        $('#txtID').val('0');
        $('#txtAccountId').val('')
        $('#ddlStatus').val('').change();
        $('#txtName').val('');
        $('#ddlGender').val('').change();
        $('#txtDB').val('');
        $('#txtPhoneNumber').val('');
        $('#ddlRole').val('').change();
        $('#txtHomeAddress').val('');
        $('#txtSD').val(''); 
        $('#txtEmail').val('');
        $('#avatar').attr('src','');
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/Employee/GetAllEmployees',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, fullName: $('#txtSearch').val() },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    console.log(data);
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            EmployeeID: item.EmployeeID,
                            FullName: item.FullName,
                            PhoneNumber: item.PhoneNumber,
                            Role: item.RoleDisplay,                    
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