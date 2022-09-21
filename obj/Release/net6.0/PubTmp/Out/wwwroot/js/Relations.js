var pressed = false;
var ids = [];
var startedStudents=0;
var lastOpened;
var lastClicked;
function ShowRelations(id, name, clicked, subject=false){
    $.ajax({
        type: 'POST',
        url: '/Main/ShowRelations',
        data: {id: id},
        dataType: 'json',
        complete: function(data){
            if(!pressed || lastOpened != id){
                if($(".Relations") != null && lastClicked != null){
                    startedStudents = 0;
                    $(lastClicked).removeClass("RelArrowClicked");
                    $(".Relations").remove();
                }
                $(clicked).addClass("RelArrowClicked");
                lastClicked = clicked;
                lastOpened = id;
                ids = [];
                var output = data.responseJSON[0];
                console.log(data);
                var StudentsDiv = $("<div class='Students Rel' id='relation'></div>");
                $(StudentsDiv).append("<h2>"+name+"</h2>")
                $(".main").append("<div class='Relations'></div>");
                $(".Relations").append(StudentsDiv)
                for (let index = 0; index < output.length; index++) {
                    var StudentsRowDiv = $("<div class='StudentsRow RelRow''></div>");
                    ids.push(data.responseJSON[1][index]);
                    for (let j = 0; j < output[index].length; j++) {
                        StudentsRowDiv.append("<input value='"+output[index][j]+"'>");
                    }
                    startedStudents += 1;
                    console.log(startedStudents);
                    StudentsRowDiv.append(`<button onclick="DeleteRow(this)" class='icon'><svg xmlns="http://www.w3.org/2000/svg" x="0px" y="0px"width="30" height="30"viewBox="0 0 30 30"style=" fill:#CC998D;"><path d="M 13 3 A 1.0001 1.0001 0 0 0 11.986328 4 L 6 4 A 1.0001 1.0001 0 1 0 6 6 L 24 6 A 1.0001 1.0001 0 1 0 24 4 L 18.013672 4 A 1.0001 1.0001 0 0 0 17 3 L 13 3 z M 6 8 L 6 24 C 6 25.105 6.895 26 8 26 L 22 26 C 23.105 26 24 25.105 24 24 L 24 8 L 6 8 z"></path></svg></button>`);
                    StudentsDiv.append($(StudentsRowDiv));
                }
                $(".Relations").append(`<button onclick='AddRelation(`+subject+`)' class="icon"><svg xmlns="http://www.w3.org/2000/svg" x="0px" y="0px"width="30" height="30"viewBox="0 0 30 30"style=" fill:#CC998D;">    <path d="M15,3C8.373,3,3,8.373,3,15c0,6.627,5.373,12,12,12s12-5.373,12-12C27,8.373,21.627,3,15,3z M21,16h-5v5 c0,0.553-0.448,1-1,1s-1-0.447-1-1v-5H9c-0.552,0-1-0.447-1-1s0.448-1,1-1h5V9c0-0.553,0.448-1,1-1s1,0.447,1,1v5h5 c0.552,0,1,0.447,1,1S21.552,16,21,16z"></path></svg></button>`)
                $(".Relations").append(`<button id='RelUpdate' onclick="Update('@Manage.table', @arrayString, @ColumnString)" class="icon"><svg xmlns="http://www.w3.org/2000/svg" x="0px" y="0px"width="30" height="30"viewBox="0 0 30 30"style=" fill:#CC998D;"><path d="M 15 3 C 8.9134751 3 3.87999 7.5533546 3.1132812 13.439453 A 1.0001 1.0001 0 1 0 5.0957031 13.697266 C 5.7349943 8.7893639 9.9085249 5 15 5 C 17.766872 5 20.250574 6.1285473 22.058594 7.9414062 L 20 10 L 26 11 L 25 5 L 23.470703 6.5292969 C 21.300701 4.3575454 18.309289 3 15 3 z M 25.912109 15.417969 A 1.0001 1.0001 0 0 0 24.904297 16.302734 C 24.265006 21.210636 20.091475 25 15 25 C 11.977904 25 9.2987537 23.65024 7.4648438 21.535156 L 9 20 L 3 19 L 4 25 L 6.0488281 22.951172 C 8.2452659 25.422716 11.436061 27 15 27 C 21.086525 27 26.12001 22.446646 26.886719 16.560547 A 1.0001 1.0001 0 0 0 25.912109 15.417969 z"></path></svg></button>`)
                $("#RelUpdate").click(function() {
                    UpdateRelation(id)
                });
                pressed = true;
            }else{
                startedStudents = 0;
                $(".Relations").remove();
                $(clicked).removeClass("RelArrowClicked");
                pressed = false;
            }
        }
    });
}
var ShowedDropDown = false;
function AddRelation(subject=false){
    if(!ShowedDropDown){
        $.ajax({
            type: 'POST',
            url: '/Main/AddRelations',
            data: {subject: subject},
            dataType: 'json',
            complete: function(output){
                var data = output.responseJSON;
                console.log(data);
                var DropDown = $("<select id='StudentsToAdd'></select>");
                for (let index = 0; index < data.length; index++) {
                    console.log(data[index]);
                    $(DropDown).append("<option value="+data[index][0]+">"+data[index][1]+"</option>");
                }
                $(".Rel").append($(DropDown));
                ShowedDropDown = true;
            }
        });
    }else{
        const array = $('#StudentsToAdd').find(":selected").text().split(",");
        var div = $("<div class='StudentsRow RelRow' id='"+$('#StudentsToAdd').find(":selected").val()+"'></div>");
        ids.push($('#StudentsToAdd').find(":selected").val());
        for (let index = 0; index < array.length; index++) {
            $(div).append("<input value="+array[index]+">");
        }
        $(div).append(`<button onclick="DeleteRow(this)" class='icon'><svg xmlns="http://www.w3.org/2000/svg" x="0px" y="0px"width="30" height="30"viewBox="0 0 30 30"style=" fill:#CC998D;"><path d="M 13 3 A 1.0001 1.0001 0 0 0 11.986328 4 L 6 4 A 1.0001 1.0001 0 1 0 6 6 L 24 6 A 1.0001 1.0001 0 1 0 24 4 L 18.013672 4 A 1.0001 1.0001 0 0 0 17 3 L 13 3 z M 6 8 L 6 24 C 6 25.105 6.895 26 8 26 L 22 26 C 23.105 26 24 25.105 24 24 L 24 8 L 6 8 z"></path></svg></button>`);
        $(".Rel").append($(div));
        $("#StudentsToAdd").remove();
    }
}
function UpdateRelation(id){
    var hidden = [];
    $(".RelRow").each(function (index) {
        hidden.push($(this).is(":hidden"));
    });
    $.ajax({
        type: 'POST',
        url: '/Main/UpdateRelation',
        data: {ids: ids, class_id: id, hidden: hidden, Started: startedStudents},
        dataType: 'text',
        complete: function(data){
            if (data.responseText != 'Success') {
                ErrorFun(".error", data.responseText, 10000);
                return false;
            }
            if (data.responseText == 'Success') {
                ErrorFun(".error", data.responseText, 10000, "green");
                location.reload();
            }
        }
    });
}