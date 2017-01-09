

var filterControl = $(".rr-filter-control form");
var filter = $(".rr-filter-control #Filter");
var pageSize = $(".rr-filter-control #PageSize");

Rx.Observable.fromEvent(pageSize, "change").subscribe(filterControlOnValue, filterControlOnError);
Rx.Observable.fromEvent(filter, "keyup").debounce(500)
    .subscribe(filterControlOnValue, filterControlOnError);

function filterControlOnValue() {
    filterControl.submit();
}

function filterControlOnError(error) {
    console.error(error);
}

var tableWrapper = $(".rr-table-wrapper");
var deleteModal = $("#deleteModal");

deleteModalSuccess = deleteModal.find("#deleteModalSuccess");

$("body")
    .on("click",
        ".rr-delete-item-button",
        function (e) {
            deleteModalSuccess.off("click");
            deleteModalSuccess.on("click",
                    function() {
                        $.ajax({
                            url: $(e.target).attr("href"),
                            type: "DELETE",
                            success: function(result) {
                                tableWrapper.replaceWith(result);
                            },
                            error: function(error) {
                                console.error(error);
                            }
                        });
                    });
            deleteModal.modal("show");
            e.preventDefault();
        });