var stringUtils = {
  
    StripVNSign: function (value) {
        //mũ
        value = value.replaceAll('Ă', 'A');
        value = value.replaceAll('ă', 'a');
        value = value.replaceAll('Â', 'A');
        value = value.replaceAll('â', 'a');
        value = value.replaceAll('Ê', 'E');
        value = value.replaceAll('ê', 'e');
        value = value.replaceAll('I', 'I');
        value = value.replaceAll('i', 'i');
        value = value.replaceAll('Ô', 'O');
        value = value.replaceAll('ô', 'o');
        value = value.replaceAll('Ơ', 'O');
        value = value.replaceAll('ơ', 'o');
        value = value.replaceAll('Ư', 'U');
        value = value.replaceAll('ư', 'u');

        //Dấu Huyền
        value = value.replaceAll('À', 'A');
        value = value.replaceAll('à', 'a');
        value = value.replaceAll('Ằ', 'A');
        value = value.replaceAll('ằ', 'a');
        value = value.replaceAll('Ầ', 'A');
        value = value.replaceAll('ầ', 'a');
        value = value.replaceAll('È', 'E');
        value = value.replaceAll('è', 'e');
        value = value.replaceAll('Ề', 'E');
        value = value.replaceAll('ề', 'e');
        value = value.replaceAll('Ì', 'I');
        value = value.replaceAll('ì', 'i');
        value = value.replaceAll('Ò', 'O');
        value = value.replaceAll('ò', 'o');
        value = value.replaceAll('Ồ', 'O');
        value = value.replaceAll('ồ', 'o');
        value = value.replaceAll('Ờ', 'O');
        value = value.replaceAll('ờ', 'o');
        value = value.replaceAll('Ù', 'U');
        value = value.replaceAll('ù', 'u');
        value = value.replaceAll('Ừ', 'U');
        value = value.replaceAll('ừ', 'u');
        value = value.replaceAll('Ỳ', 'Y');
        value = value.replaceAll('ỳ', 'y');

        //Dấu Sắc
        value = value.replaceAll('Á', 'A');
        value = value.replaceAll('á', 'a');
        value = value.replaceAll('Ắ', 'A');
        value = value.replaceAll('ắ', 'a');
        value = value.replaceAll('Ấ', 'A');
        value = value.replaceAll('ấ', 'a');
        value = value.replaceAll('É', 'E');
        value = value.replaceAll('é', 'e');
        value = value.replaceAll('Ế', 'E');
        value = value.replaceAll('ế', 'e');
        value = value.replaceAll('Í', 'I');
        value = value.replaceAll('í', 'i');
        value = value.replaceAll('Ó', 'O');
        value = value.replaceAll('ó', 'o');
        value = value.replaceAll('Ố', 'O');
        value = value.replaceAll('ố', 'o');
        value = value.replaceAll('Ớ', 'O');
        value = value.replaceAll('ớ', 'o');
        value = value.replaceAll('Ú', 'U');
        value = value.replaceAll('ú', 'u');
        value = value.replaceAll('Ứ', 'U');
        value = value.replaceAll('ứ', 'u');
        value = value.replaceAll('Ý', 'Y');
        value = value.replaceAll('ý', 'y');

        //Dấu Hỏi
        value = value.replaceAll('Ả', 'A');
        value = value.replaceAll('ả', 'a');
        value = value.replaceAll('Ẳ', 'A');
        value = value.replaceAll('ẳ', 'a');
        value = value.replaceAll('Ẩ', 'A');
        value = value.replaceAll('ẩ', 'a');
        value = value.replaceAll('Ẻ', 'E');
        value = value.replaceAll('ẻ', 'e');
        value = value.replaceAll('Ể', 'E');
        value = value.replaceAll('ể', 'e');
        value = value.replaceAll('Ỉ', 'I');
        value = value.replaceAll('ỉ', 'i');
        value = value.replaceAll('Ỏ', 'O');
        value = value.replaceAll('ỏ', 'o');
        value = value.replaceAll('Ổ', 'O');
        value = value.replaceAll('ổ', 'o');
        value = value.replaceAll('Ở', 'O');
        value = value.replaceAll('ở', 'o');
        value = value.replaceAll('Ủ', 'U');
        value = value.replaceAll('ủ', 'u');
        value = value.replaceAll('Ử', 'U');
        value = value.replaceAll('ử', 'u');
        value = value.replaceAll('Ỷ', 'Y');
        value = value.replaceAll('ỷ', 'y');

        //Dấu Ngã   
        value = value.replaceAll('Ã', 'A');
        value = value.replaceAll('ã', 'a');
        value = value.replaceAll('Ẵ', 'A');
        value = value.replaceAll('ẵ', 'a');
        value = value.replaceAll('Ẫ', 'A');
        value = value.replaceAll('ẫ', 'a');
        value = value.replaceAll('Ẽ', 'E');
        value = value.replaceAll('ẽ', 'e');
        value = value.replaceAll('Ễ', 'E');
        value = value.replaceAll('ễ', 'e');
        value = value.replaceAll('Ĩ', 'I');
        value = value.replaceAll('ĩ', 'i');
        value = value.replaceAll('Õ', 'O');
        value = value.replaceAll('õ', 'o');
        value = value.replaceAll('Ỗ', 'O');
        value = value.replaceAll('ỗ', 'o');
        value = value.replaceAll('Ỡ', 'O');
        value = value.replaceAll('ỡ', 'o');
        value = value.replaceAll('Ũ', 'U');
        value = value.replaceAll('ũ', 'u');
        value = value.replaceAll('Ữ', 'U');
        value = value.replaceAll('ữ', 'u');
        value = value.replaceAll('Ỹ', 'Y');
        value = value.replaceAll('ỹ', 'y');

        //Dẫu Nặng
        value = value.replaceAll('Ạ', 'A');
        value = value.replaceAll('ạ', 'a');
        value = value.replaceAll('Ặ', 'A');
        value = value.replaceAll('ặ', 'a');
        value = value.replaceAll('Ậ', 'A');
        value = value.replaceAll('ậ', 'a');
        value = value.replaceAll('Ẹ', 'E');
        value = value.replaceAll('ẹ', 'e');
        value = value.replaceAll('Ệ', 'E');
        value = value.replaceAll('ệ', 'e');
        value = value.replaceAll('Ị', 'I');
        value = value.replaceAll('ị', 'i');
        value = value.replaceAll('Ọ', 'O');
        value = value.replaceAll('ọ', 'o');
        value = value.replaceAll('Ộ', 'O');
        value = value.replaceAll('ộ', 'o');
        value = value.replaceAll('Ợ', 'O');
        value = value.replaceAll('ợ', 'o');
        value = value.replaceAll('Ụ', 'U');
        value = value.replaceAll('ụ', 'u');
        value = value.replaceAll('Ự', 'U');
        value = value.replaceAll('ự', 'u');
        value = value.replaceAll('Ỵ', 'Y');
        value = value.replaceAll('ỵ', 'y');
        value = value.replaceAll('Đ', 'D');
        value = value.replaceAll('đ', 'd');
        return value;
    },

  
    ToShortSMSContent: function (strContent, numToShort) {
        if (strContent.length == 0) {
            return '';
        }

        maxLength = numToShort || numToShortSMS;  // numToShortSMS duoc khai bao o layout     
        if (strContent.length <= maxLength) {
            return strContent;
        }
        else {
            var arrContent = strContent.split(' ');
            if (arrContent.length == 1) {
                if (arrContent[0].length >= maxLength) {
                    return arrContent[0].substring(0, maxLength) + '...';
                }
                else {
                    return arrContent[0];
                }
            }
            else {
                var shortContent = '';
                for (var i = 0, size = arrContent.length; i < size; i++) {
                    if (shortContent.length > maxLength || i == size - 1) {
                        shortContent = shortContent.trim() + '...';
                        break;
                    }
                    else {
                        shortContent = shortContent + arrContent[i] + ' ';
                    }
                }
                return shortContent;
            }
        }
    },
 
    HtmlDecode: function (value) {
        if (value.trim() == '') {
            return '';
        }
        if (value.indexOf("&#192;") > 0)
            value = value.replaceAll("&#192;", "À").trim();
        if (value.indexOf("&#193;") > 0)
            value = value.replaceAll("&#193;", "Á").trim();
        if (value.indexOf("&#194;") > 0)
            value = value.replaceAll("&#194;", "Â").trim();
        if (value.indexOf("&#195;") > 0)
            value = value.replaceAll("&#195;", "Ã").trim();
        if (value.indexOf("&#200;") > 0)
            value = value.replaceAll("&#200;", "È").trim();
        if (value.indexOf("&#201;") > 0)
            value = value.replaceAll("&#201;", "É").trim();
        if (value.indexOf("&#202;") > 0)
            value = value.replaceAll("&#202;", "Ê").trim();
        if (value.indexOf("&#204;") > 0)
            value = value.replaceAll("&#204;", "Ì").trim();
        if (value.indexOf("&#205;") > 0)
            value = value.replaceAll("&#205;", "Í").trim();
        if (value.indexOf("&#208;") > 0)
            value = value.replaceAll("&#208;", "Đ").trim();
        if (value.indexOf("&#210;") > 0)
            value = value.replaceAll("&#210;", "Ò").trim();
        if (value.indexOf("&#211;") > 0)
            value = value.replaceAll("&#211;", "Ó").trim();
        if (value.indexOf("&#212;") > 0)
            value = value.replaceAll("&#212;", "Ô").trim();
        if (value.indexOf("&#213;") > 0)
            value = value.replaceAll("&#213;", "Õ").trim();
        if (value.indexOf("&#217;") > 0)
            value = value.replaceAll("&#217;", "Ù").trim();
        if (value.indexOf("&#218;") > 0)
            value = value.replaceAll("&#218;", "Ú").trim();
        if (value.indexOf("&#221;") > 0)
            value = value.replaceAll("&#221;", "Ý").trim();
        if (value.indexOf("&#224;") > 0)
            value = value.replaceAll("&#224;", "à").trim();
        if (value.indexOf("&#225;") > 0)
            value = value.replaceAll("&#225;", "á").trim();
        if (value.indexOf("&#226;") > 0)
            value = value.replaceAll("&#226;", "â").trim();
        if (value.indexOf("&#227;") > 0)
            value = value.replaceAll("&#227;", "ã").trim();
        if (value.indexOf("&amp;#7841;") > 0)
            value = value.replaceAll("&amp;#7841;", "ạ").trim();
        if (value.indexOf("&#232;") > 0)
            value = value.replaceAll("&#232;", "è").trim();
        if (value.indexOf("&#233;") > 0)
            value = value.replaceAll("&#233;", "é").trim();
        if (value.indexOf("&#234;") > 0)
            value = value.replaceAll("&#234;", "ê").trim();
        if (value.indexOf("&#236;") > 0)
            value = value.replaceAll("&#236;", "ì").trim();
        if (value.indexOf("&#237;") > 0)
            value = value.replaceAll("&#237;", "í").trim();
        if (value.indexOf("&#242;") > 0)
            value = value.rea("&#242;", "ò").trim();
        if (value.indexOf("&#243;") > 0)
            value = value.replaceAll("&#243;", "ó").trim();
        if (value.indexOf("&#244;") > 0)
            value = value.replaceAll("&#244;", "ô").trim();
        if (value.indexOf("&#245;") > 0)
            value = value.replaceAll("&#245;", "õ").trim();
        if (value.indexOf("&#249;") > 0)
            value = value.replaceAll("&#249;", "ù").trim();
        if (value.indexOf("&#250;") > 0)
            value = value.replaceAll("&#250;", "ú").trim();
        if (value.indexOf("&#253;") > 0)
            value = value.replaceAll("&#253;", "ý").trim();
        if (value.indexOf("&lt;") > 0)
            value = value.replaceAll("&lt;", "<").trim();
        if (value.indexOf("&gt;") > 0)
            value = value.replaceAll("&gt;", ">").trim();
        return value;
    },

    replaceAll: function (source, stringToFind, stringToReplace) {
        var temp = source;
        var index = temp.indexOf(stringToFind);
        while (index != -1) {
            temp = temp.replace(stringToFind, stringToReplace);
            index = temp.indexOf(stringToFind);
        }
        return temp;
    }
};