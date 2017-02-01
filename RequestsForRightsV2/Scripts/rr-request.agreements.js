var agreementPanel = $(".rr-agreement-panel").clone();

$("#rr-agreements-content")
    .on("click",
        ".rr-cancel-agreement-dialog-show-button",
        function (e) {
            $(this).closest(".rr-agreement-panel").remove();
            $(".rr-request-cancel-agreement-editor").show();
            $(".rr-request-cancel-agreement-editor textarea").focus();
            $(".rr-cancel-agreement-panel").show();
            updateRequestAgreementCancelButtonState();
            $(window).scrollTop($(document).height());
            e.preventDefault();
            return false;
        });

$("#rr-agreements-content")
    .on("click",
        ".rr-add-coordinator-button",
        function (e) {
            $(this).closest(".rr-agreement-panel").remove();
            $(".rr-request-coordinator-editor").show();
            $(".rr-request-coordinator-editor #rr-new-coordinator-snp").focus();
            $(".rr-add-coordinator-panel").show();
            updateRequestAgreementAddCoordinatorButtonState();
            $(window).scrollTop($(document).height());
            e.preventDefault();
            return false;
        });

$("#rr-agreements-content").on("keyup",
    "#rr-cancel-agreement-reason",
    updateRequestAgreementCancelButtonState);

$("#rr-agreements-content")
    .on("click",
        ".rr-cancel-agreement-button",
        function(e) {
            resetAgreementReasonFormState();
            e.preventDefault();
            return false;
        });

$("#rr-agreements-content")
    .on("click",
        ".rr-cancel-coordinator-button",
        function (e) {
            resetAgreementReasonFormState();
            e.preventDefault();
            return false;
        });

$("#rr-agreements-content")
    .on("click",
        ".rr-send-coordinator-button",
        function (e) {
            var error = {};
            var formValid = true;
            var snpInput = $(this).closest("form").find("#rr-new-coordinator-snp");
            var validator = $(this).closest("form").validate();
            if ($.trim(snpInput.val()) === "") {
                error["Coordinator.Snp"] = "ФИО доп. согласующего является обязательным для заполнения";
                formValid = false;
            } else
            if (snpInput.autocomplete().selection == null) {
                error["Coordinator.Snp"] = "ФИО доп. согласующего задано некорректно";
                formValid = false;
            }
            validator.showErrors(error);
            if (!formValid) {
                e.preventDefault();
                return false;
            }
            return true;
        });

$("#rr-agreements-content").on("click", ".rr-send-agreement-button", function() {
    $("#IdRequestStateType").val(5);
});

function updateRequestAgreementCancelButtonState() {
    if ($.trim($("#rr-cancel-agreement-reason").val()) === "") {
        $(".rr-send-agreement-button").attr("disabled", "disabled");
    } else {
        $(".rr-send-agreement-button").removeAttr("disabled");
    }
}

function updateRequestAgreementAddCoordinatorButtonState() {
    if ($.trim($(".rr-request-coordinator-editor #rr-new-coordinator-snp").val()) === "") {
        $(".rr-send-coordinator-button").attr("disabled", "disabled");
    } else {
        $(".rr-send-coordinator-button").removeAttr("disabled");
    }
}

function resetAgreementReasonFormState() {
    $(".rr-agreement-panel-wrapper").append(agreementPanel);
    $(".rr-request-cancel-agreement-editor").hide();
    $(".rr-cancel-agreement-panel").hide();
    $(".rr-request-coordinator-editor").hide();
    $(".rr-add-coordinator-panel").hide();
}

function beforeSendAgreementCancel() {
    $("#rr-cancel-agreement-reason").attr("readonly", true);
    $(".rr-send-agreement-button").attr("disabled", "disabled");
}

function beforeSendAgreement() {
    $(".rr-agreement-panel").hide();
    $(".rr-agreement-sending").show();
}

function sendAgreementFailure() {
    $(".rr-agreement-error-alert").css({ "opacity": 1 }).show();
    $("#rr-cancel-agreement-reason").attr("readonly", false);
    $(".rr-agreement-send-button").removeAttr("disabled");
    $(".rr-agreement-panel").show();
    $(".rr-agreement-sending").hide();
}

function sendAgreementSuccess() {
    $("#rr-new-coordinator-snp").val("");
    $("#rr-new-coordinator-department").val("");
    $("#rr-new-coordinator-unit").val("");
    initializeCoordinatorAutocomplete();
    agreementPanel = $(".rr-agreement-panel").clone();
}

function initializeCoordinatorAutocomplete() {
    var coordinatorInput = $("#rr-agreements-content #rr-new-coordinator-snp");
    if (coordinatorInput.length === 0 || $.fn.autocomplete === undefined) return;
    $(coordinatorInput).autocomplete({
        serviceUrl: "/User/GetAdUsers",
        ajaxSettings: {
            dataType: "json"
        },
        paramName: "snpPattern",
        deferRequestBy: 0,
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
            var item = '<div class="rr-coordinator-autocomplete-snp">' + snp + "</div>";
            if (suggestion.data.Department != undefined) {
                item += '<div class="rr-coordinator-autocomplete-department">' + suggestion.data.Department + "</div>";
            }
            if (suggestion.data.Unit != undefined) {
                item += '<div class="rr-coordinator-autocomplete-unit">' + suggestion.data.Unit + "</div>";
            }
            return item;
        },
        onSelect: function (suggestion) {
            var coordinator = $(this).closest("form");
            coordinator.find("#rr-new-coordinator-login").val(suggestion.data.Login);
            coordinator.find("#rr-new-coordinator-phone").val(suggestion.data.Phone);
            coordinator.find("#rr-new-coordinator-email").val(suggestion.data.Email);
            coordinator.find("#rr-new-coordinator-department").val(suggestion.data.Department);
            coordinator.find("#rr-new-coordinator-unit").val(suggestion.data.Unit);
            $(".rr-send-coordinator-button").removeAttr("disabled");
        },
        onInvalidateSelection: function() {
            $(".rr-send-coordinator-button").attr("disabled", "disabled");
        }
    });
}

function coordinatorAddingFailure() {
    $(".rr-agreement-error-alert").css({ "opacity": 1 }).show();
    $("#rr-new-coordinator-snp").attr("readonly", false);
    $(".rr-send-coordinator-button").removeAttr("disabled");
}

function beforeCoordinatorAdding() {
    $("#rr-new-coordinator-snp").attr("readonly", true);
    $(".rr-send-coordinator-button").attr("disabled", "disabled");
}

initializeCoordinatorAutocomplete();