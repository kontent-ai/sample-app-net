; (function ($, window, document, undefined) {
    $(document).ready(function () {
        $("#spinner").bind("ajaxSend", function () {
            $(this).show();
        }).bind("ajaxStop", function () {
            $(this).hide();
        }).bind("ajaxError", function () {
            $(this).hide();
        });
    });

    $(document).ready(function () {
        $("form").submit(function () {
            if ($(this).valid()) {
                $('#spinner').show();
            }
        });
    });
}(jQuery, window._spinner = window._spinner, window, document));