var datePickerOptions = {
    format: "dd.mm.yyyy",
    weekStart: 1,
    maxViewMode: 2,
    todayBtn: "linked",
    language: "ru",
    orientation: "bottom auto",
    daysOfWeekDisabled: "0,6",
    autoclose: true,
    todayHighlight: true,
    startDate: "01/01/1753"
};

if ($.validator !== undefined) {
    $.extend($.validator.methods, {
        date: function (value, element) {
            if (this.optional(element) && value === "") {
                return true;
            }
            var dateParts = value.split(".");
            if (dateParts.length !== 3) {
                return false;
            }
            return !isNaN(Date.parse(dateParts[2] + "/" + dateParts[1] + "/" + dateParts[0]));
        }
    });
}

$(function () {
    $("body")
        .on("click",
            ".rr-show-date-picker-btn",
            function() {
                $(this).closest(".date.input-group").find("input").datepicker("show");
            });

    var timer = undefined;

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

    $("form")
            .keypress(function (e) {
                if (e.which === 13) {
                    if (e.target.tagName.toLowerCase() === "textarea" ||
                        e.target.tagName.toLowerCase() === "select") {
                        return true;
                    }
                    $(this).find(".rr-save-button").click();
                    e.preventDefault();
                    return false;
                }
                return true;
            });

    $("[data-hide]")
        .on("click",
            function () {
                $("." + $(this).attr("data-hide")).animate({ "opacity": 0 }, 300, function () { $(this).hide() });
            });

    $(".requests-menu")
        .on("click", "#rr-dropdown-menu-all-requests",
            function () {
                resetRequestIndexState();
                setCookie("Request.RequestCategory", "AllRequests", { "path": "/" });
            });

    $(".requests-menu")
        .on("click", "#rr-dropdown-menu-my-requests",
            function () {
                resetRequestIndexState();
                setCookie("Request.RequestCategory", "MyRequests", { "path": "/" });
            });

    $(".requests-menu")
        .on("click", "#rr-dropdown-menu-not-seen-requests",
            function () {
                resetRequestIndexState();
                setCookie("Request.RequestCategory", "NotSeenRequests", { "path": "/" });
            });

    $(".requests-menu")
        .on("click", ".rr-dropdown-menu-request-state-type",
            function () {
                resetRequestIndexState();
                var id = $(this).data("id");
                setCookie("Request.IdRequestStateType", id, { "path": "/" });
            });

    function resetRequestIndexState() {
        deleteCookie("Request.RequestCategory");
        deleteCookie("Request.IdRequestStateType");
        deleteCookie("Request.IdRequestType");
        deleteCookie("Request.DateOfFillingFrom");
        deleteCookie("Request.DateOfFillingTo");
        deleteCookie("Request.Filter");
    }

    function setCookie(name, value, props) {
        props = props || {};
        var exp = props.expires;
        if (typeof exp === "number" && exp) {
            var d = new Date();
            d.setTime(d.getTime() + exp * 1000);
            exp = props.expires = d;
        }
        if (exp && exp.toUTCString) { props.expires = exp.toUTCString(); }
        value = encodeURIComponent(value);
        var updatedCookie = name + "=" + value;
        var propName;
        for (propName in props) {
            if (props.hasOwnProperty(propName)) {
                updatedCookie += "; " + propName;
                var propValue = props[propName];
                if (propValue !== true) {
                    updatedCookie += "=" + propValue;
                }
            }
        }
        document.cookie = updatedCookie;
    }

    function deleteCookie(name) {
        setCookie(name, null, { expires: -1, path: "/" });
    }
});

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

function updateControl(idx, control, namePropRegex, idPropRegex) {
    $(control)
        .find("[name]")
        .filter(function (fieldIdx, field) {
            return $(field).prop("name").match(namePropRegex) != null;
        })
        .each(function (fieldIdx, field) {
            var name = $(field).prop("name").replace(namePropRegex, "$1[" + idx + "]");
            $(field).prop("name", name);
            var id = $(field).prop("id").replace(idPropRegex, "$1_" + idx + "__");
            $(field).prop("id", id);
            $(field).closest(".form-group").find("label").prop("for", id);
            $(field).closest(".form-group").find("span[data-valmsg-for]").attr("data-valmsg-for", name);
        });
}

function transliterate(word) {
    var answer = ""
      , a = {};

    a["Ё"] = "Yo"; a["Й"] = "Y"; a["Ц"] = "C"; a["У"] = "U"; a["К"] = "K"; a["Е"] = "E"; a["Н"] = "N"; a["Г"] = "G"; a["Ш"] = "Sh"; a["Щ"] = "Shch"; a["З"] = "Z"; a["Х"] = "H"; a["Ъ"] = "";
    a["ё"] = "yo"; a["й"] = "y"; a["ц"] = "c"; a["у"] = "u"; a["к"] = "k"; a["е"] = "e"; a["н"] = "n"; a["г"] = "g"; a["ш"] = "sh"; a["щ"] = "sch"; a["з"] = "z"; a["х"] = "h"; a["ъ"] = "";
    a["Ф"] = "F"; a["Ы"] = "Y"; a["В"] = "V"; a["А"] = "A"; a["П"] = "P"; a["Р"] = "R"; a["О"] = "O"; a["Л"] = "L"; a["Д"] = "D"; a["Ж"] = "Zh"; a["Э"] = "E";
    a["ф"] = "f"; a["ы"] = "y"; a["в"] = "v"; a["а"] = "a"; a["п"] = "p"; a["р"] = "r"; a["о"] = "o"; a["л"] = "l"; a["д"] = "d"; a["ж"] = "zh"; a["э"] = "e";
    a["Я"] = "Ya"; a["Ч"] = "Ch"; a["С"] = "S"; a["М"] = "M"; a["И"] = "I"; a["Т"] = "T"; a["Ь"] = ""; a["Б"] = "B"; a["Ю"] = "Yu";
    a["я"] = "ya"; a["ч"] = "ch"; a["с"] = "s"; a["м"] = "m"; a["и"] = "i"; a["т"] = "t"; a["ь"] = ""; a["б"] = "b"; a["ю"] = "yu";

    for (var i = 0; i < word.length; i++) {
        if (a[word[i]] === undefined) {
            answer += word[i];
        } else {
            answer += a[word[i]];
        }
    }
    return answer;
}