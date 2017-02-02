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
            if (e.target.tagName.toLowerCase() === "textarea" ||
                e.target.tagName.toLowerCase() === "select") {
                e.preventDefault();
                return false;
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
        function () {
            if ($(this).is(":focus")) {
                if (timer) {
                    clearTimeout(timer);
                }
                showSelectPopover($(this));
            }
        });

var timer = undefined;

$("body")
    .on("mouseenter",
        "select",
        function () {
            if (!$(this).is(":focus")) {
                if (timer) {
                    clearTimeout(timer);
                }
                var select = $(this);
                timer = setTimeout(function () { showSelectPopover(select); }, 300);
            }
        });

$("body")
    .on("blur mouseleave",
        "select",
        function () {
            if (!$(this).is(":focus")) {
                if (timer) {
                    clearTimeout(timer);
                }
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
    if (select.data("data-content") !== description) {
        select.attr("data-content", description);
        select.popover("show");
    } else {
        if (select.next(".popover:visible").length === 0) {
            select.popover("show");
        }
    }
}

function getCurrentDate() {
        var date = new Date();
        var year = date.getFullYear();
        var month = date.getMonth()+1;
        var day = date.getDate();
        return year + "-" + (month < 10 ? "0" : "") + month + "-" + (day < 10 ? "0" : "") + day;
}