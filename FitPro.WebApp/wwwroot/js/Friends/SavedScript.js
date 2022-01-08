var savedItems = {
    page: 2,
    populateSaveditems: function(item) {
        var template = $('#savedTemplate').html();

        var templateScript = Handlebars.compile(template);

        var context = {
            "idSavedItem": item.idsavedItem,
            "isWorkout": item.isWorkout,
            "link": item.link,
            "name": item.name,
            "idCurrentUser": $("#currentUserId").val()
        };

        var html = templateScript(context);
        $('#savedList').append(html);
    },
    getScrollPosition: function () {
        var s = $(window).scrollTop(),
            d = $(document).height(),
            c = $(window).height();

        return (s / (d - c)) * 100;
    },
}

$(document).scroll(function () {
    var scrollPosition = savedItems.getScrollPosition();
    if (scrollPosition > 60) {
        $.ajax({
            url: "/User/GetSavedItemsList",
            data: {
                currentPage: savedItems.page
            }
        }).done(function (data) {
            data.forEach(function (item) {
                savedItems.populateWorkouts(item);
            })
            savedItems.page++;
        })
    }
});

var detailWorkout = function (event) {
    var linkUrl = $(event.currentTarget).data("link");
    window.location.href = '/Trainer/DetailWorkout?workoutLink=' + linkUrl + '&fromSaved=true';
}

var unsave = function (event) {
    var idItem = $(event.currentTarget).data("iditem");
    var savedItemdiv = '#savedItem_' + idItem;
    $(savedItemdiv).remove();

    $.ajax({
        url: '/User/UnsaveItem',
        data: {
            itemId: idItem,
        },
    });

    $.ajax({
        url: '/User/GetOneSavedItemItem',
        data: {
            itemId: idItem,
        },
    }).done(function (data) {
        if (data != null) {
            savedItems.populateSaveditems(data);
        }

        if ($('.workout-box').length == 0) {
            $('#empty-box').show();
        }
    });
}

var shareItem = function (event) {
    var itemId = $(event.currentTarget).data("iditem");
    var userId = $(event.currentTarget).data("iduser");

    window.location.href = '/User/RecommandItem?itemId=' + itemId + '&fromPage=savedItems';
}

var detailWorkout = function (event) {
    var linkUrl = $(event.currentTarget).data("link");

    window.location.href = '/Trainer/DetailWorkout?workoutLink='
        + linkUrl +'&fromSaved=true';
}

var detailRecipe = function (event) {
    var idRecipe = $(event.currentTarget).data("idrecipe");

    window.location.href = '/Nutritionist/DetailRecipe?idRecipe='
        + idRecipe + '&fromSaved=true';
}
