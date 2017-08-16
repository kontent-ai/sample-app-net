(function ($, window, document, undefined) {
    var spinnerSelector = "#spinner";
    var maximumDuration = 120;
    var intervalId = null;
    var i = 1;

    $(document).ready(function () {
        $(spinnerSelector).bind("ajaxSend", function () {
            if (intervalId !== null) {
                clearInterval(intervalId);
            }
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
                if (intervalId !== null) {
                    clearInterval(intervalId);
                }

                $(spinnerSelector).show();

                if ($(this)[0].id === "deploySample") {
                    intervalId = startCounting(spinnerSelector, maximumDuration);
                }
            }
        });
    });

    var startCounting = function (spinner, durationSeconds) {
        i = 1;
        var interval = durationSeconds / 0.06;
        var intervalId = setInterval(function () { setPercentage(spinner); }, interval);
    };

    var setPercentage = function (spinner) {
        var percentageText = i + " %";
        $(spinner).children(".percentage").html(percentageText);
        i++;
    };
}(jQuery, window._spinner = window._spinner, window, document));