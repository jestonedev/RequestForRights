$(function() {
    $(".rr-request-delegation-from-date input").datepicker(datePickerOptions);
    $(".rr-request-delegation-to-date input").datepicker(datePickerOptions);

    $("#rr-request-form").off("click", '[name="addUser"]');

    $("#rr-request-form").on("click",
        '[name="addUser"]',
        function (e) {
            addUser($(".rr-request-users"));
            var addedUser = $(".rr-request-users").find(".rr-request-user-wrapper").last();
            addedUser.find(".rr-request-delegation-from-date input").datepicker(datePickerOptions);
            addedUser.find(".rr-request-delegation-to-date input").datepicker(datePickerOptions);
            e.preventDefault();
            return false;
        });
});