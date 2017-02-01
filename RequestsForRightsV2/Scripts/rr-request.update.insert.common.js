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

$("#rr-request-form")
    .on("click",
        '[name="addRight"]',
        function (e) {
            addRight($(this).closest(".rr-request-user").find(".rr-request-rights"));
            e.preventDefault();
            return false;
        });

$("#rr-request-form")
    .on("click",
        '[name="deleteRight"]',
        function (e) {
            var rightDeletingElem = $(this).closest(".rr-request-right");
            rightDeletingElem.remove();
            updateControls();
            refreshValidation();
            updateDeleteRightButton();
            e.preventDefault();
            return false;
        });

$(".rr-save-button").on("click", submitButtonClick);

var descriptionModifiedManual = false;
var initialDescriptionIsEmpty = !$(".rr-request-description textarea").val();
$(".rr-request-description textarea")
    .on("change",
        function () {
            descriptionModifiedManual = true;
        });

var resourceOptionsCache = undefined;
var userRightsBuffer = [];

$("#rr-request-form")
    .on("change", ".rr-request-right-grant-type select", function () {
        var rightPanel = $(this).closest(".rr-request-right");
        var resourceSelect = rightPanel.find(".rr-request-right-resource select");
        var rightIdSelect = rightPanel.find(".rr-request-right-id select");
        var idRequestRightGrantType = $(this).val();
        var idResource = resourceSelect.val();
        var idResourceRight = rightIdSelect.val();
        clearResources(rightPanel);
        resourceSelect.val("");
        var user = getUserInfo(rightPanel);
        var ajaxData = {
            date: getCurrentDate(),
            "requestUser.Login": user.Login,
            "requestUser.Snp": user.Snp,
            "requestUser.Department": user.Department,
            "requestUser.Unit": user.Unit
        };
        if (idRequestRightGrantType !== "2") {
            loadResources(resourceSelect);
            setResourceAndRightsValues(rightPanel, idResource, idResourceRight);
            if (userRightsBuffer[JSON.stringify(user)] != undefined) {
                return;
            }
            $.getJSON("/User/GetPermanentRightsOnDate",
                ajaxData,
                function(userRights) {
                    userRightsBuffer[JSON.stringify(user)] = userRights;
                });
            return;
        }
        if (userRightsBuffer[JSON.stringify(user)] != undefined) {
            loadResources(resourceSelect, userRightsBuffer[JSON.stringify(user)]);
            setResourceAndRightsValues(rightPanel, idResource, idResourceRight);
            return;
        }
        $.getJSON("/User/GetPermanentRightsOnDate", ajaxData,
            function(userRights) {
                userRightsBuffer[JSON.stringify(user)] = userRights;
                loadResources(resourceSelect, userRights);
                setResourceAndRightsValues(rightPanel, idResource, idResourceRight);
            });
    });

function clearResources(rightPanel) {
    var optGroups = rightPanel.find(".rr-request-right-resource select optgroup");
    if (resourceOptionsCache == undefined) {
        resourceOptionsCache = optGroups.clone(true);
    }
    optGroups.remove();
}

function loadResources(resourceSelect, userRights) {
    var idRequestRightGrantType = $(resourceSelect)
        .closest(".rr-request-right")
        .find(".rr-request-right-grant-type select")
        .val();
    resourceOptionsCache.each(function(idx, option) {
        resourceSelect.append($(option).clone(true));
    });
    resourceSelect.find("option").each(function (idx, option) {
        if ($(option).val() === "") {
            return;
        }
        if (idRequestRightGrantType === "2") {
            var has = false;
            for (var i = 0; i < userRights.length; i++) {
                if (userRights[i].IdResource === $(option).data("id-resource")) {
                    has = true;
                    break;
                }
            }
            if (!has) {
                $(option).remove();
            }
        }
    });
    resourceSelect.find("optgroup")
        .filter(function(idx, elem) {
            return $(elem).children().length === 0;
        })
        .remove();
}

function getUserInfo(rightPanel) {
    var userPanel = $(rightPanel).closest(".rr-request-user");
    return {
        Login: userPanel.find(".rr-request-user-login input").val(),
        Snp: userPanel.find(".rr-request-user-snp input").val(),
        Department: userPanel.find(".rr-request-user-department input").val(),
        Unit: userPanel.find(".rr-request-user-unit input").val()
    };
}

function setResourceAndRightsValues(rightPanel, idResource, idResourceRight) {
    var resourceSelect = rightPanel.find(".rr-request-right-resource select");
    var rightIdSelect = rightPanel.find(".rr-request-right-id select");
    resourceSelect.val(idResource);
    if (resourceSelect.val() == null) {
        resourceSelect.val("");
    }
    rightPanel.find(".rr-request-right-resource select").change();
    rightIdSelect.val(idResourceRight);
    if (rightIdSelect.val() == null) {
        rightIdSelect.val("");
    }
}

var rightsOptionsCache = undefined;

$("#rr-request-form")
    .on("change", ".rr-request-right-resource select",
        function () {
            var rightPanel = $(this).closest(".rr-request-right");
            var idResource = $(this).find("option:selected").data("id-resource");
            var rightIdSelect = rightPanel.find(".rr-request-right-id select");
            var idResourceRight = rightIdSelect.val();
            var options = rightIdSelect.find("option");
            if (rightsOptionsCache == undefined) {
                rightsOptionsCache = options.clone();
            }
            options.remove();

            var idRequestRightGrantType = rightPanel.find(".rr-request-right-grant-type select").val();
            var userInfo = getUserInfo(rightPanel);
            var userRights = userRightsBuffer[JSON.stringify(userInfo)];
            rightsOptionsCache.each(function (idx, option) {
                if ($(option).val() === "") {
                    rightIdSelect.append($(option).clone());
                    return;
                }
                if (idRequestRightGrantType !== "2" && $(option).data("id-resource") === idResource) {
                    rightIdSelect.append($(option).clone());
                    return;
                }
                if (userRights == undefined) {
                    return;
                }
                for (var i = 0; i < userRights.length; i++) {
                    if ($(option).data("id-resource") === idResource &&
                        userRights[i].IdResource === $(option).data("id-resource") &&
                        userRights[i].IdResourceRight.toString() === $(option).val()) {
                        rightIdSelect.append($(option).clone());
                        break;
                    }
                }
            });
            rightIdSelect.val(idResourceRight);
            if (rightIdSelect.val() === null) {
                rightIdSelect.val("");
            }
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
    if ($.fn.autocomplete === undefined) {
        return formValid;
    }
    var users = $(".rr-request-users > .rr-request-user");
    users.each(function (userIdx, userElem) {
        var userSnpElem = $(userElem).find(".rr-request-user-snp input");
        if (userSnpElem.val() === "" ||
            userSnpElem.closest(".rr-request-user-snp").hasClass("rr-no-autocomplete")) {
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
    var userNamePropRegex = /(Users)\[\d+\]/;
    var rightNamePropRegex = /(Rights)\[\d+\]/;
    var userIdPropRegex = /(Users)_\d+__/;
    var rightIdPropRegex = /(Rights)_\d+__/;
    var users = $(".rr-request-users > .rr-request-user");
    users.each(function (userIdx, userElem) {
        updateControl(userIdx, userElem, userNamePropRegex, userIdPropRegex);
        $(userElem).find(".panel-heading").attr("id", "heading" + userIdx);
        $(userElem).find(".panel-heading a").attr("href", "#collapse" + userIdx).
            attr("aria-controls", "collapse" + userIdx);
        $(userElem).find(".panel-collapse").attr("id", "collapse" + userIdx);
        var rights = $(userElem).find(".rr-request-rights > .rr-request-right");
        rights.each(function(rightIdx, rightElem) {
            updateControl(rightIdx, rightElem, rightNamePropRegex, rightIdPropRegex);
        });
    });
}

function updateControl(idx, control, namePropRegex, idPropRegex) {
    $(control)
        .find("[name]")
        .filter(function(fieldIdx, field) {
            return $(field).prop("name").match(namePropRegex) != null;
        })
        .each(function(fieldIdx, field) {
            var name = $(field).prop("name").replace(namePropRegex, "$1[" + idx + "]");
            $(field).prop("name", name);
            var id = $(field).prop("id").replace(idPropRegex, "$1_" + idx + "__");
            $(field).prop("id", id);
            $(field).closest(".form-group").find("label").prop("for", id);
            $(field).closest(".form-group").find("span[data-valmsg-for]").attr("data-valmsg-for", name);
        });
}

var userTempalteCache = undefined;

function loadUserCache(callback) {
    var idRequestType = $("input[name='RequestModel.IdRequestType']").val();
    if (idRequestType === undefined) return;
    $.get("/Request/GetEmptyUserTemplate?IdRequestType=" + idRequestType, function (template) {
        userTempalteCache = template;
        if (callback !== undefined) {
            callback(template);
        }
    });
}

function addUser(userLayout) {
    if (userTempalteCache !== undefined) {
        addUserAfterLoad(userLayout, userTempalteCache);
        return;
    }
    loadUserCache(function(template) {
        addUserAfterLoad(userLayout, template);
    });
}

function addUserAfterLoad(userLayout, template) {
    userLayout.append(template);
    updateControls();
    refreshValidation();
    updateDeleteUserButton();
    updateDeleteRightButton();
    var addedUser = $(".rr-request-user").last();
    initializeUsersAutocomplete(addedUser.find(".rr-request-user-snp input"));
    addedUser.find(".panel-heading a").click();
    addedUser.find(".rr-request-user-department select").change();
    addedUser.find(".rr-request-right .rr-request-right-resource select").change();
    $(window).scrollTop($(document).height());
}

var rightTemplateCache = undefined;

function loadRightCache(callback) {
    var idRequestType = $("input[name='RequestModel.IdRequestType']").val();
    if (idRequestType === undefined) return;
    $.get("/Request/GetEmptyRightTemplate?IdRequestType=" + idRequestType, function (template) {
        rightTemplateCache = template;
        if (callback !== undefined) {
            callback(template);
        }
    });
}

function addRight(rightLayout) {
    if (rightTemplateCache !== undefined) {
        addRightAfterLoad(rightLayout, rightTemplateCache);
        return;
    }
    loadUserCache(function (template) {
        addRightAfterLoad(rightLayout, template);
    });
}

function addRightAfterLoad(rightLayout, template) {
    rightLayout.append(template);
    updateControls();
    refreshValidation();
    updateDeleteRightButton();
    var addedRight = rightLayout.find(".rr-request-right").last();
    addedRight.find(".rr-request-right-resource select").change();
    $(window).scrollTop($(document).height());
}

function refreshValidation() {
    var form = $("#rr-request-form")
    .removeData("validator")
    .removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse(form);
    form.validate();
}

function showErrorBadgets() {
    var titleToShow = undefined;
    var panelToShow = undefined;
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
                if (titleToShow === undefined || panel.hasClass("in")) {
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
    fixIELayoutProblems();
    if (titleToShow !== undefined && !panelToShow.hasClass("in")) {
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

function updateDeleteRightButton() {
    var users = $(".rr-request-users > .rr-request-user");
    users.each(function(idx, user) {
        var rights = $(user).find(".rr-request-rights > .rr-request-right");
        if (rights.length === 1) {
            rights.find("button[name=deleteRight]").prop("disabled", true);
        } else {
            rights.find("button[name=deleteRight]").prop("disabled", false);
        }
    });
}

function initializeUsersAutocomplete(userSnp) {
    if (userSnp.length === 0 || $.fn.autocomplete === undefined ||
        userSnp.closest(".rr-request-user-snp").hasClass("rr-no-autocomplete")) return;
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
            $(user).find(".rr-request-right-grant-type select").change();
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
    $(".rr-request-user").each(function (index, userElem) {
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

showErrorBadgets();
updateControls();
updateDeleteUserButton();
updateDeleteRightButton();
refreshValidation();
initializeUsersAutocomplete($(".rr-request-user-snp input"));
loadUserCache();
loadRightCache();

$("#rr-request-form .rr-request-right-grant-type select").change();
$("#rr-request-form .rr-request-right-resource select").change();