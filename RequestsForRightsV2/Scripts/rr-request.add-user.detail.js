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