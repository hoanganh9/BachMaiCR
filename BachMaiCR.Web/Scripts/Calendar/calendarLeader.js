var enumCalendarDutyStatus = {
    Created: 1,//tạo mới
    SendApproved: 2,//gửi duyệt
    Approved: 3,//duyệt
    CancelApproved: 4 //hủy duyệt
};

var dayType = {
    Day: 10,
    Middle: 12,
    Night: 22
};
var enumCalendarChangeStatus =
    {
        Change: 1//đổi lịch
        , Deleted: 2//xóa lịch
        , Add: 3//thêm mới lịch(các trường hợp sử dụng khi lịch hủy duyệt)
    };
var calendarDataChangeId;
var createObjCalendarChange = function (objDoctor, objDoctorChange) {
    var objCalendarChange = {
        CALENDAR_DUTY_ID: calendarDutyId
        , DATE_START: objDoctor.DATE_START
        , DATE_CHANGE_START: objDoctorChange.DATE_START
        , DOCTORS_ID: objDoctor.DOCTORS_ID
        , DOCTORS_NAME: objDoctor.ABBREVIATION
        , DOCTORS_CHANGE_ID: objDoctorChange.DOCTORS_ID
        , DOCTORS_CHANGE_NAME: objDoctorChange.ABBREVIATION
        , STATUS: 1
        , STATUS_APPROVED: 1
        , CALENDAR_DELETE: 0
        , CHECK_CALENDAR_CHANGE_EXIST: objDoctor.CALENDAR_DATA_ID + "_" + (objDoctorChange.CALENDAR_DATA_ID == undefined ? objDoctor.DOCTORS_ID + "_" + objDoctorChange.DOCTORS_ID : objDoctorChange.CALENDAR_DATA_ID)
    };
    return objCalendarChange;
};
$(function () {

    $("#saveCalendar").click(function () {
        var dates = "";
        var strValues = "";
        var i = 0;

        var strInfor = $('#DATE_MONTHX').val() + "_" + $('#DATE_YEARX').val();
        var calendarContent = $('#inforContent').val();
        var errcalendarContent = $('.field-validation-valid[data-valmsg-for="errinforContent"]');
        errcalendarContent.css('color', 'red');
        if (calendarContent == "" || calendarContent == null) {
            $("#inforContent").focus();
            errcalendarContent.text('Nhập tên lịch lãnh đạo');
            return false;
        }
        errcalendarContent.text('');
        $('#selectable').find('.item-schedule').each(function () {
            dates = $(this).attr('id');
            $(this).find('.schedule-element a').each(function () {
                i++;
                if (i == 1) {
                    strValues = $(this).attr('id');
                } else {
                    strValues = strValues + "-" + $(this).attr('id');
                }
            })
        })
        $.ajax(
        {
            type: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: "/CalendarDuty/SaveCalendarLeader?strValues=" + strValues + "&strInfor=" + strInfor + "&calendarName=" + $('#inforContent').val().trim(),
            success: function (data) {
                window.notice(data, window.notice_warning);
                window.setTimeout(function () { location.href = '/CalendarDuty/CalendarLeader'; }, 3000);
            },
            error: function (result) {
                window.notice('Xin lỗi dữ liệu không hợp lệ', window.notice_warning);
            }
        });
    });
});
$("#updateCalendar").click(function () {
    var dates = "";
    var strValues = "";
    var i = 0;
    var strInfor = $('#DATE_MONTHX').val() + "_" + $('#DATE_YEARX').val() + "_" + $('#idCalendarDuty').val();
    $('#selectable').find('.item-schedule').each(function () {
        dates = $(this).attr('id');
        $(this).find('.schedule-element a').each(function () {
            var divParent = $(this).parent();
            if (!$(this).hasClass('namechange') && !divParent.hasClass('add-schedule')) {
                i++;
                if (i == 1) {
                    strValues = $(this).attr('id');
                } else {
                    strValues = strValues + "-" + $(this).attr('id');
                }
            }
        })
    })
    //console.log(lstCalendarChange);

    $.ajax(
    {
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        url: "/CalendarDuty/UpdateCalendarLeader",
        data: JSON.stringify({ strValues: strValues, strInfor: strInfor, strCalendarName: $('#inforContent').val().trim(), lstCalendarChange: lstCalendarChange }),
        success: function (data) {
            window.notice(data, window.notice_warning);
        },
        error: function (result) {
            window.notice('Xin lỗi dữ liệu không hợp lệ', window.notice_warning);
        }
    });
});

$.curCSS = function (element, attrib, val) {
    $(element).css(attrib, val);
};

$(document).delegate(".schedule-element", "hover", function (event) {
    if (event.type == 'mouseenter') {
        $(this).find(".element-close").fadeIn(50);
        $(this).find(".element-change").fadeIn(50);
    } else {
        $(this).find(".element-close").fadeOut(50);
        $(this).find(".element-change").fadeOut(50);
    }
});

//Hàm gọi dialog lấy dữ liệu danh sách lãnh đạo
function getListDirectors(pointX, pointY, optionText, isChange, dateX) {
    var idCalendarDuty = $('.TextCenter #flag_action').attr('data-type');

    $("#popupStaff").html('');
    $('#btnSearch').remove();
    $("#popupStaff").dialog({
        title: 'Danh sách lãnh đạo',
        modal: true,
        position: [pointX, pointY],
        width: 600,
        resizable: false,
        draggable: false,
    }).load("/CalendarDuty/ListDirectors/?arrayid=" + optionText + "&groupId=2" + "&idCalendarDuty=" + idCalendarDuty + "&isChange=" + isChange + "&dateX=" + dateX);
    $("#popupStaff").removeClass("ui-dialog-content ui-widget-content");
    $("#ui-dialog-title-popupStaff").css("margin-top", "-5px");
    $("#btnSearch").remove();
    $('#popupStaff').after('<input value="" id="btnSearch" style="height: 22px; width: 200px; font-size: 11px; color: black; font-weight: normal; top:4px; position: absolute; right: 25px;" placeholder="Tìm lãnh đạo"/>');

    $('#btnSearch').live('keyup', function () {
        searchTable($(this).val());
    });
}

//Hàm tìm kiếm trên table
function searchTable(inputVal) {
    var table = $('#table_list_staff');
    table.find('tr').each(function (index, row) {
        var allCells = $(row).find('td');
        if (allCells.length > 0) {
            var found = false;
            allCells.each(function (index, td) {
                var regExp = new RegExp(inputVal, 'i');
                if (regExp.test($(td).text())) {
                    found = true;
                    return false;
                }
            });
            if (found == true) $(row).show(); else $(row).hide();
        }
    });
}

//Hàm xử lý chức năng đổi lịch trực lãnh đạo
$("#" + $('#selectable').attr('id')).delegate(" td.item-schedule .schedule-element .element-change", "click", function (e) {
    var x = e.clientX;
    var y = e.clientY;
    var optionTexts = $(this).attr('id');
    calendarDataChangeId = $(this).parent().attr('data-calendar');
    getListDirectors(x, y, optionTexts, 1, "");
});


//Thực hiện mở cửa sổ thêm mới danh sách bác sĩ
//Danh sách mảng id bác sĩ sẽ được chuyền qua biến arrayid vào controller
$("#" + $('#selectable').attr('id')).delegate(" td.ui-selected", "dblclick", function (e) {
    //console.log(calendarDutyStatus);
    if (calendarDutyStatus != 3) {
        var x = e.clientX;
        var y = e.clientY;
        var optionTexts = "";
        var ii = 0;
        $('.ui-selected').find('div').each(function () {
            if (!$(this).hasClass('remove-schedule')) {
                ii++;
                if (ii == 1) {
                    optionTexts = $(this).attr('id');
                }
                else {
                    optionTexts = optionTexts + "_" + $(this).attr('id');
                }
            }
        })
        var dateX = $('.ui-selected').attr('id') + "/" + $('#DATE_MONTHX').val() + "/" + $('#DATE_YEARX').val();
        getListDirectors(x, y, optionTexts, 0, dateX);
    }
    
});

// Hiển thị thông tin cá nhân của bác sĩ trực
$("#" + $('#selectable').attr('id')).delegate(" td.item-schedule .schedule-element a", "click", function (e) {
    //Cập nhật lại tên bác sĩ theo vị trí Click
    $('#box_staff #staff_name').text($(e.currentTarget).text().trim());
    var x = e.clientX;
    var y = e.clientY;
  //  var idparent = $(this).parent('div').attr('id');
    var res = $(this).attr('id').split("_");
    var idparent = res[1];
    if (idparent != null) {
        $("#box_staff").dialog({
            title: 'Chi tiết thông tin cán bộ',
            modal: true,
            height: 'auto',
            width: 340,
            resizable: false,
            draggable: false,
            position: [x, y]
        }).load("/CalendarDuty/GetInforDoctor/?idDoctor=" + idparent + "");
        $("#box_staff").removeClass("ui-dialog-content");
        $("#box_staff").fadeIn(50);
        $("#ui-dialog-title-box_staff").css("margin-top", "-5px");
    }
});


//Gọi hàm thêm mới bác sĩ
$('#popupStaff').delegate(" #btnOk", "click", function (event) {
    if (window.isChange == 1) {
        $('#flag_accept').val(0);
        var doctorCalendarChange;
        if ($("#table_list_staff input:radio[name='groupDoctor']:checked").length == 0) {
            window.notice('Bạn chưa chọn lãnh đạo để đổi lịch', window.notice_warning);
            return false;
        }

        var doctorCalendarRequestChange = _.find(window.lstCalendarDoctor, function (obj) {
            return obj.CALENDAR_DATA_ID === parseInt(calendarDataChangeId);
        });

        if ($('#DatesChange #slDateChanges option').length > 0 && $('#DatesChange #slDateChanges').val() != '') {
            doctorCalendarChange = _.find(window.lstCalendarDoctor, function (obj) {
                return obj.CALENDAR_DATA_ID === parseInt($('#DatesChange #slDateChanges').val());
            });
        }
        else {
            var inputChecked = $('#table_list_staff input:checked');
            var item = inputChecked.parent('td').next('td').find('div');
            var strTel = item.html().trim();
            doctorCalendarChange = {
                DOCTORS_ID: window.idDoctorChange,
                ABBREVIATION: strTel
            };
        }
        if (doctorCalendarRequestChange) {
            lstCalendarChange.push(createObjCalendarChange(doctorCalendarRequestChange, doctorCalendarChange));
            addItemChangeToList(doctorCalendarRequestChange, doctorCalendarChange);
        }
    }
    else {
        var idColumnSelect = $('.ui-selected').attr('id');
        var strTel = "";
        var idDoctorNew = $('#table_list_staff').find('input:checked').map(function () { return parseInt(this.id.replace('Checked_', '')); }).get();
        removeList(idDoctorNew);
        $('#table_list_staff').find('input:checked').each(function () {
            var item = $(this).parent('td').next('td').find('div');
            strTel = item.attr('title');
            var doctorId = item.attr('id');
            var idColumn = doctorId + "_" + strTel;
            addItemToList(item, idColumnSelect, doctorId, window.isChange);
        })
    }
});

function removeList(idDoctorNew) {
    var ids = $('.ui-selected div.schedule-element').map(function () { return parseInt(this.id); }).get();
    var del = _.difference(ids, idDoctorNew);
    if (del.length > 0) {
        _.each(del, function (num) {
            var parentDiv = $('.ui-selected').find('div#' + num);
            scheduleCloseClick(parentDiv.find('span.element-close'));
        });
    }
}

function addItemChangeToList(objRequestChange, objAcceptChange) {
    var dataChange = objRequestChange.CALENDAR_DATA_ID + "_" + (objAcceptChange.CALENDAR_DATA_ID == undefined ? objRequestChange.DOCTORS_ID + "_" + objAcceptChange.DOCTORS_ID : objAcceptChange.CALENDAR_DATA_ID);
    var idColumnSelect = $('.ui-selected').attr('id');
    var day = $('.ui-selected').attr('day');
    $('.ui-selected').find('div[data-calendar="' + objRequestChange.CALENDAR_DATA_ID + '"]').remove();
    $('.ui-selected').append("<div class='schedule-element schedule-element-haschange' id='" + objRequestChange.DOCTORS_ID
                 + "' data-calendar='" + objRequestChange.CALENDAR_DATA_ID + "' data-calendar-change='" + dataChange + "'>"
                 + "<a href='javascript:void(0)' id='" + idColumnSelect + "_" + objRequestChange.DOCTORS_ID + "_" + day + "'>"
                 + objRequestChange.ABBREVIATION + "</a>"
                 + "<div style='margin-top:10px;'></div><img src='/Images/Forward_Arrow.png' width='12px' height='12px' />"
                 + "<a href='javascript:void(0)' id='" + idColumnSelect + "_" + objAcceptChange.DOCTORS_ID + "' class='namechange'> " + objAcceptChange.ABBREVIATION + "</a>"
                 + "<span class='element-close fa fa-rotate-left namechange' title='Hủy đổi lịch trực' onclick=cancelCalendarClick(this,'" + dataChange + "'," + objRequestChange.DOCTORS_ID + ")/>"
                 + "</div>");

    var itemAcceptChange = $('.item-schedule').find('div[data-calendar="' + objAcceptChange.CALENDAR_DATA_ID + '"]');
    if (itemAcceptChange.length > 0) {
        var tdParent = itemAcceptChange.parent();
        itemAcceptChange.remove();
        tdParent.append("<div class='schedule-element schedule-element-haschange' id='" + objAcceptChange.DOCTORS_ID
            + "' data-calendar='" + objAcceptChange.CALENDAR_DATA_ID + "' data-calendar-change='" + dataChange + "'>"
                     + '<a href="#" id="' + idColumnSelect + '_' + objAcceptChange.DOCTORS_ID + '_' + day + '">'
                     + objAcceptChange.ABBREVIATION + "</a>"
                     + "<div style='margin-top:10px;'></div><img src='/Images/Forward_Arrow.png' width='12px' height='12px' />"
                     + "<a href='#' id='" + idColumnSelect + "_" + objRequestChange.DOCTORS_ID + "' class='namechange'> " + objRequestChange.ABBREVIATION + "</a>"
                     + "<span class='element-close fa fa-rotate-left namechange' title='Hủy đổi lịch trực' onclick=cancelCalendarClick(this,'" + dataChange + "'," + objAcceptChange.DOCTORS_ID + ")/>"
                     + "</div>");
    }
}

// Thêm mới danh sách bác sĩ ô lựa chọn   
function addItemToList(item, idColumnSelect, doctorId, isChange) {
    var classAddSchedule = '';
    if (enumCalendarDutyStatus.CancelApproved == window.calendarDutyStatus) {
        classAddSchedule = 'add-schedule';
    }
    if ($('.ui-selected').length > 0) {
        var cellSelected = $('.ui-selected');
        var existDoctor = cellSelected.find('div#' + doctorId);
        if (existDoctor.length == 0) {
            var lenghX = "100%";
            var doctorName = item.text().trim();
            lenghX = parseFloat(doctorName.length) + 5;
            var idColumn = cellSelected.attr('id');
            var day = cellSelected.attr('day');
            cellSelected.append('<div class="schedule-element ' + classAddSchedule + '" id="' + doctorId + '">'
                                        + '<a href="#" id="' + idColumnSelect + '_' + doctorId + (day == undefined ? '' : '_' + day) + '">'
                    + doctorName + '</a>'
                    + '<span class="element-close fa fa-times-circle" title="Xóa bỏ" onclick="scheduleCloseClick(this)"/>'
                    + (isChange == 1 ? '<span id="' + doctorId + '" data-date=""  data-column="" data-duty="" class="element-change fa fa-pencil-square fa-lg" title="Đổi lịch trực"/>' : '')
                    + '</div>');
            var hour = 0;
            if (day != undefined) {
                hour = day == 'ngay' ? dayType.Day : dayType.Night;
            }
            var date = new Date(window.calendarMonth + "/" + idColumn + "/" + window.calendarYear);
            // date = date.setHours(hour);

            var objCalendarInsert = {
                CALENDAR_DUTY_ID: window.calendarDutyId,
                DOCTORS_NAME: doctorName,
                DOCTORS_ID: parseInt(doctorId),
                DATE_START: new Date(date.setHours(hour)),
                STATUS: enumCalendarChangeStatus.Add
            };

            var exist = _.find(lstCalendarChange, function (num) {
                return num.CALENDAR_DUTY_ID === objCalendarInsert.CALENDAR_DUTY_ID
                && num.DOCTORS_ID === objCalendarInsert.DOCTORS_ID
                && (new Date(num.DATE_START)).valueOf() === (objCalendarInsert.DATE_START).valueOf();
            });
            if (!exist) {
                lstCalendarChange.push(objCalendarInsert);
            }
        }
        else {
            if (existDoctor.hasClass('remove-schedule')) {
                scheduleCancelDelClick(existDoctor.find('span.element-close'));
            }
        }
    }

};
// Thực hiện xóa bác sĩ được chọn
function scheduleCloseClick(item) {
    var parentDiv = $(item).parent('.schedule-element');
    var doctorRemove = getDoctorRemove(parentDiv);
    if (parentDiv.attr('data-calendar') == undefined || enumCalendarDutyStatus.CancelApproved != window.calendarDutyStatus) {
        var id = $(item).parent('.schedule-element').find('a').attr('id');
        var itemX = $(item).parent('.schedule-element');
        $(itemX).fadeOut(150, function () {
            $(this).remove();
        });

        lstCalendarChange = _.reject(lstCalendarChange, function (obj) {
            return obj.CALENDAR_DUTY_ID === doctorRemove.CALENDAR_DUTY_ID
                && obj.DOCTORS_ID === doctorRemove.DOCTORS_ID
                && (new Date(obj.DATE_START)).valueOf() === (doctorRemove.DATE_START).valueOf();
        });
    }
    else {
        //Trường hợp xóa bỏ các lịch đã có
        removeSchedule(parentDiv);
    }
}

function getDoctorRemove(itemRemove) {
    var parentTd = itemRemove.parent('.item-schedule');
    var idColumn = parseInt(parentTd.attr('id'));
    var day = parentTd.attr('day');
    var hour = 0;
    if (day != undefined) {
        hour = day == 'ngay' ? dayType.Day : dayType.Night;
    }
    var date = new Date(window.calendarMonth + "/" + idColumn + "/" + window.calendarYear);
    var doctorRemove = {
        CALENDAR_DUTY_ID: window.calendarDutyId,
        DOCTORS_NAME: itemRemove.find('a').not('.namechange').text().trim(),
        DOCTORS_ID: parseInt(itemRemove.attr('id')),
        DATE_START: new Date(date.setHours(hour)),
        STATUS: enumCalendarChangeStatus.Deleted
    };

    return doctorRemove;
}

function removeSchedule(itemRemove) {
    var doctorRemove = getDoctorRemove(itemRemove);
    itemRemove.addClass('remove-schedule');
    var calendarDataId = itemRemove.attr('data-calendar');
    if (calendarDataId != '' && calendarDataId != 0) {
        var exist = _.find(lstCalendarChange, function (num) {
            return num.CALENDAR_DUTY_ID === doctorRemove.CALENDAR_DUTY_ID
            && num.DOCTORS_ID === doctorRemove.DOCTORS_ID
            && new Date(num.DATE_START).valueOf() === (doctorRemove.DATE_START).valueOf()
            && num.STATUS === enumCalendarChangeStatus.Deleted;
        });
        //console.log(doctorRemove);
        if (!exist) {
            lstCalendarChange.push(doctorRemove);
            itemRemove.find('span').remove();
            itemRemove.append('<span class="element-close fa fa-rotate-left" title="Hủy xóa" onclick="scheduleCancelDelClick(this)" />');
        }

        if (itemRemove.hasClass('schedule-element-haschange')) {
            itemRemove.find('img').remove();
            itemRemove.find('a.namechange').remove();
            itemRemove.find('span.namechange').remove();
            itemRemove.removeClass('schedule-element-haschange');
            var exisChange = _.find(lstCalendarChange, function (num) {
                return num.CALENDAR_DUTY_ID === doctorRemove.CALENDAR_DUTY_ID
                && num.DOCTORS_ID === doctorRemove.DOCTORS_ID
                && new Date(num.DATE_START).valueOf() === (doctorRemove.DATE_START).valueOf()
                && num.STATUS === enumCalendarChangeStatus.Change;
            });

            if (exisChange) {
                //var temp = _.clone(exisChange);
                exisChange.DOCTORS_ID = exisChange.DOCTORS_CHANGE_ID;
                exisChange.DOCTORS_NAME = exisChange.DOCTORS_CHANGE_NAME;
                exisChange.DATE_START = exisChange.DATE_CHANGE_START;
                exisChange.DOCTORS_CHANGE_ID = doctorRemove.DOCTORS_ID;
                exisChange.DOCTORS_CHANGE_NAME = doctorRemove.DOCTORS_NAME;
                exisChange.DATE_CHANGE_START = null;
            }
            else {
                var exisChange1 = _.find(lstCalendarChange, function (num) {
                    return num.CALENDAR_DUTY_ID === doctorRemove.CALENDAR_DUTY_ID
                    && num.DOCTORS_CHANGE_ID === doctorRemove.DOCTORS_ID
                    && new Date(num.DATE_CHANGE_START).valueOf() === (doctorRemove.DATE_START).valueOf()
                    && num.STATUS === enumCalendarChangeStatus.Change;
                });
                if (exisChange1) {
                    exisChange.DATE_CHANGE_START = null;
                }
                //console.log(exisChange1);
            }   
        }
    }
}

//
//Hàm hủy đổi lịch
//
function cancelCalendarClick(item, calendarChangeId, idDotor) {
    var objCalendarChangeExist = _.find(lstCalendarChange, function (num) {
        return num.CHECK_CALENDAR_CHANGE_EXIST === calendarChangeId || num.CALENDAR_CHANGE_ID === calendarChangeId;
    });
    //console.log(objCalendarChangeExist);
    if (objCalendarChangeExist) {
        (item).remove();
        var itemAcceptChange = $('.item-schedule').find('div[data-calendar-change="' + calendarChangeId + '"]');
        itemAcceptChange.attr('data-calendar-change', '');
        itemAcceptChange.each(function () {
            var divParent = $(this);
            divParent.removeClass('schedule-element-haschange');
            divParent.find('img').remove();
            divParent.find('a.namechange').remove();
            divParent.find('span.namechange').remove();
            divParent.append('<span class="element-close fa fa-times-circle" title="Xóa bỏ" onclick="scheduleCloseClick(this)" />');
            divParent.append('<span id="' + idDotor + '" data-date=""  data-column="" data-duty="" class="element-change fa fa-pencil-square fa-lg" title="Đổi lịch trực"/>');
        });

        lstCalendarChange = _.reject(lstCalendarChange, function (num) {
            return num.CHECK_CALENDAR_CHANGE_EXIST === calendarChangeId || num.CALENDAR_CHANGE_ID === calendarChangeId;
        });
    }
}

//
//Hàm hủy xóa(trong trường hợp lịch hủy duyệt)
//
function scheduleCancelDelClick(item) {
    var parentDiv = $(item).parent('.schedule-element');
    parentDiv.removeClass('remove-schedule');
    var calendarDataId = parentDiv.attr('data-calendar');
    if (calendarDataId != '' && calendarDataId != 0) {

        var parentTd = parentDiv.parent('.item-schedule');
        var idColumn = parseInt(parentTd.attr('id'));
        var day = parentTd.attr('day');
        var hour = 0;
        if (day != undefined) {
            hour = day == 'ngay' ? dayType.Day : dayType.Night;
        }
        var date = new Date(window.calendarMonth + "/" + idColumn + "/" + window.calendarYear);
        //date = date.setHours(hour);
        var doctorRemove = {
            CALENDAR_DUTY_ID: window.calendarDutyId,
            DOCTORS_NAME: parentDiv.find('a').text().trim(),
            DOCTORS_ID: parseInt(parentDiv.attr('id')),
            DATE_START: new Date(date.setHours(hour)),
            STATUS: enumCalendarChangeStatus.Deleted
        };

        lstCalendarChange = _.reject(lstCalendarChange, function (obj) {
            return obj.CALENDAR_DUTY_ID === doctorRemove.CALENDAR_DUTY_ID
                && obj.DOCTORS_ID === doctorRemove.DOCTORS_ID
                && (new Date(obj.DATE_START)).valueOf() === (doctorRemove.DATE_START).valueOf();
        });
        parentDiv.append('<span class="element-close fa fa-times-circle" title="Xóa bỏ" onclick="scheduleCloseClick(this)" />');
        parentDiv.append('<span id="' + parentDiv.attr('id') + '" data-date=""  data-column="" data-duty="" class="element-change fa fa-pencil-square fa-lg" title="Đổi lịch trực"/>');
    }
    $(item).remove();
}
function hisCalendar(idCalendarDuty) {
    //  alert("edit");
    var $self = $(this);
    $.get('/CalendarDuty/HistoryCalendarDuty', {
        idCalendarDuty: idCalendarDuty,
    },
        function (data) {
            var $categoryModel = $('#view_model_container');
            $categoryModel.html(data);
            $('#viewEntityModel').modal({
                keyboard: true
            });
        })
};
