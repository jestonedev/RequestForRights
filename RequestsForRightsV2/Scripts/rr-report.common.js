$(function() {
    $("#Date").datepicker(datePickerOptions).on("changeDate", filterControlOnValue);
    $("#DateFrom").datepicker(datePickerOptions).on("changeDate", filterControlOnValue);
    $("#DateTo").datepicker(datePickerOptions).on("changeDate", filterControlOnValue);
});

function showWaitingElement() {
    $(".rr-loading-data").show();
    $(".rr-table-wrapper").empty();
}

function hideWaitingElement() {
    $(".rr-loading-data").hide();
}