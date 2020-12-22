$(function () {
    $(".nav-link[data-service-id]").click(function () {
        $(".nav-link[data-service-id]")
            .not(this)
            .removeClass("active");
    });
});