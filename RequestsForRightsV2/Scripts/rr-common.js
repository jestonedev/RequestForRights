function debounce(func, wait, immediate) {
    var timeout;
    return function () {
        var context = this, args = arguments;
        var later = function () {
            timeout = null;
            if (!immediate) func.apply(context, args);
        };
        var callNow = immediate && !timeout;
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
        if (callNow) func.apply(context, args);
    };
};

function scrollToElement(elem) {
    $(window).scrollTop($(elem).offset().top + $(".rr-main-menu").height());
}

$(function() {
    $("[data-hide]")
        .on("click",
            function() {
                $("." + $(this).attr("data-hide")).animate({ "opacity": 0 }, 300, function() { $(this).hide() });
            });
});

$("form")
    .keypress(function (e) {
        if (e.which === 13) {
            if (e.target.type === "textarea") {
                return true;
            }
            $(this).find(".rr-save-button").click();
            e.preventDefault();
            return false;
        }
        return true;
    });