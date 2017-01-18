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
            var marginBottom = $("body").css("margin-bottom");
            $("body").css("margin-bottom", userDeletingElem.find(".panel-body").outerHeight());
            userDeletingElem.remove();
            var title = userOpeningElem.find(".panel-heading");
            title.find("a").click();
            updateControls();
            refreshValidation();
            updateDeleteUserButton();
            if (supportTransitions()) {
                // transitionend not working. I think reason is display:none before transition end
                setTimeout(function () { $("body").css("margin-bottom", marginBottom) }, 350);
            } else {
                $("body").css("margin-bottom", marginBottom);
            }
            e.preventDefault();
            return false;
        });

$(".rr-save-button").on("click", submitButtonClick);
$(".rr-request-user").first().find(".panel-heading a").click();

function submitButtonClick(e) {
    var form = $("form");
    if (formIsValid(form)) {
        form.submit();
    } else {
        showErrorBadgets();
    }
    e.preventDefault();
    return false;
}

function formIsValid(form) {
    var validator = form.validate();
    var formValid = form.valid();
    var users = $(".rr-request-users > .rr-request-user");
    users.each(function (userIdx, userElem) {
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
    return formValid;
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
        $(window).scrollTop($(document).height());
    });
}

function refreshValidation() {
    var form = $("form")
    .removeData("validator")
    .removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse(form);
    form.validate();
}

function showErrorBadgets() {
    var titleToShow = window.undefined;
    var panelToShow = window.undefined;
    $(".rr-request-user")
        .each(function (idx, elem) {
            var title = $(elem).find(".panel-title");
            var id = $(title).find("a").attr("href");
            var panel = $(elem).find(".panel-collapse" + id);
            var badge = $(title).find(".rr-badge");
            var errorCount = panel.find(".field-validation-error").length;
            if (errorCount > 0) {
                if (titleToShow === window.undefined || panel.hasClass("in")) {
                    titleToShow = title;
                    panelToShow = panel;
                }
                badge.show();
            } else {
                badge.hide();
            }
        });
    if (titleToShow !== window.undefined && !panelToShow.hasClass("in")) {
        scrollToElement(titleToShow);
        $(titleToShow).find("a").click();
    }
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
    if (userSnp.length === 0) return;
    $(userSnp).autocomplete({
        serviceUrl: "/User/GetUsers",
        ajaxSettings: {
            dataType: "json"
        },
        paramName: "snpPattern",
        deferRequestBy: 0,
        transformResult: function(response) {
            return {
                suggestions: $.map(response, function (dataItem) {
                    return { value: dataItem.Snp, data: dataItem };
                })
            };
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
        }
    });
}

showErrorBadgets();
updateControls();
updateDeleteUserButton();
refreshValidation();
initializeUsersAutocomplete($(".rr-request-user-snp input"));