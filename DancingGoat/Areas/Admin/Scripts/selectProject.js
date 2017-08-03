$("#selectProject").submit(function (event) {
    if (!$("input[name='projectId']:checked").val()) {
        event.preventDefault();
        $("#selectProjectValidation").text("You need to select a project.");
    }
    else {
        $("#selectProjectValidation").empty();
        $(this).unbind("submit").submit();
    }
});