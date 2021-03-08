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
        if (countryId != null && countryId != "") {
            $.ajax({
                type: "GET",
                url: $("#getStates").val(),
                data: { countryId: countryId },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    
                    if (data.data.States.length>0) {
                        for (var i = 0; i < data.data.States.length; i++) {
                            ddlstate.append($('<option value="' + data.data.States[i].Value + '">' + data.data.States[i].Text + '</option>'));
                        }
                    } else {
                        alert('No state found.');
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
                url: $("#getCites").val(),
                data: { stateId: stateId },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    
                    if (data.data.Cities.length>0) {
                        ddlCity.html('');
                        for (var i = 0; i < data.data.Cities.length; i++) {
                            ddlCity.append($('<option value="' + data.data.Cities[i].Value + '">' + data.data.Cities[i].Text + '</option>'));
                        }
                        
                    } else {
                        alert('No city found.');
                        return false;
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