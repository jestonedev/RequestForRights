function initializeUsersAutocomplete(userSnp) {
    if (userSnp.length === 0 || $.fn.autocomplete === undefined) return;
    $(userSnp).autocomplete({
        serviceUrl: "/User/GetUsers",
        ajaxSettings: {
            dataType: "json"
        },
        paramName: "snpPattern",
        deferRequestBy: 0,
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
            var form = $(this).closest(".rr-filter-control form");
            form.find("#Login").val(suggestion.data.Login);
            form.find("#Department").val(suggestion.data.Department);
            form.find("#Unit").val(suggestion.data.Unit);
            filterControlOnValue();
        }
    });
}

var snpInput = $(".rr-filter-control form #Snp");
snpInput.off("keyup");
initializeUsersAutocomplete(snpInput);