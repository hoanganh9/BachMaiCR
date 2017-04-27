$(function () {

    $("#updateCalendar").click(function () {
        var inforContent = $("#inforContent").val().trim();
        var errcalendarContent = $('.field-validation-valid[data-valmsg-for="errinforContent"]');
        errcalendarContent.css('color', 'red');
        if (inforContent == "" || inforContent == null) {
            $("#inforContent").focus();
            errcalendarContent.text('(Nhập tên lịch trực thường trú Ban giám đốc)');
            return false;
        }
        errcalendarContent.text('');
        var dates = "";
        var strValues = "";
        var i = 0;
        var strInfor = $('#DATE_MONTHX').val() + "_" + $('#DATE_YEARX').val() + "_" + $('#idCalendarDuty').val()
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
          type: "POST",
          url: "/CalendarDuty/UpdateCalendarDirectors",
          content: "application/json; charset=utf-8",
          data: ({ strValues: strValues, strInfor: strInfor, calendarContent: inforContent }),
          success: function (data) {
              window.notice(data, window.notice_warning);
          },
          error: function (result) {

              window.notice('@Resources.Localization.DataIsNotValid', window.notice_warning);
          }

      });

    });
});
$("#saveCalendar").click(function () {
    var dates = "";
    var strValues = "";
    var i = 0;

    var strInfor = $('#DATE_MONTHX').val() + "_" + $('#DATE_YEARX').val();
    var calendarContent = $('#inforContent').val().trim();
    var errcalendarContent = $('.field-validation-valid[data-valmsg-for="errinforContent"]');
    errcalendarContent.css('color', 'red');
    if (calendarContent == "" || calendarContent == null) {
        $("#inforContent").focus();
        errcalendarContent.text('Nhập tên lịch trực thường trú Ban giám đốc');
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
         type: "POST",
         url: "/CalendarDuty/SaveCalendarDirectors",
         content: "application/json; charset=utf-8",
         data: ({ strValues: strValues, strInfor: strInfor, calendarContent: calendarContent }),
         success: function (data) {
             window.notice(data, window.notice_warning);
         },
         error: function (result) {

             window.notice('@Resources.Localization.DataIsNotValid', window.notice_warning);
         }

     });
});

$.curCSS = function (element, attrib, val) {
    $(element).css(attrib, val);
};

$(document).delegate(".schedule-element", "hover", function (event) {
    if (event.type == 'mouseenter') {
        $(this).find(".element-close").fadeIn(50);

    } else {
        $(this).find(".element-close").fadeOut(50);


    }
});


//Thực hiện mở cửa sổ thêm mới danh sách bác sĩ
//Danh sách mảng id bác sĩ sẽ được chuyền qua biến arrayid vào controller
$("#" + $('#selectable').attr('id')).delegate(" td.ui-selected", "dblclick", function (e) {
    var x = e.clientX;
    var y = e.clientY;
    var optionTexts = "";
    var ii = 0;
    var date = "1/" + $('.selectMonths').val() + "/" + $('.selectYears').val();
    $('.ui-selected').find('div').each(function () {
        ii++;
        if (ii == 1) {
            optionTexts = $(this).attr('id');
        }
        else {

            optionTexts = optionTexts + "_" + $(this).attr('id');
        }
    })
    $("#popupStaff").html('');
    $("#popupStaff").dialog({
        title: 'Danh sách Ban giám đốc',
        modal: true,
        position: [x, y],
        width: 550,
        resizable: false,
        draggable: true,
    }).load("/CalendarDuty/ListDirectors/?arrayid=" + optionTexts + "&groupId=1" + "&idCalendarDuty=0" + "&isChange=0&dateX=" + date);
    $("#popupStaff").removeClass("ui-dialog-content ui-widget-content");
    $("#ui-dialog-title-popupStaff").css("margin-top", "-5px");
    $("#btnSearch").remove();
    $('#popupStaff').after('<input value="" id="btnSearch" style="height: 22px; width: 200px; font-size: 11px; color: black; font-weight: normal; top:4px; position: absolute; right: 35px;" placeholder="Tìm ban giám đốc"/>');
   
    $('#btnSearch').live('keyup', function () {
        searchTable($(this).val());
    });
});
//Hàm tìm kiếm trên table
function searchTable(inputVal) {
    var count = 0;
    var table = $('#table_list_staff');
    table.find('tr').each(function (index, row) {
        var allCells = $(row).find('td');
        if (allCells.length > 0) {
            var found = false;
            allCells.each(function (index, td) {
                var regExp = new RegExp(inputVal.trim(), 'i');
                if (regExp.test($(td).text())) {
                    found = true;
                    count++;
                    return false;
                }
            });
            if (found == true) $(row).show(); else $(row).hide();
        }
    });
    if (parseFloat(count) > 6) {
        $('.checkboxId').addClass("marginp");
    }
    else {
        $('.checkboxId').removeClass("marginp");
    }
}

// Hiển thị thông tin cá nhân của bác sĩ trực
$("#" + $('#selectable').attr('id')).delegate(" td.item-schedule .schedule-element a", "click", function (e) {
    //Cập nhật lại tên bác sĩ theo vị trí Click
    $('#box_staff #staff_name').text($(e.currentTarget).text().trim());
    var x = e.clientX;
    var y = e.clientY;
    var idparent = $(this).parent('div').attr('id');
    $("#box_staff").dialog({
        title: 'Chi tiết thông tin lãnh đạo',
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

});


//Gọi hàm thêm mới bác sĩ
$('#popupStaff').delegate(" #btnOk", "click", function (event) {
    var idColumnSelect = $('.ui-selected').attr('id');
    removeList(idColumnSelect);
    var strTel = "";
    $('#table_list_staff').find('input:checked').each(function () {
        var item = $(this).parent('td').next('td').find('div');
        strTel = item.attr('title');
        var doctorId = item.attr('id');
        var idColumn = doctorId + "_" + strTel;

        addItemToList(item, strTel, idColumn, idColumnSelect, doctorId);
    })
});
function removeList(idColumnSelect) {

    $('.ui-selected').find('div').each(function () {
        var id = $(this).find('a').attr('id');
        $('#content_' + id).remove();
        $(this).remove();
    })

}
// Thêm mới danh sách bác sĩ ô lựa chọn   

function addItemToList(item, strTel, idItem, idColumnSelect, doctorId) {
    $('.ui-selected').each(function () {
        // Effect               
        var idColumn = $(this).attr('id');
        var lenghX = "100%";
        var doctorName = item.parent().next().text().trim();
        if (doctorName.length > 0) {
            lenghX = parseFloat(doctorName.length) + 5 + "px";
        }
        $(this).append('<div class="schedule-element" id="' + doctorId + '" style="width:' + lenghX + '">'
                                    + '<a href="#" id="' + idColumnSelect + '_' + idItem + '">'
                + doctorName + '</a>'
                + '<span class="element-close fa fa-times-circle" title="Xóa bỏ" onclick="scheduleCloseClick(this)"/>'
                + '</div>');
        $('#tel_' + idColumn).append('<div class="itemtel" id="content_' + idColumnSelect + '_' + idItem + '">'
               + strTel
               + '</div>');
    });
};
// Thực hiện xóa bác sĩ được chọn
function scheduleCloseClick(item) {
    // var id = $(item).parent('.schedule-element').attr('id');
    var id = $(item).parent('.schedule-element').find('a').attr('id');
    var itemX = $(item).parent('.schedule-element');
    $(itemX).fadeOut(150, function () {
        $(this).remove();
        $('#content_' + id).remove();
    });
}