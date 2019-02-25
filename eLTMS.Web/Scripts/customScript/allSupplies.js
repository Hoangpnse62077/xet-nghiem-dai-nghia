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
            var code = $('#txtCode').val();
            var name = $('#txtName').val();
            var type = parseInt($('#ddlSupplyType').val());
            var supplyId = $('#txtSupplyId').val();
            var unit = $('#ddlSupplyUnit').val();
            var note = $('#txtNote').val();
            var quantity = '0';
            var isDeteted = "False";
            var supply = {
                SuppliesId: supplyId,
                SuppliesCode: code,
                SuppliesName: name,
                SuppliesTypeId: type,
                Quantity:quantity,
                Unit: unit,
                IsDeleted: isDeteted,
                Note: note
            }
            if (supply.SuppliesId == 0) {
                $.ajax({
                    url: '/WareHouse/AddSupply',
                    type: 'Post',
                    dataType: 'json',
                    data: supply,
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
                    url: '/WareHouse/UpdateSupply',
                    type: 'Post',
                    dataType: 'json',
                    data: supply,
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
            $('#lblPopupTitle').text('Thêm mới vật tư');
            homeController.resetForm();
            $('#myModal').modal('show');
        });       
        $('#btnReset').off('click').on('click', function () {
            $('#txtNameS').val('');
            $('#ddlStatusS').val('');
            homeController.loadData(true);
        });
        $('.btn-edit').off('click').on('click', function () {
            $('#lblPopupTitle').text('Cập nhật vật tư');
            $('#myModal').modal('show');
            var id = $(this).data('id');
            homeController.loadDetail(id);
        });
        //$('.btn-delete').off('click').on('click', function () {
        //    var id = $(this).data('id');
        //    homeController.deleteSupply(id);
            
        //});
        $("#txtSearch").off('change').on("change", function () {
            homeController.loadData(true);
        })
    },
    deleteSupply: function (id) {
        $.ajax({
            url: '/WareHouse/Delete',
            data: {
                supplyId: id
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
                homeController.deleteSupply(SampleGettingId);
            }
            else {
                homeController.loadData();
            }
        });

    },
    loadDetail: function (id) {
        $.ajax({
            url: '/WareHouse/SupplyDetail',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.sucess) {
                    var data = response.data;
                    $('#txtSupplyId').val(data.SuppliesId);
                    $('#txtCode').val(data.SuppliesCode);
                    $('#txtName').val(data.SuppliesName);
                    $('#ddlSupplyType').val(data.SuppliesTypeId).change();
                    $('#ddlSupplyUnit').val(data.Unit).change();
                    $('#txtNote').val(data.Note);
                   
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
        $('#txtSupplyId').val('0');
        $('#txtCode').val('');
        $('#txtName').val('')
        $('#ddlSupplyType').val('').change();
        $('#ddlSupplyUnit').val('').change();
        $('#txtNote').val('')
    },
    loadData: function (changePageSize) {
        $.ajax({
            url: '/Warehouse/GetAllSupplies',
            type: 'GET',
            dataType: 'json',
            data: { page: homeconfig.pageIndex, pageSize: homeconfig.pageSize, suppliesCode: $('#txtSearch').val() },
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            SupplyId: item.SuppliesId,
                            SuppliesCode: item.SuppliesCode,
                            SupplyName: item.SuppliesName,
                            SupplyTypeName: item.SuppliesTypeName,
                            Quantity: item.Quantity,
                            Unit: item.Unit,
                            Note: item.Note,

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