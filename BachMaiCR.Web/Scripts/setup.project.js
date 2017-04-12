/*
setup.project.js
Tạo bởi: hiepth6 - 2013
Nếu đoạn mã trong tệp này có vấn đề, hoặc bạn có đoạn mã tốt hơn hãy liên hệ với tác giả qua SĐT:0982358831 hoặc TĐT:hiepth6@viettel.com.vn
Bản quyền thuộc về Viettel-IT

Toàn bộ các cấu hình cho jquery và javascript của project đều được đặt trong file này
File này buộc phải để sau file jquery-{version}.js

- Bao gồm cấu hình jquery-ajax để tự động lấy token khi một hoặc nhiều ajax-request được gọi
- Cấu hình để tạo ra hiệu ứng tải trang cho các ajax-request
*/

$(document).ready(function () {
    ///*
    //    ajax loading
    //*/
    //$(document).ajaxStart(function () {
    //    if ($("#loadingbar").length === 0) {
    //        $("body").append("<div id='loadingbar'></div>");
    //        $("#loadingbar").addClass("waiting").append($("<dt/><dd/>"));
    //        $("#loadingbar").width((50 + Math.random() * 30) + "%");
    //    }
    //}).ajaxStop(function () {
    //    $("#loadingbar").width("101%").delay(200).fadeOut(400, function () {
    //        $(this).remove();
    //    });
    //}).ajaxComplete(function () {
    //    // $('#loadingbar').remove();
    //}).ajaxError(function () {
    //    $('#loadingbar').remove();
    //});

    /* token */
    $.ajaxSetup({
        beforeSend: function (jqXHR) {
            var token = $('input[name=__RequestVerificationToken]').last().val();
            jqXHR.setRequestHeader("__RequestVerificationToken", token);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status == 403) {
                window.location = '/Error/ShowErrorPage';
            }
            else if (jqXHR.status == 500) {

            }
        }
    });

    var $page_container = $('#page-container');
    var $page_nav_content = $('#page-nav-content');
    var $page_nav_left = $('#page-nav-left');
    var $page_footer = $('#page-footer');
    var $page_header = $('#page-header');
    var $page_login = $('#top-bar-login');
    var $page_paging = $('#page-paging');
    var $page_menu_action = $('#page-menu-action');
    /* refrest layout */
    var refreshLayout = function () {

        var viewportHeight = window.innerHeight; //$(window).height();        
        var viewportWith = window.innerWidth;//$(window).width();//;
        var left_width = (0.1 * viewportWith);
        if (left_width < 200) {
            left_width = 200;
        }
        var right_width = viewportWith - left_width;
        if ($page_container) {
            $page_container.css('height', viewportHeight + "px");
            var content_height = viewportHeight - $page_footer.height() - $page_header.height() - 35;
            if (content_height < 500) {
                content_height = 500;
            }           
            $page_nav_content.css('height', content_height + "px");
            $page_nav_left.css('margin-top', "35px");
            $page_nav_left.css('height', content_height + "px");
            $page_nav_left.css('width', left_width + "px");

            $page_nav_content.css('width', right_width + "px");
            $page_nav_content.css('margin-left', left_width + "px");
            $page_nav_content.css('margin-top', "35px");
            $page_nav_content.css('position', "absolute");
            $page_nav_content.css('overflow-x', "scroll");
        }
        if ($page_paging) {
            $page_paging.css('bottom', '0px');
            var fixWidth = 200;
            $page_paging.css('left', (left_width + fixWidth / 2) + "px");
            $page_paging.css('width', (right_width - fixWidth) + "px");
            $page_menu_action.css('left', left_width + "px");
            $page_menu_action.css('max-width', right_width + "px");
        }
        $page_nav_content.css('opacity', '1');
    };
    refreshLayout();
    $(window).resize(function () {
        refreshLayout();
    });
});