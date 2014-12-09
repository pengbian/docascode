var mdConvertor = require("../MdConvertor/MdConvertor");
var fs = require("fs");
var mdtoc_path = "./GenDocMetadata/mdtoc/GenDocMetadata";

function json2html(json, path) {
    mdtoc_path = path;
    var res = "";
    //Whole File
    res += ("<div class='title'>" + json.Id + "</div>\n");
    res += ("<div class='comment'>" + "" + "</div>\n");
    //Namespaces
    for(ni in json.Namespaces){
        var oneNamespace = json.Namespaces[ni];
        res += ("<div class='namespace'>" + id2yaml(oneNamespace.Id) + "</div>\n");
        res += ("<div class='comment'>" + getcomment(oneNamespace.Id, oneNamespace.Id, oneNamespace.Id) + "</div>\n");
        //Classes
        for (ci in oneNamespace.Classes) {
            var oneClass = oneNamespace.Classes[ci];
            res += ("<div class='class'>" + oneClass.Syntax.Content + "</div>\n");
            res += ("<div class='comment'>" + getcomment(oneNamespace.Id, oneClass.Id, oneClass.Id) + "</div>\n");
            //Methods
            for (mi in oneClass.Methods) {
                var oneMethod = oneClass.Methods[mi];
                res += ("<div class='method'>" + oneMethod.Syntax.Content + "</div>\n");
                res += ("<div class='comment'>" + getcomment(oneNamespace.Id, oneClass.Id, oneMethod.Id) + "</div>\n");
            }
        }
    }
    return res;
}
function id2path(id) {
    return id.replace(/:/,"_");
}
function id2file(id) {
    return id.replace(/:/,"_") + ".md";
}
function id2yaml(id) {
    return id.replace(/^N:/, "namespace: ").replace(/^T:/, "class: ").replace(/^M:/, "method: ");
}
function getcomment(namespaceId, fileId, targetId) {
    var namespace_path = id2path(namespaceId);
    var filename = id2file(fileId);
    var tagetyaml = id2yaml(targetId);
    var reg = "---\r\n" + tagetyaml.replace(/\./g, "\\.").replace(/\(/g, "\\(").replace(/\)/g, "\\)") + "\r\n---\r\n([\\s\\S]*?)---";
    var regExp = new RegExp(reg, "m");
    var str = fs.readFileSync(mdtoc_path + "/" + namespace_path + "/" + filename, 'utf-8') + "---";
    return mdConvertor.md2html(regExp.exec(str)[1]);
}

exports.json2html = json2html;