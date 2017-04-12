(function (bmcr, $, _, undefined) {
    if (typeof ($) === undefined) {
        throw 'Knockout is required, please ensure it is loaded before loading this plug-in';
    }
    if (typeof (_) === undefined) {
        throw 'Underscore is required, please ensure it is loaded before loading this plug-in';
    }
    var strips =
            [
                "áàảãạăắằẳẵặâấầẩẫậ",
                "ÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬ",
                "đ",
                "Đ",
                "éèẻẽẹêếềểễệ",
                "ÉÈẺẼẸÊẾỀỂỄỆ",
                "íìỉĩị",
                "ÍÌỈĨỊ",
                "óòỏõọơớờởỡợôốồổỗộ",
                "ÓÒỎÕỌƠỚỜỞỠỢÔỐỒỔỖỘ",
                "ưứừửữựúùủũụ",
                "ƯỨỪỬỮỰÚÙỦŨỤ",
                "ýỳỷỹỵ",
                "ÝỲỶỸỴ"
            ];

    var replacements =
    [
        'a',
        'A',
        'd',
        'D',
        'e',
        'E',
        'i',
        'I',
        'o',
        'O',
        'u',
        'U',
        'y',
        'Y'
    ];
    bmcr.checkboxUtils = {};
    bmcr.checkboxUtils.checkAndUnCheckAll = function (allitems, item) {
        //Check all items
        allitems.click(function () {
            item.attr('checked', this.checked);
        });
        //Item check
        item.click(function () {
            var countCheck = 0;
            var countChecked = 0;
            item.each(function () {
                countCheck++;
                if (this.checked == true) {
                    countChecked++;
                }
            });
            allitems.attr('checked', countCheck == countChecked);
        });
    };


    //Các hàm tiện ích trên table
    bmcr.tableUtils = {};
    bmcr.tableUtils.setWidthColTable = function (itemTable, numCol) {
        var widthTable = $('#'+itemTable).width();
        var colWidth = parseFloat(widthTable) / numCol;
        $('#'+itemTable +' tr td').each(function () {
            $(this).width(parseFloat(colWidth).toFixed());
        });
    };
   

    // Các hàm xử lý phím đặc biệt
    bmcr.shortkeyUtils = {};
    bmcr.shortkeyUtils.defaultButton = function (event, target) {
        if (event.keyCode == 13) {
            var src = event.srcElement || event.target;
            if (!src || (src.tagName.toLowerCase() == "input") || (src.tagName.toLowerCase() == "submit") || src.tagName.toLowerCase() =="select") {
                var defaultButton;
                if (__nonMSDOMBrowser) {
                    defaultButton = $(target);
                }
                else {
                    defaultButton = document.all[target];
                }
                if (defaultButton && typeof (defaultButton.click) != "undefined") {
                    defaultButton.click();
                    event.cancelBubble = true;
                    if (event.stopPropagation) event.stopPropagation();
                    return false;
                }
            }
        }
    };

    // Các hàm xử lý chuỗi
    bmcr.stringUtils = {};
    bmcr.stringUtils.stripVietnameseChars = function (input) {
        var stringBuilder = [];
        for (var k = 0; k < input.length; k++) {
            stringBuilder.push(input.charAt(k));
        }
        for (var i = 0; i < stringBuilder.length; i++) {
            for (var j = 0; j < strips.length; j++) {
                if (strips[j].indexOf(stringBuilder[i]) > -1) {
                    stringBuilder[i] = replacements[j];
                }
            }
        }
        return stringBuilder.join("");
    };

    bmcr.validateUtils = {};

    bmcr.validateUtils.dateTimeRange = function (objDateFrom, objDateTo) {
        var lstDate = (objDateFrom.value).split('/');
        if (lstDate.length == 0)
            return true;
        var dateFrom = new Date(lstDate[2] + '-' + lstDate[1] + '-' + lstDate[0]);
        lstDate = (objDateTo.value).split('/');
        if (lstDate.length == 0)
            return true;
        var dateTo = new Date(lstDate[2] + '-' + lstDate[1] + '-' + lstDate[0]);
        if (dateFrom > dateTo) {
            bmcr.Utils.message.showAlertMsg('Thời gian từ ngày phải nhỏ hơn hoặc bằng thời gian đến ngày!');
            $('.modalpopup').css('width','420px');
            $('.footerConfirmDialog input').click(function () {
                objDateTo.focus();
            });
            return false;
        }
        return true;
    };

    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    $.fn.toJSON = function (options) {

        options = $.extend({}, options);

        var self = this,
            json = {},
            push_counters = {},
            patterns = {
                "validate": /^[a-zA-Z][a-zA-Z0-9_]*(?:\[(?:\d*|[a-zA-Z0-9_]+)\])*$/,
                "key": /[a-zA-Z0-9_]+|(?=\[\])/g,
                "push": /^$/,
                "fixed": /^\d+$/,
                "named": /^[a-zA-Z0-9_]+$/
            };


        this.build = function (base, key, value) {
            base[key] = value;
            return base;
        };

        this.push_counter = function (key) {
            if (push_counters[key] === undefined) {
                push_counters[key] = 0;
            }
            return push_counters[key]++;
        };

        $.each($(this).serializeArray(), function () {

            // skip invalid keys
            if (!patterns.validate.test(this.name)) {
                return;
            }

            var k,
                keys = this.name.match(patterns.key),
                merge = this.value,
                reverse_key = this.name;

            while ((k = keys.pop()) !== undefined) {

                // adjust reverse_key
                reverse_key = reverse_key.replace(new RegExp("\\[" + k + "\\]$"), '');

                // push
                if (k.match(patterns.push)) {
                    merge = self.build([], self.push_counter(reverse_key), merge);
                }

                    // fixed
                else if (k.match(patterns.fixed)) {
                    merge = self.build([], k, merge);
                }

                    // named
                else if (k.match(patterns.named)) {
                    merge = self.build({}, k, merge);
                }
            }

            json = $.extend(true, json, merge);
        });

        return json;
    };

    $.fn.extend({
        trackChanges: function () {
            $(":input,select,textarea", this).change(function () {
                $(this.form).data("changed", true);
            });
        }
        ,
        isChanged: function () {
            return this.data("changed");
        }
    });

})(window.bmcr = window.bmcr || {}, window.jQuery, window._);
