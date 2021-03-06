﻿$(function () {
    var prevLogin = $("#Login").val();
    var prevDepartment = $("#Department").val();
    var prevUnit = $("#Unit").val();
    var prevSnp = $("#Snp").val();

    function initializeUsersAutocomplete(userSnp) {
        if (userSnp.length === 0 || $.fn.autocomplete === undefined) return;
        $(userSnp).autocomplete({
            serviceUrl: "/User/GetUsers",
            ajaxSettings: {
                dataType: "json"
            },
            paramName: "snpPattern",
            deferRequestBy: 0,
            noCache: true,
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
                var item = '<div class="rr-request-user-autocomplete-snp">' + snp + "</div>";
                if (suggestion.data.Department != undefined) {
                    item += '<div class="rr-request-user-autocomplete-department">' + suggestion.data.Department + "</div>";
                }
                if (suggestion.data.Unit != undefined) {
                    item += '<div class="rr-request-user-autocomplete-unit">' + suggestion.data.Unit + "</div>";
                }
                return item;
            },
            onSelect: function (suggestion) {
                var newLogin = suggestion.data.Login || "";
                var newDepartment = suggestion.data.Department || "";
                var newUnit = suggestion.data.Unit || "";
                var form = $(this).closest(".rr-filter-control form");
                var newSnp = form.find("#Snp").val();
                if (prevLogin === newLogin &&
                    prevSnp === newSnp &&
                    prevDepartment === newDepartment &&
                    prevUnit === newUnit) {
                    return;
                } else {
                    prevLogin = newLogin;
                    prevSnp = newSnp;
                    prevDepartment = newDepartment;
                    prevUnit = newUnit;
                }
                form.find("#Login").val(suggestion.data.Login);
                form.find("#Department").val(suggestion.data.Department);
                form.find("#Unit").val(suggestion.data.Unit);
                filterControlOnValue();
            },
            onSearchStart: function (query) {
                query.usersCategory = $("#UsersCategory").val();
            }
        });
    }

    var snpInput = $(".rr-filter-control form #Snp");
    snpInput.off("keyup");
    initializeUsersAutocomplete(snpInput);

    $("#UsersCategoryList a")
        .on("click",
            function() {
                $("#UsersCategory").val($(this).data("category"));
                $("#UsersCategoryCurrentLabel").text($.trim($(this).text()));
                $(".rr-filter-control form #Snp").val("");
                $(".rr-filter-control form #Login").val("");
                $(".rr-filter-control form #Department").val("");
                $(".rr-filter-control form #Unit").val("");
                prevLogin = null;
                prevSnp = null;
                prevDepartment = null;
                prevUnit = null;
                filterControlOnValue();
            });
});