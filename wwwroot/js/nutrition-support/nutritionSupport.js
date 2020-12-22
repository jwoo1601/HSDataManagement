$(function () {
    $("#tabDropdown .dropdown-item").click(function () {
        var currentDropdownItem = $(this);

        $("#tabDropdown .dropdown-item")
            .not(this)
            .removeClass("active");

        $("#tabBar .nav-link.active").removeClass("active");
        $("#tabBar .nav-link")
            .filter(function () {
                return $(this).attr("href") === currentDropdownItem.attr("href");
            }).addClass("active");

        $(this).tab("show");
    });

    $("#tabBar .nav-link").on("show.bs.tab", function () {
        var currentTabLink = $(this);

        $("#tabDropdown .dropdown-item.active").removeClass("active");
        $("#tabDropdown .dropdown-item")
            .filter(function () {
                return $(this).attr("href") === currentTabLink.attr("href");
            }).addClass("active");
    });
});