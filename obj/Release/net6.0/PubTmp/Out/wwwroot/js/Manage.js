
function Update(Table, ids, columns){
    var Data = [];
    var hidden = [];
    var searchDiv = $(".StudentsRow > input");
    for (let index = 0; index < $(".StudentsRow").length; index++) {
        var newData =[];
        console.log($(".StudentsRow#"+index+" > input"))
        $(".StudentsRow#"+index+" > input").each(function (index) {
            newData.push($(this).val());
        });
        hidden.push($(".StudentsRow#"+index).is(":hidden"));
        console.log(newData)
        Data.push(newData);
    }
    console.log(Data);
    $.ajax({
        type: "POST",
        url: "/Main/Update",
        data: {StudentData: Data, ids:ids, hidden: hidden, table: Table, Columns: columns},
        dataType: "text",
        complete: function (data) {
            console.log(data);
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
function SearchButton(columns, table, value){
    $('#sort option[value='+value+']').attr('selected','selected');
    Search(columns, table);
}
function Search(columns, table) {
    var searchVal = $(".SearchRow > input").val();
    $.ajax({
        type: "POST",
        url: "/Main/Search",
        data: { SearchRow: searchVal, Columns: columns, table: table, sort: $("#sort option:selected").text() },
        dataType: "text",
        complete: function (data) {
            console.log(data);
            location.reload();
        }
    });
}
function AddItems(InputCount) {
    var div = $("<div class='StudentsRow' id='"+($(".StudentsRow").length-1)+"'></div>");
    for (let index = 0; index < InputCount; index++) {
        $(div).append("<input>");
    }
    $(div).append('<button onclick="DeleteRow(this)" class="icon"><svg xmlns="http://www.w3.org/2000/svg" x="0px" y="0px"width="30" height="30"viewBox="0 0 30 30"style=" fill:#CC998D;"><path d="M 13 3 A 1.0001 1.0001 0 0 0 11.986328 4 L 6 4 A 1.0001 1.0001 0 1 0 6 6 L 24 6 A 1.0001 1.0001 0 1 0 24 4 L 18.013672 4 A 1.0001 1.0001 0 0 0 17 3 L 13 3 z M 6 8 L 6 24 C 6 25.105 6.895 26 8 26 L 22 26 C 23.105 26 24 25.105 24 24 L 24 8 L 6 8 z"></path></svg></button>');
    $(".Students").append($(div));
}
function DeleteRow(clicked) {
    console.log(clicked);
    $(clicked).parent().attr("hidden", true);
}