var dateUtils = {
   // convert datetime with format, default format ='dd/MM/yyyy'
    convertDateTime: function (value, format) {
        if (value == '') {
            return '';
        }

        format = format || 'dd/MM/yyyy';

        var d = new Date(value);

        minutes = d.getMinutes();

        hours = d.getHours();

        date = d.getDate();
        date = date < 10 ? "0" + date : date;

        mon = d.getMonth() + 1;
        mon = mon < 10 ? "0" + mon : mon;

        year = d.getFullYear()

        if (format.trim() == 'dd/MM/yyyy') {
            return (date + "/" + mon + "/" + year);
        }
        else if (format.trim() == 'MM/dd/yyyy') {
            return (mon + "/" + date + "/" + year);
        }
        else if (format.trim() == 'yyyy/MM/dd') {
            return (year + "/" + mon + "/" + date);
        }
        else if (format.trim() == 'yyyy/dd/MM') {
            return (year + "/" + date + "/" + mon);
        }
        else if (format.trim() == 'MM/yyyy') {
            return (mon + '/' + year);
        }
        else if (format.trim() == 'HH:mm, dd/MM/yyyy')
            return (hours + ':' + minutes + ', ' + date + "/" + mon + "/" + year);
    },

    ParseDate: function (strDate) {
        if (strDate != null && strDate != "") {
            arr = strDate.split("/");
            return new Date(arr[2], arr[1], arr[0]);
        }
        return null;
    },

    convertDate: function (value) {
        if ($.type(value) == 'string') {
            var dateCheck = "/Date("
            var pos = value.indexOf(dateCheck);
            if (pos != -1) {
                try {
                    var miliSeconds = parseInt(value.substring(dateCheck.length, value.length - 2));
                    var date = new Date(miliSeconds);
                    var year = date.getFullYear();
                    var month = date.getMonth() + 1;
                    month = month.toString();
                    if (month.length == 1) {
                        month = "0" + month;
                    }
                    var day = date.getDate().toString();
                    if (day.length == 1) {
                        day = "0" + day;
                    }
                    var strDateTime = day + "/" + month + "/" + year;
                    return strDateTime;
                } catch (err) {
                    // ignored
                }
            }
        }
    },

    getCurrentDate: function () {
        var curDate = new Date();
        var day = curDate.getDate();
        var month = curDate.getMonth() + 1;
        var year = curDate.getFullYear();

        var dayStr = day < 10 ? "0" + day : day;
        var monthStr = month < 10 ? "0" + month : month;

        return dayStr + "/" + monthStr + "/" + year;
    },

    formatDate: function formatDateString(s) {
        var pad = function (x) { return ((x.length < 2) ? "0" : "") + x; }
    , dt = new Date("" + s), d, m, y;
        if (dt.getTime()) {
            d = pad(dt.getDate());
            m = pad(dt.getMonth() + 1);
            y = dt.getFullYear();
            return [d, m, y].join('/');
        }
        return null;
    },
    verifyDate: function (strDate) {
        if (strDate != null || strDate != '') {
            var comp = strDate.split('/');
            var d = parseInt(comp[0], 10);
            var m = parseInt(comp[1], 10);
            var y = parseInt(comp[2], 10);
            var date = new Date(y, m - 1, d);
            if (date.getFullYear() == y && date.getMonth() + 1 == m && date.getDate() == d) {
                return true;
            } else {
                return false;
            }
        }
        return false;
    }
};

function checkMaxLen(txt, maxLen) {
    try {
        if (txt.value.length > (maxLen - 1)) {
            var cont = txt.value;
            txt.value = cont.substring(0, (maxLen - 1));
            return false;
        };
    } catch (e) {
    }
}