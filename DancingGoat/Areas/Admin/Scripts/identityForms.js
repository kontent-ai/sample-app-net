(function ($, identityForms, window, document, undefined) {
    var token = null;

    // Add form tabs dynamically.
    var headings = $(".form-panels form h3");
    var i = 0;

    headings.each(function (index) {
        var contents = $(this).contents()[0].textContent;
        var wholeId = "formTab-" + $(this).parent()[0].id;
        $(".form-panel-tabs").first().append("<h3><a href=\"#" + wholeId + "\" id=\"" + wholeId + "\">" + contents + "</a></h3>");
    });

    // Set correct width for tabs.
    var tabs = $(".form-panel-tabs h3");
    var tabWidth = (100 / tabs.length).toString().concat("%");
    i = 0;

    tabs.each(function (index) {
        $(this).width(tabWidth);
        $(this).addClass("tab");

        if (i === 0) {
            $(this).addClass("tab-active");
        }
        else {
            $(this).addClass("tab-inactive");
        }

        i++;
    });

    // Hide panels.
    var panels = $(".form-panels form.form-panel");
    i = 0;

    panels.each(function (index) {
        if (i !== 0) {
            $(this).addClass("hidden");
        }

        i++;
    });

    $(".tab a").click(function (eventData, handler) {
        var caller = $(eventData.target)[0];
        var strippedId = caller.id.substring(8, caller.id.length);
        switchTabsAndForms(strippedId);
    });

    $("a.form-tab-switcher").click(function (eventData, handler) {
        var caller = $(eventData.target)[0];
        var hrefAttribute = caller.getAttribute("href");
        var id = hrefAttribute.substring(9, hrefAttribute.length);
        switchTabsAndForms(id);
    });

    var switchTabsAndForms = function (id) {
        // Change the active tab.
        $(".form-panel-tabs h3").removeClass("tab-active").addClass("tab-inactive");
        var tabId = "#formTab-" + id;
        $(tabId)[0].parentElement.className = "tab tab-active";

        // Change the active panel.
        $(".form-panels form.form-panel").addClass("hidden");
        $("#" + id).removeClass("hidden");
    };

    // Redirect the original POST via a dummy form.
    $.extend(
        {
            redirectPost: function (location, args) {
                var form = $('<form></form>');
                form.attr("method", "post");
                form.attr("action", location);

                $.each(args, function (key, value) {
                    var field = $('<input></input>');

                    field.attr("type", "hidden");
                    field.attr("name", key);
                    field.attr("value", value);

                    form.append(field);
                });
                $(form).appendTo('body').submit();
            }
        });

    // Sign in
    $("#signInForm").submit(function (event) {
        if ($(this).valid()) {
            // Stop form from submitting normally.
            event.preventDefault();

            // Get some values from elements on the page:
            var $form = $(this),
                email = $form.find("input[id='signInFormEmail']").val(),
                password = $form.find("input[id='signInFormPassword']").val(),
                url = $form.attr("action");

            var credentials = { username: email, password: password };

            // Send the data using post.
            var jqXhr = ajax(url, credentials);

            jqXhr.done(function (result) {
                if (token !== undefined && token !== null) {
                    if ($(event.target).data("origin") === "index") {
                        $.redirectPost('/Admin/SelfConfig/Index', { 'token': token });
                    }
                    else if ($(event.target).data("origin") === "recheck") {
                        $.redirectPost('/Admin/SelfConfig/Recheck', { 'token': token });
                    }
                }
            });
        }
    });

    // Sign up
    $("#signUpForm").submit(function (event) {
        if ($(this).valid()) {
            // Stop form from submitting normally.
            event.preventDefault();

            // Get some values from elements on the page:
            var $form = $(this),
                firstName = $form.find("input[id='FirstName']").val(),
                lastName = $form.find("input[id='LastName']").val(),
                email = $form.find("input[id='signUpFormEmail']").val(),
                password = $form.find("input[id='signUpFormPassword']").val(),
                url = $form.attr("action");

            var credentials = { email: email, password: password, password_confirmation: password, first_name: firstName, last_name: lastName };

            // Send the data using post.
            var jqXhr = ajax(url, credentials);

            jqXhr.done(function (result) {
                if (token !== undefined && token !== null) {
                    $.redirectPost('/Admin/SelfConfig/Index', { 'token': token });
                }
            });
        }
        else if (!$("#termsAccepted").is(":checked")) {
            $("#termsAcceptedValidation").show();
            return false;
        }
    });

    var ajax = function (url, credentials) {
        return $.ajax({
            url: url,
            data: JSON.stringify(credentials),
            processData: false,
            contentType: "application/json",
            dataType: "json",
            type: "post",
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                displayErrorHideSpinner(XMLHttpRequest.status, XMLHttpRequest.statusText);
            },
            success: function (response) {
                if (response.result === "okay") {
                    token = response.token;
                }
                else if (response.result === "error") {
                    displayErrorHideSpinner(response.error, null);
                }
            }
        });
    };

    var displayErrorHideSpinner = function (status, statusText) {
        $("#spinner").hide();
        $(".messages").show();
        $(".messages p.message-text").html("").append("An error occurred. Status: \"" + status + "\"");
        if (statusText !== undefined && statusText !== null) {
            $(".messages p.message-text").append(" Status text: " + statusText + "\"");
        }
        $(".messages p.message-text").show();
    };
}(jQuery, window._identityForms = window._identityForms, window, document));