$(function () {

    $("#saveAddCalendar").click(function () {
        var dates = "";
        var strValues = "";
        var i = 0;

        var strInfor = $('.selectMonths').val() + "_" + $('.selectYears').val();
        var calendarContent = $('#inforContent').val();
        var errcalendarContent = $('.field-validation-valid[data-valmsg-for="errinforContent"]');
        errcalendarContent.css('color', 'red');
        if (calendarContent == "" || calendarContent == null) {
            $("#inforContent").focus();
            errcalendarContent.text('Nhập tên lịch đơn vị');
            return false;
        }
        errcalendarContent.text('');
        $('#selectable').find('.item-schedule').each(function () {           
            $(this).find('.schedule-element').each(function () {
                var id = $(this).attr('id');
                if (id != "") {
                    i++;
                    if (i == 1) {
                        strValues = id;
                    } else {
                        strValues = strValues + "-" + id;
                    }
                }
            })
        })     
        $.ajax(
        {           
            type: "POST",
            url: "/CalendarDuty/SaveCalendarDutyDefault",
            content: "application/json; charset=utf-8",
            data: ({ strValues: strValues, strInfor: strInfor, calendarContent: calendarContent }),
            success: function (data) {
                $('#selectable').find('.item-schedule').each(function () {
                    $(this).find('.schedule-element').each(function () {
                        $(this).removeClass("add-calendar").addClass("current-calendar");
                    });
                });
                window.notice(data, window.notice_warning);
                window.location = "/CalendarDuty";
            },
            error: function (result) {

                window.notice('@Resources.Localization.DataIsNotValid', window.notice_warning);
            }
            
        });
    });
});

        $.curCSS = function (element, attrib, val) {
            $(element).css(attrib, val);
        };      
        //Thực hiện mở cửa sổ thêm mới danh sách bác sĩ
        //Danh sách mảng id bác sĩ sẽ được chuyền qua biến arrayid vào controller
        $("#" + $('#selectable').attr('id')).delegate(" td.ui-selected", "dblclick", function (e) {
            var x = e.clientX;
            var y = e.clientY;
            var optionTexts = "";
            var ii = 0;            
            $('.ui-selected').find('div').find('.choseId').each(function () {
                var idParent = $(this).parent('.schedule-element').attr('id');
                if (idParent != "") {
                    ii++;
                    if (ii == 1) {
                        optionTexts = $(this).attr('id');
                    }
                    else {

                        optionTexts = optionTexts + "_" + $(this).attr('id');
                    }
                }
            })
            var changeId = ",";
            $('.ui-selected').find('div').find('.changeId').each(function () {
                var idParent = $(this).parent('.schedule-element').attr('id');
                var idChangeDoctor = $(this).attr('id');
                if (idParent != "" && idChangeDoctor != "") {
                    ii++;
                    if (ii == 1) {
                        changeId = idChangeDoctor;
                    }
                    else {

                        changeId = changeId + "," + idChangeDoctor;
                    }
                }
            })
            if (changeId.length > 1) {
                changeId = "," + changeId + ",";
            }           
            var calendarStatus = $('#calendarStatus').val();
            var isChange = $('#isChange').val();           
            if (isChange == 0) {
                var dateX = $(this).find(".itemspan_" + $(this).attr('id')).attr('id');
                if (calendarStatus != 3) {
                    $("#popupStaff").html('');
                    $("#popupStaff").dialog({
                        title: 'Danh sách cán bộ',
                        modal: true,
                        position: [x, y],
                        width: 540,
                        resizable: false,
                        draggable: false,
                    }).load("/CalendarDuty/ListDoctorsDefault/?arrayid=" + optionTexts + "&changeId=" + changeId + "&dateX=" + dateX);
                    $("#popupStaff").removeClass("ui-dialog-content ui-widget-content");
                    $("#ui-dialog-title-popupStaff").css("margin-top", "-5px");
                    $("#btnSearch").remove();
                    $('#popupStaff').after('<input value="" id="btnSearch" style="height: 22px; width: 200px; font-size: 11px; color: black; font-weight: normal; top:4px; position: absolute; right: 35px;" placeholder="Tìm cán bộ"/>');

                    $('#btnSearch').live('keyup', function () {
                        searchTable($(this).val());
                    });
                }
                else {
                    alert("Không cho phép lập lịch vào ngày " + dateX);
                }
            }
            else {
                var dayCurrent = $('#dayCurrent').val();
                var isEnableEdit = $('#isEnableEdit').val();
                var checkCurrentTime = $('#checkCurrentTime').val();
                var idDiv = $(this).attr('id');
                var dateX = $(this).find(".itemspan_" + idDiv).attr('id');
                if (isEnableEdit == "0") {//Truong hop chua qua gio 16h30 cho phep thi duoc them                   
                    if (calendarStatus != 3 && ((parseFloat(idDiv) >= parseFloat(dayCurrent) && checkCurrentTime == "1") || checkCurrentTime == "0")) {
                        $("#popupStaff").html('');
                        $("#popupStaff").dialog({
                            title: 'Danh sách cán bộ',
                            modal: true,
                            position: [x, y],
                            width: 540,
                            resizable: false,
                            draggable: false,
                        }).load("/CalendarDuty/ListDoctorsDefault/?arrayid=" + optionTexts + "&changeId=" + changeId + "&dateX=" + dateX);
                        $("#popupStaff").removeClass("ui-dialog-content ui-widget-content");
                        $("#ui-dialog-title-popupStaff").css("margin-top", "-5px");
                        $("#btnSearch").remove();
                        $('#popupStaff').after('<input value="" id="btnSearch" style="height: 22px; width: 200px; font-size: 11px; color: black; font-weight: normal; top:4px; position: absolute; right: 35px;" placeholder="Tìm cán bộ"/>');

                        $('#btnSearch').live('keyup', function () {
                            searchTable($(this).val());
                        });
                    }
                    else {
                        alert("Không cho phép lập lịch vào ngày " + dateX);
                    }
                }
            }
            $('.ui-widget-overlay').remove();
            
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
            if (parseFloat(count) > 7) {
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
            var idparent = $(this).attr('id');          
            $("#box_staff").dialog({
                title: 'Chi tiết thông tin cán bộ',
                modal: true,            
                height: 'auto',
                width: 340,              
                resizable: false,
                draggable: true,
                position: [x, y]
                //open: function () {
                //    {
                //         $(this).parent().appendTo($("#page-nav-content")).css({ 'top': y, 'left': x, 'position': 'absolute' });
                //    }
                //}
            }).load("/CalendarDuty/GetInforDoctor/?idDoctor=" + idparent + "");
            $("#box_staff").removeClass("ui-dialog-content");
            $("#box_staff").fadeIn(50);
            $("#ui-dialog-title-box_staff").css("margin-top", "-5px");

        });
        // Thực hiện mở của sổ đổi lịch
        // Hiển thị thông tin cá nhân của bác sĩ trực
        $("#" + $('#selectable').attr('id')).delegate(" td.item-schedule .schedule-element .element-change", "click", function (e) {
            //Cập nhật lại tên bác sĩ theo vị trí Click
            var idDoctorChange = $(this).attr('id');           
            var idCalendarDuty = $(this).attr('data-duty');
            var dataDate = $(this).attr('data-date');           
            var x = e.clientX;
            var y = e.clientY;
            //Danh sách các cán bộ trong danh sách
            var optionTexts = ",";
            var ii = 0;
            $('.ui-selected').find('div a').each(function () {              
                var id = $(this).attr('id');
                if (id != "") {
                    ii++;
                    if (ii == 1) {
                        optionTexts = id;
                    }
                    else {

                        optionTexts = optionTexts + "," + id;
                    }
                }
            })
            if (optionTexts.length > 1) {
                optionTexts = "," + optionTexts + ",";
            }
            var changeId = ",";
            $('.ui-selected').find('div').find('.changeId').each(function () {
                var idParent = $(this).parent('.schedule-element').attr('id');
                var idChangeDoctor = $(this).attr('id');
                if (idParent != "" && idChangeDoctor != "") {
                    ii++;
                    if (ii == 1) {
                        changeId = idChangeDoctor;
                    }
                    else {

                        changeId = changeId + "," + idChangeDoctor;
                    }
                }
            })
            if (changeId.length > 1) {
                changeId = "," + changeId + ",";
            }
            $("#popupStaff").html('');
            $("#popupStaff").dialog({
                title: 'Danh sách cán bộ',
                modal: true,
                position: [x, y],
                height: 300,
                width: 430,
                resizable: false,
                draggable: false,
            }).load("/CalendarDuty/ListDoctorsChangesDefault/?idDoctorChange=" + idDoctorChange + "&idCalendarDuty=" + idCalendarDuty + "&dateDoctorChange=" + dataDate + "&arrayid=" + optionTexts + "&changeId=" + changeId);
            $("#popupStaff").removeClass("ui-dialog-content ui-widget-content");
            $("#popupStaff").css('display', 'block');
            $("#ui-dialog-title-popupStaff").css("margin-top", "-5px");
        });
        //Gọi hàm thêm mới bác sĩ
        $('#popupStaff').delegate(" #btnOk", "click", function (event) {
            var idColumnSelect = $('.ui-selected').attr('id');
            var span = $('.itemspan_' + idColumnSelect).attr('id');
            var isChange = $('#isChange').val();           
            var vR = ",";          
            if (isChange == 0) {
                removeList(idColumnSelect);
            }
            else {
                //Bo sung test ngay 29/05
                var ii = 0;
                //$('.ui-selected').find('div a').each(function () {
                $('.ui-selected').find('div').find('.listdoctor').each(function () {
                    var id = $(this).attr('id');
                    if (id != "") {
                        ii++;
                        if (ii == 1) {
                            vR += id;
                        }
                        else {

                            vR = vR + "," + id;
                        }
                    }
                })
                vR = vR + ",";
            }
            //end bo sung
            var strTel = "";
            var doctorList = "";
            $('#popupStaff').find('input:checked').each(function () {
                var item = $(this).parent('td').next('td').find('div');               
                strTel = item.attr('title');
                var doctorId = item.attr('id');
                var idColumn = doctorId + "_" + strTel;
                doctorList += "," + doctorId;
                addItemToList(item, span, doctorId, idColumnSelect, vR, isChange);
            })
            doctorList += ",";           
            var res = vR.split(",");          
            if (isChange == 1) {            
                for (var i = 0; i < res.length; i++) {
                    if (res[i] != "") {
                        if (doctorList.indexOf(',' + res[i] + ',') == -1) {                           
                            var itemX = $('.ui-selected').find('.row' + res[i]);                           
                            var css = $(itemX).attr("class");
                            var addCalss = "add-calendar";
                            var changedCalss = "change-calendar";                          
                            if (css.indexOf(addCalss) != -1) {                              
                                $(itemX).fadeOut(150, function () {
                                    $(itemX).css('display', 'none');
                                });
                            }
                            else {
                                var id = $(itemX).attr('id');
                                var dataDuty = "";
                                var dataDate = "";                             
                                if (css.indexOf(changedCalss) != -1) {
                                    dataDuty = $(itemX).find('.element-close').attr('data-duty');
                                    dataDate = $(itemX).find('.element-close').attr('data-date');
                                }
                                else {                                   
                                    dataDuty = $(itemX).find('.element-change').attr('data-duty');
                                    dataDate = $(itemX).find('.element-change').attr('data-date');
                                }                               
                                $(itemX).fadeOut(150, function () {
                                    $(itemX).find('span').remove();
                                    $(itemX).css('display', 'block');
                                    //Trường hợp bỏ checkbox trên popup đặt trạng thái xóa:deleted và thay đổi:changed để tìm trạng thái lưu vào CSDL
                                    $(itemX).removeClass('current-calendar').removeClass('change-calendar').addClass('del-calendar deleted').addClass('changed');
                                    $(itemX).find('a').css('color', 'red').removeClass('choseId');
                                    //Trường hợp đổi lịch      
                                    $(itemX).attr("style", "");
                                    $(itemX).find('img').remove();
                                    $(itemX).find('.changeId').remove();
                                    $(itemX).removeClass('change-calendar');
                                    $(itemX).append('<span class="element-close fa fa-rotate-left" title="Hủy xóa" data-date="' + dataDate + '" data-duty="' + dataDuty + '" data-id="' + id + '" onclick="scheduleCancelDelClick(this,1)" />');
                                });
                                //Xem đơn vị đổi lịch cùng ngày
                                var dataData = $(itemX).find('.element-close').attr('data-data');
                                var doctorIdchange = $(itemX).find('.element-close').attr('data-doctorchange');
                                var idChange = "css" + dataData + doctorIdchange;
                                if (doctorIdchange != "") {
                                    var itemChange = $('.' + idChange);
                                    $(itemChange).fadeOut(150, function () {
                                        $(itemChange).css('display', 'block').attr("style", "").addClass('changed');

                                    });

                                }
                            }
                        }
                        else {                         
                            var itemY = $('.ui-selected').find('.row' + res[i]);                           
                            var css = $(itemY).attr("class");
                            var delCalss = "del-calendar";
                            var addCalss = "add-calendar";
                        
                            //Chỉ áp dụng với trường hợp xóa
                            if (css.indexOf(delCalss) != -1 && css.indexOf("row" + res[i]) != -1) {                                
                                var id = $(itemY).find('.element-close').attr('data-id');
                                var dataDuty = $(itemY).find('.element-close').attr('data-duty');
                                var dataDate = $(itemY).find('.element-close').attr('data-date');
                                var idDoctor = $(itemY).find('a').attr('id');                                                     
                                $(itemY).css('display', 'block');
                                $(itemY).removeClass('del-calendar').addClass('current-calendar').removeClass('deleted').removeClass('changed');
                                $(itemY).attr("style", "");
                                $(itemY).find('span').remove();
                                $(itemY).find('a').css('color', '#0b55c4');                                                     
                                $(itemY).append('<span class="element-close fa fa-times-circle" title="Xóa bỏ" onclick="scheduleCloseClick(this)"/>');
                                $(itemY).append('<span id="' + idDoctor + '" data-date="' + dataDate + '" data-column="' + dataDuty + '" data-duty="' + dataDuty + '" class="element-change fa fa-pencil-square fa-lg" title="Đổi lịch trực"/>');
                                $(itemY).find('a').addClass('choseId');
                              
                            }
                           
                        }
                    }
                }
            }
        });
        function removeList(idColumnSelect, isChange) {

            $('.ui-selected').find('div').each(function () {              
                $(this).remove();
            })

        }
        // Thêm mới danh sách bác sĩ ô lựa chọn   

        function addItemToList(item, span, doctorId, idColumnSelect, vR, isChange) {
            $('.ui-selected').each(function () {
                // Effect     
                var lenghX = "100%";
                var doctorName = item.text().trim();
                lenghX = parseFloat(doctorName.length) + 5 + "px";
                var idColumn = $(this).attr('id');
                var row = "row" + doctorId;
                if (doctorId != "" && doctorId != null) {
                    if (isChange == 1) {
                        if (vR.indexOf(',' + doctorId + ',') == -1) {
                            $(this).append('<div class="schedule-element add-calendar addnew ' + row + '"  id="' + span + ',' + doctorId + '" style="width:' + lenghX + '">'
                                                         + '<a href="#" id="' + doctorId + '" class="choseId listdoctor">'
                                    + doctorName + '</a>'
                                    + '<span class="element-close fa fa-times-circle" title="Xóa bỏ" onclick="scheduleRemoveClick(this)"/>'
                                    + '</div>');
                        }
                    }
                    else {
                        $(this).append('<div class="schedule-element add-calendar addnew ' + row + '"  id="' + span + ',' + doctorId + '" style="width:' + lenghX + '">'
                                                      + '<a href="#" id="' + doctorId + '" class="choseId listdoctor">'
                                 + doctorName + '</a>'
                                 + '<span class="element-close fa fa-times-circle" title="Xóa bỏ" onclick="scheduleRemoveClick(this)"/>'
                                 + '</div>');
                    }
                }

            });
        };
        function scheduleRemoveClick(item) {         
            var itemX = $(item).parent('.schedule-element');
            var isChange = $('#isChange').val();
            $(itemX).fadeOut(150, function () {
                $(this).remove();
            });
        }     
        // Thực hiện xóa bác sĩ được chọn
        function scheduleCloseClick(item) {
            var itemX = $(item).parent('.schedule-element');
            var id = $(itemX).attr('id');
            var dataDuty = $(itemX).find('.element-change').attr('data-duty');
            var dataDate = $(itemX).find('.element-change').attr('data-date');           
            $(itemX).fadeOut(150, function () {
                $(itemX).find('span').remove();
                $(itemX).css('display', 'block');
                $(itemX).removeClass('current-calendar').addClass('del-calendar deleted');
                $(itemX).find('a').css('color', 'red').removeClass('choseId');              
                $(itemX).append('<span class="element-close fa fa-rotate-left" title="Hủy xóa" data-date="' + dataDate + '" data-duty="' + dataDuty + '" data-id="' + id + '" onclick="scheduleCancelDelClick(this,1)" />');
              
            });
        }
        function scheduleCancelDelClick(item, type) {                
            var itemX = $(item).parent('.schedule-element');          
            var dataDuty = $(itemX).find('.element-close').attr('data-duty');
            var dataDate = $(itemX).find('.element-close').attr('data-date');
            var id = $(itemX).find('.element-close').attr('data-id');
            var idDoctor = $(itemX).find('a').attr('id');
            $(itemX).fadeOut(150, function () {              
                $(itemX).find('span').remove();
                $(itemX).css('display', 'block');
                $(itemX).removeClass('del-calendar').addClass('current-calendar');               
                $(itemX).find('a').css('color', '#0b55c4').addClass('choseId');
                $(itemX).attr("style", "");
                if (type==1) {
                    $(itemX).attr("id", id);
                    $(itemX).removeClass("deleted");
                }
                else
                {
                   // $(itemX).attr("id", "");
                    $(itemX).addClass("deleted");                   
                }             
                $(itemX).append('<span class="element-close fa fa-times-circle" title="Xóa bỏ" onclick="scheduleCloseClick(this)"/>');
                $(itemX).append('<span id="' + idDoctor + '" data-date="' + dataDate + '" data-column="' + dataDuty + '" data-duty="' + dataDuty + '" class="element-change fa fa-pencil-square fa-lg" title="Đổi lịch trực"/>');

            });
        }      
        function scheduleRemoveCalendarChange(item) {
            var itemX = $(item).parent('.schedule-element');
            var id = $(itemX).attr('id');
            var dataDate = $(item).attr('data-date');
            var dataData = $(item).attr('data-data');
            var doctorIdchange = $(item).attr('data-doctorchange');
            var doctorId = $(item).attr('data-doctor');
            var doctorName = $(item).attr('data-doctorname');
            var doctorNameChange = $(item).attr('data-doctorname-change');
            var dataDuty = $(item).attr('data-duty');
            var idChange = "css" + dataData + doctorIdchange;         
            $(itemX).fadeOut(150, function () {
                $(itemX).find('span').remove();
                $(itemX).find('img').remove();
                $(itemX).find('#' + doctorIdchange).remove();
                $(itemX).css('display', 'block');
                $(itemX).attr("style", "");
                $(itemX).removeClass('change-calendar').addClass('current-calendar').addClass('changed');
                $(itemX).append('<span class="element-close fa fa-times-circle" title="Xóa bỏ" onclick="scheduleCloseClick(this)"/>');
                $(itemX).append('<span id="' + doctorId + '" data-date="' + dataDate + '" data-column="' + dataDuty + '" data-duty="' + dataDuty + '" class="element-change fa fa-pencil-square fa-lg" title="Đổi lịch trực"/>');
                           
            });
            if (doctorIdchange != "") {
                var itemChange = $('.' + idChange);
                var dataDateChange = $(itemChange).find('.element-close').attr('data-date');
                $(itemChange).fadeOut(150, function () {
                    $(itemChange).find('span').remove();
                    $(itemChange).find('img').remove();
                    $(itemChange).find('#' + doctorId).remove();
                    $(itemChange).css('display', 'block');
                    $(itemChange).attr("style", "");               
                    $(itemChange).removeClass('change-calendar').addClass('current-calendar').addClass('changed');
                    $(itemChange).append('<span class="element-close fa fa-times-circle" title="Xóa bỏ" onclick="scheduleCloseClick(this)"/>');
                    $(itemChange).append('<span id="' + doctorIdchange + '" data-date="' + dataDateChange + '" data-column="' + dataDuty + '" data-duty="' + dataDuty + '" class="element-change fa fa-pencil-square fa-lg" title="Đổi lịch trực"/>');

                });

            }
           
        }
        function scheduleRestoreCalendarChange(item) {
            var itemX = $(item).parent('.schedule-element');
            var id = $(item).attr('data-id');
            var dataDate = $(item).attr('data-date');
            var doctorIdchange = $(item).attr('data-doctorchange');
            var doctorId = $(item).attr('data-doctor');
            var doctorName = $(item).attr('data-doctorname');
            var doctorNameChange = $(item).attr('data-doctorname-change');
            var dateChage = dataDate + "_" + doctorIdchange;          
            $(itemX).fadeOut(150, function () {               
                $(itemX).css('display', 'block');
                $(itemX).attr("id", id);
                $(itemX).find('a').remove();
                $(itemX).find('span').remove();
                $(itemX).append('<a id="' + doctorId + '" href="#">' + doctorName + '</a>');
                $(itemX).append('<img style="width:12px; height:12px;" src="/Images/Forward_Arrow.png"><a id="' + doctorIdchange + '" href="#">' + doctorNameChange + '</a>');
                $(itemX).append('<span class="element-close fa fa-rotate-left" data-doctorname-change="' + doctorNameChange + '" data-doctorname="' + doctorName
                    + '" data-doctor="' + doctorId + '" data-doctorchange="' + doctorIdchange + '" data-date="' + dataDate + '" onclick="scheduleRemoveCalendarChange(this)" title="Hủy đổi lịch trực" style="display: none;"></span>');
            });

        }

   
