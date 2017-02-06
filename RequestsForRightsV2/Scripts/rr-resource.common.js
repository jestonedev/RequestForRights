$(function() {

    $("form")
        .on("click",
            '[name="addRight"]',
            function (e) {
                addRight($(".rr-rights"));
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="deleteRight"]',
            function (e) {
                $(this).closest(".rr-right").remove();
                updateControls();
                refreshValidation();
                updateDeleteRightButton();
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="addInternetAddress"]',
            function (e) {
                addInternetAddress($(".rr-internet-addresses"));
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="deleteInternetAddress"]',
            function (e) {
                $(this).closest(".rr-internet-address").remove();
                updateControls();
                refreshValidation();
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="addDeviceAddress"]',
            function (e) {
                addDeviceAddress($(".rr-device-addresses"));
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="deleteDeviceAddress"]',
            function (e) {
                $(this).closest(".rr-device-address").remove();
                updateControls();
                refreshValidation();
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="addOwnerPerson"]',
            function (e) {
                addOwnerPerson($(".rr-owner-persons"));
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="deleteOwnerPerson"]',
            function (e) {
                $(this).closest(".rr-owner-person").remove();
                updateControls();
                refreshValidation();
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="addOwnerPersonAct"]',
            function (e) {
                addOwnerPersonAct($(this).closest(".rr-owner-person").find(".rr-owner-person-acts"));
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="deleteOwnerPersonAct"]',
            function (e) {
                $(this).closest(".rr-owner-person-act").remove();
                updateControls();
                refreshValidation();
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="addOperatorPerson"]',
            function (e) {
                addOperatorPerson($(".rr-operator-persons"));
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="deleteOperatorPerson"]',
            function (e) {
                $(this).closest(".rr-operator-person").remove();
                updateControls();
                refreshValidation();
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="addOperatorPersonAct"]',
            function (e) {
                addOperatorPersonAct($(this).closest(".rr-operator-person").find(".rr-operator-person-acts"));
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="deleteOperatorPersonAct"]',
            function (e) {
                $(this).closest(".rr-operator-person-act").remove();
                updateControls();
                refreshValidation();
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="addOperatorAct"]',
            function (e) {
                addOperatorAct($(".rr-operator-acts"));
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="deleteOperatorAct"]',
            function (e) {
                $(this).closest(".rr-operator-act").remove();
                updateControls();
                refreshValidation();
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="addUsingAct"]',
            function (e) {
                addUsingAct($(".rr-using-acts"));
                e.preventDefault();
                return false;
            });

    $("form")
        .on("click",
            '[name="deleteUsingAct"]',
            function (e) {
                $(this).closest(".rr-using-act").remove();
                updateControls();
                refreshValidation();
                e.preventDefault();
                return false;
            });

    $("form")
       .on("click",
           '[name="addAuthorityAct"]',
           function (e) {
               addAuthorityAct($(".rr-authority-acts"));
               e.preventDefault();
               return false;
           });

    $("form")
       .on("click",
           '[name="deleteAuthorityAct"]',
           function (e) {
               $(this).closest(".rr-authority-act").remove();
               updateControls();
               refreshValidation();
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

    $("form")
        .on("change",
            ".rr-act-file input[type=\"file\"]",
            function() {
                var actFile = $(this).closest(".rr-act-file");
                actFile.find(".rr-id-file input").val(0);
                actFile.find(".rr-act-file-link").remove();
                var fileNameParts = $(this).val().split("\\");
                actFile.find(".rr-act-file-name").text(fileNameParts[fileNameParts.length - 1]);
            });

    $("form")
        .on("change",
            ".rr-resource-has-not-internet-access-checkbox input[type=\"checkbox\"]",
            function() {
                if ($(this).is(":checked")) {
                    $(".rr-internet-addresses-wrapper").hide();
                } else {
                    $(".rr-internet-addresses-wrapper").show();
                }
            });

    function updateControls() {
        var rightNamePropRegex = /(Rights)\[\d+\]/;
        var rightIdPropRegex = /(Rights)_\d+__/;
        var rights = $(".rr-rights > li");
        rights.each(function (rightIdx, rightElem) {
            updateControl(rightIdx, rightElem, rightNamePropRegex, rightIdPropRegex);
        });

        var internetAddressNamePropRegex = /(ResourceInternetAddresses)\[\d+\]/;
        var internetAddressIdPropRegex = /(ResourceInternetAddresses)_\d+__/;
        var internetAddresses = $(".rr-internet-addresses > li");
        internetAddresses.each(function (internetAddressIdx, internetAddressElem) {
            updateControl(internetAddressIdx, internetAddressElem, internetAddressNamePropRegex, internetAddressIdPropRegex);
        });

        var deviceAddressNamePropRegex = /(ResourceDeviceAddresses)\[\d+\]/;
        var deviceAddressIdPropRegex = /(ResourceDeviceAddresses)_\d+__/;
        var deviceAddresses = $(".rr-device-addresses > li");
        deviceAddresses.each(function (deviceAddressIdx, deviceAddressElem) {
            updateControl(deviceAddressIdx, deviceAddressElem, deviceAddressNamePropRegex, deviceAddressIdPropRegex);
        });

        var ownerPersonNamePropRegex = /(ResourceOwnerPersons)\[\d+\]/;
        var ownerPersonIdPropRegex = /(ResourceOwnerPersons)_\d+__/;
        var ownerPersonActNamePropRegex = /(Acts)\[\d+\]/;
        var ownerPersonActIdPropRegex = /(Acts)_\d+__/;
        var ownerPersons = $(".rr-owner-persons > li");
        ownerPersons.each(function (ownerPersonIdx, ownerPersonElem) {
            updateControl(ownerPersonIdx, ownerPersonElem, ownerPersonNamePropRegex, ownerPersonIdPropRegex);
            var acts = $(ownerPersonElem).find(".rr-owner-person-acts > li");
            acts.each(function (actIdx, actElem) {
                updateControl(actIdx, actElem, ownerPersonActNamePropRegex, ownerPersonActIdPropRegex);
            });
        });

        var operatorPersonNamePropRegex = /(ResourceOperatorPersons)\[\d+\]/;
        var operatorPersonIdPropRegex = /(ResourceOperatorPersons)_\d+__/;
        var operatorPersonActNamePropRegex = /(Acts)\[\d+\]/;
        var operatorPersonActIdPropRegex = /(Acts)_\d+__/;
        var operatorPersons = $(".rr-operator-persons > li");
        operatorPersons.each(function (operatorPersonIdx, operatorPersonElem) {
            updateControl(operatorPersonIdx, operatorPersonElem, operatorPersonNamePropRegex, operatorPersonIdPropRegex);
            var acts = $(operatorPersonElem).find(".rr-operator-person-acts > li");
            acts.each(function (actIdx, actElem) {
                updateControl(actIdx, actElem, operatorPersonActNamePropRegex, operatorPersonActIdPropRegex);
            });
        });

        var operatorActNamePropRegex = /(ResourceOperatorActs)\[\d+\]/;
        var operatorActIdPropRegex = /(ResourceOperatorActs)_\d+__/;
        var operatorActs = $(".rr-operator-acts > li");
        operatorActs.each(function (operatorActIdx, operatorActElem) {
            updateControl(operatorActIdx, operatorActElem, operatorActNamePropRegex, operatorActIdPropRegex);
        });

        var usingActNamePropRegex = /(ResourceUsingActs)\[\d+\]/;
        var usingActIdPropRegex = /(ResourceUsingActs)_\d+__/;
        var usingActs = $(".rr-using-acts > li");
        usingActs.each(function (usingActIdx, usingActElem) {
            updateControl(usingActIdx, usingActElem, usingActNamePropRegex, usingActIdPropRegex);
        });

        var authorityActNamePropRegex = /(ResourceAuthorityActs)\[\d+\]/;
        var authorityActIdPropRegex = /(ResourceAuthorityActs)_\d+__/;
        var authorityActs = $(".rr-authority-acts > li");
        authorityActs.each(function (authorityActIdx, authorityActElem) {
            updateControl(authorityActIdx, authorityActElem, authorityActNamePropRegex, authorityActIdPropRegex);
        });
    }

    var rightTemplate = undefined;

    function addRight(rightLayout) {
        if (rightTemplate !== undefined) {
            addRightAfterLoad(rightLayout, rightTemplate);
            return;
        }
        $.get("/Resource/GetEmptyRightTemplate", function (template) {
            rightTemplate = template;
            addRightAfterLoad(rightLayout, rightTemplate);
        });
    }

    function addRightAfterLoad(rightLayout, template) {
        rightLayout.append(template);
        updateControls();
        refreshValidation();
        $(".rr-rights-error").remove();
        updateDeleteRightButton();
        $(window).scrollTop($(document).height());
    }

    var internetAddressTemplate = undefined;

    function addInternetAddress(internetAddressLayout) {
        if (internetAddressTemplate !== undefined) {
            addInternetAddressAfterLoad(internetAddressLayout, internetAddressTemplate);
            return;
        }
        $.get("/Resource/GetEmptyInternetAddressTemplate", function (template) {
            internetAddressTemplate = template;
            addInternetAddressAfterLoad(internetAddressLayout, template);
        });
    }

    function addInternetAddressAfterLoad(internetAddressLayout, template) {
        internetAddressLayout.append(template);
        updateControls();
        refreshValidation();
        var addedElem = $(internetAddressLayout.find(".rr-internet-address").last());
        $(window).scrollTop(addedElem.offset().top - $(".rr-main-menu").height() - 50);
    }

    var deviceAddressTemplate = undefined;

    function addDeviceAddress(deviceAddressLayout) {
        if (deviceAddressTemplate !== undefined) {
            addDeviceAddressAfterLoad(deviceAddressLayout, deviceAddressTemplate);
            return;
        }
        $.get("/Resource/GetEmptyDeviceAddressTemplate", function (template) {
            deviceAddressTemplate = template;
            addDeviceAddressAfterLoad(deviceAddressLayout, template);
        });
    }

    function addDeviceAddressAfterLoad(deviceAddressLayout, template) {
        deviceAddressLayout.append(template);
        updateControls();
        refreshValidation();
        var addedElem = $(deviceAddressLayout.find(".rr-device-address").last());
        $(window).scrollTop(addedElem.offset().top - $(".rr-main-menu").height() - 50);
    }

    var ownerPersonTemplate = undefined;

    function addOwnerPerson(ownerPersonLayout) {
        if (ownerPersonTemplate !== undefined) {
            addOwnerPersonAfterLoad(ownerPersonLayout, ownerPersonTemplate);
            return;
        }
        $.get("/Resource/GetEmptyOwnerPersonTemplate", function (template) {
            ownerPersonTemplate = template;
            addOwnerPersonAfterLoad(ownerPersonLayout, template);
        });
    }

    function addOwnerPersonAfterLoad(ownerPersonLayout, template) {
        ownerPersonLayout.append(template);
        updateControls();
        refreshValidation();
        var addedElem = $(ownerPersonLayout.find(".rr-owner-person").last());
        $(window).scrollTop(addedElem.offset().top - $(".rr-main-menu").height() - 50);
    }

    var ownerPersonActTemplate = undefined;

    function addOwnerPersonAct(ownerPersonActLayout) {
        if (ownerPersonActTemplate !== undefined) {
            addOwnerPersonActAfterLoad(ownerPersonActLayout, ownerPersonActTemplate);
            return;
        }
        $.get("/Resource/GetEmptyOwnerPersonActTemplate", function (template) {
            ownerPersonActTemplate = template;
            addOwnerPersonActAfterLoad(ownerPersonActLayout, template);
        });
    }

    function addOwnerPersonActAfterLoad(ownerPersonActLayout, template) {
        ownerPersonActLayout.append(template);
        updateControls();
        refreshValidation();
        var addedElem = $(ownerPersonActLayout.find(".rr-owner-person-act").last());
        initalizeDatePickers(addedElem.find(".rr-resource-date input"));
        $(window).scrollTop(addedElem.offset().top - $(".rr-main-menu").height() - 50);
    }

    var operatorPersonTemplate = undefined;

    function addOperatorPerson(operatorPersonLayout) {
        if (operatorPersonTemplate !== undefined) {
            addOperatorPersonAfterLoad(operatorPersonLayout, operatorPersonTemplate);
            return;
        }
        $.get("/Resource/GetEmptyOperatorPersonTemplate", function (template) {
            operatorPersonTemplate = template;
            addOperatorPersonAfterLoad(operatorPersonLayout, template);
        });
    }

    function addOperatorPersonAfterLoad(operatorPersonLayout, template) {
        operatorPersonLayout.append(template);
        updateControls();
        refreshValidation();
        var addedElem = $(operatorPersonLayout.find(".rr-operator-person").last());
        $(window).scrollTop(addedElem.offset().top - $(".rr-main-menu").height() - 50);
    }

    var operatorPersonActTemplate = undefined;

    function addOperatorPersonAct(operatorPersonActLayout) {
        if (operatorPersonActTemplate !== undefined) {
            addOperatorPersonActAfterLoad(operatorPersonActLayout, operatorPersonActTemplate);
            return;
        }
        $.get("/Resource/GetEmptyOperatorPersonActTemplate", function (template) {
            operatorPersonActTemplate = template;
            addOperatorPersonActAfterLoad(operatorPersonActLayout, template);
        });
    }

    function addOperatorPersonActAfterLoad(operatorPersonActLayout, template) {
        operatorPersonActLayout.append(template);
        updateControls();
        refreshValidation();
        var addedElem = $(operatorPersonActLayout.find(".rr-operator-person-act").last());
        initalizeDatePickers(addedElem.find(".rr-resource-date input"));
        $(window).scrollTop(addedElem.offset().top - $(".rr-main-menu").height() - 50);
    }

    var operatorActTemplate = undefined;

    function addOperatorAct(operatorActLayout) {
        if (operatorActTemplate !== undefined) {
            addOperatorActAfterLoad(operatorActLayout, operatorActTemplate);
            return;
        }
        $.get("/Resource/GetEmptyOperatorActTemplate", function (template) {
            operatorActTemplate = template;
            addOperatorActAfterLoad(operatorActLayout, template);
        });
    }

    function addOperatorActAfterLoad(operatorActLayout, template) {
        operatorActLayout.append(template);
        updateControls();
        refreshValidation();
        var addedElem = $(operatorActLayout.find(".rr-operator-act").last());
        initalizeDatePickers(addedElem.find(".rr-resource-date input"));
        $(window).scrollTop(addedElem.offset().top - $(".rr-main-menu").height() - 50);
    }

    var usingActTemplate = undefined;

    function addUsingAct(usingActLayout) {
        if (usingActTemplate !== undefined) {
            addUsingActAfterLoad(usingActLayout, usingActTemplate);
            return;
        }
        $.get("/Resource/GetEmptyUsingActTemplate", function (template) {
            usingActTemplate = template;
            addUsingActAfterLoad(usingActLayout, template);
        });
    }

    function addUsingActAfterLoad(usingActLayout, template) {
        usingActLayout.append(template);
        updateControls();
        refreshValidation();
        var addedElem = $(usingActLayout.find(".rr-using-act").last());
        initalizeDatePickers(addedElem.find(".rr-resource-date input"));
        $(window).scrollTop(addedElem.offset().top - $(".rr-main-menu").height() - 50);
    }

    var authorityActTemplate = undefined;

    function addAuthorityAct(authorityActLayout) {
        if (authorityActTemplate !== undefined) {
            addAuthorityActAfterLoad(authorityActLayout, authorityActTemplate);
            return;
        }
        $.get("/Resource/GetEmptyAuthorityActTemplate", function (template) {
            authorityActTemplate = template;
            addAuthorityActAfterLoad(authorityActLayout, template);
        });
    }

    function addAuthorityActAfterLoad(authorityActLayout, template) {
        authorityActLayout.append(template);
        updateControls();
        refreshValidation();
        var addedElem = $(authorityActLayout.find(".rr-authority-act").last());
        initalizeDatePickers(addedElem.find(".rr-resource-date input"));
        $(window).scrollTop(addedElem.offset().top - $(".rr-main-menu").height() - 50);
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

    function initalizeDatePickers(nodes) {
        $(nodes).datepicker(datePickerOptions);  
    }

    showErrorBadgets();
    updateControls();
    updateDeleteRightButton();
    initalizeDatePickers($(".rr-resource-date input"));
    $("form .rr-resource-has-not-internet-access-checkbox input[type=\"checkbox\"]").change();
});