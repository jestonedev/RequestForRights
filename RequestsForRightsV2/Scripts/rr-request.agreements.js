var cancelAgreementButton = $(".rr-cancel-agreement-dialog-show-button").clone();

$("#rr-agreements-content")
    .on("click",
        ".rr-cancel-agreement-dialog-show-button",
        function (e) {
            $(this).remove();
            $(".rr-request-cancel-agreement-editor").show();
            $(".rr-request-cancel-agreement-editor textarea").focus();
            $(".rr-cancel-agreement-panel").show();
            updateRequestAgreementCancelButtonState();
            $(window).scrollTop($(document).height());
            e.preventDefault();
            return false;
        });

$("#rr-agreements-content").on("keyup",
    "#rr-cancel-agreement-reason",
    updateRequestAgreementCancelButtonState);

$("#rr-agreements-content")
    .on("click", ".rr-cancel-agreement-button",
        function (e) {
            resetAgreementReasonFormState();
            e.preventDefault();
            return false;
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

function resetAgreementReasonFormState() {
    $(".rr-agreement-panel .btn-group").append(cancelAgreementButton);
    $(".rr-request-cancel-agreement-editor").hide();
    $(".rr-cancel-agreement-panel").hide();
}

function beforeSendAgreementCancel() {
    $("#rr-cancel-agreement-reason").attr("readonly", true);
    $(".rr-send-agreement-button").attr("disabled", "disabled");
}

function agreementFailure() {
    $(".rr-agreement-error-alert").css({ "opacity": 1 }).show();
    $("#rr-cancel-agreement-reason").attr("readonly", false);
    $(".rr-agreement-send-button").removeAttr("disabled");
}