var unitOptionsCache = undefined;

$("#rr-request-form")
    .on("change", ".rr-request-user-department select",
        function () {
            var idDepartment = $(this).find("option:selected").data("id-department");
            var unitSelect = $(this).closest(".rr-request-user").find(".rr-request-user-unit select");
            var unit = unitSelect.val();
            var options = unitSelect.find("option");
            if (unitOptionsCache == undefined) {
                unitOptionsCache = options.clone();
            }
            options.remove();
            unitOptionsCache.each(function (idx, option) {
                if ($(option).data("id-department") === idDepartment || $(option).val() === "") {
                    unitSelect.append($(option).clone());
                }
            });
            unitSelect.val(unit);
            if (unitSelect.val() === null) {
                unitSelect.val("");
            }
        });

$("#rr-request-form .rr-request-user-department select").change();

$("#rr-request-form")
    .on("keyup input propertychange",
        ".rr-request-user-snp input",
        function(e) {
            updateUserTitle(e);
            updateRequestDescription();
        });

function updateUserTitle(e)
{
    var titleElem = $(e.target).closest(".rr-request-user").find(".panel-heading .panel-title a");
    var snp = $(e.target).val();
    if ($.trim(snp) === "") {
        titleElem.text("Новый сотрудник");
    } else {
        titleElem.text("Сотрудник «"+snp+"»");
    }
}