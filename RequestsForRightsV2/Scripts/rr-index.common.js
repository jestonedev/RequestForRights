

var filterControl = $(".rr-filter-control form");
var input = $(".rr-filter-control input");
var radioButtons = $(".rr-filter-control input[type=radio]");
var select = $(".rr-filter-control select");

$(select).on("change", filterControlOnValue);
$(input).on("keyup", debounce(filterControlOnValue, 250));
$(radioButtons).on("change", filterControlOnValue);

function filterControlOnValue() {
    filterControl.submit();
}

var deleteModal = $("#deleteModal");
var deleteModalSuccess = deleteModal.find("#deleteModalSuccess");

$("body")
    .on("click",
        ".rr-delete-item-button",
        function (e) {
            var href = $(this).attr("href");
            deleteModalSuccess.off("click");
            deleteModalSuccess.on("click",
                    function() {
                        $.ajax({
                            url: href+"?_="+new Date().getTime(),
                            type: "DELETE",
                            success: function(result) {
                                $(".rr-table-wrapper").replaceWith(result);
                            },
                            error: errorLoading
                        });
                    });
            deleteModal.modal("show");
            e.preventDefault();
            return false;
        });

$("body")
    .on("click",
        ".rr-column-header a",
        function (e) {
            $.ajax({
                url: $(this).attr("href") + "&_=" + new Date().getTime(),
                success: function (result) {
                    $(".rr-table-wrapper").replaceWith(result);
                },
                error: errorLoading
            });
            e.preventDefault();
            return false;
        });

function errorLoading(error) {
    var errorAlert = $(".rr-error-alert");
    errorAlert.find(".rr-alert-message").text(error.responseText || "Произошла ошибка");
    $(".rr-error-alert").css({ "opacity": 1 }).show();
}