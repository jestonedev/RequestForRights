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

function supportTransitions() {
    return ("WebkitTransition" in document.body.style ||
            "MozTransition" in document.body.style ||
            "OTransition" in document.body.style ||
            "transition" in document.body.style);
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
            if (e.target.type === "textarea" || e.target.type === "select") {
                return true;
            }
            $(this).find(".rr-save-button").click();
            e.preventDefault();
            return false;
        }
        return true;
    });

$("body")
    .on("change focus",
        "select",
        function() {
            showSelectPopover($(this));
        });

$("body")
    .on("mouseenter",
        "select",
        function () {
            if (!$(this).is(":focus")) {
                showSelectPopover($(this));
            }
        });

$("body")
    .on("blur mouseleave",
        "select",
        function () {
            if (!$(this).is(":focus")) {
                $(this).popover("hide");
            }
        });

function showSelectPopover(select) {
    var selectedOption = select.find("option:selected");
    var description = selectedOption.data("description");
    if (!description) {
        select.popover("hide");
        return;
    }
    description = $.trim(description).replace(/\n/g, "<br>");
    if (select.data("prev-content") !== description) {
        select.data("prev-content", select.attr("data-content"));
        select.attr("data-content", description);
        select.popover("show");
    } else {
        if (select.next(".popover:visible").length === 0) {
            select.popover("show");
        }
    }
}