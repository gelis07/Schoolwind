function HomeButton()
{
    $.ajax({
        type: "POST",
        url: "/Home/LoggedIn",
        dataType: "text",
        complete: function(data){
            if(data.responseText == "true"){
                $(location).attr('href', "/Main");
            }else{
                $(location).attr('href', "/Home");
            }
        }
    })
}
function ErrorFun(classp, message, duration=1000, color="red", TextType="h3")
{
    $(classp).append("<"+TextType+" style='color:"+color+"'>"+message+"</"+TextType+">");
    $(classp + " " + TextType).fadeOut(duration);
}