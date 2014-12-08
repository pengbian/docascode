var pagedown = require("./pagedown/node-pagedown");
var converter = new pagedown.Converter();
var pagedownExtra = require("./node-pagedown-extra");
//var pretty = require("./demo/prettify");
Markdown.Extra.init(converter, { highlighter: "prettify" });


function md2html(content) {
    return converter.makeHtml(content);
}

exports.md2html = md2html;