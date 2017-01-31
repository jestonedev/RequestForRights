(function() {
    /*var userRightsBuffer = [];

    $("#rr-request-form")
        .on("change", ".rr-request-right-grant-type select",
            function () {
                var grantTypeSelect = this;
                var resourceRight = $(grantTypeSelect).closest(".rr-request-right");
                var idRequestRightGrantType = $(grantTypeSelect).val();
                var resourceSelect = $(resourceRight).find(".rr-request-right-resource select");
                var resourceRightSelect = $(resourceRight).find(".rr-request-right-id select");
                var idResource = resourceSelect.val();
                var idResourceRight = resourceRightSelect.val();
                if (idRequestRightGrantType !== "2") {
                    reloadResources(resourceRight);
                    resourceSelect.val(idResource);
                    if (resourceSelect.val()  === null) {
                        resourceSelect.val("");
                    }
                    resourceRightSelect.val(idResourceRight);
                    if (resourceRightSelect.val() === null) {
                        resourceRightSelect.val("");
                    }
                    return;
                }
                var user = {
                    Login: $(grantTypeSelect).closest(".rr-request-user").find(".rr-request-user-login input").val(),
                    Snp: $(grantTypeSelect).closest(".rr-request-user").find(".rr-request-user-snp input").val(),
                    Department: $(grantTypeSelect).closest(".rr-request-user").find(".rr-request-user-department input").val(),
                    Unit: $(grantTypeSelect).closest(".rr-request-user").find(".rr-request-user-unit input").val()
                };
                if (userRightsBuffer[JSON.stringify(user)] != undefined) {
                    filterGrantedResourceAndRights(resourceRight, userRightsBuffer[JSON.stringify(user)]);
                    return;
                }
                $.getJSON("/User/GetPermanentRightsOnDate",
                    {
                        date: getCurrentDate(),
                        "requestUser.Login": user.Login,
                        "requestUser.Snp": user.Snp,
                        "requestUser.Department": user.Department,
                        "requestUser.Unit": user.Unit
                    },
                    function (userRights) {
                        userRightsBuffer[JSON.stringify(user)] = userRights;
                        filterGrantedResourceAndRights(resourceRight, userRights);
                    });
            });

    var rightsOptionsCache = undefined;
    $("#rr-request-form").off("change", ".rr-request-right-resource select");
    $("#rr-request-form")
    .on("change", ".rr-request-right-resource select",
        function () {
            var idResource = $(this).find("option:selected").data("id-resource");
            var rightIdSelect = $(this).closest(".rr-request-right").find(".rr-request-right-id select");
            var idResourceRight = rightIdSelect.val();
            var options = rightIdSelect.find("option");
            if (rightsOptionsCache == undefined) {
                rightsOptionsCache = options.clone();
            }
            options.remove();
            rightsOptionsCache.each(function (idx, option) {
                if ($(option).data("id-resource") === idResource || $(option).val() === "") {
                    rightIdSelect.append($(option).clone());
                }
            });
            rightIdSelect.val(idResourceRight);
            if (rightIdSelect.val() === null) {
                rightIdSelect.val("");
            }
        });

    function getCurrentDate() {
        var date = new Date();
        var year = date.getFullYear();
        var month = date.getMonth()+1;
        var day = date.getDate();
        return year + "-" + (month < 10 ? "0" : "") + month + "-" + (day < 10 ? "0" : "") + day;
    }

    var resourcesCache = undefined;

    function reloadResources(resourceRight) {
        var resourceInput = resourceRight.find(".rr-request-right-resource select");
        if (resourcesCache != undefined) {
            resourceInput.find("option").remove();
            resourcesCache.each(function (idx, option) {
                resourceInput.append($(option).clone());
            });
        }
    }

    function filterGrantedResourceAndRights(resourceRight, userRights) {
        var idRequestRightGrantType = $(resourceRight).find(".rr-request-right-grant-type select").val();
        if (idRequestRightGrantType !== "2") {
            return;
        }
        var resourceInput = resourceRight.find(".rr-request-right-resource select");
        var options = resourceInput.find("option");
        if (resourcesCache == undefined) {
            resourcesCache = options.clone();
        }
        options.remove();
        resourcesCache
            .each(function (idx, option) {
                var has = false;
                for (var i = 0; i < userRights.length; i++) {
                    var idResource = userRights[i].IdResource.toString();
                    if ($(option).data("id-resource") === idResource) {
                        has = true;
                    }
                }
                if (has || $(option).val() === "") {
                    resourceInput.append($(option).clone());
                }
            });
        resourceInput.find("optgroup")
            .each(function(idx, elem) {
                if ($(elem).children().length === 0) {
                    $(elem).remove();
                }
            });
        var rightInput = resourceRight.find(".rr-request-right-id select");

    }*/

})();