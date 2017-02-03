$(function() {
    var datePickerOptions = {
        format: "dd.mm.yyyy",
        weekStart: 1,
        maxViewMode: 2,
        todayBtn: "linked",
        language: "ru",
        orientation: "bottom auto",
        daysOfWeekDisabled: "0,6",
        autoclose: true,
        todayHighlight: true,
        startDate: "01/01/1753"
    };

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

    jQuery.extend(jQuery.validator.methods, {
        date: function (value, element) {
            if (this.optional(element) && value === "") {
                return true;
            }
            var dateParts = value.split(".");
            if (dateParts.length !== 3) {
                return false;
            }
            return !isNaN(Date.parse(dateParts[2] + "/" + dateParts[1] + "/" + dateParts[0]));
        }
    });
});