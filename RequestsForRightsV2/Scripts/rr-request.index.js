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
        todayHighlight: true
    };

    $("#DateOfFillingFrom").datepicker(datePickerOptions).on("changeDate", filterControlOnValue);
    $("#DateOfFillingTo").datepicker(datePickerOptions).on("changeDate", filterControlOnValue);
});