var numberUtils = {
 
    mathRound: function (num, decimals) {
        return Math.round(num * Math.pow(10, decimals)) / Math.pow(10, decimals);
    },

    //config WHO_weight girl and boy 0-60 month
    minBoyWeight: [2.1, 2.9, 3.8, 4.4, 4.9, 5.3, 5.7, 5.9, 6.2, 6.4, 6.6, 6.8, 6.9, 7.1, 7.2, 7.4, 7.5, 7.7, 7.8, 8.0, 8.1, 8.2, 8.4, 8.5, 8.6, 8.8, 8.9, 9.0, 9.1, 9.2, 9.4, 9.5, 9.6, 9.7, 9.8, 9.9, 10.0, 10.1, 10.2, 10.3, 10.4, 10.5, 10.6, 10.7, 10.8, 10.9, 11.0, 11.1, 11.2, 11.3, 11.4, 11.5, 11.6, 11.7, 11.8, 11.9, 12.0, 12.1, 12.2, 12.3, 12.4],
    maxBoyWeight: [5.0, 6.6, 8.0, 9.0, 9.7, 10.4, 10.9, 11.4, 11.9, 12.3, 12.7, 13.0, 13.3, 13.7, 14.0, 14.3, 14.6, 14.9, 15.3, 15.6, 15.9, 16.2, 16.5, 16.8, 17.1, 17.5, 17.8, 18.1, 18.4, 18.7, 19.0, 19.3, 19.6, 19.9, 20.2, 20.4, 20.7, 21.0, 21.3, 21.6, 21.9, 22.1, 22.4, 22.7, 23.0, 23.3, 23.6, 23.9, 24.2, 24.5, 24.8, 25.1, 25.4, 25.7, 26.0, 26.3, 26.6, 26.9, 27.2, 27.6, 27.9],
    minGirlWeight: [2.0, 2.7, 3.4, 4.0, 4.4, 4.8, 5.1, 5.3, 5.6, 5.8, 5.9, 6.1, 6.3, 6.4, 6.6, 6.7, 6.9, 7.0, 7.2, 7.3, 7.5, 7.6, 7.8, 7.9, 8.1, 8.2, 8.4, 8.5, 8.6, 8.8, 8.9, 9.0, 9.1, 9.3, 9.4, 9.5, 9.6, 9.7, 9.8, 9.9, 10.1, 10.2, 10.3, 10.4, 10.5, 10.6, 10.7, 10.8, 10.9, 11.0, 11.1, 11.2, 11.3, 11.4, 11.5, 11.6, 11.7, 11.8, 11.9, 12.0, 12.1],
    maxGirlWeight: [4.8, 6.2, 7.5, 8.5, 9.3, 10.0, 10.6, 11.1, 11.6, 12.0, 12.4, 12.8, 13.1, 13.5, 13.8, 14.1, 14.5, 14.8, 15.1, 15.4, 15.7, 16.0, 16.4, 16.7, 17.0, 17.3, 17.7, 18.0, 18.3, 18.7, 19.0, 19.3, 19.6, 20.0, 20.3, 20.6, 20.9, 21.3, 21.6, 22.0, 22.3, 22.7, 23.0, 23.4, 23.7, 24.1, 24.5, 24.8, 25.2, 25.5, 25.9, 26.3, 26.6, 27.0, 27.4, 27.7, 28.1, 28.5, 28.8, 29.2, 29.5],

 
    //return 3: beo phi
    //return 2: binh thuong
    //return 1: suy dinh duong
    //return 0: khong xac dinh
    calculatorNutrition: function (weight, month, isBoy) {
        if (month < 0 || month > 60) {
            return 0;
        }
        if (isBoy == true) {
            if (weight > numberUtils.maxBoyWeight[month]) {
                return 3;
            }
            if (weight < numberUtils.minBoyWeight[month]) {
                return 1;
            }
            return 2;
        }
        else {
            if (weight > numberUtils.maxGirlWeight[month]) {
                return 3;
            }
            if (weight < numberUtils.minGirlWeight[month]) {
                return 1;
            }
            return 2;
        }
    },

    FilterMarkForPrimary: function (textVal) {
        if (!textVal) return "";

        textVal = textVal.replace(/[^\d]/gi, "");

        if (textVal.match(/^0[^0]+/))
            textVal = textVal.replace(/^0+/, "");

        if (textVal.match(/^0+/))
            textVal = "0";

        if (textVal.match(/^10\d+/))
            textVal = "10";

        if (textVal.match(/[^0]+/) && textVal != "10")
            textVal = textVal[0];

        return textVal;
    },

    FilterMarkForSecondary: function (textVal) {
        if (!textVal) return "";

        // . -> ,
        textVal = textVal.replace(/\./g, ",");

        //If contain double comma, return false
        if (textVal.match(/^.*,+.*,+.*$/)) {
            alert("Error number");
            return "";
        }

        // a-zA-Z... -> ''
        textVal = textVal.replace(/[^(\d|,)]/gi, "");

        // 0000 -> 0.0
        if (textVal.match(/^0+$/))
            textVal = "0";

        // 10.xxxxx -> 10.0
        if (textVal.match(/^10,\d*$/))
            textVal = "10";

        // xxxxxx.xxxxxx -> x.x
        if (textVal.match(/^\d+\,\d+$/) && !textVal.match(/^10,\d+$/)) {
            var pointIndex = textVal.indexOf(",");
            textVal = textVal.substring(pointIndex - 1, pointIndex) + "," + textVal.substring(pointIndex + 1, pointIndex + 2);
        }

        // 00000xxxxx -> xxxxx
        if (textVal.match(/^0+\d+$/))
            textVal = textVal.replace(/^0+/, "");

        // 10xxxxxx-> 10.0
        if (textVal.match(/^10\d*$/))
            textVal = "10";

        // xxxxx -> x
        if (textVal.match(/^\d+$/) && textVal != "10")
            textVal = textVal.substring(0, 1);

        return textVal;
    },

    roundNumber: function roundNumber(rnum, rlength) {
        var newnumber = Math.round(rnum * Math.pow(10, rlength)) / Math.pow(10, rlength);
        return parseFloat(newnumber);
    }
};