var exec = require('child_process').exec;
var builder = require("./buildHTML/HTMLbuilder").buildHTML;
var fs = require("fs");
var busy = false;
var prehtml = '<!DOCTYPE html>\
<html>\
<head>\
<meta http-equiv="Content-Type" content="text/html; charset=GBK">\
<title>OpenAuthoring</title>\
<link rel="stylesheet" type="text/css" href="css/default.css">\
<script type="text/javascript" src="script/prettify.js"></script>\
</head>\
<body onload="prettyPrint()">\
<p style="text-align:center;">\
<a href="editor/index.html" target="_blank">Want to update the document?</a>\
</p>\
<div style="margin:auto; width:70%; height:100%;" id="file_container">';
var posthtml = '</div>\
</body>\
</html>';

var templateHTML;
var navigatorMD;
var fileList = [];

function idle() {
    busy = true;
    exec('git remote show origin', function (err, stdout, stderr) {
        if (err) exit_with_log("git status failed!" + err.toString());
        else {
            //console.log('stdout:\n' + stdout + '\n\nstderr:\n' + stderr);
            if (stdout.indexOf('up to date') == -1) build();
            else busy = false;
        }
    });
};
function build() {
    console.log('One commit triggered!Builing html....');
    //pull
    exec('git pull --force', function (err, stdout, stderr) {
        if (err) exit_with_log("git pull failed!" + err.toString());
        else {
            //build
            init_navigator();
            walk('./');
            fileList.forEach(function (item) { make_one_file(item); });
            console.log('Update successfully!');
            busy = false;
            //fs.readFile('./index.md', 'utf-8', function (err, data) {
            //    if (err) exit_with_log("read md failed!" + err.toString());
            //    else {
            //        //write
            //        fs.writeFile('../webroot/index.html', prehtml + builder(data) + posthtml, 'utf-8', function (err) {
            //            if (err) exit_with_log("write html failed!" + err.toString());
            //            else {
            //                //push
            //                console.log('Update successfully!');
            //                busy = false;
            //            }
            //        });
            //    }    
            //});
        }
    });
};
function exit_with_log(msg) {
    console.log(msg);
    busy = false;
}
function make_one_file(filename) {
    var fileMD = fs.readFileSync(filename, 'utf-8');
    fs.writeFileSync('../webroot/'+filename.replace('.md','.html'), 
        templateHTML.replace('<!--here is navigator!-->', builder(navigatorMD)).replace('<!--here is content!-->', builder(fileMD)),
        'utf-8');
}
function init_template(){
    templateHTML = fs.readFileSync('../template/layout.html', 'utf-8');
}
function init_navigator() {
    navigatorMD = fs.readFileSync('./navigator.md', 'utf-8');
}
function walk(path) {
    var dirList = fs.readdirSync(path);
    dirList.forEach(function (item) {
        if (item == '.git') return;
        if (fs.statSync(path + '/' + item).isDirectory()) {
            walk(path + '/' + item);
        } else {
            fileList.push(path + '/' + item);
        }
    });
}

function main(){
    exec('sleep 2', function (err, stdout, stderr) {
        if (err) console.log("sleep failed!" + err.toString());
        else {
            if (!busy) {
                idle();
            }
            main();
        }
    });
}



//Run 
//Switch to the git directory(Because we will use git command)
process.chdir(__dirname + '/../OpenAuthoring/');

console.log("Initializing template....");
init_template();

console.log("Running buildservice....");
main();
