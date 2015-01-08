function init() {
}


$(document).ready(function () {
    $("article li.section-member a.section-member-name").click(function () {
        $(this).next("article div.height-container").animate({ 'height': 'toggle' }, 'normal');
    });
});