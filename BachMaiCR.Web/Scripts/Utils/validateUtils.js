

var validateUtils = {
    IsEditable: function (obj) {
        var input = $(obj);
        if (input.length > 0 && !input.is(":hidden") && input.css('display') != 'none'
            && !input.is(":disabled") && !input.is("[readonly]")) {
            return true;
        } else {
            return false;
        }
    },

    IsValidSelector: function (input) {
        var rquickExpr = /^(?:#([\w-]+)|(\w+)|\.([\w-]+))$/;
        var matches = rquickExpr.exec(input);
        if (matches && matches.length > 0) return true;
        return false;
    },

    isValidMark: function (input, appliedLevel, isCommenting) {    
        //Neu truyen null tra ve true
        if (typeof (input) == 'undefined' || input == null) return true;

        if (input == '') return false;

        if ((appliedLevel + '') == '1') { //Cap 1
            if ((isCommenting + '') == '1') { //Mon nhan xet
                if (input == "A" || input == "A+" || input == "B" || input == "a" || input == "a+" || input == "b")
                    return true;

                if (typeof (input) == 'string' && input.trim().length > 0) { //Kieu chuoi 
                    var retVal = true;

                    if (!validateUtils.IsValidSelector(input)) //khong phai la selector
                        return false;

                    $.each($(input), function (index, item) {
                        var textVal = $(item).val().toUpperCase();

                        if (!(textVal == "A" || textVal == "A+" || textVal == "B"))
                            retVal = false;
                    });

                    return retVal;
                }
                else
                    return false;
            } else {    //Mon tinh diem
                //Neu kieu so thi kiem tra 0 <= x <= 10
                if (typeof (input) == 'number')
                    return input >= 0 && input <= 10;

                if (typeof (input) == 'string') { //Kieu chuoi 
                    //Neu truyen truc tiep la chuoi so ("5")
                    if (input.match(/^0*\d$/) || input.match(/^0*10?$/))
                        return true;

                    if (!validateUtils.IsValidSelector(input)) //khong phai la selector
                        return false;

                    //Con lai, tham so truyen vao la selector. 
                    //Duyet cac input trong selector, kiem tra value cua cac input do
                    var retVal = true;

                    $.each($(input), function (index, item) {
                        var textVal = $(item).val();

                        if (!(textVal.match(/^0*\d$/) || textVal.match(/^0*10$/)))
                            retVal = false;
                    });

                    return retVal;
                }
                else
                    return false;
            }
        } else { //Cap 2
            if ((isCommenting + '') == '1') { //Mon nhan xet
                if (input == "Đ" || input == "CĐ" || input == "đ" || input == "cđ")
                    return true;

                if (typeof (input) == 'string' && input.trim().length > 0) { //Kieu chuoi 

                    if (!validateUtils.IsValidSelector(input)) //Kiem tra dau vao la selecter
                        return false;

                    var retVal = true;

                    $.each($(input), function (index, item) {
                        var textVal = $(item).val().toUpperCase();

                        if (!(textVal == "Đ" || textVal == "CĐ"))
                            retVal = false;
                    });

                    return retVal;
                }
                else
                    return false;
            } else { //Mon tinh diem
                //Neu kieu so thi kiem tra 0 <= x <= 10
                if (typeof (input) == 'number')
                    return input >= 0 && input <= 10;

                if (typeof (input) == 'string') { //Kieu chuoi  
                    //Neu truyen truc tiep la chuoi so ("5,365")
                    if (input.match(/^0*\d(,\d*)?$/) || input.match(/^0*10(,0*)?$/))
                        return true;

                    if (!validateUtils.IsValidSelector(input)) //Kiem tra dau vao la selecter
                        return false;

                    //Con lai, tham so truyen vao la selector. 
                    //Duyet cac input trong selector, kiem tra value cua cac input do
                    var retVal = true;

                    $.each($(input), function (index, item) {
                        var textVal = $(item).val();

                        if (!(textVal.match(/^0*\d(,\d*)?$/) || textVal.match(/^0*10(,0*)?$/)))
                            retVal = false;
                    });

                    return retVal;
                }
                else
                    return false;
            }
        }
    },
    numbersOnly: function (e, id) {
        var key;
        var keychar;
        if (window.event) {
            key = window.event.keyCode;
        } else if (e) {
            key = e.which;
        } else {
            return true;
        }
        keychar = String.fromCharCode(key);

        if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27)) {
            return true;
        } else if ((("0123456789").indexOf(keychar) > -1)) {
            return true;
        } else {
            return false;
        }
    },

    fixCommonValidate: function () {
        $("input[type=text]").each(function () {
            if ($(this).attr("data-val-length-max") != null && $(this).attr("data-val-length-max") != "") {
                $(this).attr("maxlength", $(this).attr("data-val-length-max"));
            }
            $(this).blur(function () {
                this.value = $.trim(this.value);
            });


            $("input[type=text].decimal").keypress(function (e) {
                if ((e.which < 48 || e.which > 57) && e.which != 8 && e.which != 127 && e.which != 44 && e.which != 0) {
                    return false;
                }
            });

            $("input[type=text].integer").keypress(function (e) {
                if ((e.which < 48 || e.which > 57) && e.which != 8 && e.which != 127 && e.which != 0) {
                    return false;
                }
            });
        });
    },
    /**
    * Hàm không cho phép nhập kí tự đặc biệt
    **/
    checkchar: function (e) {
        if ((e.which < 48 || (e.which > 57 && e.which < 65) || (e.which > 123 && e.which < 127) || (e.which > 91 && e.which < 96)) && e.which != 8 && e.which != 44 && e.which != 0) {
            return false;
        }
        else {
            return true;
        }

    },

    validateForm: function (formId) {
        var form = $("#" + formId);

        //alert(validateUtils.ValidateDateTime($(form)));
        if (!form.valid() || validateUtils.ValidateDateTime($(form))) {
            var inputs = form.find(".input-validation-error");
            return false;
        }
       
        return true;
    },
    //ham validate hack cho ie7+8 namta
    validateSForm: function (btn) {
        var form = $(btn).closest("form");

        if (form != null && form != undefined && form.length > 0) {
            if (!form.valid() || form.find(".t-state-error").length > 0) {
                var inputs = form.find(".input-validation-error");
            } else {
                $(form).append("<input type='submit' id='frmSubmit' style='display:none'/>");
                $("#frmSubmit").click();
            }
        }
    },

    // validate sms
    validationSMS: function (idTextarea, idMsg, idLeng, idCount) {
        //fix prefix
        $('#' + idMsg).html('');
        var content = $('#' + idTextarea).val();
        //maxlength la 1280
        if (content.length > 1280) {
            content = content.substring(0, 1280);
            $('#' + idTextarea).val(content);
        }

        var SubPrefix = content.substring(0, preFixSendSMS.length); //preFixSendSMS sms duoc gan trong layout
        if (SubPrefix != preFixSendSMS) {
            $('#' + idTextarea).val(preFixSendSMS + content.substring(preFixSendSMS.length + 1, content.length));
            $('#' + idMsg).html(msgPreFixErrorSMS); // msgPrefix duoc gan gia tri trong layout
            $('#' + idMsg).css('color', 'red');
        }
        else {
            $('#' + idMsg).html('');
            $('#' + idMsg).css('color', '');
        }

        //count sms
        var lengthContent = $('#' + idTextarea).val().trim().length;
        $('#' + idLeng).html(lengthContent);
        if (lengthContent % 160 == 0) {
            $('#' + idCount).html(lengthContent / 160);
        }
        else {
            $('#' + idCount).html(parseInt(lengthContent / 160) + 1);
        }
    },


    IsValidDate: function (value) {
        if (value == null || $.trim(value) == "") {
            return true;
        }
        var check = false;
        var re = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
        if (re.test(value)) {
            var adata = value.split('/');
            var mm = parseInt(adata[1], 10);
            var dd = parseInt(adata[0], 10);
            var yyyy = parseInt(adata[2], 10);
            var xdata = new Date(yyyy, mm - 1, dd);
            if ((xdata.getFullYear() == yyyy) && (xdata.getMonth() == mm - 1) && (xdata.getDate() == dd))
                check = true;
            else
                check = false;
        } else
            check = false;
        return check;
    },
    CheckManglength: function (Text, maxthlengthValue) {
        var length = Text.value.length;
        var str = Text.value;
        if (length > maxthlengthValue) {
            str = str.substr(0, maxthlengthValue);
            Text.value = str;
        }
    },
    //validate DateTime Namta
    ValidateDateTime: function (form) {
        var ValidateDateTime;
        ValidateDateTime = false;
        $(".t-datepicker", form).each(function () {
            var inputDt = $(this).find("input");
            $(inputDt).focus();
            if ($(inputDt).not(":disabled") && $(inputDt).val()!="" && Date.parseLocale($(inputDt).val(), "dd/MM/yyyy") == null ) {

                ValidateDateTime = true;
                if ($(this).next().hasClass('field-validation-valid')) {
                    $(this).next().attr('class','field-validation-error');
                    var strValidate = '<span class="" htmlfor="' + $(this).next().attr('data-valmsg-for') + '" generated="true">';
                    strValidate = strValidate + 'Ngày tháng không đúng định dạng';
                    strValidate = strValidate + '</span>';
                    $(this).next().html(strValidate);

                }
            }
        });
        return ValidateDateTime;
    }

};