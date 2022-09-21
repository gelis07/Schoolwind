function Update(TableName, ids, columns, colength) {
    var allVal = [];
    var hidden = [];
    var number = 1;
    $(".StudentsRow > input").each(function (index) {
        allVal.push($(this).val());
    });
    for (let index = 0; index < $(".StudentsRow > input").length; index++) {
        if (index === $(".StudentsRow > input").length) {
            if ($($(".StudentsRow > input")[index]).parent().is(":hidden")) {
                hidden.push(true);
            } else {
                hidden.push(false);
            }
            return false;
        }
        if (number === colength) {
            if ($($(".StudentsRow > input")[index]).parent().is(":hidden")) {
                hidden.push(true);
            } else {
                hidden.push(false);
            }
            number = 0;
        }
        number += 1;
    }
    console.log(allVal);
    console.log(hidden);
    $.ajax({
        type: "POST",
        url: "/Main/Update",
        data: { StudentData: allVal, hidden: hidden, ids: ids, table: TableName, Columns: columns, colength: colength },
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
function Search(columns, table, sort) {
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
function AddItems(InputCount, relations = false) {
    var div = $("<div class='StudentsRow'></div>");
    $(".Students").append(div);
    for (let index = 0; index < InputCount; index++) {
        $(div).append("<input>");
    }
    $(div).append("<button onclick='DeleteRow(this)'>Delete</button>");
    if (relations) {
        $(div).append("<button onclick='Relations(@sm.ids[i])'>Arrow Placeholder</button>");
    }
}
function Relations(id, ColRelation, TableRelation, ColMain, TableMain, WhereCol, ColPrint, columnLength) {
    if ($(".relations") != null) {
        $(".relations").remove();
    }
    $.ajax({
        type: 'POST',
        url: '/Main/Relations',
        data: { ID: id, ColRelation: ColRelation, TableRelation: TableRelation, ColMain: ColMain, TableMain: TableMain, WhereCol: WhereCol },
        dataType: 'json',
        complete: function (output) {
            var data = output.responseJSON;
            var relationsDiv = $("<div class='relations'></div>");
            $(".main").append(relationsDiv);
            console.log(relationsDiv);
            for (let index = 0; index < data[1]; index++) {
                var StudentsRow = $("<div class='StudentsRow'></div>");
                $(relationsDiv).append(StudentsRow);
                console.log(StudentsRow);
                for (let j = 0; j < data[2]; j++) {
                    //"<h3>"+data[0][j]+"</h3>"
                    StudentsRow.append("<input value=" + data[0][j + (data[2] * index)] + ">");
                }
                StudentsRow.append("<button>Delete</button>");
            }
        }
    });
}
function DeleteRow(clicked) {
    console.log(clicked);
    $(clicked).parent().attr("hidden", true);
}