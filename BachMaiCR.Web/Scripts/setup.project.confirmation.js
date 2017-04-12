/*hiep*/
(function ($) {
    var d = $(document);
    var extend = "extend";
    var r_title = "{title}";
    var r_btnOkClass = "{btnOkClass}";
    var r_yesButtonTitle = "{yesButtonTitle}";
    var r_btnCancelClass = "{btnCancelClass}";
    var r_cancelButtonTitle = "{cancelButtonTitle}";
    var html = [
        '<div id="cfm" class="popover fade top in" style="display: block;">',
            '<div class="arrow"></div>',
            '<h3 class="popover-title">', r_title, '</h3>',
            '<div class="popover-content text-center">',
                '<div class="btn-group">',
                    '<a id="cfm-yes" class="btn btn-sm ', r_btnOkClass, '" href="javascript:void(0)">',
                    r_yesButtonTitle,
                    '</a>',
                    '<a id="cfm-cancel" class="btn btn-sm ', r_btnCancelClass, '" href="javascript:void(0)">',
                    r_cancelButtonTitle,
                    '</a>',
                '</div>',
            '</div>',
        '</div>',
    ].join('');

    function init(item, options) {
        var $cfm = $('#cfm');
        if ($cfm) {
            $cfm.remove();
        }

        var testOffset = $(item).offset();
        html = html.replace(r_title, options.title).replace(r_btnOkClass, options.btnOkClass).replace(r_yesButtonTitle, options.yesButtonTitle).replace(r_btnCancelClass, options.btnCancelClass).replace(r_cancelButtonTitle, options.cancelButtonTitle);
        $('body').append(html);
        $cfm = $('#cfm');
        $cfm.css({
            'left': (testOffset.left - $cfm.width() / 2)+ 'px',
            'top': (testOffset.top- $cfm.height()) + 'px',
            'posititon': 'absolute'
        });
        var $cfmYes = $('#cfm-yes');
        $cfmYes.unbind('click');
        $cfmYes.bind('click', function (event) {
            if (options.onComplete) {
                options.onComplete(event);
            }
            $cfm.remove();
        });
        var $cfmCancel = $('#cfm-cancel');
        $cfmCancel.unbind('click');
        $cfmCancel.bind('click', function (event) {
            if (options.onCancel) {
                options.onCancel(event);
            }
            $cfm.remove();
        });
    };

    $.fn[extend]({
        confirmation: function (options) {
            var defaults = {
                'btnOkClass': 'btn-primary',
                'btnCancelClass': 'btn-cancel',
                'title': 'Bạn có muốn thực hiện?',
                'yesButtonTitle': 'ok',
                'cancelButtonTitle': 'cancel',
                'placement': 'left',
                onComplete: null,
                onCancel: null
            };
            var merge = $[extend](defaults, options);
            return this.each(function () {
                init(this, merge);
            });
        }
    });
})(jQuery);