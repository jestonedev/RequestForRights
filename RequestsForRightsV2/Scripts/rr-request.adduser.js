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