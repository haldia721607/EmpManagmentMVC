$(function () {
    var soluationName = $('#soluationName').val();
    var noImagePath = $('#noImageId').val();
    var noFilePath = $('#noFileId').val();
    //For check server side error 
    var errorId = $('#errorId').val();
    var messageId = $('#messageId').val();

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
    //For bind country and state 
    Country();
    State();

    //For multiple image upload to folder - set default single image
    let $img = jQuery.parseHTML("<img src='/EmpManagment/img/no-image.jpg' style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'>");
    $(".filearray").append($img);

    //Function for- Muliple images uplod to floder with multiple image preview
    $("#multipleImgePreview").on('change', function () {
        $(".filearray").empty();//you can remove this code if you want previous user input
        if (this.files.length > 0 && this.files.length < 5) {
            $("#spanMaxImageFolder").hide();
            for (let i = 0; i < this.files.length; ++i) {
                let dataName = this.files[0].name;
                let filereader = new FileReader();
                let $img = jQuery.parseHTML("<img src='' style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'><br /> <p style='width: 100px;' title='Click here to download - " + dataName + "'>" + dataName + "</p>");
                filereader.onload = function () {
                    $img[0].src = this.result;
                };
                filereader.readAsDataURL(this.files[i]);
                $(".filearray").append($img);
            }
        } else {
            $("#spanMaxImageFolder").show();
        }
    });

    //Function for - Only single image uplod to floder with image preview
    $('.custom-file-inputt').on("change", function () {
        //For single image name show in file upload
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);

        var files = $(this)[0].files;
        if (files.length > 0 && files.length < 2) {
            let dataName = this.files[0].name;
            $('#pUploadPreview').text(dataName);
            var oFReader = new FileReader();
            oFReader.readAsDataURL(document.getElementById("uploadImage").files[0]);

            oFReader.onload = function (oFREvent) {
                document.getElementById("uploadPreview").src = oFREvent.target.result;
            };
        }
    });

    //For multiple file upload to folder - set default file image
    let $FileFloder = jQuery.parseHTML("<img src='/EmpManagment/img/NoFile.png' style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'>");
    $(".divFile").append($FileFloder);

    //Function for - multiple file(docx/xlxs/pdf) uplod to floder
    $("#multipleFilePreview").on('change', function () {
        debugger;
        $(".divFile").empty();//you can remove this code if you want previous user input
        if (this.files.length > 0 && this.files.length < 3) {
            $("#spanMaxFileFolder").hide();
            for (var i = 0; i < $(this).get(0).files.length; ++i) {
                var fileName = $(this).get(0).files[i].name;
                var extension = fileName.substr((fileName.lastIndexOf('.') + 1));
                let $p = jQuery.parseHTML("<p>" + fileName + "</p>");
                let $img = "";
                switch (extension) {
                    case 'docx':
                        $img = jQuery.parseHTML("<img src=" + '/EmpManagment/img/Word.png' + " style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'>");
                        break;
                    case 'xlsx':
                        $img = jQuery.parseHTML("<img src=" + '/EmpManagment/img/Excel.png' + " style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'>");
                        break;
                    case 'pdf':
                        $img = jQuery.parseHTML("<img src=" + '/EmpManagment/img/Pdf.png' + " style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'>");
                        break;
                    default:
                    //alert('who knows');
                }
                $(".divFile").append($img);
                $(".divFile").append($p);
            }
        } else {
            $("#spanMaxFileFolder").show();
        }
    });

    //Function for - Only single file(docx/xlxs/pdf) uplod to floder
    $('.doc-excel-pdf').on("change", function () {
        debugger;
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);
        var extension = fileName.substr((fileName.lastIndexOf('.') + 1));
        if ($(this)[0].files.length > 0 && $(this)[0].files.length < 2) {
            $("#puploadFilePreview").empty();
            $('#puploadFilePreview').text(fileName);
            switch (extension) {
                case 'docx':
                    $('#uploadFilePreview').attr('src', '/EmpManagment/img/Word.png');
                    break;                        
                case 'xlsx':
                    $('#uploadFilePreview').attr('src', '/EmpManagment/img/Excel.png');
                    break;
                case 'pdf':
                    $('#uploadFilePreview').attr('src','/EmpManagment/img/Pdf.png');
                    break;
                default:
                    //alert('who knows');
            }
        }
    });

    //For multiple file upload to database - set default file image
    let $FileDatabase = jQuery.parseHTML("<img src='/EmpManagment/img/NoFile.png' style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'>");
    $(".divFileDatabase").append($FileDatabase);

    //Function for - multiple file(docx/xlxs/pdf) uplod to database
    $("#multipleFileDatabase").on('change', function () {
        $(".divFileDatabase").empty();//you can remove this code if you want previous user input
        if (this.files.length > 0 && this.files.length < 3) {
            $("#spanMaxFileDatabase").hide();
            for (var i = 0; i < $(this).get(0).files.length; ++i) {
                var fileName = $(this).get(0).files[i].name;
                var extension = fileName.substr((fileName.lastIndexOf('.') + 1));
                let $p = jQuery.parseHTML("<p>" + fileName + "</p>");
                let $img = "";
                switch (extension) {
                    case 'docx':
                        $img = jQuery.parseHTML("<img src=" + '/EmpManagment/img/Word.png' + " style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'>");
                        break;
                    case 'xlsx':
                        $img = jQuery.parseHTML("<img src=" + '/EmpManagment/img/Excel.png' + " style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'>");
                        break;
                    case 'pdf':
                        $img = jQuery.parseHTML("<img src=" + '/EmpManagment/img/Pdf.png' + " style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'>");
                        break;
                    default:
                    //alert('who knows');
                }
                $(".divFileDatabase").append($img);
                $(".divFileDatabase").append($p);
            }
        } else {
            $("#spanMaxFileDatabase").show();
        }
    });

    //Function for - Only single file(docx/xlxs/pdf) uplod to database
    $('.doc-excel-pdf-databse').on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);
        var extension = fileName.substr((fileName.lastIndexOf('.') + 1));
        if ($(this)[0].files.length > 0 && $(this)[0].files.length < 2) {
            $("#puploadFileDatabasePreview").empty();
            $('#puploadFileDatabasePreview').text(fileName);
            switch (extension) {
                case 'docx':
                    $('#uploadFileToDatabasePreview').attr('src', '/EmpManagment/img/Word.png');
                    break;
                case 'xlsx':
                    $('#uploadFileToDatabasePreview').attr('src', '/EmpManagment/img/Excel.png');
                    break;
                case 'pdf':
                    $('#uploadFileToDatabasePreview').attr('src', '/EmpManagment/img/Pdf.png');
                    break;
                default:
                //alert('who knows');
            }
        }
    });

    //For multiple image upload to database - set default single image
    let $imgdatabase = jQuery.parseHTML("<img src='/EmpManagment/img/no-image.jpg' style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'>");
    $(".databseFilearray").append($imgdatabase);

    //Function for- Muliple images uplod to database with multiple image preview
    $("#databaseMultipleImgePreview").on('change', function () {
        $(".databseFilearray").empty();//you can remove this code if you want previous user input
        if (this.files.length > 0 && this.files.length < 5) {
            $("#spanMaxImageDatabase").hide();
            for (let i = 0; i < this.files.length; ++i) {
                let dataName = this.files[0].name;
                let filereader = new FileReader();
                let $img = jQuery.parseHTML("<img src='' style='width: 100px; height: 100px;margin-right: 1%;' asp-append-version='true'><br /> <p style='width: 100px;' title='Click here to download - " + dataName + "'>" + dataName + "</p>");
                filereader.onload = function () {
                    $img[0].src = this.result;
                };
                filereader.readAsDataURL(this.files[i]);
                $(".databseFilearray").append($img);
            }
        } else {
            $("#spanMaxImageDatabase").show();
        }
    });

    //Function for - Only single image uplod to database with image preview
    $('.imagesavetodb').on("change", function () {
        //For single image name show in file upload
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);

        var files = $(this)[0].files;
        if (files.length > 0 && files.length < 2) {
            let dataName = this.files[0].name;
            $('#pDatabaseUplodImage').text(dataName);
            var oFReader = new FileReader();
            oFReader.readAsDataURL(document.getElementById("databaseUploadImage").files[0]);

            oFReader.onload = function (oFREvent) {
                document.getElementById("databaseUplodImage").src = oFREvent.target.result;
            };
        }
    });

    //Function for -Bulk insert to database using(xlx/xlxs)
    $('.Bulk-Insert-File-To-Database').on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);
        var extension = fileName.substr((fileName.lastIndexOf('.') + 1));
        if ($(this)[0].files.length > 0 && $(this)[0].files.length < 2) {
            $("#pBulkInsertFileName").empty();
            $('#pBulkInsertFileName').text(fileName);
            $('#imgBulkInsert').attr('src', '/EmpManagment/img/Excel.png');
        }
    });

    //Funcion for - Only number of image count show
    //$('.custom-file-inputtt').on("change", function () {
    //    //For multipale image count show in file upload
    //    var fileLabel = $(this).next('.custom-file-labell');
    //    var files = $(this)[0].files;
    //    if (files.length > 1) {
    //        fileLabel.html(files.length + ' files selected');
    //    }
    //    else if (files.length == 1) {
    //        fileLabel.html(files[0].name);
    //    }
    //});
   

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
        debugger;
        var selectedValues = $('#ddlBikeCategories').val();
        //var selectedValue = [];
        //$(".chk:checked").each(function () {
        //    checked_checkboxes.push(this.value);
        //});
        if (selectedValues.length == 0 && selectedValues == '[""]') {
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

    //$("#ddlGender").change(function () {
    //    ddlGenderOption();
    //});
    //function ddlGenderOption() {
    //    var ddlGenderOption = $('#ddlGender :selected').text(); // Here we can get the value of selected item
    //    if (ddlGenderOption == "Dropdown") {
    //        $("#divGenderDropdown").show();
    //        $("#divGenderRadio").hide();
    //        return true;
    //    } else if (ddlGenderOption == "RadioButton") {
    //        $("#divGenderRadio").show();
    //        $("#divGenderDropdown").hide();
    //        return true;
    //    } else {
    //        $("#divGenderDropdown").hide();
    //        $("#divGenderRadio").hide();
    //        return false;
    //    }
    //}

    //$("#ddlCategoriesOptions").change(function () {
    //    ddlCategoriesOptions();
    //});
    //function ddlCategoriesOptions() {
    //    var ddlCategories = $('#ddlCategoriesOptions :selected').text(); // Here we can get the value of selected item
    //    if (ddlCategories == "ChecboxList") {
    //        $("#divBikeCategoriesCheckBox").show();
    //        $("#divBikeCategoriesSelectList").hide();
    //        return true
    //    } else if (ddlCategories == "DropdownSelectList") {
    //        $("#divBikeCategoriesSelectList").show();
    //        $("#divBikeCategoriesCheckBox").hide();
    //        return true
    //    } else {
    //        $("#divBikeCategoriesSelectList").hide();
    //        $("#divBikeCategoriesCheckBox").hide();
    //        return false;
    //    }
    //}

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
    debugger;
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