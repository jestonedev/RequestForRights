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

var requestContinuing = false;

$(".rr-save-button").on("click", submitButtonClick);

$(".rr-request-user").first().find(".panel-heading a").click();
$(document)
    .ready(function () {
        $("form")
            .on("keypress",
                ".rr-request-user-snp input",
                function () {
                    requestContinuing = true;
                });
    });

$("form").validate().settings.ignore = "";

updateControls();
updateDeleteUserButton();
refreshValidation();
initializeUsersAutocomplete($(".rr-request-user-snp input"));

function submitButtonClick(e) {
    if (requestContinuing) {
        setTimeout(function () { submitButtonClick(e); }, 100);
        e.preventDefault();
        return false;
    }
    var form = $("form");
    var validator = form.validate();
    var formValid = form.valid();
    var users = $(".rr-request-users > .rr-request-user");
    users.each(function(userIdx, userElem) {
        var userSnpElem = $(userElem).find(".rr-request-user-snp input");
        if (userSnpElem.val() === "") {
            return;
        }
        var userSnpSelection = $(userSnpElem).autocomplete().selection;
        var userSnpPrevElem = $(userElem).find('[name="rr-request-user-prev-snp"]');
        if (userSnpSelection == null && userSnpElem.val() !== userSnpPrevElem.val()) {
            formValid = false;
            var error = {};
            error[userSnpElem.attr("name")] = "ФИО задано некорректно";
            validator.showErrors(error);
        }
    });
    if (formValid) {
        form.submit();
    }
    e.preventDefault();
    return false;
}

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
        var addedUser = $(".rr-request-user").last();
        initializeUsersAutocomplete(addedUser.find(".rr-request-user-snp input"));
        addedUser.find(".panel-heading a").click();
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

function initializeUsersAutocomplete(userSnp) {
    $(userSnp).autocomplete({
        serviceUrl: "/User/GetUsers",
        ajaxSettings: {
            dataType: "json"
        },
        paramName: "snpPattern",
        deferRequestBy: 250,
        transformResult: function(response) {
            return {
                suggestions: $.map(response, function (dataItem) {
                    return { value: dataItem.Snp, data: dataItem };
                })
            };
        },
        onSearchComplete: function() {
            requestContinuing = false;
        },
        onSearchError: function() {
            requestContinuing = false;
        },
        formatResult: function (suggestion, currentValue) {
            var snp = suggestion.data.Snp;
            snp = snp.replace(new RegExp(currentValue, "gi"), "<strong>$&</strong>");
            var item = '<div class="rr-request-user-autocomplete-snp">' + snp + "</div>";
            if (suggestion.data.Department != undefined) {
                item += '<div class="rr-request-user-autocomplete-department">' + suggestion.data.Department + "</div>";
            }
            if (suggestion.data.Unit != undefined) {
                item += '<div class="rr-request-user-autocomplete-unit">' + suggestion.data.Unit + "</div>";
            }
            return item;
        },
        onSelect: function (suggestion) {
            var user = $(this).closest(".rr-request-user");
            user.find(".rr-request-user-login input").val(suggestion.data.Login);
            user.find(".rr-request-user-post input").val(suggestion.data.Post);
            user.find(".rr-request-user-department input").val(suggestion.data.Department);
            user.find(".rr-request-user-unit input").val(suggestion.data.Unit);
            user.find(".rr-request-user-office input").val(suggestion.data.Office);
            user.find(".rr-request-user-phone input").val(suggestion.data.Phone);
            if (suggestion.data.Snp) {
                user.find(".panel-title a").text("Сотрудник «" + suggestion.data.Snp + "»");
            } else {
                user.find(".panel-title a").text("Новый сотрудник");
            }
            requestContinuing = false;
        }
    });
}