$(function() {
    $("body").attr("data-spy", "scroll");
    $("body").attr("data-target", "#rr-contents");

    setInterval(updateScrollSpy, 1000);

    function updateScrollSpy() {
        $('[data-spy="scroll"]')
            .each(function() {
                $(this).scrollspy("refresh");
            });
    }

    $(".rr-content p[id]")
        .each(function(idx, elem) {
            $(elem).addClass("rr-fix-p-mainmenu-offset");
        });

    $(".rr-content h1[id], .rr-content h2[id], .rr-content h3[id]")
        .each(function(idx, elem) {
            $(elem).addClass("rr-fix-h-mainmenu-offset");
        });
});