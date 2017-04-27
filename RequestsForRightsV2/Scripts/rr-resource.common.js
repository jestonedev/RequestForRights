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
                var isValid = form.valid();
                form.find("input[type=\"file\"]").each(function (idx, elem) {
                    if (!fileIsValid(elem)) {
                        isValid = false;
                    }
                });
                if (isValid) {
                    form.submit();
                } else {
                    showErrorBadgets();
                }
                e.preventDefault();
                return false;
            });

    function fileIsValid(fileElem) {
        var validator = $(fileElem).closest("form").validate();
        var fileParts = $(fileElem).val().split(".");
        var fileExtenstion = "";
        if (fileParts.length > 1) {
            fileExtenstion = fileParts[fileParts.length - 1];
        }
        if ($.inArray(fileExtenstion, ["exe", "dll", "js", "bat", "com", "vbs", "sys"]) !== -1) {
            var error = {};
            error[$(fileElem).attr("name")] = "Запрещено прикреплять исполняемые файлы";
            validator.showErrors(error);
            return false;
        } else {
            $(fileElem)
                .closest(".rr-act-file")
                .find(".field-validation-error")
                .removeClass("field-validation-error")
                .addClass("field-validation-valid")
                .empty();
        }
        return true;
    }

    $("form")
        .on("change",
            ".rr-act-file input[type=\"file\"]",
            function() {
                var actFile = $(this).closest(".rr-act-file");
                actFile.find(".rr-id-file input").val("");
                actFile.find(".rr-act-file-link").remove();
                var fileNameParts = $(this).val().split("\\");
                actFile.find(".rr-act-file-name").text(fileNameParts[fileNameParts.length - 1]);
                actFile.find(".rr-act-file-name").show();
                fileIsValid($(this));
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

    $("form")
        .on("change",
            ".rr-resource-is-dynamic-ip-address input[type=\"checkbox\"]",
            function () {
                var dhcpField = $(this).closest(".rr-internet-address").find(".rr-resource-dhcp-ip-address input");
                if ($(this).is(":checked")) {
                    dhcpField.removeAttr("disabled");
                } else {
                    dhcpField.attr("disabled", "disabled");
                    dhcpField.val("");
                    dhcpField.keyup();
                }
            });

    $("form")
        .on("change",
            ".rr-resource-equal-address-checkbox input[type=\"checkbox\"]",
            function() {
                var addressParts = $(this).closest(".rr-resource-address-panel").find(".rr-resource-address-part");
                if ($(this).is(":checked")) {
                    addressParts.hide();
                } else {
                    addressParts.show();
                }
            });

    $("form").on("click", ".rr-transliterate-button", function(e) {
        var fromId = $(this).data("from-id");
        var toId = $(this).data("to-id");
        $("#" + toId).val(transliterate($("#" + fromId).val()));
        e.preventDefault();
        return false;
    });

    $("form")
        .on("change",
            "#Resource_IdOwnerDepartment",
            function () {
                var layout = $(this).closest(".rr-resource-owner");
                var select = $(this);
                loadDepartmentInfo(layout, select);
            });

    $("form")
        .on("change",
            "#Resource_IdOperatorDepartment",
            function () {
                var layout = $(this).closest(".rr-resource-operator");
                var select = $(this);
                loadDepartmentInfo(layout, select);
            });

    var departmentsInfo = [];

    function loadDepartmentInfo(departmentInfoLayout, departmentSelect) {
        var depName = departmentSelect.find("option:selected").text();
        departmentInfoLayout.find(".rr-resource-department-name input").val(depName);
        var depId = departmentSelect.val();
        if ($.trim(depId) === "") {
            clearDepartmentFields(departmentInfoLayout);
            return;
        }
        if (departmentsInfo[depId] !== undefined) {
            setDepartmentInfo(departmentInfoLayout, departmentsInfo[depId]);
            return;
        }
        disableDepartmentFields(departmentInfoLayout);
        $.getJSON("/Resource/GetDepartmentInfo", {"IdDepartment": depId},
        function (departmentInfo) {
            departmentsInfo[depId] = departmentInfo;
            if (departmentSelect.val() !== depId) {
                return;
            }
            setDepartmentInfo(departmentInfoLayout, departmentInfo);
            enableDepartmentFields(departmentInfoLayout);
        }).fail(function() {
            enableDepartmentFields(departmentInfoLayout);
        });
    }

    function setDepartmentInfo(departmentInfoLayout, departmentInfo) {
        departmentInfoLayout.find(".rr-resource-department-tax-payer-number input").
            val(departmentInfo.TaxPayerNumber);
        departmentInfoLayout.find(".rr-resource-department-official-name-long-ru input").
            val(departmentInfo.OfficialNameLongRu);
        departmentInfoLayout.find(".rr-resource-department-official-name-long-en input").
            val(departmentInfo.OfficialNameLongEn);
        departmentInfoLayout.find(".rr-resource-department-official-name-short-ru input").
            val(departmentInfo.OfficialNameShortRu);
        departmentInfoLayout.find(".rr-resource-department-official-name-short-en input").
            val(departmentInfo.OfficialNameShortEn);
        departmentInfoLayout.find(".rr-resource-self-address-index input").
            val(departmentInfo.SelfAddressIndex);
        departmentInfoLayout.find(".rr-resource-self-address-region input").
            val(departmentInfo.SelfAddressRegion);
        departmentInfoLayout.find(".rr-resource-self-address-area input").
            val(departmentInfo.SelfAddressArea);
        departmentInfoLayout.find(".rr-resource-self-address-city input").
            val(departmentInfo.SelfAddressCity);
        departmentInfoLayout.find(".rr-resource-self-address-street input").
            val(departmentInfo.SelfAddressStreet);
        departmentInfoLayout.find(".rr-resource-self-address-house input").
            val(departmentInfo.SelfAddressHouse);
        departmentInfoLayout.find(".rr-resource-control-address-index input").
            val(departmentInfo.ControlOrgAddressIndex);
        departmentInfoLayout.find(".rr-resource-control-address-region input").
            val(departmentInfo.ControlOrgAddressRegion);
        departmentInfoLayout.find(".rr-resource-control-address-area input").
            val(departmentInfo.ControlOrgAddressArea);
        departmentInfoLayout.find(".rr-resource-control-address-city input").
            val(departmentInfo.ControlOrgAddressCity);
        departmentInfoLayout.find(".rr-resource-control-address-street input").
            val(departmentInfo.ControlOrgAddressStreet);
        departmentInfoLayout.find(".rr-resource-control-address-house input").
            val(departmentInfo.ControlOrgAddressHouse);
        var equalAddressCheckbox = departmentInfoLayout
            .find(".rr-resource-equal-address-checkbox input[type=\"checkbox\"]");
        equalAddressCheckbox.prop("checked", departmentInfo.СontrolOrgAddressesAreEqualSelfAddress);
        equalAddressCheckbox.change();

    }

    var fieldsSelectors = [
        ".rr-resource-department-tax-payer-number",
        ".rr-resource-department-official-name-long-ru",
        ".rr-resource-department-official-name-long-en",
        ".rr-resource-department-official-name-short-ru",
        ".rr-resource-department-official-name-short-en",
        ".rr-resource-self-address-index",
        ".rr-resource-self-address-region",
        ".rr-resource-self-address-area",
        ".rr-resource-self-address-city",
        ".rr-resource-self-address-street",
        ".rr-resource-self-address-house",
        ".rr-resource-control-address-index",
        ".rr-resource-control-address-region",
        ".rr-resource-control-address-area",
        ".rr-resource-control-address-city",
        ".rr-resource-control-address-street",
        ".rr-resource-control-address-house"
    ];

    function clearDepartmentFields(departmentInfoLayout) {
        for (var i = 0; i < fieldsSelectors.length; i++) {
            departmentInfoLayout.find(fieldsSelectors[i]+" input").val("");
        }
        var equalAddressCheckbox = departmentInfoLayout
            .find(".rr-resource-equal-address-checkbox input[type=\"checkbox\"]");
        equalAddressCheckbox.prop("checked", false);
        equalAddressCheckbox.change();
    }

    function disableDepartmentFields(departmentInfoLayout) {
        for (var i = 0; i < fieldsSelectors.length; i++) {
            departmentInfoLayout.find(fieldsSelectors[i] + " input").attr("readonly", "readonly");
        }
        var equalAddressCheckbox = departmentInfoLayout
            .find(".rr-resource-equal-address-checkbox input[type=\"checkbox\"]");
        equalAddressCheckbox.attr("disabled", "disabled");
    }

    function enableDepartmentFields(departmentInfoLayout) {
        for (var i = 0; i < fieldsSelectors.length; i++) {
            departmentInfoLayout.find(fieldsSelectors[i] + " input").removeAttr("readonly");
        }
        var equalAddressCheckbox = departmentInfoLayout
            .find(".rr-resource-equal-address-checkbox input[type=\"checkbox\"]");
        equalAddressCheckbox.removeAttr("disabled");
    }

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
        addedElem.find(".rr-resource-ip-address input")
        .each(function () {
            $(this).inputmask("Regex", { regex: $(this).data("val-regex-pattern") });
        });
        $(window).scrollTop(addedElem.offset().top - $(window).height() / 2 + $(addedElem).height()/2);
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
        $(addedElem)
            .find(".rr-resource-address-index input")
            .each(function() {
                $(this).inputmask("Regex", { regex: $(this).data("val-regex-pattern") });
            });

        $(window).scrollTop(addedElem.offset().top - $(window).height() / 2 + $(addedElem).height()/2);
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
        $(window).scrollTop(addedElem.offset().top - $(window).height() / 2 + $(addedElem).height()/2);
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
        $(window).scrollTop(addedElem.offset().top - $(window).height() / 2 + $(addedElem).height()/2);
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
        $(window).scrollTop(addedElem.offset().top - $(window).height() / 2 + $(addedElem).height()/2);
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
        $(window).scrollTop(addedElem.offset().top - $(window).height() / 2 + $(addedElem).height()/2);
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
        $(window).scrollTop(addedElem.offset().top - $(window).height() / 2 + $(addedElem).height()/2);
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
        $(window).scrollTop(addedElem.offset().top - $(window).height() / 2 + $(addedElem).height()/2);
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
        $(window).scrollTop(addedElem.offset().top - $(window).height() / 2 + $(addedElem).height()/2);
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

    $('.rr-resource-request-perm-all-departments-checkbox input[type="checkbox"]')
        .on("change",
        function() {
            updatePermissionsCheckboxes(true);
        });

    function updatePermissionsCheckboxes(ignoreAllDepartmentsCheckgox) {
        var allDepartmentsCheckbox = $('.rr-resource-request-perm-all-departments-checkbox input[type="checkbox"]');
        if (($('.rr-resource-request-perm-departments input[type="checkbox"]:checked').length === 0 && !ignoreAllDepartmentsCheckgox) ||
            allDepartmentsCheckbox.prop("checked")) {
            allDepartmentsCheckbox.prop("checked", true);
            $(".rr-resource-request-perm-departments .checkbox").each(function(idx, elem) {
                $(elem).addClass("disabled");
                $(elem).find('input[type="checkbox"]').prop("disabled", "disabled");
            });
        } 
        else {
            $(".rr-resource-request-perm-departments .checkbox").each(function (idx, elem) {
                $(elem).removeClass("disabled");
                $(elem).find('input[type="checkbox"]').removeProp("disabled");
            });
        }
    }

    showErrorBadgets();
    updateControls();
    updateDeleteRightButton();
    initalizeDatePickers($(".rr-resource-date input"));
    $("form .rr-resource-has-not-internet-access-checkbox input[type=\"checkbox\"]").change();
    $("form .rr-resource-equal-address-checkbox input[type=\"checkbox\"]").change();
    $("form .rr-resource-is-dynamic-ip-address input[type=\"checkbox\"]").change();

    var email = $("#Resource_EmailAdministrator");
    email.inputmask("Regex", { regex: email.data("val-regex-pattern") });

    $(".rr-resource-ip-address input")
        .each(function() {
            $(this).inputmask("Regex", { regex: $(this).data("val-regex-pattern") });
        });

    $(".rr-resource-address-index input")
        .each(function () {
            $(this).inputmask("Regex", { regex: $(this).data("val-regex-pattern") });
        });

    updatePermissionsCheckboxes(false);
});