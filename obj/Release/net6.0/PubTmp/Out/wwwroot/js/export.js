function ExportTable(){
    var Table = $("<table id='Export' hidden>");
    $("body").append(Table);
    var TableHead = $("<thead></thead>");
    Table.append(TableHead);
    TableHead.append("<th>Sr</th>");
    TableHead.append("<th>Hi</th>");
    TableHead.append("<th>Lol</th>");
    TableHead.append("<th>Heh</th>");
    ExportToExcel("xlsx", "hello")
}
function ExportToExcel(type, fn) {
    var elt = document.getElementById('Export');
    var wb = XLSX.utils.table_to_book(elt, { sheet: "sheet1" });
    return XLSX.writeFile(wb, fn+"."+type || ('MySheetName.' + (fn || 'xlsx')));
 }