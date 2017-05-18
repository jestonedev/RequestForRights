
function fixIELayoutProblems() {
    $(".rr-requests-tabs").addClass("fix-layout");
    $(".rr-requests-tabs").removeClass("fix-layout");
}

$(function() {
    $(".rr-request-users > .panel").first().find(".panel-heading a").click();
    $('.rr-request-rights [data-toggle="tooltip"]').tooltip();
});