function sendTransferUserNotificationBegin() {
    $(this).hide();
    $(this).parent().find(".rr-transfer-user-notificaton-error").remove();
    $(this).after("<strong class=\"rr-transfer-user-notificaton-continuing\"> Отправка уведомления...</strong>");
}

function sendTransferUserNotificationSuccess() {
    $(this).parent().find(".rr-transfer-user-notificaton-continuing").remove();
    $(this).replaceWith("<strong>Уведомление отправлено!</strong>");
}

function sendTransferUserNotificationFailure() {
    $(this).show();
    $(this).parent().find(".rr-transfer-user-notificaton-continuing").remove();
    var errorElem = $(this).parent().find(".rr-transfer-user-notificaton-error");
    if (errorElem.length === 0) {
        $(this).after("<strong class=\"rr-transfer-user-notificaton-error\"> (При отправке уведомления произошла ошибка)</strong>");
    }
}

$(document)
    .ready(function () {
        $(".rr-user-duplicate-notification").each(function(idx, elem) {
            var requesterSnp = $(elem).data("requester-snp");
            var requesterDepartment = $(elem).data("requesterDepartment");
            var requestUserSnp = $(elem).data("snp");
            var requestUserDepartment = $(elem).data("department");
            var requestUserUnit = $(elem).data("unit");
            $(elem).html("<div class=\"alert alert-danger text-center\"><img src=\"/Content/Images/spinner.gif\"><span style=\"margin-left: 10px;\">Производится проверка присутствия сотрудников с такими ФИО в других организациях</span></div>");
            $.get("/RequestAddUser/GetDuplicateUserNotification?requesterSnp=" +
                    encodeURIComponent(requesterSnp) +
                    "&requesterDepartment=" +
                    encodeURIComponent(requesterDepartment) +
                    "&requestUserSnp=" +
                    encodeURIComponent(requestUserSnp) +
                    "&requestUserDepartment=" +
                    encodeURIComponent(requestUserDepartment) +
                    "&requestUserUnit=" +
                    encodeURIComponent(requestUserUnit),
                    function(result) {
                        $(elem).html(result);
                    })
                .fail(function() {
                    $(elem).html("<div class=\"alert alert-danger\">Произошла непредвиденная ошибка</div>");
                });
        });
    });