var soluationName = $('#soluationName').val();
var datatable = $("#tblComplaintList").DataTable();
var bikeCategoriesDataTabe = $("#tblCategores").DataTable();
$(document).ready(function () {
    ComplaintList();
    chkFun();
    deleteMultiple();
});
//Checkbox cheked and unchecked function
function chkFun() {
    $('#tblComplaintList').on('click', '#chkAll', function () {
        $('.chkClass').not(this).prop('checked', this.checked);
    });
    $('#tblComplaintList').on('click', '#chk', function () {
        var len = $("#tblComplaintList input[name='chk']").length;
        var chkLen = $("#tblComplaintList input[name='chk']:checked").length;
        if (len == chkLen) {
            $("#tblComplaintList #chkAll").not(this).prop('checked', "checked");
        } else {
            $("#tblComplaintList #chkAll").prop("checked", false)
        }
    });
    $('#tblCategores').on('click', '#chkAllCategories', function () {
        $('.chkCategoriesClass').not(this).prop('checked', this.checked);
    });
    $('#tblCategores').on('click', '#chkCategories', function () {

        var lenCategories = $("#tblCategores .chkCategoriesClass").length;
        var checkedLen = $("#tblCategores input[name='chkCategories']:checked").length;
        if (lenCategories == checkedLen) {
            $("#tblCategores #chkAllCategories").not(this).prop('checked', "checked");
        } else {
            $("#tblCategores #chkAllCategories").prop('checked', false);
        }
    })
}
//Bind all category
function ComplaintList() {
    $.ajax({
        type: 'GET',
        url: '/' + soluationName + '/User/Complaient/GetAllComplaient',
        dataType: "json",
        success: function (DBdata) {
            datatable.destroy();
            bikeCategoriesDataTabe.destroy();
            datatable = $("#tblComplaintList").DataTable({
                destroy: true,
                data: DBdata.complainantAndDetailsViewModels,
                searching: true,
                ordering: true,
                pagingType: "full_numbers",
                "columns": [
                    {
                        title: "<input type='checkbox' name='chkAll' class='chkAllClass' id='chkAll' value=''/>", "data": "complaientId", "render": function (data) {
                            return " <input type='checkbox' name='chk' class='chkClass' id='chk' value=" + data + " />";
                        },
                        "orderable": false,
                        "searchable": false,
                        "width": "150px"
                    },
                    { title: "Name", "data": "complainantName" },
                    { title: "Email", "data": "complainantEmail" },
                    { title: "Categories Date", "data": "compaientDate" },
                    { title: "Complaient Description", "data": "complaientCategoriesDescription" },
                    { title: "Country Name", "data": "countryName" },
                    { title: "State Name", "data": "stateName" },
                    { title: "City Name", "data": "cityName" },
                    {
                        title: "Details Description", "data": "complaientDescription", "render": function (data) {
                            var str = data.substr(0, 50);
                            return "<p title='" + data + "'>" + str + " ...</p>";
                        }
                    },
                    {
                        title: "Details Date", "data": "showComplaientDate", "render": function (data) {
                            return data;
                        }
                    },
                    {
                        "data": "complaientEncryptedId", "render": function (data) {
                            return "<a class='btn btn-default btn-sm' href='/EmpManagment/User/Complaient/EditComplaient?id=" + data + "')><i class='fa fa-pencil'></i> Edit</a> <a class='btn btn-danger btn-sm' style='margin-left:5px' onclick=Delete(" + data + ")><i class='fa fa-trash'></i> Delete</a>";
                        },
                        "orderable": false,
                        "searchable": false,
                        "width": "150px"
                    }

                ],
                "language": {

                    "emptyTable": "No data found, Please click on <b>Add New</b> Button"
                }
            });
            //Bind category usung ajax get  request
            bikeCategoriesDataTabe = $("#tblCategores").DataTable({
                destroy: true,
                data: DBdata.bikeCategory,
                searching: true,
                ordering: true,
                pagingType: "full_numbers",
                "columns": [
                    {
                        title: "<input type='checkbox' name='chkAllCategories' class='chkAllCategoriesClass' id='chkAllCategories' value=''/>", "data": "bikeCategoryId", "render": function (data) {
                            return " <input type='checkbox' name='chkCategories' class='chkCategoriesClass' id='chkCategories' value=" + data + " />";
                        },
                        "orderable": false,
                        "searchable": false,
                        "width": "150px"
                    },
                    { title: "Name", "data": "name" },
                    {
                        title: "Description", "data": "description", "render": function (data) {

                            var str = data.substr(1, 100);
                            return "<p title='" + data + "'>" + str + " ...</p>";
                        }
                    },
                    {
                        title: "Created Date", "data": "createdDate", "render": function (data) {
                            return data;
                        }
                    },
                    {
                        title: "Status", "data": "status", render: function (data) {
                            if (data == true) {
                                return 'Active';
                            } else {
                                return 'Not-Active';
                            }
                        }
                    },
                    {
                        "data": "bikeCategoryId", "render": function (data) {
                            return "<a class='gridPopup btn btn-default btn-sm' href='#' id='" + data + "'><i class='fa fa-pencil'></i> Edit</a> <a class='btn btn-danger btn-sm' style='margin-left:5px' onClick=DeleteCategories(" + data + ")><i class='fa fa-trash'></i> Delete</a>";
                        },
                        "orderable": false,
                        "searchable": false,
                        "width": "150px"
                    }
                ],
                "language": {

                    "emptyTable": "No data found, Please click on <b>Add New</b> Button"
                }
            });
        },
        error: function (xmlHttpRequest, errorText, thrownError) {
            alert(xmlHttpRequest, errorText, thrownError);
        }
    });
}
//Delete Complaient function
function Delete(complaientId) {

    if (complaientId != 0) {
        if (confirm('Are You Sure to Delete this complaint Record ?')) {
            $.ajax({
                type: "POST",
                url: '/' + soluationName + '/User/Complaient/DeleteComplaient?id=' + complaientId,// '@Url.Action("DeleteComplaient", "Complaient",new { area="User"})/' + complaientId,
                success: function (data) {
                    if (data.success) {
                        ComplaintList();
                        toastr.success(message = data.message, title = 'Deleted')
                    }
                },
                error: function (xmlHttpRequest, errorText, thrownError) {
                    alert(xmlHttpRequest, errorText, thrownError);
                }
            });
        }
    }
}
//Delete categories fun
function DeleteCategories(bikeCategoryId) {
    if (bikeCategoryId != 0) {
        if (confirm('Are You Sure to Delete this Categorie Record ?')) {
            $.ajax({
                type: "Get",
                url: '/' + soluationName + '/User/Complaient/DeleteCategorie?id=' + bikeCategoryId,
                success: function (data) {
                    ComplaintList();
                    toastr.success(message = data.message, title = 'Deleted');
                },
                error: function (xmlHttpRequest, errorText, thrownError) {
                    alert(xmlHttpRequest, errorText, thrownError);
                }
            });
        }
    }
}
//Delete Multiple Complaient function
function deleteMultiple() {
    //Delete multiple complaints ajax post request
    $("#btnDeleteMultipaleComplaient").click(function () {
        var checkedCheckboxValue = new Array();
        var count = $("#tblComplaintList input[name='chk']:checked").length;
        if (count == 0) {
            alert("No rows selected to delete");
            return false;
        }
        else {
            $("#tblComplaintList input[name='chk']:checked").each(function () {
                checkedCheckboxValue.push($(this).attr('value'));
            });
        }

        if (confirm('Are You Sure to Delete ' + count + ' selected complaients ?')) {
            $.ajax({
                type: 'POST',
                url: '/' + soluationName + '/User/Complaient/MultipleComplaientDelete',//'@Url.Action("MultipleComplaientDelete", "Complaient",new { area="User"})',
                data: { ids: checkedCheckboxValue },
                success: function (data) {
                    if (data.success) {
                        ComplaintList();
                        toastr.success(message = data.message, title = 'Deleted')
                    }
                    else {
                        toastr.warning(message = data.message, title = 'Not Deleted')
                    }
                },
                error: function (xmlHttpRequest, errorText, thrownError) {
                    alert(xmlHttpRequest, errorText, thrownError);
                }
            });
        }
    });

    //Delete multiple Bike Categories ajax post request
    $('#btnDeleteMultipaleCategories').click(function () {
        var checkedChkCategories = new Array();
        var count = $("#tblCategores input[name='chkCategories']:checked").length;
        if (count == 0) {
            alert("No rows selected to delete");
            return false;
        } else {
            $("#tblCategores input[name='chkCategories']:checked").each(function () {
                checkedChkCategories.push($(this).attr('value'));
            });
        }
        if (confirm('Are you sure to delete ' + count + ' selected bike categories ?')) {
            $.ajax({
                type: "POST",
                url: '/' + soluationName + '/User/Complaient/MultipleCategoriesDelete',//'@Url.Action("MultipleCategoriesDelete","Complaient",new {area="User"})',
                data: { ids: checkedChkCategories },
                success: function (data) {
                    if (data.success) {
                        ComplaintList();
                        toastr.success(message = data.message, title = 'Deleted');
                    } else {
                        toastr.warning(message = data.message, title = 'Deleted');
                    }
                },
                error: function (xmlHttpRequest, errorText, thrownError) {
                    alert(xmlHttpRequest, errorText, thrownError);
                }
            });
        }
    });
}

//For Edit Bike Categories
$('#tblCategores').on('click', 'a.gridPopup', function (e) {
    e.preventDefault();
    var url = "/EmpManagment/User/Complaient/EditBikeCategories";//$(this).attr('href');
    var id = $(this).attr('id');
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        success: function (response) {

            $('#myModalBodyDiv').html(response);
            $('#myModal').modal("show");
            $("form#myForm").removeData("validator");
            $("form#myForm").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse($('form#myForm'));
        }
    });
})

//For Add Bike Categories
$('.tablecontainerr').on('click', 'a.popup', function (e) {
    e.preventDefault();
    var url = $(this).attr('href');
    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            $('#myModalBodyDiv').html(response);
            $('#myModal').modal("show");
        }
    });
    return false;
})

function validate() {
    var name = $('#txtName').val();
    var des = $('#txtDescription').val();
    var status = $('#ddlStatus').val();
    $('#spanName').hide();
    $('#spanDescription').hide();
    $('#spanStatus').hide();


    if (name == "") {
        $('#spanName').show();
        $("#txtName").focus();
        return false;
    } else if (des == "") {
        $('#spanDescription').show();
        $("#txtDescription").focus();
        return false;
    } else if (status == "") {
        $('#spanStatus').show();
        $("#ddlStatus").focus();
        return false;
    } else {
        return true;
    }
}

$('#myModalBodyDiv').on('submit', '#myForm', function (e) {

    e.preventDefault();
    var url = $('#myForm')[0].action;
    var valid = validate();
    if (valid == true) {
        $.ajax({
            type: "POST",
            url: url,
            data: $('#myForm').serialize(),
            success: function (data) {
                if (data.success) {
                    toastr.success(message = data.message, title = 'Create');
                    $('#myModal').modal("hide");
                    ComplaintList();
                } else {
                    $(data.errorsList).each(function () {
                        var key = $(this)[0].key;
                        var errorMessage = $(this)[0].errorMessage;
                    })
                    toastr.warning(message = data.message, title = 'Error');
                }
            }
        })
    }
    return false;
})

function UpdateUnobtrusiveValidations(idForm) {
    $(idForm).removeData("validator").removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse($(idForm));
};

