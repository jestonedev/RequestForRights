var descriptionModifiedManual =
    $(".rr-request-description textarea").val() !== "" &&
    $(".rr-request-description textarea").val() !== getRequestDescription();

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
            var userDeletingElem = $(this).closest(".rr-request-user-wrapper");
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
            addRight($(this).closest(".rr-request-user-wrapper").find(".rr-request-rights"));
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

$(".rr-send-button").on("click", submitButtonClick);

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
        updateVisibleRights(rightPanel);
    });

var loadingRights = false;

function updateVisibleRights(rightPanel) {
    if (loadingRights) {
        setTimeout(function () { updateVisibleRights(rightPanel); }, 500);
        return;
    }
    var resourceSelect = rightPanel.find(".rr-request-right-resource select");
    var rightIdSelect = rightPanel.find(".rr-request-right-id select");
    var idRequestRightGrantType = rightPanel.find(".rr-request-right-grant-type select, .rr-request-right-grant-type input").
        first().val();
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
    if (idRequestRightGrantType !== "2" && idRequestRightGrantType !== "3") {
        loadResources(rightPanel);
        setResourceAndRightsValues(rightPanel, idResource, idResourceRight);
        if (userRightsBuffer[JSON.stringify(user)] != undefined) {
            return;
        }
        loadingRights = true;
        $.getJSON("/User/GetPermanentRightsOnDate",
            ajaxData, function (userRights) {
                userRightsBuffer[JSON.stringify(user)] = userRights;
                loadingRights = false;
            }).error(function () {
                loadingRights = false;
            });
        return;
    }
    if (userRightsBuffer[JSON.stringify(user)] != undefined) {
        loadResources(rightPanel, userRightsBuffer[JSON.stringify(user)]);
        setResourceAndRightsValues(rightPanel, idResource, idResourceRight);
        return;
    }
    loadingRights = true;
    $.getJSON("/User/GetPermanentRightsOnDate", ajaxData,
        function (userRights) {
            userRightsBuffer[JSON.stringify(user)] = userRights;
            loadResources(rightPanel, userRights);
            setResourceAndRightsValues(rightPanel, idResource, idResourceRight);
            loadingRights = false;
        }).error(function () {
            loadingRights = false;
        });
}

function clearResources(rightPanel) {
    var optGroups = rightPanel.find(".rr-request-right-resource select optgroup");
    if (resourceOptionsCache == undefined) {
        resourceOptionsCache = optGroups.clone(true);
    }
    optGroups.remove();
}

function loadResources(rightPanel, userRights) {
    var idRequestRightGrantType = $(rightPanel)
        .find(".rr-request-right-grant-type select, .rr-request-right-grant-type input")
        .first().val();
    var resourceSelect = $(rightPanel).find(".rr-request-right-resource select");
    resourceOptionsCache.each(function (idx, option) {
        resourceSelect.append($(option).clone(true));
    });
    resourceSelect.find("option").each(function (idx, option) {
        if ($(option).val() === "") {
            return;
        }
        if (idRequestRightGrantType === "2" || idRequestRightGrantType === "3") {
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
        .filter(function (idx, elem) {
            return $(elem).children().length === 0;
        })
        .remove();
}

function getUserInfo(rightPanel) {
    var userPanel = $(rightPanel).closest(".rr-request-user-wrapper").find(".rr-request-user").first();
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
            var cacheRightIdSelect = $("#cache-rr-request-right-id");
            var idResourceRight = rightIdSelect.val();
            var options = cacheRightIdSelect.find("option");
            if (rightsOptionsCache == undefined) {
                rightsOptionsCache = options.clone();
            }
            rightIdSelect.empty();

            var idRequestRightGrantType = rightPanel.find(".rr-request-right-grant-type select," +
                ".rr-request-right-grant-type input").first().val();
            var userInfo = getUserInfo(rightPanel);
            var userRights = userRightsBuffer[JSON.stringify(userInfo)];
            rightsOptionsCache.each(function (idx, option) {
                if ($(option).val() === "") {
                    rightIdSelect.append($(option).clone());
                    return;
                }
                if (idRequestRightGrantType !== "2" &&
                    idRequestRightGrantType !== "3" &&
                    $(option).data("id-resource") === idResource) {
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
            var rightOptions = rightIdSelect.find("option");
            if (rightOptions.length === 2) {
                $(rightOptions[1]).prop("selected", "selected");
            } else {
                rightIdSelect.val(idResourceRight);
            }
            if (rightIdSelect.val() === null) {
                rightIdSelect.val("");
            }
        });

function submitButtonClick(e) {
    $(this).prop("disabled", "disabled");
    var form = $("#rr-request-form");
    if (formIsValid(form)) {
        form.submit();
    } else {
        showErrorBadgets();
        $(this).removeProp("disabled");
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
    var users = $(".rr-request-user");
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
        var rights = $(userElem).closest(".rr-request-user-wrapper").find(".rr-request-right");
        rights.each(function (rightIdx, rightElem) {
            var grantType = $(rightElem).find(".rr-request-right-grant-type select").val();
            if (grantType !== "1") {
                return;
            }
            var rightIdElem = $(rightElem).find(".rr-request-right-id select");
            var rightId = rightIdElem.val();
            if (rightId === "") {
                return;
            }
            var description = $(rightElem).find(".rr-request-right-description textarea").val();
            if (description !== "") {
                return;
            }
            var user = getUserInfo(rightElem);
            var currentUserRights = userRightsBuffer[JSON.stringify((user))];
            if (currentUserRights == undefined) {
                return;
            }
            if ($(currentUserRights).filter(function (idx, val) { return val.IdResourceRight === parseInt(rightId) }).length > 0) {
                formValid = false;
                var error = {};
                error[rightIdElem.attr("name")] = "У сотрудника уже имеется данное право. В случае необходимости повторной выдачи укажите в примечании к праву причину";
                validator.showErrors(error);
            }
        });

    });
    return formValid;
}

function updateControls() {
    var userNamePropRegex = /(Users)\[\d+\]/;
    var rightNamePropRegex = /(Rights)\[\d+\]/;
    var userIdPropRegex = /(Users)_\d+__/;
    var rightIdPropRegex = /(Rights)_\d+__/;
    var users = $(".rr-request-users > .rr-request-user-wrapper");
    users.each(function (userIdx, userElem) {
        updateControl(userIdx, userElem, userNamePropRegex, userIdPropRegex);
        $(userElem).find(".panel-heading").attr("id", "heading" + userIdx);
        $(userElem).find(".panel-heading a").attr("href", "#collapse" + userIdx).
            attr("aria-controls", "collapse" + userIdx);
        $(userElem).find(".panel-collapse").attr("id", "collapse" + userIdx);
        var rights = $(userElem).find(".rr-request-rights > .rr-request-right");
        rights.each(function (rightIdx, rightElem) {
            updateControl(rightIdx, rightElem, rightNamePropRegex, rightIdPropRegex);
        });
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
    updateVisibleRights(addedRight);
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
    $(".rr-request-user-wrapper")
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
    var users = $(".rr-request-users > .rr-request-user-wrapper");
    if (users.length === 1) {
        users.find("button[name=deleteUser]").prop("disabled", true);
    } else {
        users.find("button[name=deleteUser]").prop("disabled", false);
    }
}

function updateDeleteRightButton() {
    var users = $(".rr-request-users > .rr-request-user-wrapper");
    users.each(function (idx, user) {
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
        noCache: true,
        transformResult: function (response) {
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
            updateUserFieldsTitles(user);
            updateUserWrapperHeader(user.closest(".rr-request-user-wrapper"));
            var rightPanels = user.closest(".rr-request-user-wrapper").find(".rr-request-right");
            for (var i = 0; i < rightPanels.length; i++) {
                updateVisibleRights($(rightPanels[i]));
            }
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
}

function updateUserFieldsTitles(user) {
    user.find("input")
        .each(function(idx, elem) {
            if ($(elem).attr("readonly") != undefined) {
                $(elem).attr("title", $(elem).val());
            }
        });
}

function addUser(userLayout) {
    if (userTempalteCache !== undefined) {
        addUserAfterLoad(userLayout, userTempalteCache);
        return;
    }
    loadUserCache(function (template) {
        addUserAfterLoad(userLayout, template);
    });
}

function addUserAfterLoad(userLayout, template) {
    userLayout.append(template);
    updateControls();
    refreshValidation();
    updateDeleteUserButton();
    updateDeleteRightButton();
    var addedUser = $(".rr-request-user-wrapper").last();
    initializeUsersAutocomplete(addedUser.find(".rr-request-user-snp input"));
    addedUser.find(".panel-heading a").click();
    addedUser.find(".rr-request-user-department select").change();
    var rightPanels = addedUser.find(".rr-request-right");
    for (var i = 0; i < rightPanels.length; i++) {
        updateVisibleRights($(rightPanels[i]));
    }
    $(window).scrollTop($(document).height());
}

function getRequestDescription() {
    var idRequestType = $('[name="RequestModel.IdRequestType"]').val();
    var description = getRequestDescriptionPreamble(idRequestType);
    $(".rr-request-user-wrapper").each(function (index, userWrapperElem) {
        var userElems = $(userWrapperElem).find(".rr-request-user");
        var textLine = "";
        for (var i = 0; i < userElems.length; i++) {
            var snp = $(userElems[i]).find(".rr-request-user-snp input").val();
            var hasNextSnp = false;
            if (i !== userElems.length - 1 && $(userElems[i + 1]).find(".rr-request-user-snp input").val()) {
                hasNextSnp = true;
            }
            if ($.trim(snp)) {
                textLine += snp;
                if (hasNextSnp) {
                    textLine += " — ";
                }
            }
        }
        if ($.trim(textLine)) {
            textLine += ",\n";
        }
        description += textLine;
    });
    return description.replace(/,\n$/, ".");
}

function updateRequestDescription() {
    if (descriptionModifiedManual) {
        return;
    }
    var requestDescription = $(".rr-request-description textarea");
    requestDescription.off("change");
    var description = getRequestDescription();
    requestDescription.val(description);
    requestDescription.on("change",
        function () {
            descriptionModifiedManual = true;
        });
}

function getRequestDescriptionPreamble(idRequestType) {
    var isOne = $(".rr-request-user").length === 1;
    switch (parseInt(idRequestType)) {
        case 1:
            return "Произвести подключение к информационной инфраструктуре " +
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

function updateUserWrapperHeader(userWrapper) {
    var users = userWrapper.find(".rr-request-user");
    var title = "";
    var idRequestType = userWrapper.closest("form").find('[name="RequestModel.IdRequestType"]').val();

    for (var i = 0; i < users.length; i++) {
        var snp = $(users[i]).find(".rr-request-user-snp input").val();
        var hasNextSnp = false;
        if (i !== users.length - 1 && $(users[i + 1]).find(".rr-request-user-snp input").val()) {
            hasNextSnp = true;
        }
        if ($.trim(snp)) {
            title += "«" + snp + "»";
            if (hasNextSnp) {
                title += " — ";
            }
        }
    }
    if ($.trim(title) === "") {
        if (idRequestType === "4") {
            title = "Новое делегирование";
        } else {
            if (users.length === 1) {
                title = "Новый сотрудник";
            } else {
                title = "Новые сотрудники";
            }
        }
    } else {
        if (idRequestType === "4") {
            title = "Делегирование " + title;
        } else {
            if (users.length === 1) {
                title = "Сотрудник " + title;
            } else {
                title = "Cотрудники " + title;
            }
        }
    }
    userWrapper.find(".panel-title a").text(title);
}

showErrorBadgets();
updateControls();
updateDeleteUserButton();
updateDeleteRightButton();
refreshValidation();

updateUserFieldsTitles($(".rr-request-user"));
updateRequestDescription();

initializeUsersAutocomplete($(".rr-request-user-snp input"));

loadUserCache();
loadRightCache();

var rightPanels = $(".rr-request-right");
for (var i = 0; i < rightPanels.length; i++) {
    updateVisibleRights($(rightPanels[i]));
}