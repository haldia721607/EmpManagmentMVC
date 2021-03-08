$(function () {
    //On Country change bind state
    $("#ddlcountry").change(function () {
        var countryId = $("#ddlcountry").val();
        var ddlstate = $("#ddlstate");
        var ddlcity = $("#ddlcity");

        var statesProgress = $("#states-loading-progress");
        ddlstate.html('');
        ddlstate.append($('<option value="">Select State</option>'));
        ddlcity.html('');
        ddlcity.append($('<option value="">Select City</option>'));
        statesProgress.show();
        if (countryId != null && countryId !="") {
            $.ajax({
                type: "GET",
                url: "/EmpManagment/Comman/Account/GetStatelist",
                data: { countryId: countryId },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var states = data.states;
                    for (var i = 0; i < states.length; i++) {
                        ddlstate.append($('<option value="' + states[i].value + '">' + states[i].text + '</option>'));
                    }
                    statesProgress.hide();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert('Failed to retrieve states.');
                    statesProgress.hide();
                }
            });
        } 
        statesProgress.hide();
    });

     //On State change bind city
    $("#ddlstate").change(function () {
        var stateId = $("#ddlstate").val();
        var ddlCity = $("#ddlcity");
        var cityProgress = $("#city-loading-progress");
        ddlCity.html('');
        ddlCity.append($('<option value="">Select City</option>'))
        cityProgress.show();
        if (stateId != null && stateId != "") {
            $.ajax({
                type: "GET",
                url: "/EmpManagment/Comman/Account/GetCitylist",
                data: { stateId: stateId },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    ddlCity.html('');
                    var cities = data.cities;
                    for (var i = 0; i < cities.length; i++) {
                        ddlCity.append($('<option value="' + cities[i].value + '">' + cities[i].text + '</option>'));
                    }
                    cityProgress.hide();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert('Failed to retrieve states.');
                    cityProgress.hide();
                }
            });
        } 
        cityProgress.hide();
    });
});