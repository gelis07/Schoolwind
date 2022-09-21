const { Tab } = require("../lib/bootstrap/dist/js/bootstrap.bundle");

function SignUpFun() {
    if ($('#Email-Input').val() == "" || $('#Name-Input').val() == "" || $('#Password-Input').val() == "") {
        console.log("Error");
        $(".Main").append("<h3 style='color:red'>Error bro</h3>");
        $(".Main h3").fadeOut(1000);
    }
    else if ($('#Email-Input').val() != "" || $('#Name-Input').val() != "" || $('#Password-Input').val() != "") {
        $.ajax({
            type: "POST",
            url: "/Home/AccountData",
            data: { Email: $('#Email-Input').val(), Name: $('#Name-Input').val(), Password: $('#Password-Input').val() },
            dataType: "text",
            complete: function (data) {
                $(location).attr('href', "/Home/CheckGuid");
            }
        });
    }
}
function SignInFun() {
    $.ajax({
        type: "POST",
        url: "/Home/LogIn",
        data: { Email: $('#Email-Input').val(), Password: $('#Password-Input').val() },
        dataType: "text",
        complete: function (data) {
            console.log(data);
            if (data.responseText == "True") {
                $(".SignIn").append("<h3 style='color:green'>Logged in :D</h3>");
                $(".SignIn h3").fadeOut(1000);
                $(location).attr('href', "/Main");
            }
            else if (data.responseText == "False") {
                $(".SignIn").append("<h3 style='color:red'>Are you trying to hack someone 0.0</h3>");
                $(".SignIn h3").fadeOut(3000);
            }
        }
    });
}
function Guid() {
    $.ajax({
        type: "POST",
        url: "/Home/CreateAccount",
        data: {GuidInput: $("#Guid-Input").val()},
        dataType: "text",
        complete: function (data) {
            if(data.responseText == "true"){
                $(location).attr('href', "/Home/Index");
            }else if(data.responseText == "false"){
                ErrorFun(".error", "Not valid code");
            }else{
                ErrorFun(".error", data.responseText, 5000);
            }
        }
    });
}



