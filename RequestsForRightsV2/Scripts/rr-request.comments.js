var addCommentButton = $(".rr-add-comment-button").clone();

$(".rr-comment-tab-edit-panel")
    .on("click", ".rr-add-comment-button",
        function (e) {
            $(this).remove();
            $(".rr-request-comment-editor").show();
            $(".rr-request-comment-editor textarea").focus();
            $(".rr-add-comment-panel").show();
            $(".rr-comments-empty").hide();
            updateSendCommentButtonState();
            $(window).scrollTop($(document).height());
            e.preventDefault();
            return false;
        });

$("#rr-new-comment").on("keyup", updateSendCommentButtonState);

$(".rr-cancel-comment-button")
    .on("click",
        function (e) {
            resetCommentFormState();
            e.preventDefault();
            return false;
        });

function beforeAddComment() {
    $("#rr-new-comment").attr("readonly", true);
    $(".rr-send-comment-button").attr("disabled", "disabled");
}

function commentAddingSuccess() {
    $("#rr-new-comment").attr("readonly", false);
    $("#rr-new-comment").val("");
    $(".rr-send-comment-button").removeAttr("disabled");
    resetCommentFormState();
    showCommentsCountBadget();
}

function commentAddingFailure() {
    $(".rr-comment-error-alert").css({ "opacity": 1 }).show();
    $("#rr-new-comment").attr("readonly", false);
    $(".rr-send-comment-button").removeAttr("disabled");
}

function resetCommentFormState() {
    $(".rr-comment-tab-edit-panel .btn-group").append(addCommentButton);
    $(".rr-request-comment-editor").hide();
    $(".rr-add-comment-panel").hide();
    $(".rr-comments-empty").show();
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
    fixIELayoutProblems();
}

showCommentsCountBadget();