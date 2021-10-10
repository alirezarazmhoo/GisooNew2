function setInputFilter(textbox, inputFilter) {
    ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop"].forEach(function (event) {
        textbox.addEventListener(event, function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                this.value = "";
            }
        });
    });
}
function SetInputFilter(targets) {
    for (var i = 0; i < targets.length; i++) {
        setInputFilter(document.getElementById(targets[i]), function (value) {
            return /^\d*\.?\d*$/.test(value);
        });
    }
}

function SetPictures(inputtarget, target) {
    var myURL = window.URL || window.webkitURL;
    var result = "";
    var tag = "";
    var _File = document.getElementById("" + inputtarget + "").files;
    for (var i = 0; i < _File.length; i++) {
        var fileURL = myURL.createObjectURL(_File[i]);
        tag = "<img src='" + fileURL + "' style='width:80px;height:60px;'>";
        result += tag;
    }
    $('#' + target + '').html(result);
}

function PostAjax(ActionName, Parameters, callBack) {
    var fd = new FormData();
    for (var i = 0; i < Parameters.length; i++) {
        if (Parameters[i].special === 'combo') {
            fd.append(Parameters[i].id, $('#' + Parameters[i].htmlname + '').find('option:selected').val());
        }
        else if (Parameters[i].special === 'Multicombo') {
            fd.append(Parameters[i].id, $('#' + Parameters[i].htmlname + '').val());
        }
        else if (Parameters[i].special === 'radio') {
            fd.append(Parameters[i].id, $('input[name="' + Parameters[i].htmlname + '"]:checked').val());
        }
        else if (Parameters[i].special === 'file') {

            $.each($(".TheFile"), function (i, obj) {
                $.each(obj.files, function (j, file) {
                    fd.append("file", file);
                });
            });
        }
        else if (Parameters[i].special === 'music') {
            $.each($(".MusicUrl"), function (i, obj) {
                $.each(obj.files, function (j, file) {
                    fd.append("musicfiles", file);
                });
            });
        }
        else if (Parameters[i].special === 'siglefile') {
            fd.append("pictiremusic", $('#' + Parameters[i].htmlname + '')[0].files[0]);
        }
        else {
            fd.append(Parameters[i].id, $('#' + Parameters[i].htmlname + '').val());
        }
    }
    $.ajax({
        type: "POST",
        url: "" + ActionName + "",
        data: fd,
        dataType: "json",
        contentType: false,
        processData: false,
        //beforeSend: function () {
        //    $("#LoadingModal").modal('show');
        //},
        success: function (response) {
            if (response.success) {
                //$("#SuccessModal").modal('show');

                callBack();
                //setTimeout(function () { location.href = redirecturl; }, 2000);
            }
            else {
                $("#textError").text(response.responseText);
                $("#ErrorModal").modal('show');
            }
        },
        error: function (response) {
            //$("#LoadingModal").modal('show');
        }
        //},
        //complete: function () {
        //    $("#LoadingModal").modal('toggle');
        //}
    });
}


function GetData(ActionName, Target, Data, hasMap) {


    $.ajax({
        type: "GET",
        data: Data,
        url: "" + ActionName + "",
        dataType: "html",
        success: function (data) {
            $('#' + Target + '').html('');
            $('#' + Target + '').html(data);
            $('#' + Target + '').show();
            if (hasMap === true) {
                var lon = $("#longitude").val();
                var lat = $("#latitude").val();
                LoadMap(lat, lon);
            }
        }
    });
}

//function GetDataPaging(ActionName, Target, pageNo, isCalled, isPaged) {
//    debugger
//    $.ajax({
//        method: "Get",
//        data: { page: pageNo },
//        url: "" + ActionName + "",
//        success: function (data) {
//            $('#' + Target + '').append(data);
//            pageNo++;
//            isCalled = false;
//            if ($.trim(data) == "") {
//                isPaged = false;
//            }
//        }
//    });
//} 
var pageNo = 2;
var isPaged = true;
var isCalled = false;
function LoadListNotice(ActionName) {

    var hT = $('#mydiv2').offset().top,
        hH = $('#mydiv2').outerHeight(),
        wH = $(window).height(),
        wS = $(this).scrollTop();
    if (wS > (hT + hH - wH)) {
        if (isPaged && !isCalled) {
            isCalled = true;
            $.ajax({

                url: "" + ActionName + "",
                method: "Get",
                data: { page: pageNo },
                success: function (data) {
                    $("#yourDiv").append(data);
                    pageNo++;
                    isCalled = false;
                    if ($.trim(data) == "") {
                        isPaged = false;
                    }
                },
                error: function () {

                }
            })

        }
    }
}

function CreateModal(target, mode) {

    var modal = "";
    if (parseInt(mode) === 1) {
        modal = "<div class='modal fade' id='ErrorModal' role='dialog'><div class='modal-dialog modal-sm'><div class='modal-content'><div class='modal-header'><h4 class='modal-title text-danger'>خطا</h4></div><div class='modal-body text-danger'><p id='textError'></p></div><div class='modal-footer'><button type='button' class='btn btn-danger'  data-dismiss='modal'>بستن</button>";
        document.getElementById(target).innerHTML = modal;
    }
    else if (parseInt(mode) === 2) {
        modal = "<div class='modal fade' id='QuestionModalAdvertisment' tabindex='-1' role='dialog' aria-labelledby='exampleModalLabel' aria-hidden='true'><div class='modal-dialog modal-dialog-centered' role = 'document' ><div class='modal-content'><div class='modal-header'><h4 class='modal-title'>پرسش</h4><div data-dismiss='modal' ></div></div><div class='modal-body'><p>آیا تمایل به ادامه عملیات دارید ؟ </p><div class='form-group form-group-btn'><button class='btn' onclick='RemoveAdvertisment();'>تایید</button></div></div></div></div></div >";
        document.getElementById(target).innerHTML = modal;
    }
    else if (parseInt(mode) === 4) {
        modal = "<div class='modal fade' id='QuestionModalNotice' tabindex='-1' role='dialog' aria-labelledby='exampleModalLabel' aria-hidden='true'><div class='modal-dialog modal-dialog-centered' role = 'document' ><div class='modal-content'><div class='modal-header'><h4 class='modal-title'>پرسش</h4><div data-dismiss='modal' ></div></div><div class='modal-body'><p>آیا تمایل به ادامه عملیات دارید ؟ </p><div class='form-group form-group-btn'><button class='btn' onclick='RemoveNotice();'>تایید</button></div></div></div></div></div >";
        document.getElementById(target).innerHTML = modal;
    }
    else {
        modal = "<div class='modal fade' id='QuestionModal' tabindex='-1' role='dialog' aria-labelledby='exampleModalLabel' aria-hidden='true'><div class='modal-dialog modal-dialog-centered' role = 'document' ><div class='modal-content'><div class='modal-header'><h4 class='modal-title'>پرسش</h4><div data-dismiss='modal' ></div></div><div class='modal-body'><p>آیا تمایل به ادامه عملیات دارید ؟ </p><div class='form-group form-group-btn'><button class='btn' onclick='Remove();'>تایید</button></div></div></div></div></div >";
        document.getElementById(target).innerHTML = modal;
    }

}

function CreateAllModals() {
    CreateModal('Error', 1);
    CreateModal('Advertisment', 2);
    CreateModal('Question', 3);
    CreateModal('Notice', 4);
}


function RemoveFiles(ParentTarget, ActionName, ParameterName, Parametervalue) {

    var fd = new FormData();

    fd.append(ParameterName, Parametervalue);
    $.ajax({
        type: "POST",
        url: "" + ActionName + "",
        data: fd,
        dataType: "json",
        contentType: false,
        processData: false,

        success: function (response) {

        },
        error: function (response) {

        }

    });
}
function RemoveFileAdvertisment(ParentTarget, ActionName, ParameterId, ParameterIdvalue, ParameterName, ParameterNamevalue) {

    var fd = new FormData();
    fd.append(ParameterId, ParameterIdvalue);
    fd.append(ParameterName, ParameterNamevalue);
    $.ajax({
        type: "POST",
        url: "" + ActionName + "",
        data: fd,
        dataType: "json",
        contentType: false,
        processData: false,

        success: function (response) {

        },
        error: function (response) {

        }

    });
}
function LoadMap(lon, lat) {

    var theMarker = {};
    if (lon == 0 && lat == 0) {

        lon = 32.650823;
        lat = 51.668037;
    }
    var mymap = L.map('mapid').setView([lon, lat], 13);
    if (lon !== 0 && lat !== 0) {
        theMarker = L.marker([lon, lat]).addTo(mymap);

    }
    //var mymap = L.map('mapid').setView([32.650823, 51.668037], 13);
    //theMarker = L.marker([lon,lat], { icon: icon }).addTo(mymap);
    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'pk.eyJ1IjoiYWxpcmV6YXJhem1qb28iLCJhIjoiY2s1Yzg2aTM4MWo1bjNvcDN2dWQwbGs5byJ9.0MqeBvs7xijOfpnGE73R_A'
    }).addTo(mymap);
    mymap.on('click', function (e) {
        var icon = new L.Icon.Default();
        icon.options.shadowSize = [0, 0];


        lat = e.latlng.lat;
        lon = e.latlng.lng;
        if (theMarker !== undefined) {

            mymap.removeLayer(theMarker);
        }
        theMarker = L.marker([lat, lon], { icon: icon }).addTo(mymap);

        $("#latitude").val(e.latlng.lat);
        $("#longitude").val(e.latlng.lng);
    });
}

$("#mounthCount").change(function () {
    var mounthCount = $("#mounthCount").val();
    var mainAmount = parseInt($("#totalPric").html());
    if (mounthCount > 0) {
        $("#totalPricMain").html(mounthCount * mainAmount + "<small>تومان</small>");
    }

});
var mounthCount = $("#mounthCount").val();
var mainAmount = parseInt($("#totalPric").html());
if (mounthCount > 0) {
    $("#totalPricMain").html(mounthCount * mainAmount + "<small>تومان</small>");
}

function mul() {
    var price = $("#price").val();
    var lineLawVal = $("#lineLaw").val();
    var minDiscount = $("#minDiscount").val();
    var maxDiscount = $("#maxDiscount").val();
    if (lineLawVal == 1 || lineLawVal == 3) {
    var minPrice = (price * minDiscount) / 100;
    var maxPrice = (price * maxDiscount) / 100;
    $("#discountPrice").attr("max", minPrice);
        $("#discountPrice").attr("min", maxPrice);
        
    }
   
}
function mulReserve() {
    
    var price = $("#price").val();
    
    var minDiscount = $("#minDiscount").val();
    var maxDiscount = $("#maxDiscount").val();
    
    var minPrice = price- (price * minDiscount) / 100;
    var maxPrice = price- (price * maxDiscount) / 100;
        $("#discountPrice").attr("max", minPrice);
        $("#discountPrice").attr("min", maxPrice);

    $("#discountSpan").html('مقدار مبلغ وارد شده بین ' + maxPrice + ' تومان و ' +  minPrice +' تومان باید باشد');

}

function mulclass() {
    
    var price = $("#price").val();
    var classRoomLaw = $("#classRoomLaw").val();
    var minDiscount = $("#minDiscount").val();
    var maxDiscount = $("#maxDiscount").val();
    if (classRoomLaw == 1 || classRoomLaw == 3) {
        var minPrice = (price * minDiscount) / 100;
        var maxPrice = (price * maxDiscount) / 100;
        $("#discountPrice").attr("max", minPrice);
        $("#discountPrice").attr("min", maxPrice);

    }

}
function mulpro() {
    var price = $("#price").val();
   
    var minDiscount = $("#minDiscount").val();
    var maxDiscount = $("#maxDiscount").val();
   
        var minPrice = (price * minDiscount) / 100;
        var maxPrice = (price * maxDiscount) / 100;
        $("#discountPrice").attr("max", minPrice);
        $("#discountPrice").attr("min", maxPrice);
    $("#discountSpan").html('مقدار مبلغ وارد شده بین ' + maxPrice + ' تومان و ' +  minPrice +' تومان باید باشد');

   
   

}
$("#frmLine #price").focusout(function () {
    mul();
});
$("#frmLine #discountPrice").focusout(function () {
    mul();
});
$("#frmClassRoom #price").focusout(function () {
    mulclass();
});
$("#frmClassRoom #discountPrice").focusout(function () {
    mulclass();
});
$("#frmProduct #price").focusout(function () {
    mulpro();
});
$("#frmProduct #discountPrice").focusout(function () {
    mulpro();
});
$("#frmLineReserve #price").focusout(function () {
    mulReserve();
});
$("#frmLineReserve #discountPrice").focusout(function () {
    mulReserve();
});
$("#frmClassRoomForReserve #price").focusout(function () {
    mulReserve();
});
$("#frmClassRoomForReserve #discountPrice").focusout(function () {
    mulReserve();
});

var i = 1;
function AddDivTime(e) {
    let isInputEmpty = false;
    
    document.querySelector('#addOtherTime'+e+'').parentElement.parentElement.querySelectorAll('input').forEach(input => {
        if (input.value == '') {
            isInputEmpty = true
            $('#ttimeWeek').modal('show');
        } 
    })
    if (!isInputEmpty) {
        var str = "<div class='allTimes' style='float:right;width:100%;display: flex; margin-bottom:5px;'><label style='margin-right:5px;'>از ساعت: </label><input type = 'text' class='form-control  timepicker startWorkHourFirst' name = 'timeFirst" + e + "" + i + "' aria-describedby='timeFirst" + e + "" + i + "-error' id = 'timeFirst" + e + "" + i + "' style = 'width:40% !important;display:inline-block;direction:ltr' data-val='true' data-val-regex='فرمت ساعت8:20است' data-val-regex-pattern='^((?:[01]?[0-9]|2[0-3]):([0-5][0-9]|[0-9]))|([0-9][0-9])|([0-9])$' aria-invalid='false'><label style='margin-right: 5px;'>تا ساعت: </label><input type='text' class='form-control  timepicker endWorkHourFirst' name='timesecond" + e + "" + i + 1 + "' aria-describedby='timesecond" + e + "" + i + 1 + "-error' id='timesecond" + e + "" + i + 1 + "' style='width:40% !important;display:inline-block;direction:ltr' data-val='true' data-val-regex='فرمت ساعت8:20است' data-val-regex-pattern='^((?:[01]?[0-9]|2[0-3]):([0-5][0-9]|[0-9]))|([0-9][0-9])|([0-9])$' aria-invalid='false' ><span class='text-danger field-validation-valid' data-valmsg-for='timeFirst" + e + "" + i + "' data-valmsg-replace='true'></span><span class='text-danger field-validation-valid' data-valmsg-for='timesecond" + e + "" + i+1 +"' data-valmsg-replace='true'></span><button  style='border-radius: 6px; background: #dddddd; height: 40px; width: 49px; margin-right: 3px; line-height: 40px; text-align: center; ' onclick = 'RemoveDivTime(this)'> - </button></div > ";
        $("." + e + "").append(str);
        var form = $("#RegisterAllReserveForm")
            .removeData("validator") 
            .removeData("unobtrusiveValidation"); 
        $.validator.unobtrusive.parse(form);
        i += 2;
    }
    
}

function RemoveDivTime(e) {
    e.parentElement.remove();
}


$("#RegisterAllReserveForm").submit(function (event) {
    event.preventDefault();
    var isFirstEmpty = false;
    var allShanbeh = [];
    $(".fromToTime_shanbeh input").each(function () {
        if (isFirstEmpty == false) {
               allShanbeh.push($(this).val());
        }
        if ($(this).hasClass('startWorkHourFirst')==true && $(this).val()=="") {
            isFirstEmpty = true;
        }
        if ($(this).hasClass('endWorkHourFirst') == true && $(this).val() == "") {
           
            if (isFirstEmpty == false) {
                allShanbeh.pop();
                allShanbeh.pop();
            }
            else {
                allShanbeh.pop();

            }
            
        }
        isFirstEmpty == false;
        
    })
    var isFirstEmptyyek = false;
    var allyekShanbeh = [];
    $(".fromToTime_yekshanbeh input").each(function () {
        if (isFirstEmptyyek == false) {
            allyekShanbeh.push($(this).val());
        }
        if ($(this).hasClass('startWorkHourFirst') == true && $(this).val() == "") {
            isFirstEmptyyek = true;
        }
        if ($(this).hasClass('endWorkHourFirst') == true && $(this).val() == "") {
            
            if (isFirstEmptyyek == false) {
                allyekShanbeh.pop();
                allyekShanbeh.pop();
            }
            else {
                allyekShanbeh.pop();

            }
            
        }
        isFirstEmptyyek == false;

    })
    var isFirstEmptydo = false;
    var alldoShanbeh = [];
    $(".fromToTime_doshanbeh input").each(function () {
        if (isFirstEmptydo == false) {
            alldoShanbeh.push($(this).val());
        }
        if ($(this).hasClass('startWorkHourFirst') == true && $(this).val() == "") {
            isFirstEmptydo = true;
        }
        if ($(this).hasClass('endWorkHourFirst') == true && $(this).val() == "") {
           
            if (isFirstEmptydo == false) {
                alldoShanbeh.pop();
                alldoShanbeh.pop();
            }
            else {
                alldoShanbeh.pop();

            }
            
        }
        isFirstEmptydo == false;

    })
    var isFirstEmptyse = false;
    var allseShanbeh = [];
    $(".fromToTime_seshanbeh input").each(function () {
        if (isFirstEmptyse == false) {
            allseShanbeh.push($(this).val());
        }
        if ($(this).hasClass('startWorkHourFirst') == true && $(this).val() == "") {
            isFirstEmptyse = true;
        }
        if ($(this).hasClass('endWorkHourFirst') == true && $(this).val() == "") {
            
            if (isFirstEmptyse == false) {
                allseShanbeh.pop();
                allseShanbeh.pop();
            }
            else {
                allseShanbeh.pop();

            }
           
        }
        isFirstEmptyse == false;

    })
    var isFirstEmptychar = false;
    var allcharShanbeh = [];
    $(".fromToTime_charshanbeh input").each(function () {
        if (isFirstEmptychar == false) {
            allcharShanbeh.push($(this).val());
        }
        if ($(this).hasClass('startWorkHourFirst') == true && $(this).val() == "") {
            isFirstEmptychar = true;
        }
        if ($(this).hasClass('endWorkHourFirst') == true && $(this).val() == "") {
            
            if (isFirstEmptychar == false) {
                allcharShanbeh.pop();
                allcharShanbeh.pop();
            }
            else {
                allcharShanbeh.pop();

            }
            
        }
        isFirstEmptychar == false;

    })
    var isFirstEmptypanj = false;
    var allpanjShanbeh = [];
    $(".fromToTime_panjshanbeh input").each(function () {
        if (isFirstEmptypanj == false) {
            allpanjShanbeh.push($(this).val());
        }
        if ($(this).hasClass('startWorkHourFirst') == true && $(this).val() == "") {
            isFirstEmptypanj = true;
        }
        if ($(this).hasClass('endWorkHourFirst') == true && $(this).val() == "") {
            
            if (isFirstEmptypanj == false) {
                allpanjShanbeh.pop();
                allpanjShanbeh.pop();
            }
            else {
                allpanjShanbeh.pop();

            }
           
        }
        isFirstEmptypanj == false;

    })
    var isFirstEmptyjome = false;
    var alljome = [];
    $(".fromToTime_jome input").each(function () {
        if (isFirstEmptyjome == false) {
            alljome.push($(this).val());
        }
        if ($(this).hasClass('startWorkHourFirst') == true && $(this).val() == "") {
            isFirstEmptyjome = true;
        }
        if ($(this).hasClass('endWorkHourFirst') == true && $(this).val() == "") {
            
            if (isFirstEmptyjome == false) {
                alljome.pop();
                alljome.pop();
            }
            else {
                alljome.pop();

            }
           
        }
        isFirstEmptyjome == false;

    })
    var lineId= $("#lineId").val();
    
    var isFromAdmin = $("#isFromAdmin").val();
    var dateReserve = $("#dateReserve").val();
    var mounthCount = $("#mounthCount").val();
    $.ajax({
        url: "/CustomerHome/AddReserveForLine",
        method: "POST",
        dataType: "json",
        data: { allShanbeh, allyekShanbeh, alldoShanbeh, allseShanbeh, allcharShanbeh, allpanjShanbeh, alljome, lineId, dateReserve, mounthCount, isFromAdmin},
        success: function (data) {
            window.location.href = data.redirectToUrl;
        },
        error: function () {
            alert("Failed");

        }
    })
});
//$("#RegisterAllReserveForm .timepicker").focusout(function () {
//    debugger
//    $('#RegisterAllReserveForm .timepicker').mask('00:00:00', {
//        'translation': {
//           H: { pattern: /[0-23]/ },
//            M: { pattern: /[0-23]/ },
            
//        }
//    });
//})


//$("#RegisterAllReserveForm .timepicker").focusout(function () {
//    //alert($(this).val().length)
//    if ($(this).val().length < 5  ) {
//        var inputlVal = $(this).val().split(':');
//        if (inputlVal.length == 2) {
//            for (var i = 0; i < inputlVal.length; i += 2) {
//                //alert(inputlVal[i])
//                //alert(inputlVal[i + 1])
//                debugger
//                if (inputlVal[i] != "" && inputlVal[i] != undefined)

//                    inputlVal[i] = (inputlVal[i] < 10 ? "0" : "") + inputlVal[i];
//                else
//                    inputlVal[i] = "00";
//                if (inputlVal[i + 1] != "" && inputlVal[i + 1] != undefined) { inputlVal[i + 1] = (inputlVal[i + 1] < 10 ? "0" : "") + inputlVal[i + 1]; }
//                else
//                    inputlVal[i + 1] = "00";
//                $(this).val(inputlVal[i] + ":" + inputlVal[i + 1]);
//            }
//        }
//    }
//})



