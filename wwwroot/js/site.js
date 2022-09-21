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
currentId = 0;
function MoveCarousel(direction){
    var AllElements = $(".image");
    if(currentId+direction < 0){
      console.log("Hey");
      $(AllElements[currentId]).fadeOut();
      currentId = AllElements.length-1;
      $(AllElements[currentId]).fadeIn();
      return;
    }else if(currentId+direction>=AllElements.length){
      console.log("Hi");
      $(AllElements[currentId]).fadeOut();
      currentId = 0;
      $(AllElements[currentId]).fadeIn();
      return;
    }
    $(AllElements[currentId]).fadeOut();
    currentId += direction;
    $(AllElements[currentId]).fadeIn();
    console.log(currentId);
}