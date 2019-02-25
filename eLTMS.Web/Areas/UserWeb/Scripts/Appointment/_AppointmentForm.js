/*
    Author: DucBM
*/

CONFIG = {
    PATIENT_ID: 145, // hard code for dev-ing
    SAMPLE_DTOS_KEY: "SAMPLE_DTOS",
    IS_UPDATE: false,
};

if (typeof flagInitForEdit !== 'undefined' && flagInitForEdit == true) {
    CONFIG.IS_UPDATE = true;
}

var Utils = {
    formatTimeShort: function (time) {
        var hour = Math.floor(time / 60 / 60);
        var min = Math.floor(time / 60) % 60;
        if (min < 10) {
            min = "0" + min;
        }
        return hour + ':' + min;
    }, // end function
    formatTimeLong: function (time) {
        var hour = Math.floor(time / 60 / 60);
        var min = Math.floor(time / 60) % 60;
        var sec = time % 60;
        if (min < 10) {
            min = "0" + min;
        }
        if (sec < 10) {
            sec = "0" + sec;
        }
        return hour + ':' + min + ":" + sec;
    } // end function
};

var Model = {
    firebaseDB: {},
    sampleDtos: {},
    bookings: [],
    slotDtos: {},
    appointmentId: -1,
    appointmentDto: {
        //PatientId: CONFIG.PATIENT_ID,
        SampleGettingDtos: []
    },
    patientDto: {},
    suggestResult: null
}

var Controller = {
    init: function () {
        Controller.registerEvent();
    }, // end Action
    registerEvent: function () {

        $(document).ready(function () {

            if (CONFIG.IS_UPDATE) {
                $('#step-0').hide(0);
                $('#step-1').show(0);
                $('#btn-prev-10').hide(0);
                $('#btn-next-12 span').html('Bước 2');
                $('#btn-prev-21 span').html('Bước 1');
                $('#step-1 h2 span').html('Bước 1/2');
                $('#step-2 h2 span').html('Bước 2/2');
            }

            var $dob = $('#step-0 [name=p-dob]');
            var currDate = new Date(Date.now());
            var sNow = currDate.getFullYear() + "-" + (currDate.getMonth() + 1) + "-" + currDate.getDate();
            $dob.attr('max', sNow);
            
            Controller.renderLabTestList();
            $("#success-modal").modal({
                backdrop: "static",
                keyboard: false,
                show: false,
            });
            $("#fail-modal").modal({
                backdrop: "static",
                keyboard: false,
                show: false,
            });
            $("#processing-modal").modal({
                backdrop: "static",
                keyboard: false,
                show: false,
            });
        });

        $("#btn-next-01").click(function () {
            var valid = true;
            
            var fullName = $('#step-0 [name=p-name]').val();
            var $name = $('#step-0 [name=p-name] ~ .valid-mess');
            $name.html("");
            if (fullName === "") {
                $name.html('Vui lòng điền họ tên của bạn');
                valid = false;
                if (fullName.length > 100) {
                    $name.html('Họ tên tối đa 100 ký tự');
                    valid = false;
                }
            } 
            
            var gender = $('#step-0 [name=p-gender]').val();

            var dob = $('#step-0 [name=p-dob]').val();
            var $dob = $('#step-0 [name=p-dob] ~ .valid-mess');
            $dob.html("");
            if (dob === "") {
                $dob.html('Vui lòng điền ngày sinh của bạn');
                valid = false;
            } 

            var phone = $('#step-0 [name=p-phone]').val();
            var $phone = $('#step-0 [name=p-phone] ~ .valid-mess');
            const digits = "0123456789";
            var isDigits = true;
            $phone.html("");
            for (var i = 0; i < phone.length; i++) {
                if (digits.includes(phone[i]) == false) {
                    isDigits = false;
                    break;
                }
            }
            if (phone === "") {
                $phone.html('Vui lòng điền số điện thoại.');
                valid = false;
            } else if (phone.length != 10 || isDigits==false) {
                $phone.html('Số điện thoại không hợp lệ.<br>Số điện thoại hợp lệ là một dãy số 10 chữ số.');
                valid = false;
            }

            var address = $('#step-0 [name=p-address]').val();
            var $address = $('#step-0 [name=p-address] ~ .valid-mess');
            $address.html("");
            if (address === "") {
                $address.html('Vui lòng điền địa chỉ của bạn');
                valid = false;
                if (address.length > 200) {
                    $address.html('Địa chỉ tối đa 100 ký tự');
                    valid = false;
                }
            } 

            if (valid) {
                Model.patientDto = {
                    FullName: fullName,
                    Gender: gender,
                    DateOfBirth: dob,
                    PhoneNumber: phone,
                    HomeAddress: address
                };
                var duration = 200;
                $("#step-0").fadeOut(duration, function () {
                    $("#step-1").fadeIn(duration);
                });
            }
        })

        $("#btn-next-12").click(function () {
            // VALIDATION:
            var $checks = $("#step-1-form input[type='checkbox']:checked");
            ////console.log($checks);
            var noChecked = $checks.length == 0;
            if (noChecked) { // user did not check any checkbox, not allow to go to step 2
                $("html, body").animate({
                    scrollTop: $("#step-1").offset().top,
                }, 0);
                $("#alert-required-labtests").fadeIn(500);
                return;
            } // end VALIDATION
            // GO TO STEP 2:
            Controller.renderStep2Html();
            var duration = 200;
            $("#step-1").fadeOut(duration, function () {
                $("#step-2").fadeIn(duration);
            });
        });

        $("#btn-prev-10").click(function () {
            var duration = 200;
            $("#step-1").fadeOut(duration, function () {
                $("#step-0").fadeIn(duration);
            });
        });

        $("#btn-prev-21").click(function () {
            var duration = 200;
            $("#step-2").fadeOut(duration, function () {
                $("#step-1").fadeIn(duration);
            });
        });

        // fixing...
        $("#btn-submit").click(function () {
            //$("#btn-submit").attr('disabled', true);

            // VALIDATION: required fill all fields
            var hasEmptyField = false;
            $("#step-2 [type='date']").each(function (index, element) {
                if ($(this).val() == '') {
                    hasEmptyField = true;
                }
            });
            $("#step-2 select").each(function (index, element) {
                if ($(this).val() == '') {
                    hasEmptyField = true;
                }
            });
            if (hasEmptyField) {
                $("html, body").animate({
                    scrollTop: $("#step-2").offset().top,
                }, 0);
                $("#alert-required-datetime").fadeIn(500);
                return;
            } // end VALIDATION


            // create SampleGettingDtos and assign to Model
            Model.appointmentDto.SampleGettingDtos = [];
            for (var i = 0; i < Model.sampleDtos.length; i++) {
                var sampleDto = Model.sampleDtos[i];
                if (sampleDto.IsSelected) {
                    var sampleGettingDto = {};
                    sampleGettingDto.SampleId = sampleDto.SampleId;

                    var trSample = $("tr[data-sample-index='" + sampleDto.Index + "']");

                    var gettingDate = trSample.find("[type=date]").val();
                    sampleGettingDto.GettingDate = gettingDate;

                    var slotId = trSample.find("select").val();
                    sampleGettingDto.SlotId = slotId;

                    sampleGettingDto.LabTestIds = [];
                    for (var j = 0; j < sampleDto.LabTests.length; j++) {
                        if (sampleDto.LabTests[j].IsChecked) {
                            var labTestId = sampleDto.LabTests[j].LabTestId;
                            sampleGettingDto.LabTestIds.push(labTestId);
                        }
                    }

                    Model.appointmentDto.SampleGettingDtos.push(sampleGettingDto);
                }
            }

            Model.appointmentDto.PatientDto = Model.patientDto;
            //Model.appointmentDto.PatientDto = {};
            //Model.appointmentDto.PatientDto.FullName = $('#p-name').val();
            //Model.appointmentDto.PatientDto.IdentityCardNumber = $('#p-cmnd').val();
            //Model.appointmentDto.PatientDto.PhoneNumber = $('#p-phone').val();

            // ajax for create new appointment
            if (CONFIG.IS_UPDATE) {
                Model.appointmentDto.AppointmentId = Model.AppointmentId;
                //console.log(Model.appointmentDto);
            }
            var jsonData = JSON.stringify(Model.appointmentDto);
            Controller.sendToServer(jsonData);
        }); // end event handler

        newAp2 = function () {
            $("#tr-2").hide();
            $("#no-3").html("2");
            $("#ap-2").fadeIn();
        }

    }, // end Action
    renderStep1Html: function () {
        // render step 1:
        var data = { Samples: Model.sampleDtos };
        var template = $("#step-1-template").html();
        var htmlStep1 = Mustache.render(template, data);
        $("#step-1-form").html(htmlStep1);


        var handleCheckboxChanged = function () {
            var sampleIndex = $(this).closest("[data-sample-index]").data("sample-index");
            var labTestIndex = $(this).data("labtest-index");
            var checked = $(this).is(":checked");

            Model.sampleDtos[sampleIndex].LabTests[labTestIndex].IsChecked = checked;
            Model.sampleDtos[sampleIndex].nChecked += (checked ? 1 : -1);
            Model.sampleDtos[sampleIndex].IsSelected = Model.sampleDtos[sampleIndex].nChecked > 0;
        }
        $("#step-1-form [type='checkbox']").change(handleCheckboxChanged);

        if (typeof flagInitForEdit !== 'undefined' && flagInitForEdit == true) {
            flagInitForEdit = false;
            //console.log('AppointDto:', AppointDto);
            Model.AppointmentId = AppointDto.AppointmentId;
            Model.AppointmentDto = JSON.parse(JSON.stringify(AppointDto));
            //console.log(Model.AppointmentId);
            //console.log(Model.AppointmentDto);
            for (var i = 0; i < AppointDto.SampleGettingDtos.length; i++) {
                var sample = AppointDto.SampleGettingDtos[i];
                // set getting date for Model.sampleDtos
                for (var j = 0; j < Model.sampleDtos.length; j++) {
                    if (Model.sampleDtos[j].SampleId == sample.SampleId) {
                        Model.sampleDtos[j].GettingDate = sample.GettingDate;
                    }
                }
                for (var j = 0; j < sample.LabTestIds.length; j++) {
                    var labTestId = sample.LabTestIds[j];
                    var checkbox = $("[data-labtest-id='" + labTestId + "']");
                    checkbox.prop('checked', true);

                    var sampleIndex = $(checkbox).closest("[data-sample-index]").data("sample-index");
                    var labTestIndex = $(checkbox).data("labtest-index");
                    var checked = $(checkbox).is(":checked");

                    Model.sampleDtos[sampleIndex].LabTests[labTestIndex].IsChecked = checked;
                    Model.sampleDtos[sampleIndex].nChecked += (checked ? 1 : -1);
                    Model.sampleDtos[sampleIndex].IsSelected = Model.sampleDtos[sampleIndex].nChecked > 0;
                }
            }
        }

        for (var i = 0; i < Model.sampleDtos.length; i++) {
            var sample = Model.sampleDtos[i];
            for (var j = 0; j < sample.LabTests.length; j++) {
                var labTest = sample.LabTests[j];
                labTest.FmPrice = labTest.Price.toLocaleString("VN-vi");
            }
        }
        ////console.log(Model.sampleDtos);

    }, // end Action
    renderStep2Html: function () {

        var data = { Samples: Model.sampleDtos };
        var template = $("#step-2-template").html();
        var htmlStep2 = Mustache.render(template, data);
        $("#step-2-form").html(htmlStep2);

        data = { Samples: Model.sampleDtos };
        template = $("#step-2-template").html();
        htmlStep2 = Mustache.render(template, data);
        $("#step-2-form").html(htmlStep2);


        // load slot options of a Sample when change it's getting date
        $("#step-2-form input[type='date']").off("change").on("change", function () {
            //console.log("on change");
            var sampleId = $(this).closest('[data-sample-id]').data('sample-id');
            var sampleGroupId = Model.sampleDtos.find(function (item) {
                return item.SampleId == sampleId;
            }).SampleGroupId;
            var gettingDate = $(this).val();
            Controller.loadSlotOptions(sampleId, sampleGroupId, gettingDate);

            Controller.sortTableRows();
        })
        $("#step-2-form select").off("change").on("change", function () {
            Controller.sortTableRows();
        })

        // set value min max for input type date
        $('#table-step2 [type="date"]').each(function (index, el) {
            if (CONFIG.IS_UPDATE == false) { // is create
                $(el).val(Today);
            }
            $(el).attr('min', Today);
            $(el).attr('max', MaxGettingDate);
        })


        // load slot options
        flagNNotLoadedOptions = 0;
        for (var i = 0; i < Model.sampleDtos.length; i++) {
            if (Model.sampleDtos[i].IsSelected) {
                flagNNotLoadedOptions++;
            }
        }
        for (var i = 0; i < Model.sampleDtos.length; i++) {
            var sampleDto = Model.sampleDtos[i];
            if (sampleDto.IsSelected) {
                var sampleId = sampleDto.SampleId;
                var sampleGroupId = sampleDto.SampleGroupId;
                var gettingDate = $("#step-2-form [data-sample-id='" + sampleId + "'] [type='date']").val();
                Controller.loadSlotOptions(sampleId, sampleGroupId, gettingDate);
            }
        }

        // set default
        /*
        for (var i = 1; i < 6; i++) {
            var slotId = map.get(i);
            var $option = $("[data-sample-id='" + i + "'] select [value='" + slotId + "']");
            //console.log($option);
            $option.prop('selected', true);
        }/**/

        if (CONFIG.IS_UPDATE) {
            if (Model.appointmentDto.SampleGettingDtos != null && Model.appointmentDto.SampleGettingDtos.length > 0) {
                $(Model.appointmentDto.SampleGettingDtos).each(function (index, el) {
                    var $option = $("[data-sample-id='" + el.SampleId + "'] select [value='" + el.SlotId + "']");
                    // //console.log($option);
                    $option.prop('selected', true);
                });
            }
        }

        $("#step-2-form table tbody tr:not(:last-child()) td:first-child()").each(function (index, el) {
            $(this).html(index + 1);
        });

        var totalPrice = 0;
        for (var i = 0; i < Model.sampleDtos.length; i++) {
            var sample = Model.sampleDtos[i];
            for (var j = 0; j < sample.LabTests.length; j++) {
                var labTest = sample.LabTests[j];
                //console.log(labTest.IsChecked);
                if (labTest.IsChecked == true) {
                    totalPrice += labTest.Price;
                }
            }
        }
        var sTotalPrice = totalPrice.toLocaleString("VN-vi");
        $("#total-price").html(sTotalPrice);

    }, // end Action
    loadSlotOptions: function (sampleId, sampleGroupId, gettingDate) {
        //console.log('gettingDate', gettingDate);
        //console.log('sampleGroupId', sampleGroupId);
        $.ajax({
            method: "GET",
            contentType: "application/json",
            url: "/api/slot/get-slot-options",
            data: {
                sampleGroupId: sampleGroupId,
                gettingDate: gettingDate
            },
            async: true,
            success: function (data) {
                // render options
                for (var i = 0; i < data.length; i++) {
                    data[i].IsUnavailable = data[i].IsAvailable == false;
                    data[i].FmStartTime = Utils.formatTimeShort(data[i].StartTime);
                    data[i].FmFinishTime = Utils.formatTimeShort(data[i].FinishTime);
                }
                //console.log(data);
                var template = $("#options-template").html();
                var html = Mustache.render(template, { SlotOptions: data });
                $("#step-2-form [data-sample-id='" + sampleId + "'] select").html(html);

                // update to Model.sampleDtos
                for (var i = 0; i < Model.sampleDtos.length; i++) {
                    var sampleDto = Model.sampleDtos[i];
                    if (sampleDto.SampleId == sampleId) {
                        sampleDto.SlotOptions = data;
                        flagNNotLoadedOptions--;
                        if (flagNNotLoadedOptions == 0) {
                            //debugger
                            Controller.suggestSlots();
                            Controller.sortTableRows();
                        }
                        break;
                    }
                }
            }
        });
    },
    suggestSlots: function () {
        //debugger;
        var hour = 7;
        var min = 0;
        ////console.log(hour, min);
        if (hour != null && min != null) {
            var comingTime = hour * 60 * 60 + min * 60;
            // //console.log(Model.sampleDtos);
            var selectedSampleDtos = [];
            for (var i = 0; i < Model.sampleDtos.length; i++) {
                var sampleDto = Model.sampleDtos[i];
                if (sampleDto.IsSelected == true) {
                    selectedSampleDtos.push(sampleDto);
                }
            }
            var result = AppointmentSuggestor.CalcTheBestTour(selectedSampleDtos, comingTime);
            Model.suggestResult = result;
            //console.log('result: ', result);
            //Controller.renderStep2bHtml();
            // RENDER ...
            for (var i = 0; i < result.length; i++) {
                var sampleId = result[i].SampleId;
                var slotId = result[i].SlotId;
                var $option = $("[data-sample-id='" + sampleId + "'] select [value='" + slotId + "']");
                //console.log(sampleId, slotId, $option);
                $option.prop('selected', true);
            }
        }
    }, // end Action
    renderLabTestList: function () {
        // get all sampleDtos and LabTests
        var sSampleDtos = localStorage.getItem(CONFIG.SAMPLE_DTOS_KEY);
        var setIndex = function () {
            for (var i = 0; i < Model.sampleDtos.length; i++) {
                var sample = Model.sampleDtos[i];
                sample.Index = i;
                sample.nChecked = 0;
                for (var j = 0; j < sample.LabTests.length; j++) {
                    var labTest = sample.LabTests[j];
                    labTest.Index = j;
                    labTest.FmPrice = labTest.Price.toLocaleString("VN-vi");
                }
            }
        }
        if (sSampleDtos == null) {
            $.ajax({
                url: "/api/sample/get-all"
            }).success(function (data) {
                ////console.log(data);
                Model.sampleDtos = data; // CONFIG
                sSampleDtos = JSON.stringify(data);
                // save to local storage
                localStorage.setItem(CONFIG.SAMPLE_DTOS_KEY, sSampleDtos);
                setIndex();
                // render UI
                Controller.renderStep1Html();
            }) // end AJAX settings
        } else {
            // get data
            Model.sampleDtos = JSON.parse(sSampleDtos);
            setIndex();
            // render UI
            Controller.renderStep1Html();
        }
    }, // end Action
    sendToServer: function (jsonData) {
        // show processing popup
        $("#processing-modal").modal('show');
        var openingProcessingModal = true;
        $("#processing-modal").on("shown.bs.modal", function (e) {
            openingProcessingModal = false;
        });
        // func: 
        var showModalWithMessage = function ($modal, message) {
            $modal.modal('show');
            $modal.find(".out-message").html(message);
        }

        var sURL = "/api/appointment/create";
        var sMethod = "POST";
        if (CONFIG.IS_UPDATE) {
            sMethod = "PUT";
            sURL = "/api/appointment/update-appointment";
        }
        // call AJAX to create a new Appointment
        //console.log("json:", jsonData)
        $.ajax({
            method: sMethod,
            contentType: "application/json",
            url: sURL,
            dataType: "JSON",
            async: true,
            data: jsonData,
            success: function (data) {
                //$("#btn-submit").attr('disabled', false);
                var checkResult = function (data) {
                    $("#processing-modal").modal('hide');
                    if (data.Success == true) {
                        if (data.Data != null && typeof data.Data.DefaultPassword != 'undefined') {
                            var html = data.Message + "<br> Tài khoản của bạn vừa được tạo:<br>" +
                                "Số điện thoại: " + data.Data.PhoneNumber + "<br>" +
                                "Mật khẩu mặc định: " + data.Data.DefaultPassword;
                            showModalWithMessage($("#success-modal"), html);
                        } else {
                            showModalWithMessage($("#success-modal"), data.Message);
                        }
                    } else {
                        showModalWithMessage($("#fail-modal"), data.Message);
                    }
                }
                if (openingProcessingModal) {
                    $("#processing-modal").on("shown.bs.modal", function (e) {
                        e.stopPropagation();
                        openingProcessingModal = false;
                        checkResult(data);
                    });
                } else {
                    checkResult(data);
                }
            },
            fail: function (data) {
                //console.log("response: ");
                //console.log(data);
                //$("#btn-submit").attr('disabled', false);
                if (openingProcessingModal) {
                    $("#processing-modal").on("shown.bs.modal", function (e) {
                        e.stopPropagation();
                        openingProcessingModal = false;
                        $("#processing-modal").modal('hide');
                        showModalWithMessage($("#fail-modal"), "Có lỗi xảy ra."); // Lỗi 500
                    });
                } else {
                    $("#processing-modal").modal('hide');
                    showModalWithMessage($("#fail-modal"), "Có lỗi xảy ra");
                }
            }
        }); // end AJAX define

    }, // end Action
    sortTableRows: function () {
        var $table = $('#table-step2');
        var rows = $table.find('tr').get();
        rows.sort(function (a, b) {
            var dateA = $(a).find('[type=date]').val();
            var dateB = $(b).find('[type=date]').val();
            var timeA = parseInt($(a).find('option:selected').data('start-time'));
            var timeB = parseInt($(b).find('option:selected').data('start-time'));
            if (dateA < dateB) return -1;
            if (dateA > dateB) return 1;
            if (timeA < timeB) return -1;
            if (timeA > timeB) return 1;
            return 0;
        });
        $.each(rows, function (index, row) {
            $table.children('tbody').append(row);
        });
        $("#table-step2 tbody tr:not(:last-child()) td:first-child()").each(function (index, el) {
            $(this).html(index + 1);
        });
    }
}
Controller.init();

