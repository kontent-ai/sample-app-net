(function ($, selectProject, window, document, undefined) {
    $("#selectProject").submit(function (event) {
        if (!$(this).valid()) {
            event.preventDefault();
            $("#selectProjectValidation").text("You need to select a project.");
        }
    });
}(jQuery, window._selectProject = window._selectProject, window, document));