
// Hàm Convert To Date theo định dạng
// Ví dụ: 02/02/2014 ,"dd/MM/yyyy"
function getDateFromFormat(val) {
    if (val == null || val.length == 0)
        return null;
    val = val.trim();
    val = val.split("/");

    return val[2] + "/" + val[1] + "/" + val[0];
}