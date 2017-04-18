; (function ($, window, document, undefined) {

    var pluginName = "metisMenu",
        defaults = {
            toggle: true
        };

    function Plugin(element, options) {

        /*
        // Set current menu active
        // Author: Hungcd1
        // Date: 09:31 06-05-2014
        ************************************
        */
        var menu = $(element).find("li [href='" + window.location.pathname + "']").parent();
        var parent;
        if (menu.size() > 0 && menu.hasClass('li-nav-third-level')) {
            menu.addClass('active');
            parent = menu.parent().parent();
            parent.addClass('active');
            parent = parent.parent().parent();
            parent.addClass('active');
        }
        else if (menu.size() > 0 && menu.hasClass('li-nav-second-level')) {
            menu.addClass('active');
            parent = menu.parent().parent();
            parent.addClass('active');
        }
        //****************************//

        this.element = element;
        this.settings = $.extend({}, defaults, options);
        this._defaults = defaults;
        this._name = pluginName;
        this.init();
    }

    Plugin.prototype = {
        init: function () {

            var $this = $(this.element),
                $toggle = this.settings.toggle;

            $this.find('li.active').has('ul').children('ul').addClass('collapse in');
            $this.find('li').not('.active').has('ul').children('ul').addClass('collapse');

            $this.find('li').has('ul').children('a').on('click', function (e) {
                e.preventDefault();

                $(this).parent('li').toggleClass('active').children('ul').collapse('toggle');

                if ($toggle) {
                    $(this).parent('li').siblings().removeClass('active').children('ul.in').collapse('hide');
                }
            });
        }
    };

    $.fn[pluginName] = function (options) {
        return this.each(function () {
            if (!$.data(this, "plugin_" + pluginName)) {
                $.data(this, "plugin_" + pluginName, new Plugin(this, options));
            }
        });
    };

})(jQuery, window, document);
