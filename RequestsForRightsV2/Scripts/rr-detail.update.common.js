$(function() {
    var deleteModal = $("#deleteModal");
    var deleteModalSuccess = deleteModal.find("#deleteModalSuccess");

    $("body")
        .on("click",
            "#delete-button",
            function (e) {
                var href = $(this).attr("href");
                var redirectUri = $(this).data("success-redirect-to");
                deleteModalSuccess.off("click");
                deleteModalSuccess.on("click",
                        function () {
                            $.ajax({
                                url: href + "?_=" + new Date().getTime(),
                                type: "DELETE",
                                success: function () {
                                    location.href = redirectUri;
                                },
                                error: function (error) {
                                    var errorAlert = $(".rr-error-alert");
                                    errorAlert.find(".rr-alert-message").text(error.responseText || "Произошла ошибка");
                                    $(".rr-error-alert").css({ "opacity": 1 }).show();
                                }
                            });
                        });
                deleteModal.modal("show");
                e.preventDefault();
                return false;
            });
});