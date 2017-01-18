$("form")
    .on("click",
        '[name="addRight"]',
        function(e) {
            addRight($(".rr-rights"));
            e.preventDefault();
            return false;
        });

$("form")
    .on("click",
        '[name="deleteRight"]',
        function(e) {
            $(this).closest(".rr-right").remove();
            updateControls();
            refreshValidation();
            updateDeleteRightButton();
            e.preventDefault();
            return false;
        });

$(".rr-save-button")
    .on("click",
        function (e) {
            var form = $("form");
            if (form.valid()) {
                form.submit();
            } else {
                showErrorBadgets();
            }
            e.preventDefault();
            return false;
        });

function updateControls() {
    var rightNamePropRegex = /Rights\[\d+\]/;
    var rightIdPropRegex = /Rights_\d+__/;
    var rights = $(".rr-rights > li");
    rights.each(function(rightIdx, rightElem) {
            $(rightElem)
                .find("[name]")
                .filter(function(fieldIdx, field) {
                    return $(field).prop("name").match(rightNamePropRegex) != null;
                })
                .each(function(fieldIdx, field) {
                    var name = $(field).prop("name").replace(rightNamePropRegex, "Rights[" + rightIdx + "]");
                    $(field).prop("name", name);
                    var id = $(field).prop("id").replace(rightIdPropRegex, "Rights_" + rightIdx + "__");
                    $(field).prop("id", id);
                    $(field).closest(".form-group").find("label").prop("for", id);
                    $(field).closest(".form-group").find("span").attr("data-valmsg-for", name);
                });
        });
}

function addRight(rightLayout) {
    $.get("/Resource/GetEmptyRightTemplate", function (template) {
        rightLayout.append(template);
        updateControls();
        refreshValidation();
        $(".rr-rights-error").remove();
        updateDeleteRightButton();
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
    var form = $("form");
    var tabToShow = window.undefined;
    form.find(".nav-tabs li a")
        .each(function (idx, elem) {
            var id = $(elem).attr("href");
            var pane = $(".tab-pane" + id);
            var errorCount = pane.find(".field-validation-error").length;
            var badge = $(elem).find(".rr-badge");
            badge.text(errorCount);
            if (errorCount > 0) {
                var tab = $(elem).closest("li");
                if (tabToShow === window.undefined || tab.hasClass("active")) {
                    tabToShow = tab;
                }
                badge.show();
            } else {
                badge.hide();
            }
        });
    if (tabToShow !== window.undefined) {
        tabToShow.find("a").click();
    }
    $(window).scrollTop(0);
}

function updateDeleteRightButton() {
    var rights = $(".rr-rights > li");
    if (rights.length === 1) {
        rights.find("button[name=deleteRight]").prop("disabled", true);
    } else {
        rights.find("button[name=deleteRight]").prop("disabled", false);
    }
}

showErrorBadgets();
updateControls();
updateDeleteRightButton();