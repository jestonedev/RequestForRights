$("form")
    .on("click",
        '[name="addUser"]',
        function (e) {
            addUser($(".rr-request-users"));
            e.preventDefault();
            return false;
        });

$("form")
    .on("click",
        '[name="deleteUser"]',
        function (e) {
            var userDeletingElem = $(this).closest(".rr-request-user");
            var userOpeningElem = userDeletingElem.prev();
            if (userOpeningElem.length === 0) {
                userOpeningElem = userDeletingElem.next();
            }
            userDeletingElem.remove();
            updateControls();
            refreshValidation();
            updateDeleteUserButton();
            userOpeningElem.find(".panel-heading a").click();
            e.preventDefault();
            return false;
        });

$(".rr-save-button")
    .on("click",
        function (e) {
            var form = $("form");
            if (form.valid()) {
                form.submit();
            }
            e.preventDefault();
            return false;
        });

function updateControls() {
    var userNamePropRegex = /Users\[\d+\]/;
    var userIdPropRegex = /Users_\d+__/;
    var users = $(".rr-request-users > .rr-request-user");
    users.each(function (userIdx, userElem) {
        $(userElem)
            .find("[name]")
            .filter(function (fieldIdx, field) {
                return $(field).prop("name").match(userNamePropRegex) != null;
            })
            .each(function (fieldIdx, field) {
                var name = $(field).prop("name").replace(userNamePropRegex, "Users[" + userIdx + "]");
                $(field).prop("name", name);
                var id = $(field).prop("id").replace(userIdPropRegex, "Users_" + userIdx + "__");
                $(field).prop("id", id);
                $(field).closest(".form-group").find("label").prop("for", id);
                $(field).closest(".form-group").find("span").attr("data-valmsg-for", name);
            });
        $(userElem).find(".panel-heading").attr("id", "heading" + userIdx);
        $(userElem).find(".panel-heading a").attr("href", "#collapse" + userIdx).
            attr("aria-controls", "collapse" + userIdx);
        $(userElem).find(".panel-collapse").attr("id", "collapse" + userIdx);
    });
}

function addUser(userLayout) {
    var idRequestType = $("input[name='RequestModel.IdRequestType']").val();
    $.get("/Request/GetEmptyUserTemplate?IdRequestType=" + idRequestType, function (template) {
        userLayout.append(template);
        updateControls();
        refreshValidation();
        updateDeleteUserButton();
        $(".rr-request-user").last().find(".panel-heading a").click();
    });
}

function refreshValidation() {
    var form = $("form")
    .removeData("validator")
    .removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse(form);
    form.validate();
}

function updateDeleteUserButton() {
    var users = $(".rr-request-users > .rr-request-user");
    if (users.length === 1) {
        users.find("button[name=deleteUser]").prop("disabled", true);
    } else {
        users.find("button[name=deleteUser]").prop("disabled", false);
    }
}

updateControls();
updateDeleteUserButton();