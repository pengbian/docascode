var mdConvertor = require("./MdConvertor/MdConvertor");
var jsonConvertor = require("./JsonConvertor/JsonConvertor");
var fs = require("fs");

var mta_path = "./GenDocMetadata/mta/GenDocMetadata.docmta";
var mdtoc_path = "./GenDocMetadata/mdtoc/GenDocMetadata/";

var htmlTemplate = '<!DOCTYPE html>\
<html>\
<head>\
<meta http-equiv="Content-Type" content="text/html; charset=GBK">\
<title>TestDocAsCode</title>\
<link rel="stylesheet" type="text/css" href="css/default.css">\
<link rel="stylesheet" type="text/css" href="css/StyleSheet.css">\
<script type="text/javascript" src="script/prettify.js"></script>\
</head>\
<body onload="prettyPrint()">\
<div style="margin:auto; width:70%; height:100%;" id="file_container">\
<!--come here-->\
</div>\
</body>\
</html>';

function main() {
    var json = JSON.parse(fs.readFileSync(mta_path, 'utf-8'));
    var resultHtml;
    resultHtml = jsonConvertor.json2html(json, mdtoc_path);
    //resultHtml = mdConvertor.md2html(resultHtml);
    fs.writeFileSync("./Output/resultHtml.html", htmlTemplate.replace("<!--come here-->",resultHtml) , 'utf-8');
    //console.log(resultHtml);
    //console.log(jsonConvertor.getcomment("N:GenDocMetadata", "T:GenDocMetadata.AssemblyDocMetadata", "M:GenDocMetadata.AssemblyDocMetadata.TryAddNamespace(GenDocMetadata.NamespaceDocMetadata)"));
}

main();