$(function () {
    //For check server side error 
    var errorId = $('#errorId').val();
    var messageId = $('#messageId').val();

    //If any other in server that error will be show 
    if (errorId != null && errorId != "") {
        toastr.error(message = errorId, title = 'Error')
        $("#divError").show();
    } else if (messageId != null && messageId != "") {
        if (messageId == "Added") {
            toastr.success(message = 'Complaient created successfully.', title = 'Added')
        }
    } else {
        $("#divError").hide();
    }

    //Function for - Only single image uplod to floder with image preview
    $('.clSingleImageSaveToFolder').on("change", function () {
        //For single image name show in file upload
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);
        $("#pSingleImageSaveToFolderId").empty();
        $('#pSingleImageSaveToFolderId').text(fileName);
        $("#pSingleImageSaveToFolderId").attr("title", "Click here to download - " + fileName)
        var files = $(this)[0].files;
        if (files.length > 0 && files.length < 2) {
            var oFReader = new FileReader();
            oFReader.readAsDataURL(document.getElementById("iSingleImageSaveToFolder").files[0]);

            oFReader.onload = function (oFREvent) {
                document.getElementById("imgSingleImageSaveToFolderPreview").src = oFREvent.target.result;
            };
        }
    });
    //Function for- Muliple images uplod to floder with multiple image preview
    $("#iMultipleImageSaveToFolder").on('change', function () {
        $(".clMultipleImageSaveToFolder").empty();//you can remove this code if you want previous user input
        if (this.files.length > 0 && this.files.length < 5) {
            $("#spnMultipleImageSaveToFolder").hide();
            for (let i = 0; i < this.files.length; ++i) {
                let dataName = this.files[i].name;
                let filereader = new FileReader();
                let $img = jQuery.parseHTML("<img src='' style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'><br /> <p style='width: 100px;' title='Click here to download - " + dataName + "'>" + dataName + "</p>");
                filereader.onload = function () {
                    $img[0].src = this.result;
                };
                filereader.readAsDataURL(this.files[i]);
                $(".clMultipleImageSaveToFolder").append($img);
            }
        } else {
            $("#spnMultipleImageSaveToFolder").show();
        }
    });

    //Function for - Only single image uplod to database with image preview
    $('.clSingleImageSaveToDatabase').on("change", function () {
        //For single image name show in file upload
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);
        $("#pSingleImageSaveToDatabaseId").empty();
        $('#pSingleImageSaveToDatabaseId').text(fileName);
        $("#pSingleImageSaveToDatabaseId").attr("title", "Click here to download - " + fileName)
        var files = $(this)[0].files;
        if (files.length > 0 && files.length < 2) {
            var oFReader = new FileReader();
            oFReader.readAsDataURL(document.getElementById("iSingleImageSaveToDatabase").files[0]);

            oFReader.onload = function (oFREvent) {
                document.getElementById("imgSingleImageSaveToDatabase").src = oFREvent.target.result;
            };
        }
    });
    //Function for- Muliple images uplod to database with multiple image preview
    $("#iMultipleImageSaveToDatabase").on('change', function () {
        $(".clMultipleImageSaveToDatabase").empty();//you can remove this code if you want previous user input
        if (this.files.length > 0 && this.files.length < 5) {
            $("#spanMaxImageDatabase").hide();
            for (let i = 0; i < this.files.length; ++i) {
                let dataName = this.files[i].name;
                let filereader = new FileReader();
                let $img = jQuery.parseHTML("<img src='' style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'><br /> <p style='width: 100px;' title='Click here to download - " + dataName + "'>" + dataName + "</p>");
                filereader.onload = function () {
                    $img[0].src = this.result;
                };
                filereader.readAsDataURL(this.files[i]);
                $(".clMultipleImageSaveToDatabase").append($img);
            }
        } else {
            $("#spanMaxImageDatabase").show();
        }
    });

    //Function for - Only single file(docx/xlxs/pdf) uplod to floder
    $('.clSingleFileSaveToFolder').on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);
        var extension = fileName.substr((fileName.lastIndexOf('.') + 1));
        if ($(this)[0].files.length > 0 && $(this)[0].files.length < 2) {
            $("#pSingleFileSaveToFolderId").empty();
            $('#pSingleFileSaveToFolderId').text(fileName);
            $("#pSingleFileSaveToFolderId").attr("title", "Click here to download - " + fileName)
            switch (extension) {
                case 'docx':
                    $("#imgSingleFileSaveToFolder").empty();
                    $('#imgSingleFileSaveToFolder').attr('src', '/EmpManagment/img/Word.png');
                    break;
                case 'xlsx':
                    $("#imgSingleFileSaveToFolder").empty();
                    $('#imgSingleFileSaveToFolder').attr('src', '/EmpManagment/img/Excel.png');
                    break;
                case 'pdf':
                    $("#imgSingleFileSaveToFolder").empty();
                    $('#imgSingleFileSaveToFolder').attr('src', '/EmpManagment/img/Pdf.png');
                    break;
                default:
                //alert('who knows');
            }
        }
    });
    //Function for - multiple file(docx/xlxs/pdf) uplod to floder
    $("#iMultipleFileSaveToFolder").on('change', function () {
        $(".clMultipleFileSaveToFolder").empty();//you can remove this code if you want previous user input
        if (this.files.length > 0 && this.files.length < 3) {
            $("#spnMaxMultipleFileSaveToFolder").hide();
            for (var i = 0; i < $(this).get(0).files.length; ++i) {
                let dataName = this.files[i].name;
                var fileName = $(this).get(0).files[i].name;
                var extension = fileName.substr((fileName.lastIndexOf('.') + 1));
                let $img = "";
                switch (extension) {
                    case 'docx':
                        $img = jQuery.parseHTML("<img src=" + '/EmpManagment/img/Word.png' + " style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'><br /> <p style='width: 100px;' title='Click here to download - " + dataName + "'>" + dataName + "</p>");
                        break;
                    case 'xlsx':
                        $img = jQuery.parseHTML("<img src=" + '/EmpManagment/img/Excel.png' + " style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'><br /> <p style='width: 100px;' title='Click here to download - " + dataName + "'>" + dataName + "</p>");
                        break;
                    case 'pdf':
                        $img = jQuery.parseHTML("<img src=" + '/EmpManagment/img/Pdf.png' + " style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'><br /> <p style='width: 100px;' title='Click here to download - " + dataName + "'>" + dataName + "</p>");
                        break;
                    default:
                    //alert('who knows');
                }
                $(".clMultipleFileSaveToFolder").append($img);
            }
        } else {
            $("#spnMaxMultipleFileSaveToFolder").show();
        }
    });

    //Function for - Only single file(docx/xlxs/pdf) uplod to database
    $('.clSingleFileSaveToDatabase').on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);
        var extension = fileName.substr((fileName.lastIndexOf('.') + 1));
        if ($(this)[0].files.length > 0 && $(this)[0].files.length < 2) {
            $("#pSingleFileSaveToDatabase").empty();
            $('#pSingleFileSaveToDatabase').text(fileName);
            $("#pSingleFileSaveToDatabase").attr("title", "Click here to download - " + fileName)
            switch (extension) {
                case 'docx':
                    $('#imgSingleFileSaveToDatabase').attr('src', '/EmpManagment/img/Word.png');
                    break;
                case 'xlsx':
                    $('#imgSingleFileSaveToDatabase').attr('src', '/EmpManagment/img/Excel.png');
                    break;
                case 'pdf':
                    $('#imgSingleFileSaveToDatabase').attr('src', '/EmpManagment/img/Pdf.png');
                    break;
                default:
                //alert('who knows');
            }
        }
    });
    //Function for - multiple file(docx/xlxs/pdf) uplod to database
    $("#iMultipleFileSaveToDatabase").on('change', function () {
        $(".clMultipleFileSaveToDatabase").empty();//you can remove this code if you want previous user input
        if (this.files.length > 0 && this.files.length < 3) {
            $("#spnMaxMultipleFileSaveToDatabase").hide();
            for (var i = 0; i < $(this).get(0).files.length; ++i) {
                let dataName = this.files[i].name;
                var fileName = $(this).get(0).files[i].name;
                var extension = fileName.substr((fileName.lastIndexOf('.') + 1));
                let $img = "";
                switch (extension) {
                    case 'docx':
                        $img = jQuery.parseHTML("<img src=" + '/EmpManagment/img/Word.png' + " style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'><br /> <p style='width: 100px;' title='Click here to download - " + dataName + "'>" + dataName + "</p>");
                        break;
                    case 'xlsx':
                        $img = jQuery.parseHTML("<img src=" + '/EmpManagment/img/Excel.png' + " style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'><br /> <p style='width: 100px;' title='Click here to download - " + dataName + "'>" + dataName + "</p>");
                        break;
                    case 'pdf':
                        $img = jQuery.parseHTML("<img src=" + '/EmpManagment/img/Pdf.png' + " style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'><br /> <p style='width: 100px;' title='Click here to download - " + dataName + "'>" + dataName + "</p>");
                        break;
                    default:
                    //alert('who knows');
                }
                $(".clMultipleFileSaveToDatabase").append($img);
            }
        } else {
            $("#spnMaxMultipleFileSaveToDatabase").show();
        }
    });

    //Function for -Bulk insert to database using(xlx/xlxs)
    $('.clExcelFileDataSaveToDatabase').on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);
        if ($(this)[0].files.length > 0 && $(this)[0].files.length < 2) {
            $("#pExcelFileDataSaveToDatabaseId").empty();
            $('#pExcelFileDataSaveToDatabaseId').text(fileName);
            $("#pExcelFileDataSaveToDatabaseId").attr("title", "Click here to download - " + fileName)
            $('#imgExcelFileDataSaveToDatabase').attr('src', '/EmpManagment/img/Excel.png');
        }
    });

  
    $(".chk").change(function () {
        BikeCategoriesCheckBox();
    });
    function BikeCategoriesCheckBox() {
        var checked_checkboxes = [];
        $(".chk:checked").each(function () {
            checked_checkboxes.push(this.value);
        });
        if (checked_checkboxes.length == 0) {
            $("#divChk").show();
            return false;
        } else {
            $("#divChk").hide();
            return true;
        }
    }

    $(".cllRdGender").change(function () {
        GenderRadioList();
    });
    function GenderRadioList() {
        var rdGender = $("input[name='rdGender']:checked");
        if (rdGender.length == 0) {
            $("#divRd").show();
            return false;
        } else {
            $("#divRd").hide();
            return true;
        }
    }

    $("#ddlBikeCategories").change(function () {
        ddlBikeCategoriesSelectedValue();
    });
    function ddlBikeCategoriesSelectedValue() {
        var selectedValues = $('#ddlBikeCategories').val();
        if (selectedValues.length == 0) {
            $("#divBikeCategories").show();
            return false;
        } else {
            $("#divBikeCategories").hide();
            return true;
        }
    }

    $(".cchkTermsAndConditions").change(function () {
        TermsAndConditions();
    });
    function TermsAndConditions() {
        var x = $(".cchkTermsAndConditions").is(":checked");
        if (x == true) {
            $("#divTermsAndConditions").hide();
            return true;
        }
        else if (x == false) {
            $("#divTermsAndConditions").show();
            return false;
        }
    }

    //On Country change bind state
    $("#ddlcountry").change(function () {
        Country();
    });
    function Country() {
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
                url: "/EmpManagment/Comman/Account/GetStatelist",
                data: { countryId: countryId },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var states = data.states;
                    if (states.length > 0) {
                        $("#divState").show();
                        $("#divCity").hide();
                        for (var i = 0; i < states.length; i++) {
                            ddlstate.append($('<option value="' + states[i].value + '">' + states[i].text + '</option>'));
                        }
                        statesProgress.hide();
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert('Failed to retrieve states.');
                    statesProgress.hide();
                }
            });
        } else {
            $("#divState").hide();
            $("#divCity").hide();
        }
        statesProgress.hide();
    }
    //On State change bind city
    $("#ddlstate").change(function () {
        State();
    });
    function State() {
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
                    if (cities.length > 0) {
                        $("#divCity").show();
                        for (var i = 0; i < cities.length; i++) {
                            ddlCity.append($('<option value="' + cities[i].value + '">' + cities[i].text + '</option>'));
                        }
                        cityProgress.hide();
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert('Failed to retrieve states.');
                    cityProgress.hide();
                }
            });
        } else {
            $("#divCity").hide();
        }
        cityProgress.hide();
    }
});


$(document).on('click', '#btnCreate', function () {
    
    var rdGender = $("input[name='rdGender']:checked");
    var checked_checkboxes = [];
    $("input.chk:checked").each(function () {
        checked_checkboxes.push(this.value);
    });
    var selectedValues = $('#ddlBikeCategories').val();
    var termsAndConditions = $(".cchkTermsAndConditions").is(":checked");
    //var ddlGenderOption = $('#ddlGender :selected').text(); // Here we can get the value of selected item
    //var ddlCategories = $('#ddlCategoriesOptions :selected').text(); // Here we can get the value of selected item

    //if (ddlGenderOption == "Dropdown") {
    //    $("#divGenderDropdown").show();
    //    $("#divGenderRadio").hide();
    //    return true;
    //} else if (ddlGenderOption == "RadioButton") {
    //    $("#divGenderRadio").show();
    //    $("#divGenderDropdown").hide();
    //    return true;
    //} 
    //else if (ddlCategories == "ChecboxList") {
    //    $("#divBikeCategoriesCheckBox").show();
    //    $("#divBikeCategoriesSelectList").hide();
    //    return true;
    //} else if (ddlCategories == "DropdownSelectList") {
    //    $("#divBikeCategoriesSelectList").show();
    //    $("#divBikeCategoriesCheckBox").hide();
    //    return true;
    //}
    if (rdGender.length == 0) {
        $("#divRd").show();
        $("#divRd").focus();
        return false;
    } else if (checked_checkboxes.length == 0) {
        $("#divChk").show();
        return false;
    } else if (selectedValues.length == 0) {
        $("#divBikeCategories").show();
        return false;
    } else if (termsAndConditions == false) {
        $("#divTermsAndConditions").show();
        return false;
    } else if (selectedValues.length == 0) {
        $("#divBikeCategories").show();
        return false;
    }
    else {
        $("#divRd").hide();
        $("#divChk").hide();
        $("#divBikeCategories").hide();
        $("#divBikeCategories").hide();
        $("#divTermsAndConditions").hide();
        return true;
    }
});