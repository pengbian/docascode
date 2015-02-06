function init() {
    
}

$(document).ready(function () {
    $("em.sub-title-methodhider").click(function () {
        $("div#"+this.id.replace('hider','detail')).animate({ 'height': 'toggle' }, 'normal');        
    });
    $("em.sub-title-classhider").click(function () {
        $("div#" + this.id.replace('hider', 'detail')).animate({ 'height': 'toggle' }, 'normal');
    });
    $("p.toc-namespace").click(function () {
        $("div#" + this.id.replace('ns', 'nsdetail')).animate({ 'height': 'toggle' }, 'normal');
    });
});