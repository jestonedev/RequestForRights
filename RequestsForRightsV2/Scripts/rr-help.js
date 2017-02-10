$(function() {
    $("body").attr("data-spy", "scroll");
    $("body").attr("data-target", "#rr-contents");

    setInterval(updateScrollSpy, 1000);
    var updateLocationHrefTimeout = setTimeout(updateLocationHref, 1000);
    var notUpdateLocationHref = false;

    $("img")
        .on("load",
            function() {
                updateScrollSpy();
                updateLocationHref();
            });

    $("body")
        .on("click mousewheel keypress",
            function () {
                clearTimeout(updateLocationHrefTimeout);
                notUpdateLocationHref = true;
                $("body").off("click mousewheel keypress");
            });

    function updateScrollSpy() {
        $('[data-spy="scroll"]')
            .each(function() {
                $(this).scrollspy("refresh");
            });
    }

    function updateLocationHref() {
        if (notUpdateLocationHref) return;
        if ($.inArray("#", location.href) !== -1) {
            location.href = location.href;
        }
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