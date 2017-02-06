$(function() {
    $("#DateOfFillingFrom").datepicker(datePickerOptions).on("changeDate", filterControlOnValue);
    $("#DateOfFillingTo").datepicker(datePickerOptions).on("changeDate", filterControlOnValue);
});