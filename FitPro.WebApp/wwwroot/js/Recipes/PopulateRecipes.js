var currentUserId = $('#CurrentUserId').val();
var currentUserRole = $('#CurrentUserRole').val();



var recipes = {
    page: 1,
    PossibleNextPage: true,
    getRecipes: function (item) {
        var template = $('#recipesTemplate').html();

        var templateScript = Handlebars.compile(template);


        var context = {
            "idUser": currentUserId,
            "role": currentUserRole,
            "name": item.name,
            "idRecipe": item.idRecipe,
            "isSaved": item.isSaved
        };

        var html = templateScript(context);

        $('#recipesList').append(html);

        var savediv = '#save_' + item.idRecipe;
        var unsavediv = '#unsave_' + item.idRecipe;

        if (item.isSaved) {
            $(savediv).hide();
            $(unsavediv).show();
        } else {
            $(savediv).show();
            $(unsavediv).hide();
        }
    },
    getScrollPosition: function () {
        var s = $(window).scrollTop(),
            d = $(document).height(),
            c = $(window).height();
        return (s / (d - c)) * 100;
    },
}

var filters = {
    hide: true
}

var getFilter = function () {
    return {
        SortColumn: $('#select-sort-W').val().split(" ")[0],
        SortColumnIndex: $('#select-sort-W').val().split(" ")[1],
        SearchString: $('#search-input-W').val(),
        LowerTimeLimit: $('#select-time').val().split(" ")[0],
        UpperTimeLimit: $('#select-time').val().split(" ")[1],
        LowerCaloriesLimit: $('#select-calories').val().split(" ")[0],
        UpperCaloriesLimit: $('#select-calories').val().split(" ")[1],
        SelectedCategories: $('#select-categories').val(),
        SelectedNutritionist: $('#select-nutritionists').val()
    };
}

$(document).ready(function () {
    $('#select-categories').select2({ multiple: true });
    $('#select-nutritionists').select2({ multiple: true });

    $('#addFilter').click(function () {
        if (filters.hide) {
            filters.hide = false;
            $('.recipeFilters').show();
        } else {
            filters.hide = true;
            $('.recipeFilters').hide();
        }
    });

    $('#filterNow').click(function () {
        $('.recipeFilters').hide();


        recipes.page = 1;

        $.ajax({
            url: '/Nutritionist/GetRecipesList',
            data: {
                currentPage: recipes.page,
                FilterJsonString: JSON.stringify(getFilter()),
            },
        }).done(function (data) {
            $('#recipesList').empty();

            if (Array.isArray(data)) {
                data.forEach(function (item) {
                    recipes.getRecipes(item);
                });
            }
        });
    });
})

$(document).scroll(function () {
    var scrollPosition = recipes.getScrollPosition();

    if (scrollPosition > 60) {
        recipes.page++;
        $.ajax({
            url: "/Nutritionist/GetRecipesList",
            data: {
                currentPage: recipes.page,
                FilterJsonString: JSON.stringify(getFilter()),
            }
        }).done(function (data) {
            if (Array.isArray(data)) {
                data.forEach(function (item) {
                    recipes.getRecipes(item);
                });
            }
        })
    }
});


var detailRecipe = function (event) {
    var idRecipe = $(event.currentTarget).data("idrecipe");

    $.ajax({
        url: '/Nutritionist/DetailRecipe',
        data: {
            idRecipe: idRecipe
        },
        success: function (data) {
            window.location.href = '/Nutritionist/DetailRecipe?idRecipe=' + idRecipe;
        }
    });
}

var editRecipe = function (event) {
    var idRecipe = $(event.currentTarget).data("idrecipe");

    $.ajax({
        url: '/Nutritionist/EditRecipe',
        data: {
            idRecipe: idRecipe
        },
        success: function (data) {
            window.location.href = '/Nutritionist/EditRecipe?idRecipe=' + idRecipe;
        }
    });
}

var deleteRecipe = function (event) {
    var idRecipe = $(event.currentTarget).data("idrecipe");

    $.ajax({
        url: '/Nutritionist/DeleteRecipe',
        data: {
            idRecipe: idRecipe,
            currentId: userId
        },
        success: function (data) {
            window.location.href = '/Nutritionist/DeleteRecipe?idRecipe=' + idRecipe;
        }
    });
}

var saveRecipe = function (event) {
    var recipeId = $(event.currentTarget).data("idrecipe");
    var userId = $(event.currentTarget).data("iduser")

    var savediv = '#save_' + recipeId;
    var unsavediv = '#unsave_' + recipeId;

    $(savediv).hide();
    $(unsavediv).show();

    $.ajax({
        url: '/User/SaveItem',
        data: {
            currentUserId: userId,
            itemId: recipeId,
        },
    });
}

var unsaveRecipe = function (event) {
    var recipeId = $(event.currentTarget).data("idrecipe");
    var userId = $(event.currentTarget).data("iduser");

    var savediv = '#save_' + recipeId;
    var unsavediv = '#unsave_' + recipeId;

    $(savediv).show();
    $(unsavediv).hide();

    $.ajax({
        url: '/User/UnsaveItem',
        data: {
            currentUserId: userId,
            itemId: recipeId,
        },
    });
}

var shareRecipe = function (event) {
    var recipeId = $(event.currentTarget).data("idrecipe");
    var userId = $(event.currentTarget).data("iduser");

    window.location.href = '/User/RecommandItem?currentUserId=' + userId
        + '&itemId=' + recipeId + '&fromPage=recipes';
}