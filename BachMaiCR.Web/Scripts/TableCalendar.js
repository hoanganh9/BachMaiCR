var $table = $('table.scrollzx'),
             $bodyCells = $table.find('tbody tr:first').children(),
             colWidth;

// Adjust the width of thead cells when window resizes
$(window).resize(function () {
   
    // Get the tbody columns width array
    colWidth = $bodyCells.map(function () {
        return $(this).width();
    }).get();

    // Set the width of thead columns
    $table.find('thead tr').children().each(function (i, v) {
       $(v).width(colWidth[i]);
    });
}).resize();


$(function () {
    var heightX = ($(window).height() - 390).toString() + "px";
   // $('.scrollz').parent().css({ "height": heightX,"overflow":"auto" });
    //$('.scrollz').find('tbody').css({ "height": heightX });
});


