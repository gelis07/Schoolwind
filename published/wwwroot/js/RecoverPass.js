function SendEmail(){
    $.ajax({
        type: "POST",
        url: "/Home/RecoverPassword",
        data: {Mail: $("#RecEmail-Input").val()},
        dataType: "text",
        complete: function (data) {
            if(data.responseText == "true"){
                $(location).attr('href', '/Home/ChangePass');
            }else{
                ErrorFun(".error", data.responseText, 5000);
            }
        }
    });
}
function ChangePassword(){
    $.ajax({
        type: "POST",
        url: "/Home/ChangePassword",
        data: {code: $("#Code-Input").val(), NewPassword: $("#NewPass-Input").val()},
        dataType: "text",
        complete: function (data) {
            if(data.responseText == "true"){
                $(location).attr('href', '/Home/SignIn')
            }else{
                ErrorFun(".error", "Wrong Code");
            }
        }
    });
}
