jQuery.validator.unobtrusive.adapters.add
    ("phonenumber",[],
    function (options) {
        options.rules['phonenumber'] = {
            other: options.params.other
        };
        options.messages['phonenumber'] = options.message;
    }
);

jQuery.validator.unobtrusive.adapters.add
    ("strongpassword", [],
    function (options) {
        options.rules['strongpassword'] = {
            other: options.params.other
        };
        options.messages['strongpassword'] = options.message;
    }
);

jQuery.validator.addMethod("phonenumber", function (value, element, params) {
    var patt = /^\+?[0-9]{7,15}$/i;
    return value == null || value == "" || patt.test(value);
});

jQuery.validator.addMethod("strongpassword", function (value, element, params) {
    var patt = /(?=^.{6,100}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{:;'?/>.<,])(?!.*\s)/;
    return value == null || value == "" || patt.test(value);
});
