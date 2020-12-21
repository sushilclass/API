$(document).ready(function () {

    $("#btnLogin").click(function (event) {
        event.preventDefault();
        Login();
       // return false;
    });

    $("#signup").click(function (event) {
        event.preventDefault();
        SignUp();
    });

    $("#getUserProfile").click(function (event) {
        event.preventDefault();
        $("#userdisplay").show();
        $("#businessdiaplay").hide();
        GetUserProfile();
    });

    $("#getUserBusiness").click(function (event) {
        event.preventDefault();
        $("#userdisplay").hide();
        $("#businessdiaplay").show();
        GetUserBusinessDetails();
    });    

});

function Login() {   
        if (LoginValidation() == false) {
            return false;
        }  
        $.ajax({
            async: true,
            type: "POST",
            url: "http://dbprod/Outreach-API/UserValidate",
           
            data: JSON.stringify({
                EmailId: $('#login-username').val(),
                Password: $('#login-password').val()
            }),
           
            dataType: "json",
            contentType: "application/json", 
            success: function (result) {
              //  alert(JSON.stringify(result.user.userId));
                sessionStorage.setItem("UserId", result.user.userId);               
               location.href = "/Home/Dashboard";               
            },
            error: function (result) {
                alert(JSON.stringify(result));
            }
        });

    return false;
}

function LoginValidation() {
    var count = 0;
    var UserEmail = $('#login-username').val();
    var UserPassword = $('#login-password').val();

    //Email Validation
    if (UserEmail.length < 1) {
        alert("Enter EmailId");
        count = count + 1;
        return false;
    } else {
        var regEx = /^([_\-\.0-9a-zA-Z]+)@([_\-\.0-9a-zA-Z]+)\.([a-zA-Z]){2,7}$/;
        var validEmail = regEx.test(UserEmail);
        if (!validEmail) {
            alert("Enter Valid Email");
            count = count + 1;
            return false;
        }
    }
    //Password Validation
    if (UserPassword.length < 1) {
        alert("Enter Password");
        count = count + 1;
        return false;
    }
    if (count > 0) {
        return false;
    }
    else {
        return true;
    }
}

function SignUp() {
    $.ajax({
        async: true,
        type: "POST",
        url: "http://dbprod/Outreach-API/UserSignUp",

        data: JSON.stringify({
            UserType: '2',
            FirstName: $('#firstname').val(),
            LastName: $('#lastname').val(),
            EmailId: $('#Email').val(),
            MobileNo: $('#mobile').val(),
            Password: $('#password').val(), 
            ConfirmPassword: $('#confirmpassword').val(),
            ReferralCode: $('#referralcode').val(),
            UserApprovalStatus: 'Approved',
            UserStatus: 'Active',
            Address: {
                Street: $('#street').val(),
                City: parseInt($('#city').val()), 
                State: parseInt($('#state').val()),
                ZipCode: $('#zipcode').val(),
                Country: parseInt($('#country').val()), 
            },
            BusinessResearch: {
                BusinessName: $('#businessname').val(),
                BusinessAddress: $('#businessstreet').val(),
                BusinessCity: parseInt($('#businesscity').val()),
                BusinessState: parseInt($('#businessstate').val()),
                BusinessCountry: parseInt($('#businesscountry').val()),
                BusinessZipCode: $('#businesszip').val(),
                BusinessRating: $('#businessrating').val(),
                BusinessOpenNow: 'Yes',
                BusinessPhotoReference: null,
                BusinessKeywords: $('#businesskeywords').val(),
                BusinessTypes: $('#businesstype').val(),
                BusinessLink: $('#businesslink').val()
            }
        }),

        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            console.log(JSON.stringify(result));
        },
        error: function (result) {
            alert(JSON.stringify(result));
        }
    });
}

function GetUserProfile() {
    $.ajax({
        async: true,
        type: "POST",
        url: "http://dbprod/Outreach-API/GetUserDetails",
        data: JSON.stringify({
            UserId: parseInt(sessionStorage.getItem("UserId"))
        }),
        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            console.log(result.user[0].firstName);
            $("#firstName").val(result.user[0].firstName);
            $("#lastName").val(result.user[0].lastName);
            $("#mobile").val(result.user[0].mobileNo);
            $("#emailid").val(result.user[0].emailId);
            $("#referralcode").val(result.user[0].referralCode);
            $("#street").val(result.user[0].addressDetails.street);
            $("#city").val(result.user[0].addressDetails.city);
            $("#state").val(result.user[0].addressDetails.state);
            $("#country").val(result.user[0].addressDetails.country);
            $("#zipcode").val(result.user[0].addressDetails.zipCode);           
        },
        error: function (result) {
            alert(JSON.stringify(result));
        }
    });
}

function GetUserBusinessDetails() {
  //  alert("Business Profile");
    $.ajax({
        async: true,
        type: "POST",
        url: "http://dbprod/Outreach-API/GetBusinessProfile",
        data: JSON.stringify({
            UserId: parseInt(sessionStorage.getItem("UserId"))
        }),
        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            $("#tb1").empty();
            //console.log(JSON.stringify(result));  
            $("#tb1").append($("<tr>"));
            for (var i = 0; i < result.data.length; i++) {
                $("#tb1").append($("<tr>"));
                appendElement = $("#tb1 tr").last();
                appendElement.append($("<td>").html(result.data[i].restaurantId));
                appendElement.append($("<td>").html(result.data[i].userId));
                appendElement.append($("<td>").html(result.data[i].name));
                appendElement.append($("<td>").html(result.data[i].address));
                appendElement.append($("<td>").html(result.data[i].city));
                appendElement.append($("<td>").html(result.data[i].state));
                appendElement.append($("<td>").html(result.data[i].country));
                appendElement.append($("<td>").html(result.data[i].zipcode));
                appendElement.append($("<td>").html(result.data[i].keyWords));
                appendElement.append($("<td>").html(result.data[i].link));
                appendElement.append($("<td>").html(result.data[i].businessType));
            }
            //$.each(result, function (index, value) {
            //    console.log(result.data.length);
            //    $("tbody").append($("<tr>"));
            //    appendElement = $("tbody tr").last();
            //   // appendElement.append($("<td>").html(result.data[].restaurantId));
               
            //    //appendElement.append($("<td>").html(value["name"]));
            //    //appendElement.append($("<td>").html(value["startLocation"]));
            //    //appendElement.append($("<td>").html(value["endLocation"]));
            //    //appendElement.append($("<td>").html("<a href=\"UpdateReservation.html?id=" + value["id"] + "\"><img src=\"icon/edit.png\" /></a>");
            //    //appendElement.append($("<td>").html("<img class=\"delete\" src=\"icon/close.png\" />"));
            //});
        },
        error: function (result) {
            alert(JSON.stringify(result));
        }
    });
}