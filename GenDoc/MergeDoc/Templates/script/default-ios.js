function init() {
}


$(document).ready(function () {
    $("article li.section-member a.section-member-detail").click(function () {
        $(this).next("article div.height-container").animate({ 'height': 'toggle' }, 'normal');
    });

    $("article li.section-member").hover(
        function () {
            $(this).find("div.btn").fadeIn();
        }, function () {
            $(this).find("div.btn").fadeOut();
        }
    );
});