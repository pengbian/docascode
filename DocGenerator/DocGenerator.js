var mdConvertor = require("./MdConvertor/MdConvertor");
var jsonConvertor = require("./JsonConvertor/JsonConvertor");
var fs = require("fs");

//var mta_path = "./GenDocMetadata/mta/GenDocMetadata.docmta";
//var mdtoc_path = "./GenDocMetadata/mdtoc/GenDocMetadata/";

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
    if (process.argv[2]) {
        var fileList = fs.readdirSync(process.argv[2] + "/mta");
        //For every one .docmta file, build one .html file
        fileList.forEach(function (item) {
            var json = JSON.parse(fs.readFileSync(process.argv[2] + "/mta/" + item, 'utf-8'));
            var resultHtml;
            resultHtml = jsonConvertor.json2html(json, process.argv[2] + "/mdtoc/" + item.replace(".docmta", ""));
            if (!fs.existsSync(process.argv[2] + "/OutputHtml/")) {
                fs.mkdirSync(process.argv[2] + "/OutputHtml/", 0755);
            }
            fs.writeFileSync(process.argv[2] + "/OutputHtml/" + item.replace(".docmta",".html"), htmlTemplate.replace("<!--come here-->",resultHtml) , 'utf-8');
        });
    }
    else {
        console.log("You should input the metadata file path!");
    }
}

main();