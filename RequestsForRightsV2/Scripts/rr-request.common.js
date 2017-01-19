$("#rr-request-form")
    .on("click",
        '[name="addUser"]',
        function (e) {
            addUser($(".rr-request-users"));
            e.preventDefault();
            return false;
        });

$("#rr-request-form")
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
            updateRequestDescription();
            e.preventDefault();
            return false;
        });

$(".rr-save-button").on("click", submitButtonClick);

var descriptionModifiedManual = false;
var initialDescriptionIsEmpty = !$(".rr-request-description textarea").val();
$(".rr-request-description textarea")
    .on("change",
        function() {
            descriptionModifiedManual = true;
        });

var addCommentButton = $(".rr-add-comment-button").clone();

$(".rr-comment-tab-edit-panel")
    .on("click", ".rr-add-comment-button",
        function(e) {
            $(this).remove();
            $(".rr-request-comment-editor").show();
            $(".rr-request-comment-editor textarea").focus();
            $(".rr-add-comment-panel").show();
            updateSendCommentButtonState();
            $(window).scrollTop($(document).height());
            e.preventDefault();
            return false;
        });

$("#rr-new-comment").on("keyup", updateSendCommentButtonState);

$(".rr-send-cancel-button")
    .on("click",
        function (e) {
            resetCommentFormState();
            e.preventDefault();
            return false;
        });

function submitButtonClick(e) {
    var form = $("#rr-request-form");
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
                $(field).closest(".form-group").find("span[data-valmsg-for]").attr("data-valmsg-for", name);
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
    var form = $("#rr-request-form")
    .removeData("validator")
    .removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse(form);
    form.validate();
}

function showErrorBadgets() {
    var titleToShow = window.undefined;
    var panelToShow = window.undefined;
    var totalErrors = 0;
    $(".rr-request-user")
        .each(function (idx, elem) {
            var title = $(elem).find(".panel-title");
            var id = $(title).find("a").attr("href");
            var panel = $(elem).find(".panel-collapse" + id);
            var badge = $(title).find(".rr-badge");
            var errorCount = panel.find(".field-validation-error").length;
            if (errorCount > 0) {
                totalErrors += errorCount;
                if (titleToShow === window.undefined || panel.hasClass("in")) {
                    titleToShow = title;
                    panelToShow = panel;
                }
                badge.text(errorCount);
                badge.show();
            } else {
                badge.hide();
            }
        });
    var badge = $("#rr-request-tab .rr-badge");
    if (totalErrors > 0) {
        badge.text(totalErrors);
        badge.show();
    } else {
        badge.hide();
    }
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
            updateUserFields(user, suggestion);
            updateRequestDescription();
        }
    });
}

function updateUserFields(user, suggestion) {
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

function updateRequestDescription() {
    if (descriptionModifiedManual || !initialDescriptionIsEmpty) {
        return;
    }
    var requestDescription = $(".rr-request-description textarea");
    requestDescription.off("change");
    var idRequestType = $('[name="RequestModel.IdRequestType"]').val();
    var description = getRequestDescriptionPreamble(idRequestType);
    $(".rr-request-user").each(function(index, userElem) {
        description += $(userElem).find(".rr-request-user-snp input").val() + ",\n";
    });
    requestDescription.val(description.replace(/,\n$/, "."));
    requestDescription.on("change",
        function () {
            descriptionModifiedManual = true;
        });
}

function getRequestDescriptionPreamble(idRequestType) {
    var isOne = $(".rr-request-user").length === 1;
    switch (parseInt(idRequestType)) {
        case 1:
            return "Произвести подключение к информационной инфраструктуры " +
                (isOne ? "следующего сотрудника" : "следующих сотрудников") +
                ":\n";
        case 2:
            return "Выдать/лишить прав доступа к информационным ресурсам " +
                (isOne ? "следующего сотрудника" : "следующих сотрудников") +
                ":\n";
        case 3:
            return "Произвести отключение от информационной инфраструктуры " +
                (isOne ? "следующего сотрудника" : "следующих сотрудников") +
                ":\n";
        case 4:
            return "Временно делегировать права доступа " +
                (isOne ? "следующего сотрудника" : "следующих сотрудников") +
                ":\n";
        default:
            return "";
    }
}

function beforeAddComment() {
    $("#rr-new-comment").attr("readonly", true);
}

function commentAddingSuccess() {
    $("#rr-new-comment").attr("readonly", false);
    $("#rr-new-comment").val("");
    resetCommentFormState();
    showCommentsCountBadget();
}

function commentAddingFailure() {
    $(".rr-comment-error-alert").css({ "opacity": 1 }).show();
    $("#rr-new-comment").attr("readonly", false);
}

function resetCommentFormState() {
    $(".rr-comment-tab-edit-panel .btn-group").append(addCommentButton);
    $(".rr-request-comment-editor").hide();
    $(".rr-add-comment-panel").hide();
}

function updateSendCommentButtonState() {
    if ($.trim($("#rr-new-comment").val()) === "") {
        $(".rr-send-comment-button").attr("disabled", "disabled");
    } else {
        $(".rr-send-comment-button").removeAttr("disabled");
    }
}

function showCommentsCountBadget() {
    var badge = $("#rr-comments-tab .rr-comments-badge");
    var commentsCount = $("#rr-comments-content .rr-comment").length;
    if (commentsCount > 0) {
        badge.text(commentsCount);
        badge.show();
    } else {
        badge.hide();
    }
}

showErrorBadgets();
showCommentsCountBadget();
updateControls();
updateDeleteUserButton();
refreshValidation();
initializeUsersAutocomplete($(".rr-request-user-snp input"));
$(".rr-request-user").first().find(".panel-heading a").click();