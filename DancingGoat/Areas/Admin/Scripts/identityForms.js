; (function ($, identityForm, window, document, undefined) {
    // jquery extend function
    $.extend(
        {
            // Function to redirect the original POST via a dummy form
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

        // Stop form from submitting normally
        event.preventDefault();

        // Get some values from elements on the page:
        var $form = $(this),
            email = $form.find("input[id='Email']").val(),
            password = $form.find("input[id='Password']").val(),
            url = $form.attr("action");

        var credentials = { username: email, password: password };

        // Send the data using post
        var posting = ajax(url, credentials);
    });

    // Sign up
    $("#signUpForm").submit(function (event) {

        // Stop form from submitting normally
        event.preventDefault();

        if (!$("#termsAccepted").is(":checked")) {
            $("#termsAcceptedValidation").show();
        }
        else {
            // Get some values from elements on the page:
            var $form = $(this),
                firstName = $form.find("input[id='FirstName']").val(),
                lastName = $form.find("input[id='LastName']").val(),
                email = $form.find("input[id='Email']").val(),
                password = $form.find("input[id='Password']").val(),
                url = $form.attr("action");

            var credentials = { email: email, password: password, password_confirmation: password, first_name: firstName, last_name: lastName };

            // Send the data using post
            var posting = ajax(url, credentials);
        }
    });

    var ajax = (function (url, credentials) {
        $.ajax({
            url: url,
            data: JSON.stringify(credentials),
            processData: false,
            contentType: 'application/json',
            dataType: 'json',
            type: 'post',
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $("#warning").html('').append('An error occurred. Status:' + XMLHttpRequest.status + ', status text: ' + XMLHttpRequest.statusText);
            },
            success: function (response) {
                if (response.result == 'okay') {
                    $.redirectPost('/admin/selfconfig/', { 'token': response.token });
                }
            }
        });
    });
}(jQuery, window._identityForms = window._identityForms, window, document));