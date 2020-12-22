$(function () {
    $("#tabMenuList").one("shown.bs.tab", function () {
        displayMenuListInitialTab();
    });

    $("#dropdownBtnMenuList").one("click", function () {
        displayMenuListInitialTab();
    });

    $("[data-category=menuList]")
        .on("click", "[data-action=reload]", function () {
            var fetcher;

            var category = getCurrentMenuCategory();
            switch (category) {
                case "food":
                    fetcher = fetchFoodList;
                    break;
                case "category":
                    fetcher = fetchFoodCategoryList;
                    break;
                case "ingredient":
                    fetcher = fetchFoodIngredientList;
                    break;
            }

            if (fetcher) {
                fetcher();
            }
        })
        .on("click", "[data-action=add]", function () {
            var openDialog;

            var category = getCurrentMenuCategory();
            switch (category) {
                case "food":
                    openDialog = openAddFoodDialog;
                    break;
                case "category":
                    openDialog = openAddFoodCategoryDialog;
                    break;
                case "ingredient":
                    openDialog = openAddFoodIngredientDialog;
                    break;
            }

            if (openDialog) {
                openDialog();
            }
        });
});

var isInitialTabShown = false;
function displayMenuListInitialTab() {
    if (!isInitialTabShown) {
        $("#menuListTabFood").tab("show");

        isInitialTabShown = true;
    }
}

function getCurrentMenuCategory() {
    return $("#menuListTabBar .nav-link.active").data("category");
}