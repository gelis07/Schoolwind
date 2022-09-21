function DeleteAccount()
{
    $(".Confirm").append("<button id='ConfirmB' class='NormalButton'>Are you sure?</button>");   
    $("#ConfirmB").click(function(){
        $.ajax({
            type: "POST",
            url: "/Settings/DeleteAccount",
            dataType: "text",
            complete: function (data) {
                $(location).attr('href', "/Home/Index");
            }
        });
    });
}